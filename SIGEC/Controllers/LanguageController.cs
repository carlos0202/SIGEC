namespace SIGEC.Controllers 
{
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    //[OutputCache(Duration = 60, VaryByCustom = "culture")]
    /// <summary>
    /// Clase controlador con acciones utilizadas para proveer
    /// los mensajes javascript de la aplicación de acuerdo al
    /// lenguaje utilizado por el cliente.
    /// </summary>
    [AllowAnonymous]
    public class LanguageController : BaseController 
    {

        /// <summary>
        /// Método que retorna un resultado en formato javascript en el
        /// cual se retorna un diccionario cuyas claves se utilizan para
        /// obtener los mensajes en los distintos idiomas soportados por
        /// la aplicación.
        /// </summary>
        /// <returns>un JavascriptResult con los mensajes localizados.</returns>
        public JavaScriptResult Language()
        {
            return GetResourceScript(Resources.Language.ResourceManager);
        }

        /// <summary>
        /// Método utilizado para retornar y obtener el diccionario
        /// de mensajes localizados utilizando la cultura actual de
        /// la aplicación, y si el diccionario no está en la cache
        /// del HttpRuntime, se inserta.
        /// </summary>
        /// <param name="resourceManager">objeto para manejar los recursos localizados.</param>
        /// <returns>diccionario con mensajes localizados en javascript.</returns>
        JavaScriptResult GetResourceScript(ResourceManager resourceManager)
        {
            var cacheName = string.Format("ResourceJavaScripter.{0}", CultureInfo.CurrentCulture.Name);
            var value = HttpRuntime.Cache.Get(cacheName) as JavaScriptResult;
            if (value == null)
            {
                JavaScriptResult dictionary = CreateResourceScript(resourceManager);
                HttpContext.Cache.Insert(cacheName, dictionary);
                return dictionary;
            }
            return value;
        }

        /// <summary>
        /// Método utilizado para crear el diccionario javascript con
        /// los mensajes localizados que se utilizan desde el código
        /// javascript de la aplicación.
        /// </summary>
        /// <param name="resourceManager">objeto para manejar los recursos localizados.</param>
        /// <returns>un objeto javascript con los mensajes localizados de la aplicación.</returns>
        static JavaScriptResult CreateResourceScript(ResourceManager resourceManager)
        {
            var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            var sb = new StringBuilder("Code52.Language.Dictionary={");
            foreach (DictionaryEntry dictionaryEntry in resourceSet)
            {
                var s = dictionaryEntry.Value as string;
                if (s == null)
                {
                    continue;
                }
                string value = resourceSet.GetString((string)dictionaryEntry.Key) ?? s;
                sb.AppendFormat(
                    "\"{0}\":\"{1}\",",
                    dictionaryEntry.Key,
                    Microsoft.Security.Application.Encoder.JavaScriptEncode(value.Replace("\"", "\\\"").Replace('{', '[').Replace('}', ']'), false)
                );
            }
            string script = sb.ToString();
            if (!string.IsNullOrEmpty(script) && script.Last() != '{')
            {
                script = script.Remove(script.Length - 1);
            }
            script += "};";
            return new JavaScriptResult { Script = script };
        }
    }
}
