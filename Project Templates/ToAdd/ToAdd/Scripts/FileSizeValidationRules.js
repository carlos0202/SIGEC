jQuery.validator.unobtrusive.adapters.add(
    'maxfilesize', ['maxsize'], function (options) {
        options.rules['maxfilesize'] = options.params;
        if (options.message) {
            options.messages['maxfilesize'] = options.message;
        }
        else {
            options.messages['maxfilesize'] = 'Archivo supera el tamaño máximo permitido';
        }
    }
);

jQuery.validator.addMethod("maxfilesize",
    function (value, element, params) {
         
        if (element.files.length < 1) {
            // Ningun archivo seleccionado
            return true;
        }

        if (!element.files || !element.files[0].size) {
            // Este navegador no soporta el API de HTML5
            return true;
        }
        var comparator = typeof params.maxsize === 'undefined' ? params : params.maxsize;
        if (element.files[0].size < 0 || element.files[0].size == undefined)
            return true;

        return element.files[0].size < comparator;
    }
);