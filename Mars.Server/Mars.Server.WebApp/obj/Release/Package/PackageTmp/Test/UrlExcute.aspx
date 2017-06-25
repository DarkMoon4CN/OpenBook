<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UrlExcute.aspx.cs" Inherits="Mars.Server.WebApp.Test.UrlExcute" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
  
</head>
<body>
    <form id="form1" runat="server">
        <div id="content" runat="server">
        </div>
        <br />
        <br />
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="上传" OnClick="Button1_Click" />
            <br />
            <br />


            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:TextBox ID="txtKey" runat="server"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="删除图片" OnClick="Button2_Click" />

            <br />
            <br />
            图片水印
        <br />
            <asp:Image ID="imgWater" ImageUrl="/image/pp.jpg" runat="server" />
            <br />
            指定键：  
            <asp:TextBox ID="txtWaterKey" runat="server"></asp:TextBox>
            <asp:Button ID="Button3" runat="server" Text="获取水印" OnClick="Button3_Click" />
            <br />
            <br />

            图片缩略图<br />
            <asp:Image ID="imgMogrify" ImageUrl="/image/pp.jpg" runat="server" />
            <asp:TextBox ID="txtMogrify" runat="server"></asp:TextBox>
            <asp:Button ID="Button4" runat="server" Text="获取水印" OnClick="Button4_Click" />
        </div>
        <br /><br />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="邮件服务开启" />
        <asp:Button ID="Button6" runat="server" OnClick="Button6_Click" Text="邮件服务关闭" />
    </form>
</body>
</html>
