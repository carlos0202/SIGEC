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

$.validator.methods.number = function (value, element) {
    return this.optional(element) ||
        !isNaN(Globalize.parseFloat(value));
}


jQuery.extend(jQuery.validator.methods, {
    range: function (value, element, param) {
        //Use the Globalization plugin to parse the value        
        var val = Globalize.parseFloat(value);
        return this.optional(element) || (
            val >= param[0] && val <= param[1]);
    }
});

$.fn.modal.defaults.maxHeight = function () {
    // subtract the height of the modal header and footer
    return $(window).height() - 175;
}

//$.widget.bridge('uibutton', $.ui.button);
//Resolver colisión entre el nombre el jquery ui tooltip
//y el Bootstrap tooltip
$.widget.bridge('uitooltip', $.ui.tooltip);

function FnSetPopupGenerics() {
    $('a.edit_link').attr('title', Code52.Language.Dictionary.lblEdit).attr('data-toggle', 'tooltip');
    $('a.delete_link').attr('title', Code52.Language.Dictionary.lblDelete).attr('data-toggle', 'tooltip');
    $('a.view_link').attr('title', Code52.Language.Dictionary.lblDetails).attr('data-toggle', 'tooltip');
}

$(document).ajaxComplete(function () {
    FnSetPopupGenerics();
});

$(document).ready(function () {
    $(document.body).tooltip({
        selector: '[data-toggle="tooltip"]'
    });

    $(document.body).popover({
        selector: '[data-toggle="popover"]'
    });

    FnSetPopupGenerics();

    //Arregla los margenes de los action links de navegacion
    //de la sección de ayuda
    $('a[name]').each(function () {
        $(this).addClass('anchor');
    });

    //Inicialización por defecto para los elementos chosen single.
    $(".chosen").chosen({ width: '100%', allow_single_deselect: true });
    $(".chosen").attr('data-placeholder', Code52.Language.Dictionary.lblSelectOption);
    $(".chosen").chosen().change();
    $(".chosen").trigger('chosen:updated');
    $(".chosen-multiple").chosen({ width: '100%' });
    $(".chosen-multiple").attr('data-placeholder', Code52.Language.Dictionary.lblSelectOption);
    $(".chosen-multiple").chosen().change();
    $(".chosen-multiple").trigger('chosen:updated');

    //Datepicker initializations
    $('.defaultPicker').datepicker({
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true
    });

    $('.modal-body :input').each(function () {
        $(this).addClass('ignore');
    });

    $('.futurePicker').datepicker({
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true,
        minDate: 1
    });

    var decimal, thousands;

    var culture = $('html').attr('lang');
    Globalize.culture(culture);
    if (culture == 'es') {
        decimal = ',';
        thousands = '.';
        $.datepicker.setDefaults($.datepicker.regional['es']);
        $.timepicker.setDefaults($.timepicker.regional['es']);
        $(".CtimePicker").timepicker("option", $.timepicker.regional['es']);
    }
    else {
        $.datepicker.setDefaults($.datepicker.regional['']);
        $.timepicker.setDefaults($.timepicker.regional['']);
        $(".CtimePicker").timepicker("option", $.timepicker.regional['']);
        decimal = '.';
        thousands = ',';
    }

    $('.CtimePicker').timepicker({
        hourMin: 8,
        hourMax: 22,
        timeFormat: "HH:mm:ss",
        addSliderAccess: true,
        sliderAccessArgs: { touchonly: false }
    });

    //end datepicker initialization

    //Inicializacion de mascarillas usando el plugin Jquery Input mask
    var phoneMask = '(999) 999-9999'; //phone mask
    var dniMask = '999-9999999-9'; //dni mask
    var rncMask = '199-99999-9'; // rnc mask 

    //Remover datos si estos no estan completos.
    $('.phone').inputmask(phoneMask, { "clearIncomplete": true });
    $('.dni').inputmask(dniMask, { "clearIncomplete": true });
    $('.rnc').inputmask(rncMask, { "clearIncomplete": true });

    //Money mask
    $(".money").maskMoney({ allowNegative: false, thousands: '.', decimal: ',', affixesStay: false });

    //digit mask
    $('.digits').mask('000000000');

    //unmask inputs on submit action
    $('form').submit( function () {
        if ($(this).valid()) {
            $('.phone').inputmask('remove');
            $('.dni').inputmask('remove');
            $('.money').maskMoney('destroy');
        }
    });

    //tab pagination logic
    $('a.tabPager').on('click', function (evt) {
        var me = $(this);
        $('div#' + me.data('val')).find(':input').each(function () {
            if ($(this).hasClass('ignore')) {
            }
            else {
                $(this).valid();
            }
        });

        if ($('div#' + me.data('val')).find('div.has-error').length > 0) {
            $('div#' + me.data('val')).find('div.has-error').first().find(':input:first').focus();
            evt.preventDefault();
        }
        else {
            $('a[href="' + me.attr('href') + '"]').tab('show');
        }
    });

    //logica del checkbox para permitir cambiar datos de inicio de sesion
    $('#changeLoginInfo').on('change', function () {
        if ($(this).prop('checked') == true) {
            $('.loginInfo').show();
        }
        else {
            $('.loginInfo').hide();
        }
    });
    $('#changeLoginInfo').trigger('change');

    //Configuración de popups para los iconos por defecto de acciones en tablas.
    
});

//Dialogo para mostrar error de exceso de peticiones
$('.excesiveRequest').dialog({
    autoOpen: false,
    width: 'auto',
    maxWidth: 600,
    height: 'auto',
    modal: true,
    fluid: true, //new option
    resizable: false,
    buttons: {
        "OK": function () {
            $(this).dialog("close");
        }
    }
});

$(function () {
    $('.table').each(function () {
        var datatable = $(this);
        // estilo y label inline para el input de busqueda
        var search_input = datatable.closest('.dataTables_wrapper').find('div[id$=_filter] input');
        search_input.attr('placeholder', Code52.Language.Dictionary.lblSearch);
        search_input.addClass('form-control input-sm');

        // posicionamiento de la informacion de los records filtrados
        var length_sel = datatable.closest('.dataTables_wrapper').find('div[id$=_info]');
        length_sel.css('margin-top', '2px');
    });
});

//Definición del jquery datatable
$('.table').dataTable({
    "sPaginationType": "full_numbers",
    "oLanguage": {
        "sLengthMenu": Code52.Language.Dictionary.sLengthMenu,
        "sZeroRecords": Code52.Language.Dictionary.sZeroRecords,
        "sInfo": Code52.Language.Dictionary.sInfo,
        "sInfoEmpty": Code52.Language.Dictionary.sInfoEmpty,
        "sInfoFiltered": Code52.Language.Dictionary.sInfoFiltered,
        "sSearch": "",
        "oPaginate": {
            "sFirst": Code52.Language.Dictionary.sFirst,
            "sPrevious": Code52.Language.Dictionary.sPrevious,
            "sNext": Code52.Language.Dictionary.sNext,
            "sLast": Code52.Language.Dictionary.sLast
        },
        "bJQueryUI": true
    }
});

/*** Function to parse dynamic loaded content with unobstrusive jQuery validation ***/
function fnValidateDynamicContent(element) {
    $('.standard-form').removeData("validator");
    $('.standard-form').removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($('.standard-form'));
    // This line is important and added for client side validation to trigger, 
    // without this it didn't fire client side errors.
    $('.standard-form').validate();
}

//Funcion para actualizar los selects luego de cambios en sus opciones (bd, regla de negocio)
function updateDropdown(data, url, dropdownElement, chosenElement) {
    var html = '<option value="" ></option>';
    var select = "";
    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data == undefined || data == "") {
                dropdownElement.html(html);

            }
            else {
                $.each(data, function (key, row) {
                    //fill the dropdown
                    select = (row.Selected) ? ' selected = "true" ' : "";
                    html +=
                        '<option value="' + row.Value + '"' + select + '>'
                        + row.Text +
                        '</option>';
                });
                dropdownElement.html(html);
            }
            if (chosenElement != null) {
                chosenElement.trigger('chosen:updated');
            }
        }
    });
}

//Funcion para actualizar los datatables luego de cambios realizados en la base de datos
function RefreshTable(tableId, urlData, editUrl, deleteUrl, params) {
    // Retrieve the new data with $.getJSON. You could use it ajax too
    return $.getJSON(urlData, null, function (json) {
        table = $(tableId).dataTable();
        oSettings = table.fnSettings();

        table.fnClearTable(this);
        for (var i = 0; i < json.aaData.length; i++) {
            var links = '';
            links +=
                (!editUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="edit_link button_link editAction" title="' + Code52.Language.Dictionary.lblEdit + '" data-url="' + editUrl + '/' + json.aaData[i][params] + '" >Editar</a>';
            links +=
                (!deleteUrl) ? '' : '<a href="javascript:void(0);" data-toggle="tooltip" class="delete_link button_link deleteAction" title="' + Code52.Language.Dictionary.lblDelete + '" data-url="' + deleteUrl + '/' + json.aaData[i][params] + '" >Eliminar</a>';
            json.aaData[i][params] = links;
            table.oApi._fnAddData(oSettings, json.aaData[i]);
        }

        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        table.fnDraw();
        //$('.dataTables_wrapper').addClass('mn');
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

$('[readonly]').each(function (e) {
    $(this).attr('onfocus', 'this.blur()');
});

//Función para reusar la configuración asociada
//Al manejo de telefonos (Create). 
(function ($) {
    $.fn.phonesInit = function (primaryText, cancelText) {
        $('.phoneAddC').on('click', function () {
            if (!$('#phoneNumber').val() || !$('#type').val()) {
                alert(Code52.Language.Dictionary.lblPhoneAddMsg);
            }
            else {
                var phone = $('#phoneNumber').inputmask('unmaskedvalue');
                var phoneDisplay = $('#phoneNumber').val();
                var type = $('#type').val();
                var typeDisplay = $('#type option:selected').text();
                var notes = $('#notes').val();
                var table = $('.defaultTable').dataTable();
                var exit = false;
                $.each(table.fnGetData(), function (i, row) {
                    if (row[0] == phoneDisplay) {
                        alert(Code52.Language.Dictionary.lblEqPhoneErr);
                        exit = true;
                        return;
                    }
                });
                if (exit) return;
                $('.uPhone').val($('#phoneNumber').inputmask('unmaskedvalue'));
                $.ajax({
                    type: 'POST',
                    url: $('#checkPhoneUrl').val(),
                    data: $('.modal :input').serialize(),
                    success: function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblPhoneExist);
                            exit = true;
                            return;
                        }
                    },
                    dataType: 'json',
                    async: false
                });
                if (exit) return;
                var numRecords = table.fnSettings().fnRecordsTotal();
                //var link = '<a class="edit_link button_link" title="Editar" >Editar</a>';
                var link = '<a data-toggle="tooltip" class="delete_link button_link" title="' + Code52.Language.Dictionary.lblDelete + '" >Eliminar</a>';
                link += '<input name="Uphones" type="hidden" value="' + phone + '|' + type + '|' + notes + '" />';
                var added = table.fnAddData([phoneDisplay, typeDisplay, notes, link]);
                var n = table.fnSettings().aoData[added[0]].nTr;
                n.setAttribute("name", "rows");
                $('#phoneNumber').val('');
                $('#type').val('');
                $('#notes').val('');
                alert(Code52.Language.Dictionary.lblPhoneAdded);
                $('.modal').modal('hide');
            }
        });

        $('.defaultTable').on('click', '.delete_link', function (event) {
            event.preventDefault();
            if (confirm(Code52.Language.Dictionary.lblPhoneDeleteConfirm)) {
                var table = $('.defaultTable').dataTable();
                var nRow = $(this).closest("tr").get(0);
                table.fnDeleteRow(nRow);
            }
        });

        $('.openModal').on('click', function () {
            $('.modal').dialog('open');
        });
    };
})(jQuery);

(function ($) {
    $.fn.reloadMasks = function () {
        //Inicializacion de mascarillas usando el plugin Jquery Input mask
        var phoneMask = '(999) 999-9999'; //phone mask
        var dniMask = '999-9999999-9'; //dni mask
        var rncMask = '199-99999-9'; // rnc mask

        //Remover datos si estos no estan completos.
        $('.phone').inputmask(phoneMask, { "clearIncomplete": true });
        $('.dni').inputmask(dniMask, { "clearIncomplete": true });
        $('.rnc').inputmask(rncMask, { "clearIncomplete": true });
    }
})(jQuery);

//Función para reusar la configuración asociada
//Al manejo de telefonos (Edit). 
(function ($) {
    $.fn.phonesEditInit = function (primaryText, cancelText, entityId, isUser) {
        var urlExtra = '/?id=' + entityId + '&' + 'isUser=' + isUser;
        var editPhoneUrl = $('#editPhoneUrl').val();
        var deletePhoneUrl = $('#deletePhoneUrl').val();

        $('.phoneSaveBtn').on('click', function () {
            if (!$('#phonesEditModal #phoneNumber').val() || !$('#phonesEditModal #type').val()) {
                alert(Code52.Language.Dictionary.lblPhoneAddMsg);
            }
            else {
                $('.cVal').val($('.ePhone').inputmask('unmaskedvalue'));
                $.post(
                    $('#checkPhoneUrl').val(),
                    $('#phonesEditModal :input').serialize(),
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblPhoneExist);
                            return;
                        }
                    }
                ).done(function (data) {
                    if (data) {
                        $.post(
                            $('#updatePhoneUrl').val(),
                            $('#phonesEditModal :input').serializeArray(),
                            function (data) {
                                RefreshTable('.defaultTable', $('#updatePhonesTableUrl').val() + urlExtra, editPhoneUrl, deletePhoneUrl, 3);
                                alert(data);
                            }
                        );
                    }
                });
                $('#phonesEditModal').modal('hide');
            }
        });

        $('.phoneAddC').on('click', function () {
            $('.uPhone').val($('.cPhone').inputmask('unmaskedvalue'));
            if (!$('.cPhone').val() || !$('.cType').val()) {
                alert(Code52.Language.Dictionary.lblPhoneAddMsg);
            }
            else {
                var values = $('#phonesAddModal :input').serialize();
                $.post(
                    $('#checkPhoneUrl').val(),
                    values,
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblPhoneExist);
                        }
                    }
                ).done(function (data) {
                    if (data) {
                        $.post(
                            $('#AddPhoneUrl').val(),
                            values,
                            function (data) {
                                RefreshTable('.defaultTable', $('#updatePhonesTableUrl').val() + urlExtra, editPhoneUrl, deletePhoneUrl, 3);
                                alert(data);
                            }
                        );
                        $('#phonesAddModal').modal('hide');
                    }
                });
            }
        });

        $('.openModal').on('click', function () {
            $('.cModal').dialog('open');
        });

        $('.defaultTable').on('click', '.editAction', function (event) {
            $.post(
                $(this).data('url'),
                function (data) {
                    $('#phoneNumber').val(data.number);
                    $('#type').val(data.type);
                    $('#notes').val(data.notes);
                    $('#phoneID').val(data.ID);
                    $('#phonesEditModal').modal();
                }
            );
            event.preventDefault();
        });
        $('.defaultTable').on('click', '.deleteAction', function (event) {
            if (confirm(Code52.Language.Dictionary.Delete_Confirmation)) {
                $.post(
                    $(this).data('url'),
                    function (data) {
                        RefreshTable('.defaultTable', $('#updatePhonesTableUrl').val() + urlExtra, editPhoneUrl, deletePhoneUrl, 3);
                        alert(data);
                    }
                );
            }
            event.preventDefault();
        });
        RefreshTable('.defaultTable', $('#updatePhonesTableUrl').val() + urlExtra, editPhoneUrl, deletePhoneUrl, 3);
    }
})(jQuery);

//Logica de configuración para la edición de permisos en /Authorizations/Edit
(function ($) {
    $.fn.authorizationsInit = function () {
        $('.authorizationRole').on('change', function () {
            if ($('.authorizationRole option:selected').val() != "") {
                $.post(
                    $('#authorizationsUrl').val(),
                    { id: $('#RoleId option:selected').val() },
                    function (data) {
                        $(".partial").html("");
                        $(data).appendTo('.partial');
                    }
                );
            }
            else {
                $(".partial").html("");
            }
        });

        if ($('.authorizationRole option').length > 0) {
            $('.authorizationRole').trigger("change");
        }
    }
})(jQuery);

//Lógica de configuración para el historial médico del paciente /Patients/MedicalHistory
(function ($) {
    $.fn.medicalHistoryInit = function () {
        var editFDUrl = $('#editFDUrl').val();
        var deleteFDUrl = $('#deleteFDUrl').val();
        RefreshTable('.fbackTable', $('#updateFDTable').val(), editFDUrl, deleteFDUrl, 2);
        $('.addfBack').on('click', function () {
            $('.fBackModal').addClass('fBackCreate').removeClass('fBackEdit');
            $('.fbTitle').text(Code52.Language.Dictionary.lblFamilyMemBackAdd);
        });

        $('.fbackTable').on('click', '.edit_link', function () {
            var me = $(this);
            $('.fBackModal').addClass('fBackEdit').removeClass('fBackCreate');
            $('.fbTitle').text(Code52.Language.Dictionary.lblFamilyMemBackEdit);
            $.get(
                me.data('url'),
                function (data) {
                    $('#familyMember').val(data.familyMember);
                    $('#disease').val(data.disease); fmDiseaseID
                    $('#fmDiseaseID').val(data.ID);
                    $('#familybackmodal').modal('show');
                }
            );

        });

        $('.fbackTable').on('click', '.delete_link', function () {
            var me = $(this);
            if (confirm(Code52.Language.Dictionary.Delete_Confirmation)) {
                $.post(
                    me.data('url'),
                    function (data) {
                        if (data) {
                            if (data) {
                                alert(Code52.Language.Dictionary.lblRegistryDeleted);
                                RefreshTable('.fbackTable', $('#updateFDTable').val(), editFDUrl, deleteFDUrl, 2);
                            }
                            else {
                                alert(Code52.Language.Dictionary.lblRegistryDeleteError);
                            }
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.fBackCreate', function () {
            if (!$('#familyMember').val() || !$('#disease').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    $('#addFbackUrl').val(),
                    $('div.familyBackForm :input').serialize(),
                    function (data) {
                        if (data) {
                            alert(Code52.Language.Dictionary.lblRegistryAdded);
                            RefreshTable('.fbackTable', $('#updateFDTable').val(), editFDUrl, deleteFDUrl, 2);
                            $('div.familyBackForm :input').each(function () {
                                $(this).val('');
                            });
                            $('#familybackmodal').modal('hide');
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryAddError);
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.fBackEdit', function () {
            if (!$('#familyMember').val() || !$('#disease').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    editFDUrl,
                    $('div.familyBackForm :input').serialize(),
                    function (data) {
                        if (data) {
                            alert(Code52.Language.Dictionary.lblRegistryEdited);
                            RefreshTable('.fbackTable', $('#updateFDTable').val(), editFDUrl, deleteFDUrl, 2);
                            $('div.familyBackForm :input').each(function () {
                                $(this).val('');
                            });
                            $('#familybackmodal').modal('hide');
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryEditError);
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.fbClose', function () {
            $('div.familyBackForm :input').each(function () {
                $(this).val('');
            });
        });
    }
})(jQuery);

//Logica javascript para la seccion Consultations/Index
(function ($) {
    $.fn.consultationIndexInit = function () {
        $('.createConsultation').on('click', function (evt) {
            evt.preventDefault();
            $('#selectPatient').modal();
        });

        $('.btn-select').on('click', function () {
            if (!$('#patientID').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                window.location = $('.createConsultation').attr('href') + '?' + $('#patientID').serialize();
            }
        });
        
    }
})(jQuery);

//Logica javascript para la busqueda de pacientes en citas.
(function ($) {
    $.fn.appointmentsPSearchInit = function () {
        $('.patient-search').on('click', function () {
            $('.psData').hide();
            $('.psBody :input').each(function () {
                $(this).val('');
            });
            $('.modal').modal();
        });

        $('.startSearch').on('click', function () {
            if (!$('#dni').val()) {
                alert(Code52.Language.Dictionary.lblSearchCriteriaErr);
            }
            else {
                $.post(
                    $('#patientSearchUrl').val(),
                    { dni: $('#dni').inputmask('unmaskedvalue') },
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblNotFound);
                            $('#pID').val(0);
                            $('.psData').hide();
                        }
                        else {
                            $('#firstName').val(data.firstName);
                            $('#lastName').val(data.lastName);
                            $('#username').val(data.username);
                            $('#pID').val(data.ID);
                            $('.psData').show();
                        }
                    }
                );
            }
        });

        $('.btn-select').on('click', function () {
            if ($('#pID').val() == 0) {
                alert(Code52.Language.Dictionary.lblNoPatientSelectErr);
                return false;
            }
            else {
                $('#patientID').val($('#pID').val());
                $('#patientID').chosen().trigger('chosen:updated');
                $('.modal').modal('hide');
            }
        });

        //Evento para obtener el id del paciente de la fila seleccionada
        //de la tabla.
        $(".table tr").on("click", function (event) {
            var oTable = $('.table').dataTable();
            $(oTable.fnSettings().aoData).each(function () {
                $(this.nTr).removeClass('row_selected');
            });
            $(event.target.parentNode).addClass('row_selected');
            $('#pID').val($(event.target.parentNode).attr('id'));
        });

        $('#generalSearch').on('change', function () {
            $('#pID').val(0);
            if ($(this).prop('checked') == true) {
                $('div.general').removeClass('dn');
                $('div.dni').addClass('dn');
                $('.psData').addClass('dn').hide();
            }
            else {
                $('div.general').addClass('dn');
                $('div.dni').removeClass('dn');
            }
        });
        
    }
})(jQuery);

//Logica para la pantalla de pagos de consultas Payments/PayConsultation
(function ($) {
    $.fn.consultationPaymentsInit = function () {

        $('#doctorID').on('change', function () {
            var me = $(this);
            if (!me.val()) {
                $('#price').val(0);
                $('#discount').val(0);
            }
            else {
                $.post(
                    $('#GetPriceUrl').val(),
                    { doctorID: me.val() },
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblPriceNotFoundErr);
                        }
                        else {
                            $('#price').val(data);
                            if ($('#paymentForm').val() == "Exoneration") {
                                $('#discount').val($('#price').val());
                                setPatientAmount();
                            }
                            setPatientAmount();
                        }
                    }
                );
            }
        });

        $('#insurer').on('keyup', function () {
            var me = $(this);
            if (!me.val()) {
                me.val(0);
            }
            if (me.maskMoney('unmasked')[0] + $('#discount').maskMoney('unmasked')[0] > $('#price').maskMoney('unmasked')[0]) {
                alert(Code52.Language.Dictionary.lblPaymentDataErr);
                me.val(0);
                setPatientAmount();
            }
            else {
                setPatientAmount();
            }
        });
        $('#discount').on('keyup', function () {
            var me = $(this);
            if (!me.val()) {
                me.val(0);
            }
            if (me.maskMoney('unmasked')[0] + $('#insurer').maskMoney('unmasked')[0] > $('#price').maskMoney('unmasked')[0]) {
                alert(Code52.Language.Dictionary.lblPaymentDataErr);
                me.val(0);
                setPatientAmount();
            }
            else {
                setPatientAmount();
            }
            
        });

        //$('form').submit(function () {
        //    if ($(this).valid()) {
        //        $('input.money').each(function () {
        //            $(this).val($(this).maskMoney('unmasked')[0]);

        //        });
        //    }
        //});

        $('#patientWApp').on('change', function () {
            var data = { hasAppointment : $(this).prop('checked') };
            updateDropdown(data, $('#GetPatientsUrl').val(), $('#patientID'), $('#patientID').chosen());
        });

        $('#paymentForm').on('change', function () {
            if ($(this).val() == "Exoneration") {
                $('#discount').val($('#price').val());
                $('#discount').attr('readonly', 'true').addClass('control-readonly');
                $('#insurer').attr('readonly', 'true').addClass('control-readonly');
                setPatientAmount();
            }
            else {
                $('#discount').val(0);
                $('#discount').attr('readonly', 'false').removeClass('control-readonly');;;
                $('#insurer').attr('readonly', 'false').removeClass('control-readonly');;;
                setPatientAmount();
            }
        });
        

        $('#patientWApp').trigger('change');

        function setPatientAmount() {
            var total = $('#price').maskMoney('unmasked')[0] - $('#discount').maskMoney('unmasked')[0] - $('#insurer').maskMoney('unmasked')[0];
            $('#patient').val((total >= 0) ? total : 0);
        }
    }
})(jQuery);

//lógica prara el view de editar consultas Consultations/Edit
(function ($) {
    $.fn.consultationEditInit = function () {
        var editDiagUrl = $('#editDiagUrl').val();
        var deleteDiagUrl = $('#deleteDiagUrl').val();
        var editMedUrl = $('#editMedUrl').val();
        var deleteMedUrl = $('#deleteMedUrl').val();
        $('.addDiagnostic').on('click', function () {
            $('.diagnosticAction').addClass('diagCreate').removeClass('diagEdit');
            $('.diagTitle').text(Code52.Language.Dictionary.lblDiagnoseAdd);
            FnCleanInputs('.diagBody :input');
        });

        $('.addMedicine').on('click', function () {
            $('.medicineAction').addClass('medCreate').removeClass('medEdit');
            $('.medTitle').text(Code52.Language.Dictionary.lblMedicamentAdd);
            FnCleanInputs('.medBody :input');
        });
        
        $('.diagnosticsTable').on('click', '.editAction', function () {
            FnCleanInputs('.diagBody :input');
            $('.diagnosticAction').addClass('diagEdit').removeClass('diagCreate');
            $('.diagTitle').text(Code52.Language.Dictionary.lblDiagnoseEdit);
            $.get(
                $(this).data('url'),
                function (data) {
                    $('#diagnoseCode').val(data.diagnoseCode);
                    $('#diagnoseCode').chosen().trigger('chosen:updated');
                    $('.mObservations').val(data.observations);
                    $('#diseaseID').val(data.ID);
                    $('#diagnosticsModal').modal();
                }
            );
        });

        $('.medicinesTable').on('click', '.editAction', function () {
            FnCleanInputs('.medBody :input');
            $('.medicineAction').addClass('medEdit').removeClass('medCreate');
            $('.medTitle').text(Code52.Language.Dictionary.lblDMedicamentEdit);
            $.get(
                $(this).data('url'),
                function (data) {
                    $('#medicineID').val(data.medicineID);
                    $('#medicineID').chosen().trigger('chosen:updated');
                    $('#administration').val(data.administration);
                    $('#pmID').val(data.ID);
                    $('#medicinesModal').modal();
                }
            );
        });

        $('.diagnosticsTable').on('click', '.deleteAction', function (event) {
            if (confirm(Code52.Language.Dictionary.Delete_Confirmation)) {
                $.post(
                    $(this).data('url'),
                    function (data) {
                        
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryDeleteError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryDeleted);
                            RefreshTable('.diagnosticsTable', $('#updateDiagnosticTableUrl').val(), editDiagUrl, deleteDiagUrl, 2);
                        }
                    }
                );
            }
            event.preventDefault();
        });

        $('.medicinesTable').on('click', '.deleteAction', function (event) {
            if (confirm(Code52.Language.Dictionary.Delete_Confirmation)) {
                $.post(
                    $(this).data('url'),
                    function (data) {

                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryDeleteError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryDeleted);
                            RefreshTable('.medicinesTable', $('#updateMedicinesTableUrl').val(), editMedUrl, deleteMedUrl, 2);
                        }
                    }
                );
            }
            event.preventDefault();
        });
        
        $('.modal-footer').on('click', '.diagCreate', function () {
            if(!$('#diagnoseCode').val()){
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    $('#addDiagnosticUrl').val(),
                    $('.diagBody :input').serialize() + '&' +  $('#ID').serialize(),
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryAddError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryAdded);
                            RefreshTable('.diagnosticsTable', $('#updateDiagnosticTableUrl').val(), editDiagUrl, deleteDiagUrl, 2);
                            $('#diagnosticsModal').modal('hide');
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.medCreate', function () {
            if (!$('#medicineID').val() || !$('#administration').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    $('#addMedicineUrl').val(),
                    $('.medBody :input').serialize() + '&' + $('#ID').serialize(),
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryAddError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryAdded);
                            RefreshTable('.medicinesTable', $('#updateMedicinesTableUrl').val(), editMedUrl, deleteMedUrl, 2);
                            $('#medicinesModal').modal('hide');
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.diagEdit', function () {
            if (!$('#diagnoseCode').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    editDiagUrl,
                    $('.diagBody :input').serialize(),
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryEditError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryEdited);
                            RefreshTable('.diagnosticsTable', $('#updateDiagnosticTableUrl').val(), editDiagUrl, deleteDiagUrl, 2);
                            $('#diagnosticsModal').modal('hide');
                        }
                    }
                );
            }
        });

        $('.modal-footer').on('click', '.medEdit', function () {
            if (!$('#medicineID').val() || !$('#administration').val()) {
                alert(Code52.Language.Dictionary.lblMustFillAll);
            }
            else {
                $.post(
                    editMedUrl,
                    $('.medBody :input').serialize(),
                    function (data) {
                        if (!data) {
                            alert(Code52.Language.Dictionary.lblRegistryEditError);
                        }
                        else {
                            alert(Code52.Language.Dictionary.lblRegistryEdited);
                            RefreshTable('.medicinesTable', $('#updateMedicinesTableUrl').val(), editMedUrl, deleteMedUrl, 2);
                            $('#medicinesModal').modal('hide');
                        }
                    }
                );
            }
        });

        $('form').submit(function (event) {
            if (confirm(Code52.Language.Dictionary.lblSaveNCConfirmation)) {
                return true;
            }
            else {
                return false;
                event.preventDefault();
            }
        });
        
        RefreshTable('.diagnosticsTable', $('#updateDiagnosticTableUrl').val(), editDiagUrl, deleteDiagUrl, 2);
        RefreshTable('.medicinesTable', $('#updateMedicinesTableUrl').val(), editMedUrl, deleteMedUrl, 2);
    }
})(jQuery);

(function ($) {
    $.fn.doctorRulesInit = function () {
        var minPrev, maxPrev;

        $('.minHour').on('change', function () {
            var me = $(this);
            var max = $('.maxHour');

            if (!max.val() || !me.val()) return;

            var start = me.datetimepicker('getDate');
            var end = max.datetimepicker('getDate');
            if(start >= end) {
                me.val(minPrev);
            }
            else {
                minPrev = me.val();
            }
        });

        $('.maxHour').on('change', function () {
            var me = $(this);
            var min = $('.minHour');

            if (!min.val() || !me.val()) return;

            var start = min.datetimepicker('getDate');
            var end = me.datetimepicker('getDate');
            if (start >= end) {
                me.val(maxPrev);
            }
            else {
                maxPrev = me.val();
            }
        });
    }
})(jQuery);

//funcion para limpiar el valor de los inputs
//utilizados en los modals.
function FnCleanInputs(elements) {
    $(elements).each(function () {
        var me  = $(this);
        me.val('');
        if (me.hasClass('chosen') || me.hasClass('chosen-multiple')) {
            me.chosen().trigger('chosen:updated');
        }
    });
}
