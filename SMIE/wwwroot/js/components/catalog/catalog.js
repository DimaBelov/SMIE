function openVideo(id) {
    if (!window.user) {
        console.log('not auth');
        UIkit.modal(document.getElementById('not-authenticated-dialog')).show();
        return;
    }

    console.log('open');
    window.open(`/Video/${id}`, '_self');   
}

$(document).ready(function () {
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