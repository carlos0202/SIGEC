using PWDTK_DOTNET451;
using SIGEC.Models;
using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGEC.CustomCode
{
    public class GlobalHelpers
    {

        /// <summary>
        /// Metodo utilitario privado para traspasar los valores
        /// de las propiedades con igual nombre desde un objeto
        /// a otro.
        /// </summary>
        /// <param name="source">Instancia del objeto del cual se obtendrán los datos.</param>
        /// <param name="target">Instancia del objeto que recibirá los datos.</param>
        static void Transfer(object source, object target, List<string> toSkip = null)
        {
            var sourceType = source.GetType(); //tipo de objeto de instancia fuente
            var targetType = target.GetType(); //tipo de objeto de instancia destino

            //creación de parámetros para la expresión lambda
            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var targetParameter = Expression.Parameter(typeof(object), "target");

            //creación de variables para la expresión lambda
            var sourceVariable = Expression.Variable(sourceType, "castedSource");
            var targetVariable = Expression.Variable(targetType, "castedTarget");

            var expressions = new List<Expression>();
            //agregar variables y parámetros a las expresiones lambda a ejecutar
            expressions.Add(Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, sourceType)));
            expressions.Add(Expression.Assign(targetVariable, Expression.Convert(targetParameter, targetType)));

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead) // verificar si la propiedad fuente admite lectura.
                    continue;

                if (toSkip != null)
                    if (toSkip.Contains(property.Name))
                        continue;

                var targetProperty = targetType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null
                        && targetProperty.CanWrite //se puede escribir en la propiedad de destino?
                        && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
                {
                    expressions.Add(
                        Expression.Assign( //expresión para la asignación de las propiedades de los objetos.
                            Expression.Property(targetVariable, targetProperty),
                            Expression.Convert(
                                Expression.Property(sourceVariable, property), targetProperty.PropertyType)));
                }
            }

            // creación formal de la expresión lambda a ejecutar.
            var lambda =
                Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });

            var del = lambda.Compile(); //compilar expresión lambda y obtener el delegado.

            del(source, target); //ejectuar la expresión lambda utilizando el delegado obtenido.
        }

        /// <summary>
        /// Metodo para copiar los datos de propiedades con igual nombre
        /// desde una instancia de una clase hacia otra.
        /// </summary>
        /// <typeparam name="SourceType">Tipo de datos del objeto fuente (proveedor de datos)</typeparam>
        /// <typeparam name="TargetType">Tipo de datos del objeto destino (receptor de datos)</typeparam>
        /// <param name="source">Instancia del objeto fuente de los datos.</param>
        /// <param name="targetObj">Instancia opcional del objeto recibidor de los datos</param>
        /// <returns></returns>
        public static TargetType Transfer<SourceType, TargetType>(SourceType source, object targetObj = null, string toSkip = null)
            where TargetType : class, new()
        {
            TargetType target = new TargetType();
            if (targetObj != null)
            {
                target = (TargetType)targetObj;
            }
            if (toSkip != null)
            {
                List<string> skipList = toSkip.Split(',').ToList();
                Transfer(source, target, skipList);
            }
            else
            {
                Transfer(source, target);
            }
            return target;
        }

        private static ResourceManager manager = new ResourceManager(typeof(Language));
        public static string t(string var)
        {
            var translatedMsg = manager.GetString(var);
            return string.IsNullOrEmpty(translatedMsg) ? var : translatedMsg;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool CheckUser(int userID, string username, string password)
        {
            var valid = false;

            using (SIGECContext db = new SIGECContext())
            {
                var user = db.Users.Find(userID);
                if (user == null)
                {
                    return false;
                }
                var passwordHelper = new PasswordHelper();
                valid = passwordHelper.ComparePassword(password, user.password, user.salt);
            }

            return valid;
        }

        public static string CreateRandomPassword(int length)
        {
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890#_";
            string randomPassword = "";
            Random randomizer = new Random();

            while (0 < length--)
            {
                randomPassword += validChars[randomizer.Next(validChars.Length)];
            }

            return randomPassword;
        }

        /// <summary>
        /// Metodo utilizado para almacenar los nombres de los controladores
        /// y sus acciones asociadas en la base de datos.
        /// </summary>
        public static void InsertMenusAndActions()
        {
            MenuActionProvider cp = new MenuActionProvider();
            bool insertNew = false;
            using (SIGECContext db = new SIGECContext())
            {
                foreach (string controllerName in cp.GetControllerNames())
                {
                    Menu m = db.Menus.FirstOrDefault(t => t.name == controllerName);

                    if (m == null)
                    {
                        insertNew = true;
                        m = new Menu();
                        m.name = controllerName;
                    }

                    List<SIGEC.Models.Action> actions = new List<SIGEC.Models.Action>();


                    string[] lstActions = cp.GetActionNames(controllerName).Distinct().ToArray();
                    foreach (string action in lstActions)
                    {
                        SIGEC.Models.Action a = new SIGEC.Models.Action();
                        a.name = action;
                        actions.Add(a);
                    }
                    if (insertNew)
                    {
                        m.Actions = actions;
                    }
                    else
                    {
                        foreach (SIGEC.Models.Action a in actions)
                        {
                            if (!m.Actions.Contains(a, new Compare<SIGEC.Models.Action>((r, l) => r.name == l.name)))
                            {
                                m.Actions.Add(a);
                            }
                        }
                    }
                    if (db.Menus.FirstOrDefault(con => con.name == controllerName) == null)
                    {
                        db.Menus.Add(m);
                    }
                    try
                    {
                        if (insertNew)
                        {
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Entry(m).State = System.Data.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.InnerException.ToString());
                    }
                    insertNew = false;
                }
            }
        }

        private static SimpleRoleProvider roleProvider = (SimpleRoleProvider)Roles.Provider;

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un paciente.
        /// </summary>
        public static bool isPatient
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Patient");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un empleado.
        /// </summary>
        public bool isEmployee
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Employee");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un doctor.
        /// </summary>
        public static bool isDoctor
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Doctor");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un administrador.
        /// </summary>
        public static bool isAdmin
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Admin");
            }
        }
    }

    /// <summary>
    /// Enumeración para los géneros.
    /// </summary>
    public enum Genders
    {
        Male = 'M',
        Female = 'F'
    }

    /// <summary>
    /// Enumeración para el Estado Civil.
    /// </summary>
    public enum MaritalStatus
    {
        Single = 'S',
        Married = 'C'
    }

    /// <summary>
    /// Enumeración para los tipos de teléfono.
    /// </summary>
    public enum PhoneTypes
    {
        Home = 1,
        Movil = 2,
        Fax = 3,
        Work = 4,
        Other = 5
    }

    /// <summary>
    /// Enumeración para los estatus básicos.
    /// </summary>
    public enum StatusTypes
    {
        Inactive = 0,
        Active = 1
    }

    /// <summary>
    /// Enumeración para los tipos de roles de la aplicación.
    /// </summary>
    public enum RoleTypes
    {
        Admin = 1,
        Patient = 2,
        Employee = 3,
        Doctor = 4

    }

    /// <summary>
    /// Enumeración para los estatus de las citas al momento de
    /// eliminarse y ser registradas en la tabla histórica.
    /// </summary>
    public enum AppointmentStatus
    {
        Cancelled,
        Assisted
    }

    /// <summary>
    /// Enumración para establecer la forma de pago al momento
    /// del pago de una factura en el consultorio médico.
    /// </summary>
    public enum PaymentForms
    {
        Cash,
        DebitCard,
        Check,
        Exoneration,
        Other
    }

    /// <summary>
    /// Clase utilitaria para comparar 2 objetos de una
    /// misma clase, para en base a una función de comparación
    /// lambda, establecer si estas son iguales o no en base a
    /// dicho criterio de comparación.
    /// </summary>
    /// <typeparam name="T">Tipo de objeto a ser comaprado</typeparam>
    public class Compare<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Función de comparación para establecer la comparación.
        /// </summary>
        Func<T, T, bool> compareFunction;
        /// <summary>
        /// Función de hash para comparación de los objetos en base
        /// a una clave hash.
        /// </summary>
        Func<T, int> hashFunction;

        /// <summary>
        /// Constructor para la clase de comparación que establece la
        /// función de comparación en base a la función de comparación
        /// enviada como argumento.
        /// </summary>
        /// <param name="compareFunction">
        /// Función de comparación a ser usada para realizar la comparación.
        /// </param>
        public Compare(Func<T, T, bool> compareFunction)
        {
            this.compareFunction = compareFunction;
        }

        /// <summary>
        /// Constructor para la clase de comparación que establece la
        /// función de comparación y la función de hash en base a los
        /// argumentos de comparación y hash recibidos.
        /// </summary>
        /// <param name="compareFunction">
        /// Función de comparación a ser usada para realizar la comparación.
        /// </param>
        /// <param name="hashFunction">
        /// Función de hash a ser usada para la comparación hash.
        /// </param>
        public Compare(Func<T, T, bool> compareFunction, Func<T, int> hashFunction)
        {
            this.compareFunction = compareFunction;
            this.hashFunction = hashFunction;
        }

        /// <summary>
        /// Método utilizado para realizar la comparación de igualdad entre 2 objetos
        /// enviados por argumento, utilizando la función de comparación establecida
        /// para realizar la comparación.
        /// </summary>
        /// <param name="x">Objeto x del tipo T a ser comparado</param>
        /// <param name="y">Objeto y del tipo T a ser comparado</param>
        /// <returns>
        /// valor Booleano indicando si los objetos son iguales en base al criterio
        /// de comparación utilizado.
        /// </returns>
        public bool Equals(T x, T y)
        {
            return compareFunction(x, y);
        }

        /// <summary>
        /// Función que calcula la clave hash de un objeto recibido por argumento
        /// en base a la función de hash definida.
        /// </summary>
        /// <param name="obj">Objeto al que se le calculará la clave hash.</param>
        /// <returns>
        /// un valor del tipo entero con la clave hash de ese objeto en base a la
        /// función hash utilizada para el cálculo de la clave.
        /// </returns>
        public int GetHashCode(T obj)
        {
            return hashFunction(obj);
        }
    }

    /// <summary>
    /// Clase utilitaria para definir políticas de contraseña que deben ser respetadas
    /// por los usuarios de la aplicación a la hora de establecer contraseñas de acceso
    /// a la aplicación.
    /// </summary>
    public class PasswordHelper
    {
        //Below is used to generate a password policy that you may use to check that passwords adhere to this policy
        private const int numberUpper = 0;
        private const int numberNonAlphaNumeric = 0;
        private const int numberNumeric = 0;
        private const int minPwdLength = 6;
        private const int maxPwdLength = 32;
        private PWDTK.PasswordPolicy PwdPolicy = new PWDTK.PasswordPolicy(numberUpper, numberNonAlphaNumeric, numberNumeric, minPwdLength, maxPwdLength);
        //Number of hash iterations
        private const int iterations = 1500;
        //Salt length
        private const int saltSize = PWDTK.CDefaultSaltLength;
        public Byte[] Salt { get; set; }
        public Byte[] Hash { get; set; }

        public bool HashPassword(string password)
        {
            //A check to make sure the supplied password meets our defined password
            //policy before using CPU resources to calculate hash, this step is optional
            if (PasswordMeetsPolicy(password, PwdPolicy))
            {
                //Get a random salt
                Salt = PWDTK.GetRandomSalt(saltSize);
                //Generate the hash value
                Hash = PWDTK.PasswordToHash(Salt, password, iterations);

                return true;
            }
            return false;
        }

        public bool HashGeneratedPassword(string password)
        {
            try
            {
                Salt = PWDTK.GetRandomSalt(saltSize);
                Hash = PWDTK.PasswordToHash(Salt, password, iterations);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ComparePassword(string password, string hash, string salt)
        {
            Hash = PWDTK.HashHexStringToBytes(hash);
            Salt = PWDTK.HashHexStringToBytes(salt);

            return PWDTK.ComparePasswordToHash(Salt, password, Hash, iterations);
        }

        private bool PasswordMeetsPolicy(String Password, PWDTK.PasswordPolicy PassPolicy)
        {

            if (PWDTK.TryPasswordPolicyCompliance(Password, PassPolicy))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Clase utilitaria en la cual se definen métodos extensión para ser utilizados
    /// en la construcción de objetos y estructuras html a partir de listas de objetos.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Método extensión utilizado para crear un arreglo de objetos del tipo
        /// SelectListItem utilizado para construir instancias de objetos html 
        /// utilizando la sintaxis de razor que requieren un arreglo de objetos
        /// de este tipo tales como DropDownList, ListBox, etc.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto del cual generar el arreglo.</typeparam>
        /// <param name="items">Colección de objetos utilizados para generar el arreglo.</param>
        /// <param name="nameSelector">
        /// Función lambda utilizada para establecer el texto a mostrar por el objeto html
        /// cuando dicho item sea seleccionado.
        /// </param>
        /// <param name="valueSelector">
        /// Función lambda utilizada para establecer el valor interno que representará al
        /// item seleccionado del objeto html generado.
        /// </param>
        /// <param name="selected">
        /// Función lambda utilizada para establecer cual(es) objeto(s) está(n) seleccionado(s)
        /// por defecto en el objeto html generado utilizando el arreglo generado.
        /// </param>
        /// <returns>
        /// Un arreglo del tipo SelecListItem para ser usado para la generación de objetos html.
        /// </returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>
            (
             this IEnumerable<T> items,
             Func<T, string> nameSelector,
             Func<T, string> valueSelector,
             Func<T, bool> selected
            )
        {
            return items.OrderBy(item => nameSelector(item))
                   .Select(item =>
                           new SelectListItem
                           {
                               Selected = selected(item),
                               Text = nameSelector(item),
                               Value = valueSelector(item)
                           });
        }

        /// <summary>
        /// Método extensión utilizado para crear un arreglo de objetos del tipo
        /// SelectListItem utilizado para construir instancias de objetos html 
        /// utilizando la sintaxis de razor que requieren un arreglo de objetos
        /// de este tipo tales como DropDownList, ListBox, etc.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto del cual generar el arreglo.</typeparam>
        /// <param name="items">Colección de objetos utilizados para generar el arreglo.</param>
        /// <param name="nameSelector">
        /// Función lambda utilizada para establecer el texto a mostrar por el objeto html
        /// cuando dicho item sea seleccionado.
        /// </param>
        /// <param name="valueSelector">
        /// Función lambda utilizada para establecer el valor interno que representará al
        /// item seleccionado del objeto html generado.
        /// </param>
        /// <returns>
        /// Un arreglo del tipo SelecListItem para ser usado para la generación de objetos html.
        /// </returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>
            (
             this IEnumerable<T> items,
             Func<T, string> nameSelector,
             Func<T, string> valueSelector
            )
        {
            return items.OrderBy(item => nameSelector(item))
                   .Select(item =>
                           new SelectListItem
                           {
                               Text = nameSelector(item),
                               Value = valueSelector(item)
                           });
        }

        public static IEnumerable<SelectListItem> EnumToList<TEnum, TAttributeType>(this TEnum enumObj, bool useIntegerValue) where TEnum : struct
        {
            Type type = typeof(TEnum);

            var fields = type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);

            var values = from field in fields
                         select new SelectListItem
                         {
                             Value = (useIntegerValue) ? field.GetRawConstantValue().ToString() : field.Name,
                             Text = field.GetCustomAttributes(typeof(TAttributeType), true).
                                        FirstOrDefault().ToString() ?? field.Name,
                             Selected =
                                 (useIntegerValue)
                                     ? (Convert.ToInt32(field.GetRawConstantValue()) &
                                        Convert.ToInt32(enumObj)) ==
                                       Convert.ToInt32(field.GetRawConstantValue())
                                     : enumObj.ToString().Contains(field.Name)
                         };


            return values;
        }

        public static IEnumerable<SelectListItem> EnumToList<TEnum>(this TEnum enumObj, bool useIntegerValue) where TEnum : struct
        {
            Type type = typeof(TEnum);

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select
                             new SelectListItem
                             {
                                 Value =
                                     (useIntegerValue)
                                         ? Enum.Format(type, Enum.Parse(type, e.ToString()), "d")
                                         : e.ToString(),
                                 Text = GlobalHelpers.t(e.ToString()),
                                 Selected =
                                     (useIntegerValue)
                                         ? (Convert.ToInt32(e) & Convert.ToInt32(enumObj)) == Convert.ToInt32(e)
                                         : enumObj.ToString().Contains(e.ToString())
                             };


            return values;
        }
    }
}