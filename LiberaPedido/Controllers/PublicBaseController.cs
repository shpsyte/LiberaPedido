using Data.Context;
using LiberaPedido.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LiberaPedido.Controllers
{

    [iFilterLog, HandleError]

    public class PublicBaseController : Controller
    {


        #region variavel
        protected b2yweb_entities db = null;
        protected short cd_usuario { get; set; }
        protected string nome_usuario { get; set; }
        protected DateTime dt_atual_com_hora_sql { get; set; }
        protected DateTime dt_atual_sem_hora_sql { get; set; }
        protected List<int> list_regional = new List<int>();
        protected HttpCookie cookie;
        protected SendEmail _email = new SendEmail();


        #endregion variavel


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            db = new b2yweb_entities("bavatos");
            dt_atual_com_hora_sql = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dt_atual_sem_hora_sql = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            cookie = HttpContext.Request.Cookies.Get("PortalProcedimento");


            try
            {
                cd_usuario = Convert.ToInt16(cookie.Values["cd_usuario"].ToString());
            }
            catch
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
            }



        }



        protected virtual ActionResult InvokeHttpNotFound()
        {

            var routeData = new RouteData();

            routeData.Values.Add("controller", "Crm");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("area", "");


            //return RedirectToAction("Error", "Crm",  new { controller = "Crm", action = "Error", area = "" } );
            return RedirectToAction("Error");
        }



        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}