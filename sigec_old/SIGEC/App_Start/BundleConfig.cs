using System.Web;
using System.Web.Optimization;

namespace SIGEC
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
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap*", "~/Scripts/bootstrapmodal/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                        "~/Scripts/jquery-ui-sliderAccess.js",
                        "~/Scripts/jquery-ui-timepicker-es.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                        "~/Scripts/globalize/globalize.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_cookie").Include(
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/maskedinput").Include(
                        "~/Scripts/jquery.inputmask/jquery.inputmask-{version}.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.extensions-{version}.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions-{version}.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/foolprof").Include(
                        "~/Scripts/MvcFoolproofJQueryValidation.js",
                         //"~/Scripts/MvcFoolproofValidation.js",
                        "~/Scripts/mvcfoolproof.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                       "~/Scripts/DataTables-1.9.4/media/js/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Scripts/chosen*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/js/Global.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/DataTables-1.9.4/media/css/css").Include(
                        "~/Content/DataTables-1.9.4/media/css/*.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapmodal").Include("~/Content/bootstrap-modal-bs3patch.css", "~/Content/bootstrap-modal.css"));

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
                        "~/Content/chosen.css"));
        }
    }
}