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
$.widget.bridge('uitooltip', $.ui.tooltip);

$.fn.bootstrapSwitch.defaults.size = 'large';
$.fn.bootstrapSwitch.defaults.onColor = 'success';

//focus on first input of each modal dialog.
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
        SwalSuccess($('#ResultMessage').val(), '');
    }
    if ($('#mensajeRespuesta').val()) {
        if ($('#codigoRespuesta').val() == 1) {
            SwalSuccess($('#mensajeRespuesta').val());
        }
        else {
            SwalError($('#mensajeRespuesta').val());
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
    $('.switchInput').bootstrapSwitch({
        onColor: "primary",
        offColor: "danger",
        onText: "SI",
        offText: "NO"
    });
    //Inicializando Bootstrap Switch
    $("input[type=checkbox].switchInput ").bootstrapSwitch({
        onColor: "primary",
        offColor: "danger",
        onText: "SI",
        offText: "NO"
    });
    //if ($('#messageModal')) {
    //    $('#messageModal').modal('show');
    //}
    
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

    //bootstrap checkbox initializations
    //$('input[type="checkbox"]').bootstrapSwitch();

    //Datepicker initializations
    $('.defaultPicker').datepicker({
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true
    });

    $('.modal-body :input[type="hidden"]').each(function () {
        $(this).addClass('ignore');
    });

    $('.futurePicker').datepicker({
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true,
        minDate: 1
    });
}

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

//Funcion para actualizar los datatables luego de cambios realizados en la base de datos
//function RefreshTable(tableId, urlData, editUrl, deleteUrl, params) {
//    // Retrieve the new data
//    $.ajax({
//        url: urlData,
//        type: 'POST',
//        dataType: 'json',
//        contentType: 'application/json',
//        global: false,
//        success: function (json) {
//            table = $(tableId).dataTable();
//            oSettings = table.fnSettings();

//            table.fnClearTable(this);
//            for (var i = 0; i < json.aaData.length; i++) {
//                var links = '';
//                links +=
//					(!editUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="' + tableLinksColor + 'editAction" title="Editar" data-url="' + editUrl + '/' + json.aaData[i][params] + '" ><i class="glyphicon glyphicon-pencil "></i></a>';
//                links +=
//					(!deleteUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="' + tableLinksColor + 'deleteAction" title="Eliminar" data-url="' + deleteUrl + '/' + json.aaData[i][params] + '" ><i class="glyphicon glyphicon-remove-sign "></a>';
//                json.aaData[i][params] = links;
//                table.oApi._fnAddData(oSettings, json.aaData[i]);
//            }

//            oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
//            table.fnDraw();
//        }
//    });

//}

function NRefreshTable(tableClass, urlData, editUrl, deleteUrl) {
    $.post(
        urlData,
        null,
        function (result) {
            var tbl = $(tableClass).DataTable();
            var index = result.aaData.length > 0 ? result.aaData[0].length - 1 : 0;
            tbl.clear();
            for (var i = 0; i < result.aaData.length; i++) {
                var links = '';
                links +=
					(!editUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="' + tableLinksColor + 'editAction" title="Editar" data-url="' + editUrl + '/' + result.aaData[i][index] + '" ><i class="glyphicon glyphicon-pencil "></i></a>';
                links +=
					(!deleteUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="' + tableLinksColor + 'deleteAction" title="Eliminar" data-url="' + deleteUrl + '/' + result.aaData[i][index] + '" ><i class="glyphicon glyphicon-remove-sign "></a>';
                result.aaData[i][index] = links;
            }
            tbl.rows.add(result.aaData).draw();
        },
        "json"
    );
}


function CRefreshTable(tableClass, urlData, linksFunc) {
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
    $.post(
       urlData,
       null,
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

//function FnSetAjaxResultMsg(message, result) {
//    $('.ajaxMessage').html(message);
//    if (result) {
//        $('.ajaxModalHeader').removeClass('bg-danger')
//			.addClass('bg-info');
//        $('.ajaxModalButton').removeClass('btn-danger')
//			.addClass('btn-info');
//    }
//    else {
//        $('.ajaxModalHeader').removeClass('bg-info')
//			.addClass('bg-danger');
//        $('.ajaxModalButton').removeClass('btn-info')
//			.addClass('btn-danger');
//    }
//    if (message != '' && message != undefined) {
//        $('#ajaxResultModal').modal('show');
//    }
//}

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

var validParameters = [];
var availableParameters = [];
var paramTypes = [];
function GetProcedureParams() {
    /// <summary>
    /// Función utilizada para obtener los parámetros de entrada
    /// de un procedimiento.
    /// </summary>
    /// <param name="validParameters">
    /// Array en el cual almacenar los parámetros válidos.
    /// </param>
    var me = $('#PROCEDIMIENTO');
    var standarized = $('#ESTANDARIZADO');
    if (me.valid()) {
        $.ajax({
            url: $('#GetParamsDataUrl').val(),
            data: { PROCEDIMIENTO: me.val() },
            dataType: 'json',
            type: 'POST',
            global: false,
            success: function (result) {
                var r = '';
                var allParams = [];
                if (result.Object != undefined) {
                    validParameters.length = 0;
                    for (key in result.Object) {
                        allParams.push(result.Object[key].NAME);
                        if (result.Object[key].IN_OUT.indexOf('IN') != -1) {
                            paramTypes[result.Object[key].NAME] = result.Object[key].DATA_TYPE;
                            validParameters.push(result.Object[key].NAME);
                            r += 'Nombre: ' + result.Object[key].NAME + ", ";
                            r += 'Tipo de Datos: ' + result.Object[key].DATA_TYPE;
                            r += "\n";
                        }
                    }
                    $('#Parameters').val(r);
                    availableParameters = validParameters;
                    if($.inArray("MENSAJE_SALIDA", allParams) != -1 && 
                        $.inArray("CODIGO_SALIDA", allParams) != -1) {
                        standarized.val('1');
                    }
                    else {
                        standarized.val('0');
                    }
                }
            }
        });
    }
    else {
        standarized.val('0');
        validParameters.length = 0;
        $('#Parameters').val('');
    }
}

(function ($) {

    //Controlador: Procedimientos
    //Accion: DetalleProcedimiento
    $.fn.ExecuteProcedure = function () {
        $('.datepicker').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true
        });
    };

    //Controlador: Roles
    //Accion: Crear Rol
    $.fn.CreateRol = function () {
        fnValidateDynamicContent();
        $('.createRoleSubmit').on('click', function () {
            if ($('.createRoleFrm').valid()) {
                $.post(
					$('.createRoleFrm').attr('action'),
					$('.createRoleFrm').serialize(),
					function (data) {
					    NRefreshTable('.rolesTable', $('#updateRolesTableUrl').val(), $('#editRoleUrl').val(), $('#deleteRoleUrl').val());
					    FnSetAjaxResultMsg(data.Message, data.Success);
					    $('#createRoleModal').modal('hide');
					    FnClearValues();
					}
				);
            }
        });
        $(document).on('click', '.deleteAction', function (evt) {
            evt.preventDefault();
            var me = $(this);
            var tmpUrl = me.data('url');
            var url = tmpUrl.substr(0, tmpUrl.lastIndexOf('/'));
            var ID = tmpUrl.substr(tmpUrl.lastIndexOf('/') + 1);
            SwalConfirm('¿Esta seguro que desea eliminar este rol?', function (result) {
                if (result) {
                    $.post(
					    url,
					    { id: ID },
					    function (data) {
					        FnSetAjaxResultMsg(data.Message, data.Success);
					        NRefreshTable('.rolesTable', $('#updateRolesTableUrl').val(), $('#editRoleUrl').val(), $('#deleteRoleUrl').val());
					    }
				    );
                }
            });
        });

        $(document).on('click', '.editAction', function (evt) {
            evt.preventDefault();
            var me = $(this);
            var tmpUrl = me.data('url');
            var url = tmpUrl.substr(0, tmpUrl.lastIndexOf('/'));
            var ID = tmpUrl.substr(tmpUrl.lastIndexOf('/') + 1);
            $.get(
				url,
				{ id: ID },
				function (data) {
				    $('.partialModalEdit').html(data);
				    fnValidateDynamicContent();
				    $('#editRoleModal').modal('show');
				}
			);
        });

        $(document).on('click', '.editRoleSubmit', function () {
            if ($('.editRoleFrm').valid()) {
                $.post(
					$('.editRoleFrm').attr('action'),
					$('.editRoleFrm').serialize(),
					function (data) {
					    FnSetAjaxResultMsg(data.Message, data.Success);
					    NRefreshTable('.rolesTable', $('#updateRolesTableUrl').val(), $('#editRoleUrl').val(), $('#deleteRoleUrl').val());
					    $('#editRoleModal').modal('hide');
					    FnClearValues();
					}
				);
            }
        });
    };


    //Controller: Users
    //Action : CreateUser
    $.fn.CreateUser = function () {
        $('.refreshUsers').on('click', function () {
            updateDropdown(null, $('#refreshUsersUrl').val(), $('#UserId'), $('#UserId').chosen());

        });
    };

    //Controller: Forms
    //Action: CreateForm
    $.fn.CreateForm = function () {
        //Variables globales de esta seccion.
        //var validParameters = [];
        //var availableParameters = [];
        //var paramTypes = [];
        var prmsRows = 0;
        var numParameters = 0;
        var table = $('.table').DataTable();
        var editModal = $('.editParameterModal').find('#createParameterModal');
        var createParamRef = $('.parameterModal').find('.modal-body');
        var editParamRef = editModal.find('.modal-body');
        var actualPrmsEdit = '';
        var editRow;
        var editIndex = 0;
        var reloaded = false;

        //Metodos utilitarios usados globalmente
        fnValidateDynamicContent();
        editModal.attr('id', 'editParamModal');
        editModal.find('.createParameterSubmit')
            .addClass('editParamSubmit').removeClass('createParameterSubmit');


        $('.addParameter').on('click', function (evt) {
            if (!$('#PROCEDIMIENTO').valid()) {
                evt.preventDefault();
            }
            else {
                if (reloaded) {
                    var paramNameC = createParamRef.find('#PARAMETRO');
                    var paramNameE = editParamRef.find('#PARAMETRO');
                    paramNameC.html('<option value="" ></option>');
                    paramNameE.html('<option value="" ></option>');
                    $.each(availableParameters, function (key, value) {
                        paramNameC.append(
                            $('<option />').text(value).val(value)
                        );

                        paramNameE.append(
                            $('<option />').text(value).val(value)
                        );

                    });
                    paramNameC.chosen().trigger('chosen:updated');
                    paramNameE.chosen().trigger('chosen:updated');
                    reloaded = false;
                }
                $('#createParameterModal').modal('show');
            }
        });

        $('#PROCEDIMIENTO').rules().remote.complete = function (xhr) {
            GetProcedureParams();
            numParameters = validParameters.length;
            table.clear().draw();
            reloaded = true;
        };

        $('.createParameterSubmit').on('click', function () {

            if (createParamRef.find('.dynamicForm').valid()) {
                var paramName = $('#PARAMETRO').val().toUpperCase();
                if ($.inArray(paramName, table.column(1).data()) != -1) {
                    SwalInfo('Este parámetro ya ha sido agregado.');
                }
                else if ($.inArray(paramName, validParameters) == -1) {
                    SwalError('El parámetro ' + paramName + ' no pertenece al procedimiento ' + $('#PROCEDIMIENTO').val(), 'Parámetro inválido');
                }
                else {
                    var nombreCampo = createParamRef.find('#NOMBRE').val();
                    var nombreParametro = createParamRef.find('#PARAMETRO').val();
                    var tipoDeDatos = createParamRef.find('#TIPO option:selected').text();
                    var tipoDeDatosInner = createParamRef.find('#TIPO option:selected').val();
                    var requerido = createParamRef.find($('.bootstrap-switch-id-REQUERIDO')).hasClass('bootstrap-switch-on') ? "SI" : "NO";
                    var requeridoInner = createParamRef.find($('.bootstrap-switch-id-REQUERIDO')).hasClass('bootstrap-switch-on') ? 1 : 0;
                    var opciones = '<a title="Editar" class="black editParameter" href="javascript:void(0);" data-toggle="tooltip" data-index="' + prmsRows + '" ><i class="glyphicon glyphicon-pencil " ></i></a> &nbsp;';
                    opciones += '<a title="Eliminar" class="black deleteParameter" href="javascript:void(0);" data-toggle="tooltip"><i class="glyphicon glyphicon-remove-sign "></i></a>';
                    var hiddens = '';
                    hiddens += FnCreateInputElement('PFD_PARAMETROS.Index', 'hidden', prmsRows);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + prmsRows + '].NOMBRE', 'hidden', nombreCampo);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + prmsRows + '].PARAMETRO', 'hidden', nombreParametro.toUpperCase());
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + prmsRows + '].TIPO', 'hidden', tipoDeDatosInner);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + prmsRows + '].REQUERIDO', 'hidden', requeridoInner);
                    table.row.add([nombreCampo, nombreParametro.toUpperCase(), requerido, tipoDeDatos, opciones + hiddens]).draw();
                    SwalSuccess('Parámetro agregado satisfactoriamente.');
                    $('#createParameterModal').modal('hide');
                    FnClearValues();
                }
                prmsRows++;
            }
        });

        $(document).on('click', '.allowedParams', function () {
            var me = $(this);
            var valueRef = $(me.closest('form')[0]).find('#PARAMETRO').val();
            if (valueRef == '') {
                SwalInfo('Introduzca el Nombre del parámetro primero.');
            }
            else {
                SwalInfo('Tipo de datos(Base de datos):\n ' + paramTypes[valueRef]);
            }

        });

        $('form').submit(function (event) {
            if (validParameters.length == 0 || table.data().length == 0) {
                SwalError('Definición de procedimiento y parametros incompleta.');
                event.preventDefault();
                return false;
            }
            if ($(this).valid() && table.data().length < validParameters.length) {
                if (window.confirm('Numero de parametros inferior a la cantidad disponible de parametros del procedimiento\n¿Desea continuar?')) {
                    return true;
                }
                else {
                    event.preventDefault();
                    return false;
                }
            }
            else if ($(this).valid() && table.data().length == validParameters.length) {
                return true;
            }
            else {
                event.preventDefault();
                return false;
            }
        });

        $('.parametersTable').on('click', '.deleteParameter', function () {
            var me = $(this);
            SwalConfirm('¿Está seguro que desea eliminar este parámetro?', function () {
                SwalSuccess('Parámetro eliminado!', '');
                table.row(me.closest("tr").get(0)).remove().draw(false);
            });
        });

        $('.parametersTable').on('click', '.editParameter', function () {
            editRow = $(this).closest("tr").get(0);
            editIndex = $(this).data('index');
            var data = table.row(editRow).data();
            actualPrmsEdit = data[1];
            editParamRef.find('#NOMBRE').val(data[0]);
            editParamRef.find('#PARAMETRO').val(data[1]);
            editParamRef.find('#PARAMETRO').chosen().trigger('chosen:updated');
            var selectedOpt = $(this).siblings('input[name*="TIPO"]').val();
            editParamRef.find('#TIPO').val(selectedOpt);
            editParamRef.find('#TIPO').chosen().trigger('chosen:updated');
            if (data[2] == "SI") {
                editParamRef.find($('.bootstrap-switch-id-REQUERIDO'))
                    .addClass('bootstrap-switch-on').removeClass('bootstrap-switch-off');
            }
            editModal.modal('show');
        });

        $('.editParamSubmit').on('click', function () {
            if (editParamRef.find('.dynamicForm').valid()) {
                var paramName = editParamRef.find('#PARAMETRO').val().toUpperCase();
                if ($.inArray(paramName, table.column(1).data()) != -1 && paramName != actualPrmsEdit.toUpperCase()) {
                    SwalInfo('Este parámetro ya ha sido agregado.');
                }
                else if ($.inArray(paramName, validParameters) == -1) {
                    SwalError('El parámetro ' + paramName + ' no pertenece al procedimiento ' + $('#PROCEDIMIENTO').val(), 'Parámetro inválido');
                }
                else {
                    var nombreCampo = editParamRef.find('#NOMBRE').val();
                    var nombreParametro = editParamRef.find('#PARAMETRO').val();
                    var tipoDeDatos = editParamRef.find('#TIPO option:selected').text();
                    var tipoDeDatosInner = editParamRef.find('#TIPO option:selected').val();
                    var requerido = editParamRef.find($('.bootstrap-switch-id-REQUERIDO')).hasClass('bootstrap-switch-on') ? "SI" : "NO";
                    var requeridoInner = editParamRef.find($('.bootstrap-switch-id-REQUERIDO')).hasClass('bootstrap-switch-on') ? 1 : 0;
                    var opciones = '<a title="Editar" class="black editParameter" href="javascript:void(0);" data-toggle="tooltip"  data-index="' + editIndex + '"><i class="glyphicon glyphicon-pencil " ></i></a> &nbsp;';
                    opciones += '<a title="Eliminar" class="black deleteParameter" href="javascript:void(0);" data-toggle="tooltip"><i class="glyphicon glyphicon-remove-sign "></i></a>';
                    var hiddens = '';
                    hiddens += FnCreateInputElement('PFD_PARAMETROS.Index', 'hidden', editIndex);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + editIndex + '].NOMBRE', 'hidden', nombreCampo);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + editIndex + '].PARAMETRO', 'hidden', nombreParametro.toUpperCase());
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + editIndex + '].TIPO', 'hidden', tipoDeDatosInner);
                    hiddens += FnCreateInputElement('PFD_PARAMETROS[' + editIndex + '].REQUERIDO', 'hidden', requeridoInner);
                    table.row(editRow).data([nombreCampo, nombreParametro.toUpperCase(), requerido, tipoDeDatos, opciones + hiddens]).draw();
                    SwalSuccess('Parámetro actualizado satisfactoriamente.');
                    editModal.modal('hide');
                    FnClearValues();
                }
            }
        });
        
    };
    //Controlador: Procedimientos
    //Accion: DetalleProcedimiento
    $.fn.ExecuteProcedure = function () {

        //Validaciones para formularios dinamicos
        $("#procedureForm .form-control").each(function () {
            var input = $(this);

            if (input.hasClass('requiredValidation')) {

                input.rules('add', {
                    required: true,
                    messages: {
                        required: "Required input"
                    }
                });
            }

            if (input.hasClass('numberValidation')) {

                input.rules('add', {
                    number: true
                });
            }

            if (input.hasClass('dateValidation')) {

                input.rules('add', {
                    localDate: true
                });
            }

        });

        $('.datepicker').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true
        });
        $(document).on('submit', 'form.executeProcedureForm', function (event) {
            //event.preventDefault();
        });

    };

    //Controller: Users
    //Action : CreateUser
    $.fn.CreateUser = function () {
        $('.refreshUsers').on('click', function () {
            updateDropdown(null, $('#refreshUsersUrl').val(), $('#UserId'), $('#UserId').chosen());

        });
    };

    $.fn.EditForm = function () {
        //var validParameters = [];
        var prmsRows = 0;
        var confirmed = false;
        var reloaded = true;
        var numParameters = 0;
        var table = $('.table').DataTable();
        var procedureLastValue = $('#PROCEDIMIENTO').val();
        var actualParamEdit = '';
        fnValidateDynamicContent();
        GetProcedureParams();
        numParameters = validParameters.length;
        var editDiv = $('.editParameterModal');
        var createParamRef = $('.parameterCreateModal').find('.modal-body');

        $('.parametersTable').on('click', '.deleteAction', function (event) {
            event.preventDefault();
            var me = $(this);
            SwalConfirm('¿Esta seguro que desea eliminar este parámetro?', function (confirmed) {
                if (confirmed) {
                    $.post(
					me.data('url'),
					function (data) {
					    if (data.Success) {
					        NRefreshTable('.parametersTable', $('#EditParamsTableUrl').val(), $('#EditParamsUrl').val(), $('#DeleteParamUrl').val());
					    }
					}
				);
                }
            });
        });

        $('.parametersTable').on('click', '.editAction', function (event) {
            event.preventDefault();
            var me = $(this);
            $.get(
				me.data('url'),
				function (data) {
				    $('.parameterEditModal').html(data);
				    me.trigger('loaded-content');
				    actualParamEdit = $('#editParameterModal').find('#ParameterInternal').val().toUpperCase();
				    var paramNameE = $('#editParameterModal').find('#PARAMETRO');
				    paramNameE.html('<option value="" ></option>');
				    $.each(availableParameters, function (key, value) {
				        paramNameE.append(
                            $('<option />').text(value).val(value)
                        );
				    });
				    paramNameE.val(actualParamEdit);
				    paramNameE.chosen().trigger('chosen:updated');
				    $('#editParameterModal').modal('show');
				}
			);
        });

        $('#PROCEDIMIENTO').on('change', function (event) {
            if (confirmed || table.data().length == 0) return;

            if (window.confirm('Al cambiar los datos del procedimiento se eliminaran los datos de los parametros.\n' +
				'Esta acción no podrá deshacerse.\n\n¿Desea continuar?')) {
                confirmed = true;
                $.post(
					$('#DeleteParamsUrl').val(),
					function (result) {
					    if (result.Result) {
					        table.clear().draw();
					    }
					}
				);
            }
            else {
                event.preventDefault();
                $(this).val(procedureLastValue);
            }
        });

        $('#PROCEDIMIENTO').rules().remote.complete = function (xhr) {
            GetProcedureParams();
            numParameters = validParameters.length;
            reloaded = true;
        };

        $('form.GlobalForm').submit(function (event) {
            numParameters = validParameters.length;
            if (numParameters == 0 || table.data().length == 0) {
                SwalError('Definición de procedimiento y parametros incompleta.', 'Faltan datos');
                event.preventDefault();
                return false;
            }
            if ($(this).valid()) {
                if (table.data().length < validParameters.length) {
                    if (window.confirm('Numero de parametros inferior a la cantidad disponible de parametros del procedimiento.' +
									   '\n¿Desea continuar?')) {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            else {
                event.preventDefault();
                return false;
            }
        });

        $('.addParameter').on('click', function (evt) {
            if (!$('#PROCEDIMIENTO').valid()) {
                evt.preventDefault();
            }
            else {
                if (table.data().length > 0) {
                    confirmed = false;
                }

                if (reloaded) {
                    var paramNameC = createParamRef.find('#PARAMETRO');
                    paramNameC.html('<option value="" ></option>');
                    $.each(availableParameters, function (key, value) {
                        paramNameC.append(
                            $('<option />').text(value).val(value)
                        );
                    });
                    paramNameC.chosen().trigger('chosen:updated');
                    reloaded = false;
                }
                $('#createParameterModal').modal('show');
            }
        });

        $('.createParameterSubmit').on('click', function () {

            if ($('.createParameterFrm').valid()) {
                var paramName = $('.createParameterFrm :input#PARAMETRO').val().toUpperCase();
                if ($.inArray(paramName, validParameters) == -1) {
                    SwalError('El parametro ' + paramName + ' no pertenece al procedimiento ' + $('#PROCEDIMIENTO').val(), 'Parámetro inválido');
                }
                else if ($.inArray(paramName, table.column(1).data()) != -1) {
                    SwalInfo('Este parámetro ya ha sido agregado.');
                }
                else {
                    $.post(
						$('#AddParamUrl').val(),
						$('.createParameterFrm').serialize(),
						function (data) {
						    if (data.Success) {
						        $('#createParameterModal').modal('hide');
						        FnClearValues();
						        NRefreshTable('.parametersTable', $('#EditParamsTableUrl').val(), $('#EditParamsUrl').val(), $('#DeleteParamUrl').val());
						    }
						}
					);
                }
            }
        });

        $(document).on('click', '.editParameterSubmit', function () {
            var updatedParam = $('#editParameterModal').find('#PARAMETRO').val();
            var sameParam = (actualParamEdit == updatedParam);
            if ($('.editParameterFrm').valid()) {
                if ($.inArray(updatedParam, validParameters) == -1) {
                    SwalError('Este parámetro no pertenece al procedimiento.', 'Parámetro erróneo');
                }
                else if ($.inArray(updatedParam, table.column(1).data()) != -1 && !sameParam) {
                    SwalInfo('Este parámetro ya ha sido agregado.');
                }
                else {
                    $.post(
						$('#EditParamsUrl').val(),
						$('.editParameterFrm').serialize(),
						function (data) {
						    if (data.Success) {
						        $('#editParameterModal').modal('hide');
						        FnClearValues();
						        NRefreshTable('.parametersTable', $('#EditParamsTableUrl').val(), $('#EditParamsUrl').val(), $('#DeleteParamUrl').val());
						    }
						}
					);
                }
            }
        });

        $(document).on('click', '.allowedParams', function () {
            var me = $(this);
            var valueRef = $(me.closest('form')[0]).find('#PARAMETRO').val();
            if (valueRef == '') {
                SwalInfo('Introduzca el Nombre del parámetro primero.');
            }
            else {
                SwalInfo('Tipo de datos(Base de datos):\n ' + paramTypes[valueRef]);
            }
        });
    };

    $.fn.FormsIndex = function () {
        $(document).on('click', '.showLogs', function (event) {
            var me = $(this);
            event.preventDefault();
            $.get(
                me.attr('href'),
                null,
                function (data) {
                    $('.logsModalDiv').html(data);
                    $(document).trigger('loaded-content');
                    $('.logsModalDiv').css('display', 'block');
                    $('.dynamicTable').DataTable().columns.adjust().draw();
                    $('#logModal').modal('show');
                }
            );
        });

        $(document).on('show.bs.modal', '.modal', function () {
            var height = $(window).height() - 200;
            $(this).find(".modal-body").css("max-height", height);
        });
    };

})(jQuery);

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
}(jQuery));