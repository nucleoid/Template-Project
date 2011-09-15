$(document).ready(function () {
    $('form').one('submit', function (e) {
        e.preventDefault();
        var names = jQuery.map($('[flaggedenum="true"]'), function (checkbox) {
            return $(checkbox).attr("name");
        });
        var uniqueNames = jQuery.unique(names);
        $(uniqueNames).each(function () {
            var multiChecked = jQuery.map($('[name="' + $(this) + '"]:checked'), function (multiCheck) {
                return multiCheck.value;
            }).join(',');
            var hiddenName = this.toString().replace("Not.", "");
            $('[name="' + hiddenName + '"').val(multiChecked);
        });
        $(this).submit();
    });
});
