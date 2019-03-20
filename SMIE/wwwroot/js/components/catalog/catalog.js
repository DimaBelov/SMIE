function openVideo(id) {
    if (!window.user) {
        console.log('not auth');
        UIkit.modal(document.getElementById('not-authenticated-dialog')).show();
        return;
    }

    console.log('open');
    window.open(`/Video/${id}`, '_self');   
}

function videoPosterMouseEnter(id) {
    console.log(id);
    UIkit.modal(document.getElementById(`video-tooltip-${id}`)).show();
}

function videoPosterMouseLeave(id) {
    console.log(id);
    UIkit.modal(document.getElementById(`video-tooltip-${id}`)).hide();
}

$(document).ready(function () {
    $.get("Catalog/Get")
        .done(function (data) {
            $("#catalog").html(data);
        })
        .fail(function (error) {
            console.log(error);
            var message = error.responseJSON != null ? error.responseJSON.error : error.responseText;
            UIkit.notification({ message: message, pos: 'top-right', status: 'danger', timeout: 0 });
        })
        .always(function () {
            removeCatalogSpinner();
        });
    
    function removeCatalogSpinner() {
        var elem = document.getElementById('catalog-spinner');
        elem.parentNode.removeChild(elem);
    }
});