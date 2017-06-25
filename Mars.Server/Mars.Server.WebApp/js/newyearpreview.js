$(function () {
    $("#return").click(function () {
        if ($("#hid_lastpage").val() != undefined || $("#hid_lastpage").val()!="") {
            window.location.href = $("#hid_lastpage").val();
        }
    });
    $('.share-lead').click(function () {
        $(this).hide();
    })
})

var title = "迎新春，晒春联";
var desc = "猴年大吉，来看我创作的春联！";
var imgurl = "http://7xkwie.com2.z0.glb.qiniucdn.com/20160128/d1e3e55a-b473-44cf-99f7-ee3d5121cf34.jpg?imageView2/0/w/96/h/96/interlace/1/format/jpg";
var shareurl = $("#hid_url").val();

var shareMyCouplet = function () {
    var url = $("#hid_url").val();
    if (browser.versions.weixin) {
        $('.share-lead').show();
    } else {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            window.location.href = 'openbook://share?{"imageUrl":"' + imgurl + '","title":"' + title + '","url":"' + url + '","content":"' + desc + '","activityId":"nycouplet"}';
            //WithImg_title_url_content_activityId_?"+imgurl+"&"+title+"&"+url+"&"+desc+"&nycouplet";
        }
        else if (browser.versions.android) {
            window.add_obj.openShare(title, desc, imgurl, url, 'nycouplet');
        } else {
            //alert('web click');
        }
    }
}

var MyCouplet = function () {
    var sloc = window.location.href.indexOf("?");
    if (parseInt(sloc) > 0) {
        window.location.href = "index.aspx?" + window.location.href.substring(parseInt(sloc) + 1);
    } else {
        window.location.href = "index.aspx";
    }
}

wx.config({
    debug: false,
    appId: 'wx14b829fae5b27750',
    timestamp: $("#hid_ts").val(),
    nonceStr: $("#hid_ns").val(),
    signature: $("#hid_sign").val(),
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
        title: title,
        desc: desc,
        link: shareurl,
        imgUrl: imgurl,
        type: 'link',
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
        link: shareurl, // 分享链接
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
        link: shareurl, // 分享链接
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
        link: shareurl, // 分享链接
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
        link: shareurl, // 分享链接
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