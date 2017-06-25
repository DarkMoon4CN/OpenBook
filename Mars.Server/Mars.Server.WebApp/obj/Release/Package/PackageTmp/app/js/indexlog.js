(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
})(jQuery);//捕捉参数

$(function () {
    var chn = "";
    if ($.getUrlParam('chn') != undefined) {
        chn = $.getUrlParam('chn')
    } else if ($.getUrlParam('channel') != undefined) {
        chn = $.getUrlParam('channel')
    }
    $.get("/handlers/LogController/CountDownloadPageView.ashx", { "chn": chn, "share": $.getUrlParam('share'), "ts": new Date().getTime() }, function (data) { });
});