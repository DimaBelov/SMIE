$(document).ready(function() {
    console.log('personal');

    $('#user-name').text(window.user.username);
    $('#user-email').text(window.user.email);
});