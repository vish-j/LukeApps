var IndexConfig = {
    ModalDetailPage: 'Details',
    ButtonSelector: '#dtBtnSection',
    dtConfig: {
        dom: "<'row'<'#dtBtnSection.col-sm-9'l><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        scrollY: 550,
        scrollX: true,
        deferRender: true,
        scroller: true,
        lengthChange: false,
        "processing": true,
        columnDefs: [
            {
                "targets": [-1],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [-2],
                "visible": false
            }
        ],
        select: {
            style: 'single'
        },
        order: [0, 'desc'],
        "rowCallback": function () {
        }
    },
    modalCallback: function () {        
    },
};

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function showmodal(id) {
    console.log("showing modal...");
    var urlValue = buildRoute(IndexConfig.ModalDetailPage, id);
    showProgress();

    $.ajax({
        async: true,
        type: "GET",
        cache: false,
        url: urlValue
    }).done(function (data, textStatus, jqXHR) {
        console.log("pushing previous url to history...");
        var url = buildRoute("", id);
        window.history.pushState({ id: id }, url, url);
        $('#viewContainer').html(data);
        $('#viewModal').modal('show');
        IndexConfig.modalCallback(id);
        console.log('success');
    }).fail(function () {
        $.notify('ID ' + id + 'does not exist', { type: 'warning' });
    });
}

function buildRoute(action, id) {
    console.log("starting to build route...");
    var routeStrings = rootURL.split('/');

    var route = "";
    var params = "";
    for (var i in routeStrings) {
        if (i > 0) {
            if (routeStrings[i].indexOf('?') !== -1) {
                var s = routeStrings[i].split('?');
                route += '/' + s[0];
                params = s[1];
            }
            else {
                route += '/' + routeStrings[i];
            }
        }
    }

    if (action === "")
        route = route.replace(/\/$/, "");
    else
        route += action;

    var url = "";

    if (params !== "")
        if (id === "")
            url = route + "?" + params;
        else
            url = route + "?id=" + id + "&" + params;
    else
        url = route + "?id=" + id;

    console.log(url);
    return url;
}

var spinnerVisible = false;

function showProgress() {
    if (!spinnerVisible) {
        $("div#spinner").fadeIn();
        spinnerVisible = true;
    }
    setTimeout(function () {
        $("#spinner span").fadeOut(function () {
            $(this).text("Please Wait");
        }).fadeIn();
    }, 3000);
}