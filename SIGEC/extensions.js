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

//focus en el primer input de cada dialogo modal.
$(document).on("shown.bs.modal", '.modal', function () {
    $(this).find(".form-control:first").focus();
});
//borrar valores del modal al cerrarse.
$(document).on('hidden.bs.modal', function () {
    FnClearValues();
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

$(document).ready(function () {
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
    
});

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