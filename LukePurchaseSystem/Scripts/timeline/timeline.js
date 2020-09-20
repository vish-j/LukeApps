$("span.possiblefiles").each(function (i, obj) {
    var keys = $(obj).text().split(",");
    var html = "";
    for (var i in keys) {
        var key = keys[i].trim();
        if (isGuid(key) === true) {
            html = html + ' <a href="' + downloadUrl + key + '" class="btn btn-xs btn-default">File Download</a>';
        } else {
            console.log('This is not a valid GUID Key.');

            html = ((html === "") ? '' : html + ', ') + key;
        }
    }
    $(obj).html(html);
});

function isGuid(stringToTest) {
    if (stringToTest[0] === "{") {
        stringToTest = stringToTest.substring(1, stringToTest.length - 1);
    }
    var regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
    return regexGuid.test(stringToTest);
}