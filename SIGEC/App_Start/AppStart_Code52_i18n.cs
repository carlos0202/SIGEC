[assembly: WebActivator.PreApplicationStartMethod(typeof(SIGEC.App_Start.Code52_i18n), "Start")]
namespace SIGEC.App_Start {

    using System.Web.Mvc;
    using System.Web.Routing;
	using Code52.i18n;

    public class Code52_i18n {
        /// <summary>
        /// Metodo utilizado para agregar la ruta para el controller que contiene
        /// la localización de los mensajes manejados desde el código javascript.
        /// </summary>
        public static void Start() {
            RouteTable.Routes.MapRoute("Language", "i18n/Code52.i18n.language.js", new { controller = "Language", action = "Language" });
			GlobalFilters.Filters.Add(new LanguageFilterAttribute());
        }
    }
}
