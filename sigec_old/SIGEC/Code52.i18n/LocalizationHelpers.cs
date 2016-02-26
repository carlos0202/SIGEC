namespace SIGEC.Code52.i18n
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class LocalizationHelpers {
        /// <summary>
        /// Metodo utilizado para construir la etiqueta meta en la que se establece
        /// la preferencia de cultura para el usuario de la aplicación.
        /// </summary>
        /// <typeparam name="T">Tipo generico del view que ejecuta el método</typeparam>
        /// <param name="html">Objeto html utilizado para construir el tag.</param>
        /// <returns></returns>
        public static IHtmlString MetaAcceptLanguage<T>(this HtmlHelper<T> html) {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(String.Format("<meta name=\"accept-language\" content=\"{0}\">", acceptLanguage));
        }
    }
}