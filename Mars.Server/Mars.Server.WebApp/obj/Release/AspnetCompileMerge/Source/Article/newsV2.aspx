<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Article/Newsjquery.Master" CodeBehind="newsV2.aspx.cs" Inherits="Mars.Server.WebApp.Article.newsV2" %>

<asp:Content ContentPlaceHolderID="css" ID="con_css" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="con_script" runat="server">
    <OpenBook:OBScript Src="~/js/plugin/jquery.lazyload.min.js" runat="server" ID="lazyload_js" ScriptType="Javascript" />
    <OpenBook:OBScript Src="~/js/newscomment.js" runat="server" ID="newscomment_js" ScriptType="Javascript" />
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
    <div style="clear: both"></div>
    <section class="comment-box" id="commentlist">
        <div class="comment-title">
            <div class="new-comment">最新评论</div>
        </div>
        <OpenBook:TemplateWrapper ID="tmpEventItemCommentList" runat="server" TemplateSrc="~/Templates/EventItemCommentListTemplate.ascx"
            DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" PageSize="10" UseRequestCache="false" AutoLoadData="false" />
    </section>
    <%--<div id="gotop">
        <img src="../images/lanren_top.jpg" border="0">
    </div>--%>
    <div class="dialog-box" role="alert">
       <div class="dialog-content">
            <p>您确认删除此条评论吗?</p>
            <div class="choose-btn">
                <a href="javascript:" onclick="javascript:deleteComment();" class="sure-a" cid="0">确定</a>
                <a href="javascript:" onclick="javascript:closeDialog();">取消</a>
            </div>
       </div>
    </div>
    <input class="searchpart" type="hidden" key="_eguid" id="hidGlobalGUID" runat="server" />
    <input type="hidden" id="hidNewsPath" runat="server" />

    <input class="searchpart" type="hidden" key="_uid" id="hiduid" value="0" />
    <input class="searchpart" type="hidden" key="_isshare" id="hidshare" value="<%=isShatre %>" />
    <%= moblieShareFooter%>
</asp:Content>

<asp:Content ContentPlaceHolderID="footer" ID="con_footer" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="inlinescripts" ID="con_inlinescript" runat="server">
    <OpenBook:OBScript runat="server" ID="news_js" Src="~/js/newsjq.js" ScriptType="Javascript" />

    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        wx.config({
            debug: false,
            appId: 'wx14b829fae5b27750',
            timestamp: '<%=hid_timestamp%>',
            nonceStr: '<%=hid_nonceStr%>',
            signature: '<%= hid_signature%>',
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
                        alert('分享成功');
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                        alert('分享失败');
                    }
                });
                wx.error(function (res) {
                });
            });

    </script>
</asp:Content>
