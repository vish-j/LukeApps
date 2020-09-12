'use strict';

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

$.fn.InitFileUploader = function () {
    var isDeleteEnabled = arguments.length <= 0 || arguments[0] === undefined ? true : arguments[0];

    new FileControl(this, isDeleteEnabled);
    return this;
};

var FileControl = (function () {
    function FileControl(control, isDeleteEnabled) {
        _classCallCheck(this, FileControl);

        this.mainControl = control;
        this.uploadUrl = control.data('upload-url');
        this.downloadUrl = control.data('download-url');
        this.dataType = control.data('type');
        this.filesDisplay = control.find('.filesDisplay');
        this.keysControl = control.find('.fileKeys');
        this.filesNames = control.find('.fileNames');
        this.filesControl = control.find('.fileUploadInput');
        this.enableDelete = isDeleteEnabled;
        this.initFileUpload();
        this.initiateFileDisplay();
    }

    _createClass(FileControl, [{
        key: 'initiateFileDisplay',
        value: function initiateFileDisplay() {
            var _this = this;

            var getState = function getState() {
                return _this;
            };
            var keysControlval = this.keysControl.val();
            if (keysControlval) {
                var keysArray = keysControlval.split(',');
                var filesNamesVal = this.filesNames.val();
                if (filesNamesVal) {
                    $.each(filesNamesVal.split(','), function (index, name) {
                        if (name !== '') {
                            var key = keysArray[index];
                            getState().addFile(key, name);
                        }
                    });
                }
            }
        }
    }, {
        key: 'initFileUpload',
        value: function initFileUpload() {
            var _this2 = this;

            var getState = function getState() {
                return _this2;
            };

            this.filesControl.fileupload({
                url: this.uploadUrl,
                dataType: 'json',
                sequentialUploads: true,
                autoUpload: true,
                done: function done(e, data) {
                    console.log(data);
                    $.each(data.result.Files, function (index, file) {
                        getState().addFile(file.FileCode, file.FileName);
                    });
                },
                progressall: function progressall(e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    var progressDisplay = $(this).closest('.form-group').find('.progress .progress-bar');

                    progressDisplay.css('width', progress + '%');
                    progressDisplay.text(progress + '%');
                },
                fail: function fail(e, data) {
                    alert('Fail!');
                }
            }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');
        }
    }, {
        key: 'removeFile',
        value: function removeFile(key, name) {
            $('#' + key).find('.remove-file').off('click');
            $('#' + key).remove();
            this.keysControl.val(this.removeValue(this.keysControl.val(), key).replace(/(^,)|(,$)/g, ""));
            this.filesNames.val(this.removeValue(this.filesNames.val(), name).replace(/(^,)|(,$)/g, ""));

            if (this.dataType === 'single') {
                this.mainControl.find('.fileUploadInput').removeAttr('disabled');
            }
        }
    }, {
        key: 'addFile',
        value: function addFile(key, name) {
            var _this3 = this;

            var getState = function getState() {
                return _this3;
            };
            this.appendDisplay(key, name);

            $('#' + key).find('.remove-file').bind('click', function () {
                getState().removeFile(key, name);
            });

            if (this.dataType === 'single') {
                this.mainControl.find('.fileUploadInput').attr('disabled', 'disabled');
                this.keysControl.val(key.replace(/(^,)|(,$)/g, ""));
                this.filesNames.val(name.replace(/(^,)|(,$)/g, ""));
            } else {
                this.keysControl.val(this.addValue(this.keysControl.val(), key).replace(/(^,)|(,$)/g, ""));
                this.filesNames.val(this.addValue(this.filesNames.val(), name).replace(/(^,)|(,$)/g, ""));
            }
        }
    }, {
        key: 'appendDisplay',
        value: function appendDisplay(key, name) {
            var delhtml = '<a class="remove-file"><span class="glyphicon glyphicon-remove"></span></a>';
            var filehtml = '<div class="row"><div class="col-md-9"><span class="glyphicon glyphicon-file"></span><a href="' + this.downloadUrl + '?key=' + key + '">' + name + '</a></div><div class="col-md-3">';

            if (this.enableDelete) {
                filehtml = filehtml + delhtml;
            }
            filehtml = filehtml + '</div></div>';

            $('<div id=' + key + ' class="list-group-item" />').html(filehtml).appendTo(this.filesDisplay);
        }
    }, {
        key: 'addValue',
        value: function addValue(list, value, separator) {
            separator = separator || ",";
            var values = list.split(separator);
            for (var i = 0; i < values.length; i++) {
                if (values[i] === value) {
                    return list;
                }
            }
            values.push(value);
            return values.join(separator);
        }

        //https://stackoverflow.com/questions/1306164/remove-value-from-comma-separated-values-string
    }, {
        key: 'removeValue',
        value: function removeValue(list, value, separator) {
            separator = separator || ",";
            var values = list.split(separator);
            for (var i = 0; i < values.length; i++) {
                if (values[i] === value) {
                    values.splice(i, 1);
                    return values.join(separator);
                }
            }
            return list;
        }
    }]);

    return FileControl;
})();

