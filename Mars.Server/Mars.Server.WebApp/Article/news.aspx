<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Article/News.Master" CodeBehind="news.aspx.cs" Inherits="Mars.Server.WebApp.Article.news" %>

<asp:Content ContentPlaceHolderID="css" ID="con_css" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="con_script" runat="server">
    <%-- <OpenBook:OBScript runat="server" ID="imglazdload_js" Src="~/js/plugin/qrcode.js" ScriptType="Javascript" />--%>
</asp:Content>

<asp:Content ID="con_header" runat="server" ContentPlaceHolderID="header">

</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="con_content" runat="server">
    <%= moblieShareHeader%>
    <article>
        <div class="end">
            <p id="divtitle" class="end_title"></p>
            <div id="divpublishtime" class="end_time"></div>
          <%--<div id="divpublishsource" class="end_source"></div>--%>
            <div id="divimglogo" class="end_logo hide">
            </div>
            <div id="divactivetime" class="end_activetime hide"></div>
            <div id="divactiveplace" class="end_activeplace hide"></div>

            <div id="divcontent" class="end_con">
            </div>
        </div>
    </article>
    <div style="clear:both"></div>
    <div id="gotop"></div>
    <input type="hidden" id="hidGlobalGUID" runat="server" />
    <input type="hidden" id="hidNewsPath" runat="server" />
    <%= moblieShareFooter%>
</asp:Content>

<asp:Content ContentPlaceHolderID="footer" ID="con_footer" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="inlinescripts" ID="con_inlinescript" runat="server">
    <OpenBook:OBScript runat="server" ID="news_js" Src="~/js/news.js" ScriptType="Javascript" />
    
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        wx.config({
            debug: false,
            appId:'wx14b829fae5b27750',
            timestamp:'<%=ts%>',
            nonceStr: '<%=ns%>',
           signature: '<%= sign%>',
           jsApiList: [
               'onMenuShareAppMessage',
           ]
   });
   wx.ready(function () {
       wx.onMenuShareAppMessage({
           title: '<%= title %>', // 分享标题
                 desc: '<%= desc %>', // 分享描述
                 link: '<%= link %>', // 分享链接
                 imgUrl: '<%= imgUrl %>', // 分享图标
                 type: 'link',
                 dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                 success: function () {
                   
                 },
                 cancel: function () {
                     // 用户取消分享后执行的回调函数
                 }
             });
             wx.error(function (res) {
             });
         });
    </script>
</asp:Content>
