var validdata = function (txtid, pwdid, validid) {
    var errstr = "";
    if ($("#" + txtid).val() == "") {
        errstr += "请输入用户名 \n\r";
    }

    if ($("#" + pwdid).val() == "") {
        errstr += "请输入密码 \n\r";
    }

    if ($("#" + validid).val() == "") {
        errstr += "请输入验证码 \n\r";
    }

    if (errstr != "") {
        alert(errstr);
        return false;
    }
    else
        return true;
};

var trylogin = function (txtid, pwdid, validid) {
    if (validdata(txtid, pwdid, validid)) {
        var btnid = "btnlogin";
        var info = { "un": $("#" + txtid).val(), "pw": $("#" + pwdid).val(), "va": $("#" + validid).val(),"url":$("#returnUrl").val(), "ts": new Date().getTime() };

        try {
            $.ajax({
                type: "post",
                url: _root + "handlers/AdminController/AdminLogins.ashx",
                data: info,
                beforeSend: function (XMLHttpRequest) {
                    $("#" + btnid).attr("disabled", "disabled");
                },
                success: function (result, textStatus) {
                    var json = eval("(" + result + ")");                  

                    if (json["state"] == "-9") {//账户名或密码错误
                        alert("传递参数不完整");
                        $("#" + btnid).removeAttr("disabled");
                    }
                    else if (json["state"] == "-8") {
                        alert("验证码输入不正确。");
                        $("#" + btnid).removeAttr("disabled");
                    }
                    else if (json["state"] == "-7") {
                        alert("您未被授权使用本系统。");
                        $("#" + btnid).removeAttr("disabled");
                    }
                    else if (json["state"] == "-6") {
                        alert("您未被分配任何功能使用权限。");
                        $("#" + btnid).removeAttr("disabled");
                    }
                    else if (json["state"] == "-1") {
                        alert("用户名或密码输入不正确。");
                        $("#" + btnid).removeAttr("disabled");
                    }
                    else if (json["state"] == "1") {
                        if (json["url"])
                        {
                            location.href = json["url"];
                        }
                        else
                        {
                            location.href = "../Default.aspx";
                        }                       
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    //alert(textStatus);
                    if (textStatus != "success") {
                        $("#" + btnid).removeAttr("disabled");
                    }
                },
                error: function () {
                    alert("系统请求服务异常");
                }
            });
        }
        catch (e) {
            alert("系统异常");
        }
    }
}
