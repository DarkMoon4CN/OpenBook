<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="Mars.Server.WebApp.NewYear.view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no"/>
    <meta name="format-detection" content="telephone=no"/>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="apple-mobile-web-app-status-bar-style" content="white"/>
    <title>迎新春，晒春联</title>
    <link rel="shortcut icon" href="../images/logo/favicon.ico" /> 
    <link href="../css/share-reset.css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/newYear.css" />
    <OpenBook:OBScript runat="server" ID="jquery_js" Src="~/js/plugin/jquery-1.11.3.min.js" ScriptType="Javascript" />
    <script type="text/javascript"> var _root = '<%= Mars.Server.Utils.WebMaster.WebRoot %>'; </script>
    <OpenBook:OBScript runat="server" ID="common_js" Src="~/js/common.js" ScriptType="Javascript" />
    <%= wordcss %>
</head>
<body>
    <figure class="top">
            <img src="../images/newyear/top.png" alt="" />
        </figure>
        <figure class="juanzhou">
            <img src="../images/newyear/juanzhou-top.png" alt="" />
        </figure>
    <div class="share-box">
        <div class="house" style="margin:0 auto 0 !important;">
            <div class="couplet-box">
                <div class="<%=wordstyle %> right couplet"><%=item.UpCouplet %></div>
                <div class="<%=wordstyle %> left couplet"><%=item.DownCouplet %></div>
                <div class="<%=wordstyle %> horizontal couplet"><%=item.HorizontalCouplet %></div>
                <div class="happniess-img">
                    <img src="../<%=imageItem.ImageUrl %>"/>
                </div>
                <input type="hidden" id="hid_c" value="<%=cid %>" />
                <input type="hidden" id="hid_i" value="<%=imgType %>" />
                
            </div>                   
         </div>
        <p class="enough cssword">已有<span style="font-style:italic;"><%=shareCount %></span>位书业人士晒出了新年的对联</p>
        <div class="share-btn" id="a_share" onclick="javascript:shareMyCouplet();">
            <a href="javascript:" class="share cssword ">我也要晒</a>
        </div> 
         <div class="copy"></div>
    </div>
    <figure style="margin-top:-11px;">
            <img src="../images/newyear/juanzhou-top.png" alt="" />
        </figure>
    <input type="hidden" id="hid_url" value="<%=shareurl %>" />
    <input type="hidden" id="hid_ts" value="<%=ts %>" />
    <input type="hidden" id="hid_ns" value="<%=ns %>" />
    <input type="hidden" id="hid_sign" value="<%=sign %>" />
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <OpenBook:OBScript runat="server" ID="OBScript1" Src="~/js/common.js" ScriptType="Javascript" />
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
    <script src="../js/newyearpreview.js" type="text/javascript"></script>
    <OpenBook:OBScript runat="server" ID="slog_js" Src="~/js/newYear.js" ScriptType="Javascript" />
    <script type="text/javascript">
        var shareMyCouplet = function(){
            var sloc = window.location.href.indexOf("?");
            if(parseInt(sloc)>0){
                window.location.href="index.aspx?"+window.location.href.substring(parseInt(sloc)+1);
            }else{
                window.location.href="index.aspx";
            }
        }
    </script>
    <%= worddraw %>
</body>

</html>
