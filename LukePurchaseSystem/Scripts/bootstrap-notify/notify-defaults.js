$.notifyDefaults({
    // settings
    element: 'body',
    position: null,
    type: "info",
    allow_dismiss: true,
    newest_on_top: true,
    showProgressbar: false,
    placement: {
        from: "top",
        align: "right"
    },
    offset: 20,
    spacing: 10,
    z_index: 3000,
    delay: 15000,
    timer: 1000,
    url_target: '_blank',
    mouse_over: 'pause',
    animate: {
        enter: 'animated fadeInRight',
        exit: 'animated fadeOutRight'
    },
    onShow: null,
    onShown: null,
    onClose: null,
    onClosed: null,
    icon_type: 'class'
});