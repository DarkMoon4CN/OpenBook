$(function () {
    if ($.getUrlParam('from') != undefined)
    {
        //window.location.href = "http://mp.weixin.qq.com/s?__biz=MzIwMDUyNDkwOQ==&mid=404968559&idx=1&sn=a35b78bdcdb811a2d236fdb93b191ae7&scene=1&srcid=123169LjNXGe1dzrWXfI8Ro5#wechat_redirect";
    }

    var QRCode = function (searchtype) {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            if (searchtype == 1) {
                window.location.href = "exhibitorWebToSearchView";
            } else if (searchtype == 2) {
                window.location.href = "activityWebToSearchView";
            }
        }
        else if (browser.versions.android) {
            window.search_obj.onClickSearch();
        }
    }

    var iphone = $('.iphone-btn');
    iphone.click(function () {
        if (browser.versions.weixin) {
            $('.leadPage').show();
        }else{
            if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                this.href = "https://itunes.apple.com/cn/app/kai-juan-ri-li/id1057442967?mt=8";
            }
            else if (browser.versions.android) {
                this.href = "javascript:"
            } else {
                this.href = "https://itunes.apple.com/cn/app/kai-juan-ri-li/id1057442967?mt=8"
            }
        }

        var chn = "";
        if ($.getUrlParam('chn') != undefined) {
            chn = $.getUrlParam('chn')
        } else if ($.getUrlParam('channel') != undefined) {
            chn = $.getUrlParam('channel')
        }
        $.get("/handlers/LogController/CountDownloadClick.ashx", { "chn": chn, "share": $.getUrlParam('share'), "systype": "ios", "ts": new Date().getTime() }, function (data) { });
    });
    $('.leadPage').click(function () {
        $(this).hide();
    });
    var btn = $('.android');
    btn.click(function () {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            this.href = "javascript:";
        }
        else if (browser.versions.android ) {
            this.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.openbook.mars"
        } else {
            this.href = "http://android.myapp.com/myapp/detail.htm?apkName=com.openbook.mars"
        }
        var chn = "";
        if ($.getUrlParam('chn') != undefined) {
            chn = $.getUrlParam('chn')
        } else if ($.getUrlParam('channel') != undefined) {
            chn = $.getUrlParam('channel')
        }
        $.get("/handlers/LogController/CountDownloadClick.ashx", { "chn": chn, "share": $.getUrlParam('share'), "systype": "android", "ts": new Date().getTime() }, function (data) { });
    });
});