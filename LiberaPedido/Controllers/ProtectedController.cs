using LiberaPedido.Models;
using Data.Context;
using Domain.Entity;
using LiberaPedido.App_Helpers;
using Microsoft.Ajax.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiberaPedido.Controllers
{
    [AllowAnonymous]
    public class ProtectedController : Controller
    {
        private b2yweb_entities db = null;


        // GET: /Login/
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            Logout();
            // We do not want to use any existing identity information
            EnsureLoggedOut();
            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }
        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid) return View(viewModel);
            // instancia a entidade com a conexão do cliente
            db = new b2yweb_entities("bavatos");
            //crip senha
            string senha = crypto.Criptografa(viewModel.Password.Trim().ToUpper());
            // recupera o usuario com os dados passados
            var oUsuario = db.Usuario.Where(s => s.login.ToUpper().Equals(viewModel.Email.ToUpper()))
                                     .Where(s => s.senha.Equals(senha))
                                     .Where(s => s.situacao.Equals("A")).FirstOrDefault();

            if (oUsuario != null)
            {
                HttpCookie cookie = new HttpCookie("PortalProcedimento");
                cookie.Values.Add("usuario", oUsuario.nome);
                cookie.Values.Add("cd_usuario", oUsuario.cd_usuario.ToString());
                Response.SetCookie(cookie);
                Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(oUsuario.nome, viewModel.RememberMe);
                return RedirectToLocal(viewModel.ReturnUrl);
            }
            // No existing user was found that matched the given criteria
            ModelState.AddModelError("", "Login ou senha inválidos.");
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }
        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("Index", "Home");
        }
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated) Logout();
        }
        public ActionResult SairDoSistema()
        {
            //first clear session
            Session.Clear();
            //second remove cache
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //tirth session abandon
            Session.Abandon();
            FormsAuthentication.SignOut();
            //after clean the authentication ticket like always
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            return RedirectToAction("Login");
        }
        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            //first clear session
            Session.Clear();
            //second remove cache
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //tirth session abandon
            Session.Abandon();
            FormsAuthentication.SignOut();
            //after clean the authentication ticket like always
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                Request.Cookies.Remove(cookie);
            }
            return RedirectToLocal();
        }
    }

}