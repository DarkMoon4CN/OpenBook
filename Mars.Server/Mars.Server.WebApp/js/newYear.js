$(function () {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return "";
    }

    $.get(_root + "handlers/NewYearController/ShareCount.ashx", { "s": $.getUrlParam('s'), "m": $.getUrlParam('m'), "st": $.getUrlParam('st'), "v": $.getUrlParam('v'), "c": $.getUrlParam('c'), "i": $.getUrlParam('i'), "ts": new Date().getTime() }, function (data) { return true; });
});