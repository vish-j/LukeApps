function gotoAuditTrail() {
    var query = window.location.search;
    query = query.replace(new RegExp('&', 'g'), '%26');
    window.open(auditTrailURL + "?returnUrl=" + window.location.origin + window.location.pathname + query, "_self")
}