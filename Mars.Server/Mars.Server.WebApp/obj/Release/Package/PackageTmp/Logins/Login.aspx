<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Mars.Server.WebApp.Logins.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录 - [信息发布平台--控制台]</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- basic styles -->
    <OpenBook:OBScript runat="server" ID="bootstrap" Src="~/css/bootstrap.min.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="bootstrap_responsive" Src="~/css/bootstrap-responsive.min.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="font_awesome" Src="~/css/font-awesome.min.css" ScriptType="StyleCss" />
    <!--[if IE 7]>
    <OpenBook:OBScript runat="server" ID="font_awesome_ie7" Src="~/css/font-awesome-ie7.min.css" ScriptType="StyleCss"/>
    <![endif]-->
    <!-- basic styles -->

    <!-- ace styles -->
    <OpenBook:OBScript runat="server" ID="ace" Src="~/css/ace.min.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="ace_responsive" Src="~/css/ace-responsive.min.css" ScriptType="StyleCss" />
    <!--[if lt IE 9]>
    <OpenBook:OBScript runat="server" ID="ace_ie" Src="~/css/ace-ie.min.css" ScriptType="StyleCss"/>
	<![endif]-->
    <!-- ace styles -->
</head>

<body class="login-layout">

    <div class="container-fluid" id="main-container">
        <div id="main-content">
            <div class="row-fluid">
                <div class="span12">

                    <div class="login-container">
                        <div class="row-fluid">
                            <div class="center">
                                <h1><i class="icon-leaf green"></i><span class="red">信息发布平台</span> <span class="white">[<%=title %>]</span></h1>
                            </div>
                        </div>
                        <div class="space-6"></div>
                        <div class="row-fluid">
                            <div class="position-relative">
                                <div id="login-box" class="visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header lighter bigger">请输入您的登录信息</h4>

                                            <div class="space-6"></div>

                                            <fieldset>
                                                <label>
                                                    <span class="block input-icon input-icon-right">用户名：
                                                            <input type="text" class="span12" placeholder="用户名" id="txtusername"/>
                                                        <i class="icon-user"></i>
                                                    </span>
                                                </label>
                                                <label>
                                                    <span class="block input-icon input-icon-right">密&nbsp;码：
                                                            <input type="password" class="span12" placeholder="密码"  id="txtuserpass"/>
                                                        <i class="icon-lock"></i>
                                                    </span>
                                                </label>
                                                <label>
                                                    <span class="block input-icon input-icon-right">验证码：
                                                            <input type="text" class="span12" placeholder="验证码" id="txtuservalid"/>
                                                        <i class="icon-lock"></i>
                                                    </span>
                                                </label>
                                                <div class="space"></div>
                                                <div class="row-fluid">
                                                    <button onclick="javascript:trylogin('txtusername','txtuserpass','txtuservalid');" class="span12 btn btn-small btn-primary enter" id="btnlogin"><i class="icon-key"></i>登&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;录</button>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <!--/widget-main-->
                                    </div>
                                    <!--/widget-body-->
                                </div>
                                <!--/login-box-->
                            </div>
                            <!--/position-relative-->
                        </div>
                    </div>
                </div>
                <!--/span-->
            </div>
            <!--/row-->
        </div>
    </div>
    <!--/.fluid-container-->

    <input type="hidden"  id="returnUrl" runat="server"/>

    <!-- basic javascript -->
    <OpenBook:OBScript runat="server" ID="jqueryjs" Src="~/js/plugin/jquery.min.js" ScriptType="Javascript" />
    <script type="text/javascript">window.jQuery || document.write("<script src='js/plugin/jquery-1.9.1.min.js'>\x3C/script>");</script>
    <OpenBook:OBScript runat="server" ID="bootstrapjs" Src="~/js/plugin/bootstrap.min.js" ScriptType="Javascript" />
    <script type="text/javascript"> var _root = '<%= Mars.Server.Utils.WebMaster.WebRoot %>'; </script>
    <!-- basic javascript -->

    <!-- page specific plugin scripts -->
    <OpenBook:OBScript runat="server" ID="OBScript1" Src="~/js/adminlogin.js" ScriptType="Javascript" />
    <!-- page specific plugin scripts -->

    <!-- inline scripts related to this page -->

    <script type="text/javascript">
        $(function () {
            document.onkeydown = function (e) {
                if (e.keyCode == 13) {                  
                    var btn = $(".enter");
                    if (btn.length > 0) {
                        var target = $(".enter");
                        target.get(0).click();
                    }
                }
            };
        });
    </script>
</body>
</html>
