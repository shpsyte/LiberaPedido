using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LiberaPedido
{
    public class MvcApplication : System.Web.HttpApplication
    {
    //    protected void Application_AcquireRequestState(object sender, EventArgs e)
    //    {
    //        //CultureInfo ci = new CultureInfo("en-USN");
    //        //CultureInfo ci = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
    //        //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
    //        //System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
    //        if (HttpContext.Current.Session != null)
    //        {
    //            CultureInfo cultureInfo = (CultureInfo)this.Session["Culture"];
    //            if (cultureInfo == null)
    //            {
    //                string langName = "en";

    //                if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
    //                    langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);

    //                cultureInfo = new CultureInfo(langName);
    //                this.Session["Culture"] = cultureInfo;
    //            }

    //            //Finally setting culture for each request
    //            Thread.CurrentThread.CurrentUICulture = cultureInfo;
    //            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
    //        }

    //    }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
    }
}
