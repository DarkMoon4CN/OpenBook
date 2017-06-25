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
                        $("#divimglogo").removeClass("hide").append("<img class=\"ui-imglazyload preload\" data-url='" + themepicpath + "' data-id=\"img1\" data-attr=\"attr\"/>");
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
        },
        beforeSend: function (xhr) {
           
        },
        complete: function (xhr, status) {
            
        },
        error: function () {
           
        }
    });
    
    $.post(_root + "handlers/NewsController/NewsBrowserCounter.ashx", { "pid": $("#content_hidGlobalGUID").val(), "ts": new Date().getTime() }, function (data) { });


    $("#divMoblieShareHeader").click(function () {
        window.location.href = "http://www.kjrili.com/app/download.html";
    });
});

$(function () {  
    $('#gotop').gotop();     
});

function initLazyload(logoImg, contentImgs) {
    $('.ui-imglazyload').on('startload', function () {
        $(this).removeClass('preload');
    }).imglazyload();

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

    $.get("/handlers/LogController/CountNewsPageView.ashx", { "chn": $.getUrlParam('chn'), "share": $.getUrlParam('s'), "browse": $.getUrlParam('b'), "token": $.getUrlParam('t'), "sharetype": $.getUrlParam('st'), "e": e, "ts": new Date().getTime() }, function (data) { });
});

Zepto(document).ready(function ($) {
    $("#good").click(function (e) {
        if ($("#good").attr("isclicked") == undefined || $("#good").attr("isclicked") != "1")
        {


            $("#good").attr("isclicked", "1");

            $('#good .comment-like_icon').addClass('nice-in').removeClass('comment-like_icon');
            var $span = $("#good span"), $b = $("<b>").text("+1"), n = parseInt($span.text());
            var left = parseInt($(this).offset().left) + 8, top = parseInt($(this).offset().top) - 17;
            $b.css({
                "bottom": 0,
                "z-index": 999,
                'color': '#ff9f37',
                'left': left + 'px',
                'top': top + 'px',
                'position': 'absolute'
            });
            $span.text(n + 1);
            $("#good").append($b);
            $b.animate({ "bottom": 100, "opacity": 0 }, 1000, function () { $b.remove(); });
            var d = setInterval(function () {
                clearInterval(d);
                if ($("#good b").length == 1) {
                    $.post("", { zan: $span.text() })
                }
            }, 1000)
            e.stopPropagation();
        }
        


    });
});


