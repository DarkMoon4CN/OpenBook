<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxControllerTest.aspx.cs" Inherits="Mars.Server.WebApp.Test.AjaxControllerTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="../js/plugin/jquery-1.9.1.min.js"></script>
      <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
     <script type="text/javascript"> var _root = '/'; </script>
     <script type="text/javascript">

         $(function () {
             getTitle();
         });

         var getTitle = function () {

             $.post(_root + "handlers/TestController/QueryTitleById.ashx", { id: 15, ts: new Date().getTime() }, function (data) {
                 var json = eval("(" + data + ")");

                 var divtitle = $("#divtitle");
                 divtitle.html("通过AJAX调用<br/>" + "姓名：" + json[0]["name"] + "  个人简介：" + json[0].desc);
             });
         }
      </script>
</head>
<body>
    <form id="form1" runat="server">

        <br />
         <input type="text" name="txtStartTime"  class="Wdate"  onclick="WdatePicker();" />
        <br />
    <div>
      <div id="divtitle"></div>
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
        <asp:Image ID="imgWater"  ImageUrl="/image/pp.jpg" runat="server" />
        <br />
        指定键：   <asp:TextBox ID="txtWaterKey" runat="server"></asp:TextBox>
        <asp:Button ID="Button3" runat="server" Text="获取水印" OnClick="Button3_Click" />
        <br /><br />

        图片缩略图<br />
         <asp:Image ID="imgMogrify" ImageUrl="/image/pp.jpg" runat="server" />
         <asp:TextBox ID="txtMogrify" runat="server"></asp:TextBox>
        <asp:Button ID="Button4" runat="server" Text="获取水印" OnClick="Button4_Click" />
    </div>
    </form>
</body>
</html>
