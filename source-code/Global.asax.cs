using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string MoneySymbol = "$";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SyncMoneySymbol();

        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            SyncMoneySymbol();
        }
        public static void SyncMoneySymbol()
        {
            var orderSetting = new veebdbEntities().ordersettings.Where(t => t.status == 1).FirstOrDefault();
            if (orderSetting!=null)
            {
                MoneySymbol = orderSetting.code;
            }
        }
    }
}
