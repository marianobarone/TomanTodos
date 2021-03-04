using System.Web;
using System.Web.Optimization;

namespace TomanTodos
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js")
                    .Include("~/Content/lib/dataTables/jquery.dataTables.min.js")
                    );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                    .Include("~/Scripts/bootstrap.js")
                    .Include("~/Content/lib/dataTables/datatables.min.js")
                    .Include("~/Content/lib/dataTables/dataTables.bootstrap4.min.js")
                    
                    );

            bundles.Add(new StyleBundle("~/Content/css")
                    .Include("~/Content/bootstrap.css",
                      "~/Content/site.css")
                    .Include("~/Content/lib/dataTables/dataTables.min.css")
                    .Include("~/Content/lib/dataTables/dataTables.bootstrap4.min.css")
                    //.Include("~/Content/lib/dataTables/jquery.dataTables.min.css")
                    );
        }
    }
}
