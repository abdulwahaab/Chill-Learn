
    FB.init({
        appId: '565023847660764', // App ID
        channelUrl: '//' + window.location.hostname + '/channel', // Path to your
        scope: 'email', // This to get the user details back
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });
    FB.Event.subscribe('auth.statusChange', OnLogin);

function OnLogin(response) {
    if (response.authResponse) {
        FB.api('/me?fields=email', LoginFacebokk);
    }
}
function LoginFacebokk(me) {
    var objModal = {};
    if (me.email) {
        objModal.Email = me.email;
        $.ajax({
            data: JSON.stringify(objModal),
            type: "POST",
            url: "/Account/LoginFacebook",
            datatype: "Json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data === "True") {
                    window.location.href = "/";
                } else {
                    alert("not registered with facebook! " + data);
                }
            },
            error: function (_xhr, _ajaxOptions, _thrownError) {
                alert("error");
            }
        });
    }
}