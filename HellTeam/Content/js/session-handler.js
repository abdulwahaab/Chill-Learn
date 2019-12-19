$(function () {
    SetKeepSessionAlive();
});

function SetKeepSessionAlive() {
    setTimeout("KeepSessionAlive()", 20000); // every 5 min
}

function KeepSessionAlive() {
    $.get("../sessionhandler.ashx", function (data) {
        SetKeepSessionAlive();
    });
}