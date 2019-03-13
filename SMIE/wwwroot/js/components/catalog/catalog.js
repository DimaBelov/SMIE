$(document).ready(function() {
    $.get("Catalog/Get")
        .done(function (data) {
            $("#catalog").html(data);
        })
        .fail(function (error) {
            alert("error: " + error);
        });
});