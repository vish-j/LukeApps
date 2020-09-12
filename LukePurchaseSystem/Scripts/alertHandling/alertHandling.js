function loadAlerts(url) {
    $.ajax({
        async: true,
        type: "GET",
        cache: false,
        url: url,
    }).done(function (partial) {
        $('#toDoList').html(partial);
        if ($('#toDoList > .alertsApp').hasClass('noAlerts')) {
            closeTodoList();
        }
        setToggleColor();
    });
}

function dismissAlert(key) {
    var url = $('#toDoList').attr('data-dismiss-Url');
    $.ajax({
        async: true,
        type: "POST",
        cache: false,
        url: url,
        data: { key: key },
    }).done(function (data, textStatus, jqXHR) {
        $.notify('Task Cleared', notifySettings('success'));
    }).fail(function () {
        $.notify('Error', notifySettings('warning'));
    }).always(function (data, textStatus, jqXHR) {
        deleteList(key);
    });
}

function dismissAll() {
    var url = $('#toDoList').attr('data-dismissAll-Url');
    var keys = $(".alertItem").map(function () {
        return this.id;
    }).get();
    if (keys.length !== 0) {
        $.ajax({
            async: true,
            type: "POST",
            cache: false,
            url: url,
            data: { keys: keys },
        }).done(function (data, textStatus, jqXHR) {
            $.notify('All Tasks Cleared', notifySettings('success'));
        }).fail(function () {
            $.notify('Error', notifySettings('warning'));
        }).always(function (data, textStatus, jqXHR) {
            for (var i in keys) {
                deleteList(keys[i], keys.length - 1 === i ? null : deleteList(keys[i + 1]));
            }
        });
    }
    else {
        $.notify('No Task Found', notifySettings('success'));
    }
}

function deleteList(key, callback) {
    var listElement = $('#' + key);
    var listGroup = listElement.closest("ul");
    listElement.slideUp("normal", function () {
        $(this).remove();
        if (listGroup.children().length === 1) {
            $('<li class="list-group-item" style="display:none">No Tasks</li>').appendTo(listGroup).slideDown("slow");
            $('#toDoList > .alertsApp').addClass('noAlerts');
        }
        setToggleColor();
        if (typeof (callback) !== 'undefined') {
            callback();
        }
    });
}
var mainContent = $('#mainTableContent');
var alertSection = $('#alertSection');
var toDoList = $('#toDoList');
var alertTitle = $('#alertTitle');
var alertToggle = $('#alertToggle');

function toggleListSection() {
    if (alertSection.hasClass('alertOpen')) {
        closeTodoList();
    }
    else {
        openTodoList();
    }
}

function openTodoList() {
    alertSection.removeClass('alertClose');
    alertSection.removeClass('col-md-1');
    mainContent.removeClass('col-md-11');
    alertSection.addClass('alertOpen');
    alertSection.addClass('col-md-3');
    mainContent.addClass('col-md-9');
    alertToggle.html('<span class="glyphicon glyphicon-remove"></span> Close');
    toDoList.show();
    alertTitle.show();
    table.draw();
}

function closeTodoList() {
    alertSection.removeClass('alertOpen');
    alertSection.removeClass('col-md-3');
    mainContent.removeClass('col-md-9');
    alertSection.addClass('alertClose');
    alertSection.addClass('col-md-1');
    mainContent.addClass('col-md-11');
    alertToggle.html('<span class="glyphicon glyphicon-list-alt"></span> Open To Do List');
    toDoList.hide();
    alertTitle.hide();
    table.draw();
    setToggleColor();
}

function setToggleColor() {
    if ($('#toDoList > .alertsApp').hasClass('noAlerts')) {
        alertToggle.removeClass('btn-danger');
        alertToggle.addClass('btn-default');
    }
    else {
        alertToggle.removeClass('btn-default');
        alertToggle.addClass('btn-danger');
    }
}

function notifySettings(t) {
    return {
        type: t,
        delay: 3000,
        timer: 1000,
        placement: {
            from: "bottom",
            align: "right"
        }
    }
}