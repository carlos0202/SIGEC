(function (window) {
    var i18n = function () {
        var base = this;
        base.init = function () {
            base.lang.init();
        };
        base.lang = {
            settings: {
            },
            init: function () {
                $('.language > li > a').on('click', function () {
                    if ($(this).hasClass(Globalize.culture().name.toLowerCase()))
                        return false;
                    $.cookie("_culture", $(this).attr("class"), { expires: 365, path: '/SIGEC' /*, domain: 'example.com' */ });
                    window.location.reload(); // reload
                    return false;
                });
            }
        };
        base.init();
    };
    window.Code52 = {};
    window.Code52.Language = {
        instance: null,
        defaults: {
        },
        Init: function (culture) {
            Globalize.culture(culture);
            window.Code52.Language.instance = new i18n();
        }
    };
})(window);