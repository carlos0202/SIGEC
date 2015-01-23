using System.Web;
using System.Web.Optimization;

namespace DGII_PFD
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725ç
        /// <summary>
        /// Secciones de agrupación y minificación de archivos javascript y css
        /// para minimizar la cantidad de peticiones de archivos desde el servidor.
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.ui.datepicker-es.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/FileSizeValidationRules.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js", 
                        "~/Scripts/bootstrap-switch.js",
                        "~/Scripts/sweet-alert.js",
                        "~/Scripts/bootstrapPager.1.0.6.min.js",
                        "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                        "~/Scripts/jquery-ui-sliderAccess.js",
                        "~/Scripts/jquery-ui-timepicker-es.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                       "~/Scripts/DataTables-1.10.2/jquery.dataTables.js", "~/Scripts/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Scripts/chosen*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/js/Global.js")
                .Include("~/js/Global2.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/Scripts/js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/site.css", "~/Content/Site2.css",  "~/Content/sweet-alert.css")
                .Include("~/Content/chosen.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap-datepicker3.css"));
            //bundles.Add(new StyleBundle("~/Content/bootstrapmodal").Include("~/Content/bootstrap-modal-bs3patch.css", "~/Content/bootstrap-modal.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/bootstrap-switch.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables-1.10.2/css/sytles")
                            .Include("~/Content/DataTables-1.10.2/css/jquery.dataTables.css", new CssRewriteUrlTransform())
                            .Include("~/Content/DataTables-1.10.2/css/dataTables.colVis.css")
                            .Include("~/Content/DataTables-1.10.2/css/dataTables.tableTools.css"));
        }
    }
}