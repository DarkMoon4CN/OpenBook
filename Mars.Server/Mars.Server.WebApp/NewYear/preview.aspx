<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="preview.aspx.cs" Inherits="Mars.Server.WebApp.NewYear.preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="white" />
    <meta name="description" content="定制您个人专属春联，分享给朋友，送上您的祝福。" />
    <title>迎新春，晒春联</title>
    <link rel="shortcut icon" href="../images/logo/favicon.ico" /> 
    <link href="../css/share-reset.css" rel="stylesheet"/>
    <link href="../css/newYear.css" rel="stylesheet" />
    <script type="text/javascript"> var _root = '<%= Mars.Server.Utils.WebMaster.WebRoot %>'; </script>
    <%= wordcss %>
</head>
<body>
    <div class="preview">
        <div style="display:none;">
            <img src="../images/newyear/monkey.jpg" />
        </div>
        <div class="share-lead">
            <img src="../images/newyear/share-lead.png" />
        </div>
        <figure class="top">
            <img src="../images/newyear/top.png" alt="" />
        </figure>
        <div class="preview-box" id="div_pre">
            <figure class="juanzhou">
                <img src="../images/newyear/juanzhou-top.png" alt="" />
            </figure>
            <div class="content-box">
                <div class="house" style="margin: -2px auto 0;">
                    <div class="couplet-box">
                        <div class="horizontal couplet">
                            <div class="<%=wordstyle %>" id="div_h_v">
                                <%=item.HorizontalCouplet %>
                            </div>
                        </div>
                        <div class="left couplet">
                            <div class="<%=wordstyle %>" id="div_d_v">
                                <%=item.DownCouplet %>
                            </div>
                        </div>
                        <div class="right couplet">
                            <div class="<%=wordstyle %>" id="div_u_v">
                                <%=item.UpCouplet %>
                            </div>
                        </div>
                        <div class="happniess-img">
                            <img class="happniess-img-info" src="../<%=imageItem.ImageUrl %>" />
                        </div>
                    </div>
                </div>
                <div>
                    <% if (IsView)
                        { %>
                    <p class="enough cssword">已有<span style="font-style:italic;"><%=shareCount %></span>位书业人士晒出了新年的对联</p>
                    <div class="share-btn" id="a_share" onclick="javascript:MyCouplet();">
                        <a href="javascript:" class="share cssword ">我也要晒</a>
                    </div> 
                    <%}
                    else { %>
                    <div class="share-a">
                        <a href="javascript:" class="share cssword" id="return">返回</a>
                        <a href="javascript:" class="share cssword" id="share_w" onclick="javascript:shareMyCouplet();">晒一下</a>
                    </div>
                    
                    <%} %>
                </div>
                <div class="copy">
                    <p><a href="http://www.kjrili.com/app/index.html">活动之外更精彩，请下载开卷日历APP</a></p>
                </div>
            </div>
            <figure class="juanzhou">
                <img src="../images/newyear/juanzhou-top.png" alt="" />
            </figure>
        </div>    
    </div>  
    <input type="hidden" id="hid_url" value="<%=shareurl %>" />
    <input type="hidden" id="hid_ts" value="<%=ts %>" />
    <input type="hidden" id="hid_ns" value="<%=ns %>" />
    <input type="hidden" id="hid_sign" value="<%=sign %>" />
    <input type="hidden" id="hid_lastpage" value="<%=lastpage %>" />
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <OpenBook:OBScript runat="server" ID="common_js" Src="~/js/common.js" ScriptType="Javascript" />
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
    <script src="../js/newyearpreview.js" type="text/javascript"></script>
    <OpenBook:OBScript runat="server" ID="slog_js" Src="~/js/newYear.js" ScriptType="Javascript" />
    <%= worddraw %>
</body>
</html>
