jQuery.validator.addMethod('namefirstlettercaps', function(value, element, params) {
    return !value.match(new RegExp(params));
}, '');

jQuery.validator.unobtrusive.adapters.add('namefirstlettercaps', ['letterregex'], function (options) {
    options.rules['namefirstlettercaps'] = options.params.letterregex;
    options.messages['namefirstlettercaps'] = options.message;
});

