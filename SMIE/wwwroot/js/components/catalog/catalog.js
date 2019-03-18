$(document).ready(function() {
    $.get("Catalog/Get")
        .done(function (data) {
            $("#catalog").html(data);
        })
        .fail(function (error) {
            alert("error: " + error);
        })
        .always(function () {
            removeCatalogSpinner();
        });
    
    function removeCatalogSpinner() {
        var elem = document.getElementById('catalog-spinner');
        elem.parentNode.removeChild(elem);
    }
});