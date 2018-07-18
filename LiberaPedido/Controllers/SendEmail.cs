using Data.Context;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;


namespace LiberaPedido.Controllers
{

    public class SendEmail
    {
        private b2yweb_entities db = null;

        private string _email;
        private string _username;
        private string _password;
        private NetworkCredential _logininfo;
        private SmtpClient _smtpcient;

        public SendEmail()
        {
            this._email = ConfigurationManager.AppSettings["MailFrom"].ToString();
            this._username = ConfigurationManager.AppSettings["MailUserName"].ToString();
            this._password = ConfigurationManager.AppSettings["MailPwd"].ToString();
            this._logininfo = new NetworkCredential(_username, _password);
            this._smtpcient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString(), Int32.Parse(ConfigurationManager.AppSettings["Port"]));

            //temp
            //_email = "jose.luiz@iscosistemas.com.br";
            //_username = "jose.luiz@iscosistemas.com.br";
            //_password = "Jymkatana_6985";
            //_logininfo = new NetworkCredential(_username, _password);
            //_smtpcient = new SmtpClient("smtp.iscosistemas.com.br", 587);

            _smtpcient.EnableSsl = false;
            _smtpcient.UseDefaultCredentials = false;
            _smtpcient.Credentials = _logininfo;


        }


        public void EnviarEmailCampanha(int cd_empresa, int nr_pedido, string situacao, string obs)
        {
            db = new b2yweb_entities("bavatos");


            string body = "";
            string url = "";
            string _lastinformation = "";
            string _assunto = "";
            string _situacaoAtual = "";

            var msg = new MailMessage();
            msg.To.Add(new MailAddress("arthur@bavatos.com.br", "Arthur"));
            msg.To.Add(new MailAddress("leila@bavatos.com.br", "Leila"));
            //msg.To.Add(new MailAddress("jose.luiz@iscosistemas.com.br", "José Luiz"));

            string _situacao = "";

            switch (situacao)
            {
                case "L":
                    _situacao = "Liberado";
                    break;
                case "X":
                    _situacao = "Cancelado";
                    break;
            }

            body = "Pedido da empresa " + cd_empresa.ToString() + " de número " + nr_pedido.ToString() + " foi " + _situacao;
            body += " msg " + obs;


            msg.From = new MailAddress(_email);
            msg.Subject = "[pedido]" + " " + nr_pedido.ToString();
            msg.Body = body;
            msg.IsBodyHtml = true;

            try
            {
                _smtpcient.Send(msg);
            }
            catch (Exception e)
            {
                return;
            }



        }


        

        

        private string PopulateBody(
            string userName,
            string title,
            string url,
            string lastdescription,
            string Number,
            string Assunto,
            string SituacaoAtual,
            string modelo

            )
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/" + modelo)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{Number}", Number);
            body = body.Replace("{Assunto}", Assunto);
            body = body.Replace("{LastInformation}", lastdescription);
            body = body.Replace("{SituacaoAtual}", SituacaoAtual);

            return body;
        }



    }
}