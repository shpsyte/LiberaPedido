using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiberaPedido.Controllers
{
    public class HomeController : PublicBaseController
    {
        public ActionResult Index()
        {
            //  var data = db.Pedidos.ToList();
            return View();
        }

        public ActionResult ReadAccountJs(ParametersDataTable param)
        {
            string[] centrocusto = db.Usuario.Where(a => a.cd_usuario == cd_usuario).Select(a => a.email_particular).FirstOrDefault().Trim().Split(';');

            var allItem = db.Pedidos.Where(a => centrocusto.Contains(a.descricao.Trim())).ToList();
            
            IEnumerable<VPedidoVenda> filteredCompanies;

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<VPedidoVenda, string> orderingFunction = (c => sortColumnIndex == 0 ? c.nr_pedido.ToString() :
                                                                sortColumnIndex == 1 ? c.nr_original :
                                                                sortColumnIndex == 2 ? c.descricao :
                                                                sortColumnIndex == 3 ? c.vl_pedido.ToString() :
                                                                c.nr_pedido.ToString());


            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredCompanies = db.Pedidos.ToList().Where(o => centrocusto.Contains(o.descricao.Trim()))
                         .Where(c => c.nr_pedido.ToString().ToUpper().Contains(param.sSearch.ToUpper())
                                     || c.descricao.ToUpper().Contains(param.sSearch.ToUpper())
                          );
            }
            else
            {
                filteredCompanies = allItem;
            }


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);


            var displayedCompanies = filteredCompanies
                         .Skip(param.iDisplayStart)
                         .Take(param.iDisplayLength);


            var result = from c in displayedCompanies
                         select new
                         {
                             acao = "",
                             pedido = Convert.ToString(c.nr_pedido),
                             empresa = Convert.ToString(c.cd_empresa),
                             data = c.dt_pedido.ToShortDateString(),
                             original = c.nr_original,
                             descricao = c.descricao,
                             valor = c.vl_pedido.ToString("c"),
                             Usuario = c.firstname  + " " + c.lastname
                         };



            JsonResult jsonResult = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allItem.Count(),
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;


            return jsonResult;



        }


        public PartialViewResult GetDetailsFromDoc(int cd_empresa, int nr_pedido)
        {
            string sql = "";
            sql = String.Format(" Select a.cd_item, b.descricao, a.qt_pedida, a.vl_unitario, a.vl_total from pedidoitem a inner join item b on a.cd_item = b.cd_item  " +
                                " where A.cd_empresa = {0} and a.nr_pedido = {1}", cd_empresa, nr_pedido);
            var Itens = db.Database.SqlQuery<GetItensDoc>(sql).ToList();

            return PartialView(Itens);
        }



        public ActionResult Resposta(int cd_empresa, int nr_pedido)
        {
            Pedido_Resposta_Model data = new Pedido_Resposta_Model { Obs = "", VPedidoVenda = db.Pedidos.Find(cd_empresa, nr_pedido) };

            if (data.VPedidoVenda == null)
            {
                return InvokeHttpNotFound();
            }

            return View(data);


        }

        [HttpPost]
        [ParameterBasedOnFormName("save-naolibera", "naoLibera")]
        public ActionResult Resposta(Pedido_Resposta_Model data, int cd_empresa, int nr_pedido, bool naoLibera)
        {
            if (string.IsNullOrEmpty(data.Obs))
            {
               ModelState.AddModelError(" ", "Campo Obs é obrigatório");
            }


            string situacao = "L";
            if (data.VPedidoVenda == null)
            {
                return InvokeHttpNotFound();
            }

            string sqlcancela = "";

            if (naoLibera)
            {
                situacao = "X";
                sqlcancela = string.Format(" INSERT INTO pedidocancela (cd_empresa, nr_pedido, cd_cancelamento, observacao, cd_usuario ) VALUES( {0}, {1}, {2}, \'{3}\', {4} ) ", cd_empresa, nr_pedido, 1, data.Obs, cd_usuario);

            }




            string sql = string.Format(" UPDATE PedidoVenda SET situacao = \'{0}\' WHERE cd_empresa = {1} and nr_pedido = {2}  ", situacao, cd_empresa, nr_pedido);
            string sqlcompl = string.Format(" UPDATE PedidoComplemento SET descricao = \'{0}\' WHERE cd_empresa = {1} and nr_pedido = {2}  ", data.Obs, cd_empresa, nr_pedido);
            string sqlobs = string.Format(" INSERT INTO PedidoMsg (cd_empresa, nr_pedido, msg ) VALUES( {0}, {1}, \'{2}\') ", cd_empresa, nr_pedido, data.Obs);



            try
            {
                db.Database.ExecuteSqlCommand(sql);
                db.Database.ExecuteSqlCommand(sqlobs);
                db.Database.ExecuteSqlCommand(sqlcompl);
                
                if (!string.IsNullOrEmpty(sqlcancela))
                {
                    db.Database.ExecuteSqlCommand(sqlcancela);
                }

                _email.EnviarEmailCampanha(cd_empresa, nr_pedido, situacao, data.Obs);

                return RedirectToAction("Index", "Home");

            }
            catch (Exception error)
            {
                throw new Exception(error.ToString());
            }





            return View(data);


        }



    }
}