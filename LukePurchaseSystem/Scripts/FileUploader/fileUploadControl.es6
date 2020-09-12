$.fn.InitFileUploader = function (isDeleteEnabled = true) {
    new FileControl(this, isDeleteEnabled);
    return this;
};

class FileControl {
    constructor(control, isDeleteEnabled) {
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
    initiateFileDisplay() {
        const getState = () => {
            return this;
        };
        var keysControlval = this.keysControl.val()
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
    initFileUpload() {
        const getState = () => {
            return this;
        };

        this.filesControl.fileupload({
            url: this.uploadUrl,
            dataType: 'json',
            sequentialUploads: true,
            autoUpload: true,
            done: function (e, data) {
                console.log(data);
                $.each(data.result.Files, function (index, file) {
                    getState().addFile(file.FileCode, file.FileName);
                });
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                var progressDisplay = $(this).closest('.form-group').find('.progress .progress-bar');

                progressDisplay.css(
                    'width',
                    progress + '%'
                );
                progressDisplay.text(progress + '%');
            },
            fail: function (e, data) {
                alert('Fail!');
            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    }
    removeFile(key, name) {
        $('#' + key).find('.remove-file').off('click');
        $('#' + key).remove();
        this.keysControl.val(this.removeValue(this.keysControl.val(), key).replace(/(^,)|(,$)/g, ""));
        this.filesNames.val(this.removeValue(this.filesNames.val(), name).replace(/(^,)|(,$)/g, ""));

        if (this.dataType === 'single') {
            this.mainControl.find('.fileUploadInput').removeAttr('disabled');
        }
    }
    addFile(key, name) {
        const getState = () => {
            return this;
        };
        this.appendDisplay(key, name);

        $('#' + key).find('.remove-file').bind('click', function () {
            getState().removeFile(key, name);
        });

        if (this.dataType === 'single') {
            this.mainControl.find('.fileUploadInput').attr('disabled', 'disabled');
            this.keysControl.val(key.replace(/(^,)|(,$)/g, ""));
            this.filesNames.val(name.replace(/(^,)|(,$)/g, ""));
        }
        else {
            this.keysControl.val(this.addValue(this.keysControl.val(), key).replace(/(^,)|(,$)/g, ""));
            this.filesNames.val(this.addValue(this.filesNames.val(), name).replace(/(^,)|(,$)/g, ""));
        }
    }
    appendDisplay(key, name) {
        var delhtml = '<a class="remove-file"><span class="glyphicon glyphicon-remove"></span></a>';
        var filehtml = '<div class="row"><div class="col-md-9"><span class="glyphicon glyphicon-file"></span><a href="' + this.downloadUrl + '?key=' + key + '">' + name + '</a></div><div class="col-md-3">';

        if (this.enableDelete) {
            filehtml = filehtml + delhtml;
        }
        filehtml = filehtml + '</div></div>';

        $('<div id=' + key + ' class="list-group-item" />').html(filehtml).appendTo(this.filesDisplay);
    }
    addValue(list, value, separator) {
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
    removeValue(list, value, separator) {
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
}