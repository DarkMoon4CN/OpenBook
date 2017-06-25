function getquerystring(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return (r[2]); return null;
}

$(function () {
    document.onkeydown = function (e) {
        if (e.keyCode == 13) {
            var btn = $(".enter");
            if (btn.length > 0) {
                var target = $(".enter");
                invokeClick(target.get(0));
            }
        }
    };
});

function repstr(src, replace, to) {
    if (src == replace) {
        return to;
    }
    return src;
}
function isdef(obj) {
    return !(obj == undefined || obj == null);
}

function is_empty(obj) {
    return isdef(obj) ? obj == "" : true;
}


function deepCopy(a) {
    b = {};
    for (var k in a) {
        b[k] = a[k];
    }
    return b;
}

function json2str(json) {
    var str = "{"
    for (var k in json) {
        if (k != "serdate") {
            str += "\"" + k + "\":\"" + json[k] + "\",";
        }
    }
    str += "\"serdate\":\"" + new Date().getTime() + "\"}"
    return str;
}

function invokeClick(element) {
    if (element.click) element.click();
    else if (element.fireEvent) element.fireEvent('onclick');
    else if (document.createEvent) {
        var evt = document.createEvent("MouseEvents");
        evt.initEvent("click", true, true);
        element.dispatchEvent(evt);
    }
}


function getsearchurl(key, value) {
    if (value.length > 0) {
        return _root + "BookList/" + encodeURIComponent(key) + "_" + encodeURIComponent(value) + ".html";
    } else {
        return "javascript:";
    }
}

function getsearchurl(key, value, para) {
    if (value.length > 0) {
        return _root + "BookList/" + encodeURIComponent(key) + "_" + encodeURIComponent(value) + "_" + para + ".html";
    } else {
        return "javascript:";
    }
}

function $J(str) {
    //return eval("(" + str + ")");
    var _tmpfuc = new Function(" return " + str + ";");
    return _tmpfuc();
}

//function $R(str) {
//    var _tmpfuc = new Function(str);
//    _tmpfuc();
//}

function F(id) {
    return "#Y_" + id;
}
function Y(id) {
    return document.getElementById(id);
}
function TObj(id) {
    var __obj;
    eval("(__obj=obj_" + id + ")");
    return __obj;
}
function j2U(json) {
    if (typeof (json) == 'undefined')
        return "";
    else {
        var tmps = [];
        for (var key in json) {
            tmps.push(key + '=' + json[key]);
        }
        return tmps.join('&');
    }
}

function dateFormat(date, withtime) {
    var d = new Date(date);
    if (d.getFullYear() == 4000) {
        return "永不过期";
    }
    if (withtime == 1) {
        return d.getFullYear() + "-" + (parseInt(d.getMonth()) + 1) + "-" + (parseInt(d.getDay()) + 1) + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    }
    else {
        return d.getFullYear() + "-" + (parseInt(d.getMonth()) + 1) + "-" + d.getDate();
    }
}

function iN(src) {
    if (arguments.length > 1) {
        for (var i = 1; i < arguments.length; i++) {
            if (arguments[i] == src) {
                return true
            }
        }
        return false;
    }
    else
        return true;
}

function contains(array, element) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == element) {
            return true
        }
    }
    return false;
}

function strLike(src) {
    if (arguments.length > 1) {
        for (var i = 1; i < arguments.length; i++) {
            if (src.indexOf(arguments[i]) >= 0) {
                return true
            }
        }
        return false;
    }
    else
        return true;
}

function isnull(v, defaultv) {
    if (is_empty(v)) {
        return defaultv;
    }
    else {
        return v;
    }
}

function isrecommend(val) {
    var returnvalue = "0";
    if (val != undefined) {
        if (val != "" && val != "否") {
            returnvalue = "1";
        }
    }
    return returnvalue;
}

function isNegativeOne(v, defaultv) {
    if (v == "-1") {
        return defaultv;
    }
    else {
        return v;
    }
}

function replaceLast(src, replace) {
    if (src.substring(src.length - 1, src.length) == replace) {
        return src.substring(0, src.length - 1);
    }
    return src;
}

function replaceinfo(src, replacestr, replaceto) {
    return src.replace(replacestr, replaceto);
}

function toMoney(obj) {
    if (obj == null || obj == "0") {
        return "--";
    }
    else {
        return (obj).toFixed(2);
    }
}

function getCheckState(obj, objvalue) {
    if (obj != objvalue) {
        return "checked='checked'";
    }
    else {
        return "";
    }
}

function getCheckStateByTrueFalse(val) {
    if (val) {
        return "checked='checked'";
    }
    else {
        return "";
    }
}

function getCheckState(obj, objvalue,bool) {
    if (bool) {
        if (obj== objvalue) {
            return "checked='checked'";
        }
        else {
            return "";
        }
    } else {
        if (obj != objvalue) {
            return "checked='checked'";
        }
        else {
            return "";
        }
    }
}

function toDateTime(src) {
    var ind = src.indexOf("-", 6);

    if (ind >= 0) {
        return src.substring(0, ind + 3);
    } else {
        return src;
    }
}

//书店比率
function cal_rate(c, total) {
    if (total == null || total == "0") {
        return "--";
    }
    else {
        return (c * 100.0 / total).toFixed(2) + "%";
    }
}

function cal_rate2(c, total) {
    if (total == null || total == "0") {
        return "--";
    }
    else {
        return (c * 100.0 / total).toFixed(1);
    }
}

function cal(c, total, floatcnt) {
    if (total == null || total == "0") {
        return "--";
    }
    else {
        var fc = floatcnt == undefined ? 2 : floatcnt;
        return (c / total).toFixed(fc);
    }
}
var tiptimer;
function s_alert(info, t) {
    var s = "<div class='wintip'><div>" + info + "<br />此窗口将在<span id='sp_time' style='color:red'>" + t + "</span>秒钟后自动<a href='javascript:void(0)' onclick='s_close()'>关闭</a></div></div>";
    $(document.body).append(s);
    //alert(window.screen.width); 
    $(".wintip").css({ "top": document.documentElement.scrollTop + document.body.scrollTop + 300, "left": (document.documentElement.clientWidth - 330) / 2 });
    tiptimer = setInterval(s_interval, 1000);
}
function s_interval() {
    var t = parseInt($("#sp_time").text());
    if (t == 1) {
        s_close();
    }
    else {
        $("#sp_time").text(t - 1);
    }
}
function s_close() {
    clearInterval(tiptimer);
    $(".wintip").remove();
}

function getYears(start, end, len) {
    var rt = [];
    if (len != undefined) {
        for (var i = 0; i < len; i++) {
            rt.push(start - i);
        }
    }
    else {
        rt.push(start);
        if (start >= end) {
            var current = start - 1;
            while (current >= end) {
                rt.push(current);
                current = current - 1;
            }
        }
    }
    return rt;
}

function getMonthes(start, end, len) {
    var rt = [];
    var newtime = start;
    if (len != undefined) {
        rt.push(start);
        for (var i = 0; i < len; i++) {
            newtime = handleNums(newtime - 1, 12);
            rt.push(newtime);
        }
    }
    else {
        rt.push(start);
        if (start >= end) {
            var current = start - 1;
            while (newtime > end) {
                newtime = handleNums(newtime - 1, 12);
                rt.push(newtime);
            }
        }
    }
    return rt;
}
function handleNums(current, tags) {
    var flag = current % 100;
    var year = parseInt(current / 100);
    return flag == 0 ? (year - 1) * 100 + tags : current;
}
function getWeeks(start, end, len) {
    var rt = [];
    var newtime = start;
    if (len != undefined) {
        rt.push(start);
        for (var i = 0; i < len; i++) {
            newtime = handleNums(newtime - 1, 52);
            rt.push(newtime);
        }
    }
    else {
        rt.push(start);
        if (start >= end) {
            var current = start - 1;
            while (newtime > end) {
                newtime = handleNums(newtime - 1, 52);
                rt.push(newtime);
            }
        }
    }
    return rt;
}

function getsubstring(str, len) {
    if (str.length > len) {
        return str.substring(0, len);
    } else {
        return str;
    }
}

function getsubstringandendwith(str, len, word) {
    if (str.length > len) {
        return str.substring(0, len) + word;
    } else {
        return str;
    }
}

function showbookinfo(obj, title, bid) {
    alert(bid + "__" + title);
}

function booktitle(title, isprv) {
    if (isprv == "1") {
        title = "(★)" + title;
    }
    return title;
}

function hadstockshow(info, ishadstock, defstr) {
    if (ishadstock == "1") {
        return info;
    } else {
        return defstr;
    }
}

var getpublishername = function (name) {
    if (name.indexOf(",") >= 0) {
        var names = name.split(",");
        for (i = 0; i < names.length; i++) {
            if (names[i].length > 0) {
                return names[i];
            }
        }
    } else {
        return name;
    }
}

//(function ($) {
//    $.getUrlParam = function (name) {
//        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
//        var r = window.location.search.substr(1).match(reg);
//        if (r != null) return unescape(r[2]); return null;
//    }
//})(jQuery);//捕捉参数

var browser = {
    versions: function () {
        var u = navigator.userAgent, app = navigator.appVersion;
        return {//移动终端浏览器版本信息 
            trident: u.indexOf('Trident') > -1, //IE内核
            presto: u.indexOf('Presto') > -1, //opera内核
            webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
            mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //是否为移动终端
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
            iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
            iPad: u.indexOf('iPad') > -1, //是否iPad
            webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
            weixin: u.toLowerCase().match(/MicroMessenger/i) == "micromessenger"
        };
    }(),
    language: (navigator.browserLanguage || navigator.language).toLowerCase()
};