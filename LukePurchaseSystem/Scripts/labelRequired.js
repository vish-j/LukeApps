asterisk();
function asterisk() {
    (function ($) {
        $('[data-val-required]').each(function () {
            var label = $('label[for="' + $(this).attr('id') + '"]');
            var text = label.text();
            if (text.length > 0) {
                label.addClass("required");
            }
        });
    })(jQuery);
}