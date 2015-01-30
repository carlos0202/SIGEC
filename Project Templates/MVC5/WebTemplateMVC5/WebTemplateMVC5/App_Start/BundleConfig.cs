using System.Web;
using System.Web.Optimization;

namespace WebTemplateMVC5
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/sweet-alert.js",
                        "~/Scripts/bootstrapPager.1.0.6.min.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/chosen.jquery.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/sweet-alert.css").Include("~/Content/bootstrap-chosen.css", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/bundles/extensions").Include(
                       "~/Scripts/extensions.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                       "~/Scripts/DataTables-1.10.4/jquery.dataTables.js", "~/Scripts/DataTables-1.10.4/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/DataTables-1.10.4/css/sytles")
                            .Include("~/Content/DataTables-1.10.4/css/jquery.dataTables.css", new CssRewriteUrlTransform())
                            .Include("~/Content/DataTables-1.10.4/css/dataTables.colVis.css")
                            .Include("~/Content/DataTables-1.10.4/css/dataTables.tableTools.css"));
        }
    }
}
