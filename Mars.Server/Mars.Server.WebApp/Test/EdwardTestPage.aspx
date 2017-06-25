<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdwardTestPage.aspx.cs" Inherits="Mars.Server.WebApp.Test.EdwardTestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <OpenBook:OBScript runat="server" ID="common_css" Src="~/css/common.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="jquery_js" Src="~/js/plugin/jquery-1.11.3.min.js" ScriptType="Javascript" />
    
    <style>
         @font-face 
            {font-family: 'chunlian';
            src: url('../fonts/classes.eot');
            src: url('../fonts/classes.eot?#iefix') format('embedded-opentype'),     
            url('../fonts/classes.woff') format('woff'),     
            url('../fonts/classes.ttf') format('truetype'),     
            url('../fonts/classes.svg') format('svg');  
             }
          .cssword{font-family: 'chunlian';}
    </style>
    <script type="text/javascript">

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        第一次读数据:
        <div class="dataloading"></div>

        滑动读数据
        <div class="scrollingloading"></div>

    <asp:Button ID="btn_test" runat="server" Text="日志测试" OnClick="btn_test_Click" />

        <div class="cssword" id="div_info" style="font-size:xx-large;">
           金猴贺岁家兴旺   富贵平安福满堂   猴年大吉
            天赐平安福禄寿  地生金玉富贵春    吉祥如意
            春满乾坤兜有钱	天增岁月人任性	醉了也是
            和气平添春色蔼	祥光常与日华新	百福人家
        </div>
        <input type="text" id="txt_info" onchange="javascript:writrInfo(this);" onkeyup="javascript:writrInfo(this);" />
        <asp:TextBox ID="txt_word" runat="server" Rows="20" Width="90%" TextMode="MultiLine"></asp:TextBox>
        <asp:Button ID="btn_del" runat="server" Text="去重" OnClick="btn_del_Click" />
        
    </div>
    </form>
    
</body>
</html>
