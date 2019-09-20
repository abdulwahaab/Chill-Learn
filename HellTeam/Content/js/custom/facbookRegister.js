//$("document").ready(function () {
    FB.init({
        appId: '2354798001501872', // App ID
        channelUrl: '//' + window.location.hostname + '/channel', // Path to your
        scope: 'first_name,last_name,email', // This to get the user details back
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });
    FB.Event.subscribe('auth.statusChange', OnLogin);
//});

function OnLogin(response) {

    if (response.authResponse) {
        FB.api('/me?fields=email,first_name,last_name', LoadValues);
    }
}
function LoadValues(me) {
    var objModal = {};
    if (me.email) {
        objModal.Email = me.email;
        objModal.FirstName = me.first_name;
        objModal.LastName = me.last_name;
        $.ajax({
            data: JSON.stringify(objModal),
            type: "POST",
            url: "/Account/RegisterFacebook",
            datatype: "Json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                alert("success");
            },
            error: function (_xhr, _ajaxOptions, _thrownError) {
                alert("error");
            }
        });
    }
}