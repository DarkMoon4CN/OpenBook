<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="Details.aspx.cs" Inherits="Mars.Server.WebApp.SignBook.Details" %>
<%@ MasterType VirtualPath="~/Common.Master" %>
<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript runat="server" ID="bootstrap" Src="~/app/css/style.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
     <section class="wrap" style="min-width:15%;width:100%;">
		<!--header-->
		<header class="invitation-header">
			<figure class="invitation-header-img">
				<img src="../app/img/invitation.gif" alt="invitation-header-img"/>
			</figure>
			<p class="invitation-header-title infor">2016全国书店经理人年会</p>
		</header>
		<!--main-->
         <input  type="hidden" id="hsid" value="<%=sid %>"/>
		<section class="invitation-main">
			<p class="invitation-main-title">尊敬的 <span class="guest infor" style="width:140px" id="customer"></span> 老师：</p>
			<article class="invitation-content">
				  <p>欢迎您参加第15届全国书店经理人年会。</p> 
       			  <p>您的参会编号是 <span class="guest-num infor" id="luckNumber">001</span>，以下是您的签到二维码，也是本届会议现场签到的唯一凭证。请提前保存到手机以便顺利参会。期待您的光临！</p>
				  <p class="infor">时间：2016年1月6日14:00-16:30（13:00开始签到） </p>
				  <p class="infor">地点：北京亮马河大厦会议中心 &bull; 万黛大厅 </p>
				  <p class="invite-company invite">北京开卷信息技术有限公司</p>
				  <p class="invite-time invite">2015年12月</p>
			</article>
		</section>
		<div class="footer-con">
			<figure class="app_ewm">
				<img  style="BORDER-BOTTOM: #ffcccc 4px solid; BORDER-LEFT: #ffcccc 4px solid; BORDER-TOP: #ffcccc 4px solid; BORDER-RIGHT: #ffcccc 4px  solid" id="qrCode" />
			</figure>
			<p>
				出示上方二维码，轻松签到
			</p>
		</div>
	</section>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">  
        $(function () {
            //加载总页数
            var hsid = $("#hsid").val();
            if (hsid == null || hsid == "")
            {
                return false;
            }
            $.post(_root + "handlers/SignBookController/GetInfo.ashx", { "sid":hsid,"ts": new Date().getTime() }, function (data) {
                var json = $.parseJSON(data);
                if (data != "false")
                {
                    $("#customer").html(json.Company + "  " + json.Customer);
                    $("#luckNumber").html(json.LuckyNumber);
                    $("#qrCode").attr("src",  json.SignURL);
                }
            });
        });

    </script>
</asp:Content>