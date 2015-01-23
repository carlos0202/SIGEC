$(document).on('change', '.btn-file :file', function() {
  var input = $(this),
      numFiles = input.get(0).files ? input.get(0).files.length : 1,
      label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
  input.trigger('fileselect', [numFiles, label]);
});
//Configuración del Jquery validate para soportar el formato de fecha local dd/MM/yyyy
$.validator.addMethod(
    "localDate",
    function (value, element) {
        return this.optional(element) || $.datepicker.parseDate('yy/mm/dd', value) !== "Invalid date";
    }
);
function FnGenerarTooltips() {
    $('a.linkDetalle').attr('title', 'Ver Detalle').attr('data-toggle', 'tooltip');
    $('a.linkEjecutar').attr('title', 'Ejecutar Procedimiento').attr('data-toggle', 'tooltip');
}
// Limpia el file input
$(document).on('click', '.btnClearInput .clearFileInput', function () {
    var control = $(this).parents('.input-group').find(':file');
    var input = $(this).parents('.input-group').find(':text');
    control.replaceWith(control.val('').clone(true));
    input.val('');
});
function FnAgregarReglasDeValidacion() {
    //Validaciones para formularios dinamicos
    $("#procedureForm .form-control").each(function () {
        var input = $(this);

        if (input.hasClass('requiredValidation')) {

            input.rules('add', {
                required: true,
                messages: {
                    required: "Campo requerido"
                }
            });
        }

        if (input.hasClass('numberValidation')) {

            input.rules('add', {
                number: true,
                messages: {
                    number: "El valor introducido debe ser numérico"
                }
            });
        }

        if (input.hasClass('dateValidation')) {

            input.rules('add', {
                localDate: true,
                messages: {
                    localDate: "Fecha inválida"
                }
            });
        }

        if (input.attr('type') == 'file') {
            input.rules('add', {
                maxfilesize: 102354,
                messages: {
                    maxfilesize: 'Archivo no debe ser mayor de {0} bytes.'
                }
            });
        }

    });
}
$(document).ready(function () {
    FnGenerarTooltips();
});
(function ($) {

    //Controlador: Procedimientos
    //Accion: EjecutarProcedimiento
    $.fn.ExecuteProcedureFn = function () {

        FnAgregarReglasDeValidacion();
        $('.btn-file :file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' archivos seleccionados' : label;

            if (input.length) {
                input.val(log);
            }

        });

        $('.datepicker').datepicker({
            format: "yyyy/mm/dd",
            autoclose: true
        });
        $(document).on('submit', 'form.executeProcedureForm', function (event) {
            var form = $(this);

            $("#procedureForm .form-control").each(function () {
                if (!$(this).valid()) {
                    valid = false;
                    event.preventDefault();
                }
            });
            if (!$("#procedureForm .form-control").valid()) {
                return false;
            }
        });

    };
})(jQuery);