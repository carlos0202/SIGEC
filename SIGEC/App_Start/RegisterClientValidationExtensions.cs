using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SIGEC.App_Start.RegisterClientValidationExtensions), "Start")]
///configuracion para el plugin DataAnnotationExtensions 
namespace SIGEC.App_Start {
    //Registro de la configuracion de la validacion en el cliente(Jquery)
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}