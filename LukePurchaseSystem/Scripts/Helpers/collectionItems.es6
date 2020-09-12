
class CollectionHandler {
    constructor(callback) {

        if (typeof (callback) !== 'undefined')
            this.appendCallback = callback;
        else
            this.appendCallback = function () { };
        this.initCollection()
    }

    initCollection() {
        var inst1 = this;
        $(".deleteRow").unbind("click");
        $('.collectionGroup').on('click', '.deleteRow', function () {
            $(this).closest('.contentRow').remove();
            return false;
        });
        $(".addCollectionItem").unbind("click");
        $('.addCollectionItem').on('click', function () {
            var inst = this;
            $.ajax({
                async: true,
                cache: false,
                url: $(inst).attr('data-ajax-url')
            }).done(function (partialView) {
                var collectionGroup = $(inst).parent().parent('.collectionControl').find('.collectionGroup')[0];
                
                $(collectionGroup).append(partialView);
                inst1.initCollection();

                inst1.appendCallback(collectionGroup);
            });
        });
    }     
}

