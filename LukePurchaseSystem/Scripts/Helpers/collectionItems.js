"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var CollectionHandler = (function () {
    function CollectionHandler(callback) {
        _classCallCheck(this, CollectionHandler);

        if (typeof callback !== 'undefined') this.appendCallback = callback;else this.appendCallback = function () {};
        this.initCollection();
    }

    _createClass(CollectionHandler, [{
        key: "initCollection",
        value: function initCollection() {
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
    }]);

    return CollectionHandler;
})();

