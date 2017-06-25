<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recommend.aspx.cs" Inherits="Mars.Server.WebApp.app.recommend" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="keywords" content="图书人，日历，开卷，书业" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0"/>
    <title>开卷日历12月上线下载</title>
    <link rel="stylesheet" type="text/css" href="/app/css/common.css" />
    <link rel="stylesheet" type="text/css" href="/app/css/online.css" />
    <OpenBook:OBScript runat="server" ID="jqueryjs" Src="~/js/plugin/jquery-1.11.3.min.js" ScriptType="Javascript" />
    <script type="text/javascript"> var _root = '<%= Mars.Server.Utils.WebMaster.WebRoot %>'; </script>
</head>
<body>
    
    <div>
        <input class="" type="text" id="txt_mobile" placeholder="请输入您的手机号码" style="text-align:center;"/>
        <a class="" href="javascript:" id="btn_submit" style="color:black;">绑定手机</a>
    </div>
    <input type="hidden" id="hid_recommendid" value="<%=strRecommendID %>" /> 

    <script src="/app/js/jquery-1.8.3.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            $("#btn_submit").click(function () {
                //alert($("#txt_mobile").val());
                if (checkMobile())
                {
                    var pms = {
                        "_r": $("#hid_recommendid").val(),
                        "_m": $("#txt_mobile").val(),
                        "ts": new Date().getTime()
                    };

                    $.post(_root + "handlers/UserController/WriteNumber.ashx", pms, function (data) {
                        var json = eval("(" + data + ")");
                        if (json.state == "1") {
                            
                        } 
                    });

                }
            });
        });
        
        var checkMobile = function () {
            var re = /^1\d{10}$/;
            if (re.test($("#txt_mobile").val())) {
                return true;
            } else {
                return false;
            }
        }
    </script>
</body>
</html>
