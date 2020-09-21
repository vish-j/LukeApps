$('.LumpSumCheck').each(function () {
    action(this);
});

addLumpsumEvent();
function addLumpsumEvent() {
    $('.LumpSumCheck').change(function () {
        action(this);
    });
}


function action(ctx) {
    var value = $(ctx).prop('checked');
    var closestContentRow = $(ctx).closest('.contentRow');
    var unit = closestContentRow.find('.LumpSumUnit');
    var quantity = closestContentRow.find('.LumpSumQ');
    var unitLabel = closestContentRow.find('.LumpSumUnitLabel');
    var quantityLabel = closestContentRow.find('.LumpSumQLabel');
    var unitVer = closestContentRow.find('.LumpSumUnitVer');
    var quantityVer = closestContentRow.find('.LumpSumQVer');
    var unitPriceLabel = closestContentRow.find('.LumpSumUPLabel');
    if (value) {
        unit.val('LumpSum');
        quantity.val('1');
        unit.addClass('hide');
        quantity.addClass('hide');
        unitLabel.addClass('hide');
        quantityLabel.addClass('hide');
        unitVer.addClass('hide');
        quantityVer.addClass('hide');
        unitPriceLabel.text('LumpSum Price')
    }
    else {
        if (unit.val() == 'LumpSum') {
            unit.val('');
            quantity.val('');
        }
        unit.removeClass('hide');
        quantity.removeClass('hide');
        unitLabel.removeClass('hide');
        quantityLabel.removeClass('hide');
        unitVer.removeClass('hide');
        quantityVer.removeClass('hide');
        unitPriceLabel.text('Unit Price')
    }
}