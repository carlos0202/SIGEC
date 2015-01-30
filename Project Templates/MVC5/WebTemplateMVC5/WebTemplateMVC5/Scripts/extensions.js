//Configuración del Jquery validate para soportar los estilos de bootstrap
$.validator.setDefaults({
    highlight: function (element) { //añadir clases de error
        $(element).closest('.form-group').addClass('has-error');
        $(element).closest('.field-validation-error').addClass('help-block');
    },
    unhighlight: function (element) { //remover clases de error
        $(element).closest('.form-group').removeClass('has-error');
    },
    errorElement: 'span', //elemento contenedor de los mensajes de validacion
    errorClass: 'help-block', //clase css para dar estilos de rror
    errorPlacement: function (error, element) { //logica para el posicionamiento de los errores
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    },
    ignore: '.ignore', //ignorar validacion en elementos con dicha clase
    onkeyup: false, //no activar validacion al presionado de cada tecla
    //onfocusout: false,
    onsubmit: true //activar validacion en cada submit de formulario
});

//global variables (use carefully)
var tableLinksColor = ' iconLink ';

//$.widget.bridge('uibutton', $.ui.button);
//Resolver colisión entre el nombre el jquery ui tooltip
//y el Bootstrap tooltip
//$.widget.bridge('uitooltip', $.ui.tooltip);

//focus en el primer input de cada dialogo modal.
$(document).on("shown.bs.modal", '.modal', function () {
    $(this).find(".form-control:first").focus();
});

$(document).on('hidden.bs.modal', function () {
    FnClearValues();
})

/// Evento para mostrar resultados de peticiones ajax
/// luego de que se haya terminado de cargar la página.
$(document).ajaxComplete(function (event, xhr, options) {
    var data = $.parseJSON(xhr.responseText);
    FnSetAjaxResultMsg(data.Message, data.Success);
});

function FnClearValues() {
    /// <summary>
    /// Función para limpiar los datos de un formulario modal.
    /// </summary>
    $('.modal-body :input.form-control').each(function () {
        $(this).val('');
        if ($(this).hasClass('chosen') || $(this).hasClass('chosen-multiple')) {
            $(this).chosen().trigger('chosen:updated');
        }
    });
}

/*Sweetalert Global Function Shorthands*/
function SwalInfo(text, title, buttonClass, buttonText) {
    /// <summary>
    /// Función de extensión para mostrar mensaje de información
    /// personalizado para los usuarios.
    /// </summary>
    /// <param name="text">Mensaje a mostrar.</param>
    /// <param name="title">Título utilizado para el mensaje.</param>
    /// <param name="buttonClass">Clase de estilo bootstrap para el botón.</param>
    /// <param name="buttonText">Texto del botón.</param>

    /*Default values*/
    title = (typeof title !== 'undefined') ? title : 'Información!';
    buttonClass = (typeof buttonClass !== 'undefined') ? buttonClass : 'btn-info';
    buttonText = (typeof buttonText !== 'undefined') ? buttonText : 'OK';

    swal({
        title: title,
        text: text,
        type: "info",
        confirmButtonClass: buttonClass,
        confirmButtonText: buttonText
    });
}

function SwalError(text, title, buttonClass, buttonText) {
    /// <summary>
    /// Función de extensión para mostrar mensaje de error
    /// personalizado para los usuarios.
    /// </summary>
    /// <param name="text">Mensaje a mostrar.</param>
    /// <param name="title">Título utilizado para el mensaje.</param>
    /// <param name="buttonClass">Clase de estilo bootstrap para el botón.</param>
    /// <param name="buttonText">Texto del botón.</param>

    /*Default values*/
    title = (typeof title !== 'undefined') ? title : 'Error!';
    buttonClass = (typeof buttonClass !== 'undefined') ? buttonClass : 'btn-danger';
    buttonText = (typeof buttonText !== 'undefined') ? buttonText : 'OK';

    swal({
        title: title,
        text: text,
        type: "error",
        confirmButtonClass: buttonClass,
        confirmButtonText: buttonText
    });
}

function SwalSuccess(text, title, buttonClass, buttonText) {
    /// <summary>
    /// Función de extensión para mostrar mensaje de resultado positivo
    /// personalizado para los usuarios.
    /// </summary>
    /// <param name="text">Mensaje a mostrar.</param>
    /// <param name="title">Título utilizado para el mensaje.</param>
    /// <param name="buttonClass">Clase de estilo bootstrap para el botón.</param>
    /// <param name="buttonText">Texto del botón.</param>

    /*Default values*/
    title = (typeof title !== 'undefined') ? title : 'Correcto!';
    buttonClass = (typeof buttonClass !== 'undefined') ? buttonClass : 'btn-success';
    buttonText = (typeof buttonText !== 'undefined') ? buttonText : 'OK';

    swal({
        title: title,
        text: text,
        type: "success",
        closeOnConfirm: false,
        confirmButtonClass: buttonClass,
        confirmButtonText: buttonText
    });
}

function SwalWarning(text, title, buttonClass, buttonText) {
    /// <summary>
    /// Función de extensión para mostrar mensaje de alerta
    /// personalizado para los usuarios.
    /// </summary>
    /// <param name="text">Mensaje a mostrar.</param>
    /// <param name="title">Título utilizado para el mensaje.</param>
    /// <param name="buttonClass">Clase de estilo bootstrap para el botón.</param>
    /// <param name="buttonText">Texto del botón.</param>

    /*Default values*/
    title = (typeof title !== 'undefined') ? title : 'Cuidado!';
    buttonClass = (typeof buttonClass !== 'undefined') ? buttonClass : 'btn-warning';
    buttonText = (typeof buttonText !== 'undefined') ? buttonText : 'OK';

    swal({
        title: title,
        text: text,
        type: "warning",
        confirmButtonClass: buttonClass,
        confirmButtonText: buttonText
    });
}


function SwalConfirm(text, CallBack, title, buttonClass, buttonText) {
    /// <summary>
    /// Función de extensión para mostrar un diálogo de confirmación
    /// personalizado para los usuarios.
    /// </summary>
    /// <param name="text">Mensaje a mostrar.</param>
    /// <param name="CallBack">Función de callback para procesar el resultado.</param>
    /// <param name="title">Título utilizado para el mensaje.</param>
    /// <param name="buttonClass">Clase de estilo bootstrap para el botón.</param>
    /// <param name="buttonText">Texto del botón.</param>

    /*Default values*/
    title = (typeof title !== 'undefined') ? title : '';
    buttonClass = (typeof buttonClass !== 'undefined') ? buttonClass : 'btn-warning';
    buttonText = (typeof buttonText !== 'undefined') ? buttonText : 'OK';
    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: buttonClass,
        confirmButtonText: buttonText,
        cancelButtonText: 'Cancelar'
    }, CallBack);
}

/// Lógica de animación (Cargando) para peticiones ajax. 
/// Además de la lógica para mostrar los mensajes de resultado
/// de peticiones enviadas al servidor que recargan la página o
/// redireccionan hacia otra url.
$(window).load(function () {
    $(".pageloadSpinner").fadeOut("slow");
    if ($('#ResultMessage').val()) {
        if ($('#MessageCode').val() == '1') {
            SwalSuccess($('#ResultMessage').val(), '');
        }
        else {
            SwalError($('#ResultMessage').val());
        }
    }
});

$(document).ajaxSend(function (event, xhr, options) {
    $(".ajaxSpinner").show();
}).ajaxComplete(function () {
    $(".ajaxSpinner").hide();
}).ajaxStop(function () {
    $(".ajaxSpinner").hide();
});

/// Recargar definiciones de los plugins utilizados en
/// los diferentes componentes (tablas, inputs, etc).
$(document).on('loaded-content', function () {
    FnReloadDefinitions();

}).trigger('loaded-content');

function FnReloadDefinitions() {
    $('.dynamicTable').DataTable({
        pagingType: "bootstrapPager",
        "oLanguage": {
            "sLengthMenu": 'Mostrar _MENU_ registros por páginas',
            "sZeroRecords": 'No hay registros entontrados.',
            "sInfo": 'Mostrando _START_ a _END_ de _TOTAL_ registros',
            "sInfoEmpty": 'Mostrando 0 a 0 de 0 records',
            "sInfoFiltered": '(filtrado desde _MAX_ total registros)',
            "sSearch": "",
            "oPaginate": {
                "sFirst": "",
                "sPrevious": "",
                "sNext": "",
                "sLast": ""
            }
        },
        "sPaginationType": "full_numbers"
    });

    $(document.body).tooltip({
        selector: '[data-toggle="tooltip"]'
    });

    $(document.body).popover({
        selector: '[data-toggle="popover"]'
    });

    //Inicialización por defecto para los elementos chosen single.
    $(".chosen").chosen({ width: '100%', allow_single_deselect: true });
    $(".chosen").attr('data-placeholder', 'Seleccione una opción');
    $(".chosen").chosen().change();
    $(".chosen").trigger('chosen:updated');
    $(".chosen-multiple").chosen({ width: '100%' });
    $(".chosen-multiple").attr('data-placeholder', 'Seleccione las opciones');
    $(".chosen-multiple").chosen().change();
    $(".chosen-multiple").trigger('chosen:updated');

    $('.modal-body :input[type="hidden"]').each(function () {
        $(this).addClass('ignore');
    });

}

//Prevenir evento de click en botones con la clase especificada.
$(document).on('click', '.preventButton', function (evt) {
    evt.preventDefault();
});

$(document).ready(function () {
    FnReloadDefinitions();

    //Valor de input con mayúsculas.
    $(document).on('keypress keyup', '.upperCase', function () {
        $(this).val($(this).val().toUpperCase());
    });
});

$(function () {
    $('.table').each(function () {
        var datatable = $(this);
        // estilo y label inline para el input de busqueda
        var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
        search_input.attr('placeholder', 'Buscar');
        search_input.addClass('form-control input-sm');

        // posicionamiento de la informacion de los records filtrados
        var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_info]');
        length_sel.css('margin-top', '2px');
    });
});

$('.table').DataTable({
    pagingType: "bootstrapPager",
    "oLanguage": {
        "sLengthMenu": 'Mostrar _MENU_ registros por páginas',
        "sZeroRecords": 'No hay registros entontrados.',
        "sInfo": 'Mostrando _START_ a _END_ de _TOTAL_ registros',
        "sInfoEmpty": 'Mostrando 0 a 0 de 0 records',
        "sInfoFiltered": '(filtrado desde _MAX_ total registros)',
        "sSearch": "",
        "oPaginate": {
            "sFirst": "",
            "sPrevious": "",
            "sNext": "",
            "sLast": ""
        }
    },
    "sPaginationType": "full_numbers"
});

function fnValidateDynamicContent() {
    /// <summary>
    /// Función para agregar soporte de validación unobstrusiva en el cliente
    /// usando jQuery validator a contenido cargado dinámicamente usando ajax.
    /// </summary>
    $('.standard-form').removeData("validator");
    $('.standard-form').removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($('.standard-form'));
    // This line is important and added for client side validation to trigger, 
    // without this it didn't fire client side errors.
    $('.standard-form').validate();
    AttachEnterKeystrokeValidation();
}

function AttachEnterKeystrokeValidation() {
    /// <summary>
    /// Función para deshabilitar la tecla de ENTER en formularios modales.
    /// </summary>
    $('.dynamicForm').bind("keyup keypress", function (e) {
        var code = e.which || e.keyCode;
        if (code == 13) {
            e.preventDefault();
            return false;
        }
    });
}

//Funcion para actualizar los selects luego de cambios en sus opciones (bd, regla de negocio)
function updateDropdown(data, url, dropdownElement, chosenElement) {
    var html = '<option value="" ></option>';
    var select = "";
    dropdownElement.html(html);
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data.Object == undefined || data.Object == "") {
            }
            else {
                $.each(data.Object, function () {
                    dropdownElement.append(
                        $('<option />').text(this.Text).val(this.Value).prop('selected', this.Selected)
                    );
                });
            }
            if (chosenElement != null) {
                chosenElement.trigger('chosen:updated');
            }
        }
    });
}

//Funcion de ayuda para imprimir propiedades de un objeto
function printObject(o) {
    var out = '';
    for (var p in o) {
        out += p + ': ' + o[p] + '\n';
    }
    alert(out);
}


function CRefreshTable(tableClass, urlData, linksFunc, data) {
    /// <summary>
    /// FUnción para actualizar los datos de una tabla que utilice
    /// el plugin Jquery datatable con el resultado de una petición
    /// ajax.
    /// </summary>
    /// <param name="tableClass">
    /// Clase css de la tabla (utilizando el .) ej. (.tableClass)
    /// </param>
    /// <param name="urlData">
    /// Url que devuelve el resultado json utilizado para actualizar
    /// los datos de la tabla. (Debe usar propiedad nombrada denominada
    /// aaData y retornar el id de los registros en la ultima posición
    /// de cada uno.
    /// </param>
    /// <param name="linksFunc">
    /// función utilizada para generar los actionLinks de acciones
    /// sobre los registros de la tabla. (dicha función debe recibir
    /// como parámetro el ID del registro.
    /// </param>
    data = typeof data !== 'undefined' ? data : null;
    $.post(
       urlData,
       data,
       function (result) {
           var tbl = $(tableClass).DataTable();
           var index = result.aaData.length > 0 ? result.aaData[0].length - 1 : 0;
           tbl.clear();
           for (var i = 0; i < result.aaData.length; i++) {
               var links = linksFunc(result.aaData[i][index]);
               if (links != '') {
                   result.aaData[i][index] = links;
               }
           }
           tbl.rows.add(result.aaData).draw();
       },
       "json"
   );
}

$('[readonly]').each(function (e) {
    $(this).attr('onfocus', 'this.blur()');
});


function FnCleanInputs(elements) {
    /// <summary>
    /// funcion para limpiar el valor de los inputs
    /// utilizados en los modals. 
    /// </summary>
    /// <param name="elements">
    /// variable que referencia al contenedor de los
    /// inputs a limpiar.
    /// </param>
    $(elements).each(function () {
        var me = $(this);
        me.val('');
        if (me.hasClass('chosen') || me.hasClass('chosen-multiple')) {
            me.chosen().trigger('chosen:updated');
        }
    });
}

function FnSetAjaxResultMsg(message, result) {
    /// <summary>
    /// Función para mostrar mensajes al usuario de
    /// resultados de peticiones mediante ajax.
    /// </summary>
    /// <param name="message">Mensaje para mostrar.</param>
    /// <param name="result">Resultado de la petición.</param>
    if (message != '' && message != undefined) {
        if (result) {
            SwalSuccess(message);
        }
        else {
            SwalError(message);
        }
    }
}

function FnCreateInputElement(name, type, value, getOuterHtml) {
    /// <summary>
    /// Función para crear un input element dinámicamente desde
    /// javascript con los parámetros suplidos.
    /// </summary>
    /// <param name="name">Atributo nombre del input.</param>
    /// <param name="type">Atributo tipo del input.</param>
    /// <param name="value">Atributo del valor interno del input.</param>
    /// <param name="getOuterHtml">
    /// Especifica el formato de retorno del input creado desde
    /// la función. true (Retorna la representación en cadena del input),
    /// false(Retorna la representación como objeto del input).
    /// </param>
    /// <returns type="">El input Creado en formato string u objeto.</returns>
    //default values
    getOuterHtml = typeof getOuterHtml !== 'undefined' ? getOuterHtml : true;

    var input = document.createElement('input');
    input.name = name;
    input.type = type;
    input.value = value;

    return (getOuterHtml) ? input.outerHTML : input;
}

//String format usando placeholders.
String.prototype.format = function (placeholders) {
    var s = this;
    for (var propertyName in placeholders) {
        var re = new RegExp('{' + propertyName + '}', 'gm');
        s = s.replace(re, placeholders[propertyName]);
    }
    return s;
};

//desde http://stackoverflow.com/questions/2017456/with-jquery-how-do-i-capitalize-the-first-letter-of-a-text-field-while-the-user
(function ($) {
    $.fn.extend({

        // With every keystroke capitalize first letter of ALL words in the text
        upperFirstAll: function () {
            $(this).keyup(function (event) {
                var box = event.target;
                var txt = $(this).val();
                var start = box.selectionStart;
                var end = box.selectionEnd;

                $(this).val(txt.toLowerCase().replace(/^(.)|(\s|\-)(.)/g,
				function (c) {
				    return c.toUpperCase();
				}));
                box.setSelectionRange(start, end);
            });
            return this;
        },

        // With every keystroke capitalize first letter of the FIRST word in the text
        upperFirst: function () {
            $(this).keyup(function (event) {
                var box = event.target;
                var txt = $(this).val();
                var start = box.selectionStart;
                var end = box.selectionEnd;

                $(this).val(txt.toLowerCase().replace(/^(.)/g,
				function (c) {
				    return c.toUpperCase();
				}));
                box.setSelectionRange(start, end);
            });
            return this;
        },

        // Converts with every keystroke the hole text to lowercase
        lowerCase: function () {
            $(this).keyup(function (event) {
                var box = event.target;
                var txt = $(this).val();
                var start = box.selectionStart;
                var end = box.selectionEnd;

                $(this).val(txt.toLowerCase());
                box.setSelectionRange(start, end);
            });
            return this;
        },

        // Converts with every keystroke the hole text to uppercase
        upperCase: function () {
            $(this).keyup(function (event) {
                var box = event.target;
                var txt = $(this).val();
                var start = box.selectionStart;
                var end = box.selectionEnd;

                $(this).val(txt.toUpperCase());
                box.setSelectionRange(start, end);
            });
            return this;
        }

    });

    $.fn.Examples = function () {
        $('.ajaxLink').on('click', function (e) {
            e.preventDefault();
            var me = $(this);
            $.get(me.attr('href'), null, function () { });
            return false;
            //$.ajax({
            //    url: me.attr('href'),
            //    data: null,
            //    dataType: 'json',
            //    type: 'POST',
            //    global: false,
            //    success: function (result) {
            //    }
            //});
        });
    }
}(jQuery));