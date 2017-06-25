$.getUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return "";
}

$(function () {
    var length = $(".happiness-img_box li").length;
    function animate(e) {
        var ulWidth = $('.happiness-img_box').width();
    }
    $('.share-lead').click(function () {
        $(this).hide();
    })
    var num = 0;

    loadImg(1);
});

var getUrl = function () {
    var pars = "";
    if ($.getUrlParam('s') != "") {
        if (pars == "") {
            pars += "s=" + $.getUrlParam('s');
        } else {
            pars += "&s=" + $.getUrlParam('s');
        }
    }
    if ($.getUrlParam('m') != "") {
        if (pars == "") {
            pars += "m=" + $.getUrlParam('m');
        } else {
            pars += "&m=" + $.getUrlParam('m');
        }

    }
    if ($.getUrlParam('st') != "") {
        if (pars == "") {
            pars += "st=" + $.getUrlParam('st');
        } else {
            pars += "&st=" + $.getUrlParam('st');
        }

    }
    if ($.getUrlParam('v') != "") {
        if (pars == "") {
            pars += "v=" + $.getUrlParam('v');
        } else {
            pars += "&v=" + $.getUrlParam('v');
        }

    }
    return pars;
    //"s=" + $.getUrlParam('s') + "&m=" + $.getUrlParam('m') + "&st=" + $.getUrlParam('st') + "&v=" + $.getUrlParam('v') + "";
}

var getShareUrl = function () {
    var url = "";
    var c = $("#hid_coupletid").val();
    var i = $("#hid_imgid").val();

    url = $("#hid_domain").val() + "newyear/view.aspx?" + getUrl() + "&c=" + c + "&i=" + i + "";
    return url;
}
var title = "迎新春，晒春联";
var desc = "猴年大吉，来看我创作的春联！";
var imgurl = "http://7xkwie.com2.z0.glb.qiniucdn.com/20160128/d1e3e55a-b473-44cf-99f7-ee3d5121cf34.jpg?imageView2/0/w/96/h/96/interlace/1/format/jpg";
//var url = window.location.href;

wx.config({
    debug: false,
    appId: 'wx14b829fae5b27750', 
    timestamp:$("#hid_ts").val(),
    nonceStr:$("#hid_ns").val(),
    signature:$("#hid_sign").val(),
    jsApiList: [
     'onMenuShareAppMessage',
     'onMenuShareTimeline',
     'onMenuShareQQ',
     'onMenuShareWeibo',
     'onMenuShareQZone',
    ] 
});
wx.ready(function () {
    wx.onMenuShareAppMessage({
        title:title,
        desc:desc,
        link: getShareUrl(),
        imgUrl: imgurl,
        type:'link',
        dataUrl: '',
        success: function () {
            return true;
        },
        cancel: function () {
            // 用户取消分享后执行的回调函数
            return false;
        }
    });
    wx.onMenuShareTimeline({
        title: '猴年大吉，来看我创作的春联！', // 分享标题
        link: getShareUrl(), // 分享链接
        imgUrl: imgurl, // 分享图标
        success: function () { 
            // 用户确认分享后执行的回调函数
            return true;
        },
        cancel: function () { 
            // 用户取消分享后执行的回调函数
            return false;
        }
    });
    wx.onMenuShareQQ({
        title: title, // 分享标题
        desc: desc, // 分享描述
        link: getShareUrl(), // 分享链接
        imgUrl: imgurl, // 分享图标
        success: function () { 
            // 用户确认分享后执行的回调函数
            return true;
        },
        cancel: function () { 
            // 用户取消分享后执行的回调函数
            return false;
        }
    });
    wx.onMenuShareWeibo({
        title: title, // 分享标题
        desc: desc, // 分享描述
        link: getShareUrl(), // 分享链接
        imgUrl: imgurl, // 分享图标
        success: function () { 
            // 用户确认分享后执行的回调函数
            return true;
        },
        cancel: function () { 
            // 用户取消分享后执行的回调函数
            return false;
        }
    });
    wx.onMenuShareQZone({
        title: title, // 分享标题
        desc: desc, // 分享描述
        link: getShareUrl(), // 分享链接
        imgUrl: imgurl, // 分享图标
        success: function () { 
            // 用户确认分享后执行的回调函数
            return true;
        },
        cancel: function () { 
            // 用户取消分享后执行的回调函数
            return false;
        }
    });
});  
wx.error(function (res) {
});

$(function () {
    $('.menu li').click(function () {
        var i = $(this).index();
        $(this).addClass('actived-menu').siblings().removeClass('actived-menu');
        $('.box').eq(i).show().siblings().hide();
        $('.btns a').eq(i).show().css("display","block").siblings().hide();
    });
});

var getCouplet = function (id) {
    var json = eval($("#hid_coupletlist").val());
    for (var i = 0; i < json.length; i++) {
        if (json[i].CoupletID == id) {
            return json[i];
        }
    }
    return json[27];
}

var changeCouplet = function (isNext) {
    var cur = $("#hid_coupletid").val();

    if (isNext) {
        if (cur == 29) {
            cur = 1;
        } else {
            cur = parseInt(cur) + 1;
        }

    } else {
        if (cur == 1) {
            cur = 29;
        } else {
            cur = parseInt(cur) - 1;
        }
    }

    var item = getCouplet(cur);
    $("#div_h").html(item.HorizontalCouplet);
    $("#div_d").html(item.DownCouplet);
    $("#div_u").html(item.UpCouplet);
    $("#hid_coupletid").val(cur);
}



var loadImg = function (pageindex) {
    var ipage = pageindex;
    $("#li_fu img").remove();
    var json = eval($("#hid_fuImagelist").val());
    for (var i = 0; i < json.length; i++) {
        if (i >= ((parseInt(ipage) - 1) * 4) && i < parseInt(ipage) * 4) {
            $("#li_fu").append('<img iid="' + json[i].ImageID + '" src="../' + json[i].ImageUrl + '" alt="" onclick="javascript:setFuImage(this);"/>');
        }
    }
}
var changeImage = function (isNext) {
    var cur = $("#hid_imgpage").val();

    if (isNext) {
        if (cur == 6) {
            cur = 1;
        } else {
            cur = parseInt(cur) + 1;
        }

    } else {
        if (cur == 1) {
            cur = 6;
        } else {
            cur = parseInt(cur) - 1;
        }
    }

    loadImg(cur);
    $("#hid_imgpage").val(cur);
}

var setFuImage = function (obj) {
    $(".happniess-img-info").attr("src", $(obj).attr("src"));
    $("#hid_imgid").val($(obj).attr("iid"));
}



var getPewViewUrl = function () {
    var url = "";
    var c = $("#hid_coupletid").val();
    var i = $("#hid_imgid").val();

    var pars = getUrl();
    if (pars == "") {
        url = $("#hid_domain").val() + "newyear/preview.aspx?c=" + c + "&i=" + i + "";
    } else
    {
        url = $("#hid_domain").val() + "newyear/preview.aspx?" + pars + "&c=" + c + "&i=" + i + "";
    }
    
    return url;
}

var shareMyCouplet = function () {
    window.location.href = getPewViewUrl();  //+"&wri=1";
}

var previewMyCouplet = function () {
    if (checkForm()) {
        var pms = {
            "_u": escape($.trim($("#txt_up_w").val())),
            "_d": escape($.trim($("#txt_down_w").val())),
            "_h": escape($.trim($("#txt_hiz_w").val())),
            "ts": new Date().getTime()
        };

        $.post(_root + "handlers/NewYearController/SubmitCouplet.ashx", pms, function (data) {
            var json = eval("(" + data + ")");
            if (json.state == "1") {
                $("#hid_coupletid").val(json.msg);
                shareMyCouplet();
            }
        });
    }
}
var checkForm = function () {
    var error = "";

    if ($.trim($("#txt_up_w").val()) == "" || $.trim($("#txt_up_w").val()).length > 9) {
        error += "up";
        $("#txt_up_w").addClass("errorinput");
    } else {
        $("#txt_up_w").removeClass("errorinput");
    }

    if ($.trim($("#txt_down_w").val()) == "" || $.trim($("#txt_down_w").val()).length > 9) {
        error += "down";
        $("#txt_down_w").addClass("errorinput");
    } else {
        $("#txt_down_w").removeClass("errorinput");
    }

    if ($.trim($("#txt_hiz_w").val()) == "" || $.trim($("#txt_hiz_w").val()).length > 9) {
        error += "hiz";
        $("#txt_hiz_w").addClass("errorinput");
    } else {
        $("#txt_hiz_w").removeClass("errorinput");
    }

    if (error != "") {
        return false;
    }

    return true;
}
