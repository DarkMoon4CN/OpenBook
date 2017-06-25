$(function () {
    var contentImgs;
    var logoImg;
    $.ajax({
        type: "post",
        url: _root + "handlers/NewsController/GetEventItemContent.ashx",
        data: { "pid": $("#content_hidGlobalGUID").val(), "ts": new Date().getTime() },
        success: function (data) {
                var json = eval("(" + data + ")");       
                if (json.status == "1") {
                    $("#divFirstTypeName").text(json.item.FirstTypName);
                    $("#divtitle").text(json.item.Title);
                    $("#divpublishtime").html($.trim(json.item.PublishTime) + "&nbsp;" +  $.trim(json.item.PublishSource));
                    //$("#divpublishsource").text("来源:" + json.item.PublishSource);
                    if (json.item.ThemePicturePath)
                    {
                        var themepicpath = json.item.Domain + json.item.ThemePicturePath + "?imageView2/0/interlace/1/format/jpeg";
                        $("#divimglogo").removeClass("hide").append("<img class=\"ui-imglazyload preload\" data-url='" + themepicpath + "' data-original='" + themepicpath + "' data-id=\"img1\" data-attr=\"attr\"/>");
                        logoImg = themepicpath;
                    }
                    
                    if (json.item.ThemePicturePath && json.item.ActiveTimeDesc) //(json.item.ActiveTimeDesc && json.item.ActivePlace)
                    {                       
                        $("#divactivetime").removeClass("hide").text("时间：" + json.item.ActiveTimeDesc);                      
                    }
                    if(json.item.ActiveTimeDesc == "4000-01-01至4000-01-01")
                    {
                        $("#divactivetime").hide();
                    }

                    if (json.item.ThemePicturePath && json.item.ActivePlace)
                    {                       
                        $("#divactiveplace").removeClass("hide").text("地址：" + json.item.ActivePlace);
                    }

                    $("#divcontent").html(json.item.Html);
                    contentImgs = document.getElementById("divcontent").getElementsByTagName("img");
                    $("#divMoblieShareHeader").show();
                    $("#divMoblieShareFooter").show();
                }
                else {
                    $("#divcontent").html("<p style='padding-bottom:15px;'>当前文章不存在或已删除！</p>");
                }
                initLazyload(logoImg,contentImgs);
            //初始化页面参数
                $("#commentlist").show();
                //if (!browser.versions.ios && !browser.versions.iPhone && !browser.versions.iPad && !browser.versions.android) {
                    var obj = TObj("tmpEventItemCommentList");
                    if ($("#hidshare").val() == "1") {
                        obj.paginationType = 0;
                    }
                    obj._prmsData.ts = new Date().getTime();
                    obj.S();
                //}
        },
        beforeSend: function (xhr) {
           
        },
        complete: function (xhr, status) {
            
        },
        error: function () {
           
        }
    });
    
    $.post(_root + "handlers/NewsController/NewsBrowserCounter.ashx", { "pid": $("#content_hidGlobalGUID").val(), "ts": new Date().getTime() }, function (data) { return true; });


    $("#divMoblieShareHeader").click(function () {
        window.location.href = "http://www.kjrili.com/app/download.html";
    });
});

function initLazyload(logoImg, contentImgs) {
    $('.ui-imglazyload').load(function () {
        $(this).removeClass('preload');
        $(this).get(0).src = $(this).attr("data-url");
    });

    $(".ui-imglazyload").lazyload({ effect: "fadeIn" });

    $(".ui-imglazyload").click(function () {
        var selectedImage = $(this).attr('data-url');    
        var myimgeUrl = new Array(contentImgs.length+1);
        var subScript = "?st=";
        var imageListStr = "&img=";
        var isLength = true;
        if (typeof (logoImg) != "undefined") {
            myimgeUrl[0] = logoImg;
        }
        else
        {
            myimgeUrl = new Array(contentImgs.length);
            isLength = false;
        }
        for (var i = 0; i < contentImgs.length; i++) {
            var imgs = contentImgs[i];
            var str = $(imgs).attr('data-url');
            if (isLength) {
                myimgeUrl[i + 1] = str;
            } else
            {
                myimgeUrl[i] = str;
            }
        }
        
        for (var i = 0; i < myimgeUrl.length; i++) {
            if (selectedImage == myimgeUrl[i])
            {
                subScript += i;
            }
            imageListStr += myimgeUrl[i] + "_";
        }
        window.location.href = "/Article/EnlargePic.aspx" + subScript + imageListStr;
    });
}

//$(function () {   
//    var qrcode = new QRCode($("#qrcode")[0], {
//        width: 200,
//        height: 200
//    });
//    qrcode.makeCode($("#content_hidNewsPath").val());  
//});

function showloading()
{
    //if ($(".loading").css("display") == "none")
    //{
    //    $(".loading").show();
    //    return;
    //}
    $(".loading").show();
}

function hideloading() {
    //if ($(".loading").css("display") != "none") {
    //    $(".loading").hide();
    //    return;
    //}
    $(".loading").hide();
}

$(function () {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    var regurl = new RegExp(".*/news/?(1_)?(.*).html.*");
    var e = "";
    if (regurl.test(window.location.href)) {
        e = window.location.href.match(regurl)[2];
    }

    $.get("/handlers/LogController/CountNewsPageView.ashx", { "chn": $.getUrlParam('chn'), "share": $.getUrlParam('s'), "browse": $.getUrlParam('b'), "token": $.getUrlParam('t'), "sharetype": $.getUrlParam('st'), "e": e, "w": $.getUrlParam('w'), "ts": new Date().getTime() }, function (data) { return true; });
});

var likeClick = function (obj,cid,event)
{
    if (!isLogined()) {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            window.location.href = "openbook://gotoLoginController";
        }
        else if (browser.versions.android) {
            window.add_obj.addLanding();
        }
    } else {
        if ($(obj).attr("isclicked") == undefined || $(obj).attr("isclicked") != "1") {
            $.post(_root + "handlers/CommentController/ILikeThis.ashx", { "_id": cid, "_uid": $("#hiduid").val(), "ts": new Date().getTime() }, function (data) {
                var json = eval("(" + data + ")");
                if (json.state == "1") {
                    $(obj).attr("isclicked", "1");

                    $(obj).find('.comment-like_icon').addClass('nice-in').removeClass('comment-like_icon');
                    var $span = $(obj).find('span'), $b = $("<b>").text("+1"), n = parseInt($span.text());
                    $b.css({
                        'color': '#ff9f37',
                        'left': '65' + 'px',
                        'top': '-15' + 'px',
                        'position': 'absolute'
                    });
                    $span.text(n + 1);
                    $span.css({ 'color': '#ec7b00' });
                    $(obj).append($b);
                    $b.animate({ "bottom": 100, "opacity": 0 }, 1000, function () { $b.remove(); });
                    var d = setInterval(function () {
                        clearInterval(d);
                        if ($(obj).find('b').length == 1) {
                            $.post("", { zan: $span.text() })
                        }
                    }, 1000)
                }
                else if (json.state == "-2") {
                    $(obj).attr("isclicked", "1");
                    $(obj).find('.comment-like_icon').addClass('nice-in').removeClass('comment-like_icon');
                    $span.css({ 'color': '#ec7b00' });

                    if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                        window.location.href = "openbook://haveClickTankuang";
                    }
                    else if (browser.versions.android) {
                        window.add_obj.addshowToast();
                    }
                }
            });
        } else {
            if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                window.location.href = "openbook://haveClickTankuang";
            }
            else if (browser.versions.android) {
                window.add_obj.addshowToast();
            }
        }
    }
}

var isLogined = function () {
    if ($("#hiduid").val() == undefined || $("#hiduid").val() < 1) {
        return false;
    } else {
        return true;
    }
}

function b() {
    h = $(window).height();
    t = $(document).scrollTop();
    if (t > h) {
        $('#gotop').show();
    } else {
        $('#gotop').hide();
    }
}
$(document).ready(function (e) {
    b();
    $('#gotop').click(function () {
        $(document).scrollTop(0);
    })
});

$(window).scroll(function (e) {
    b();
})
