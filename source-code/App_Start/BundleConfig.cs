using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/myscript").Include(
                        "~/Scripts/myscript.js",
                        "~/Scripts/facebox.js"));
           // bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/myscript.js"));
          //  bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-editable-select.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/facebox.css"));

            //admin 
            bundles.Add(new StyleBundle("~/Content/admin/css").Include(
                     "~/Content/admin/admin-style.css",
                     "~/Content/admin/all.css",
                     "~/Content/admin/bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/Content/admin/js").Include(
                     "~/Content/js/admin.js",
                     "~/Content/js/jquery.js"
                     ));
        }
    }
}
