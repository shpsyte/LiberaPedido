jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    },
    range: function (value, element, param) {
        return (Globalize.parseFloat(value) >= Globalize.parseFloat(param[0]) && Globalize.parseFloat(value) <= Globalize.parseFloat(param[1]));
    }
});



function BloqueiaLetras(evento) {
    var tecla;

    if (window.event) { // Internet Explorer
        tecla = event.keyCode;
    }
    else { // Firefox
        tecla = evento.which;
    }

    if (tecla == 46) return false;
    return true;
}