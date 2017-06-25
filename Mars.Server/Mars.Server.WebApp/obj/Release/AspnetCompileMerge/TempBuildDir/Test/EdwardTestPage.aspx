<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdwardTestPage.aspx.cs" Inherits="Mars.Server.WebApp.Test.EdwardTestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <OpenBook:OBScript runat="server" ID="common_css" Src="~/css/common.css" ScriptType="StyleCss" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        第一次读数据:
        <div class="dataloading"></div>

        滑动读数据
        <div class="scrollingloading"></div>

    <asp:Button ID="btn_test" runat="server" Text="日志测试" OnClick="btn_test_Click" />
        <br />
        <a href="http://itunes.apple.com/cn/app/kai-juan-ri-li/id1057442967?mt=8" >测试url scheme</a>
        <br /><br /><br /><br /><br /><br />
        <a href="tel:13901352058" >测试打电话</a>

        <img src="../images/oberweima.png" />
    </div>
    </form>
    
</body>
</html>
