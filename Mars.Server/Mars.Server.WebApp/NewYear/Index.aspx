<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mars.Server.WebApp.NewYear.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="description" content="定制您个人专属春联，分享给朋友，送上您的祝福。" />
    <meta name="apple-mobile-web-app-status-bar-style" content="white" />
    <link rel="shortcut icon" href="../images/logo/favicon.ico" />
    <link rel="stylesheet" href="../css/share-reset.css" />
    <link rel="stylesheet" href="../css/newYear.css" />
    <title>迎新年 晒春联</title>
    <script type="text/javascript"> var _root = '<%= Mars.Server.Utils.WebMaster.WebRoot %>'; </script>
</head>
<body>
    <div class="newYear-box">
        <figure class="top">
            <img src="../images/newyear/top.png" alt="" />
        </figure>
        <figure class="juanzhou">
            <img src="../images/newyear/juanzhou-top.png" alt="" />
        </figure>
        <div class="content-box">
            <div class="menu-box">
                <ul class="menu">
                    <li class="actived-menu">选春联</li>
                    <li>写春联</li>
                </ul>
                <div style="clear:both;"></div>
                <div class="tab">
                    <div class="choose-box box" style="display: block;">
                        <div class="house">
                            <div class="couplet-box">
                                <div class="horizontal couplet">
                                    <div class="cssword" id="div_h">
                                        猴年大吉
                                    </div>
                                </div>
                                <div class="left couplet">
                                    <div class="cssword" id="div_d">
                                        金猴贺岁家兴旺
                                    </div>
                                </div>
                                <div class="right couplet">
                                    <div class="cssword" id="div_u">
                                        富贵平安福满堂
                                    </div>
                                </div>
                                <div class="happniess-img">
                                    <img class="happniess-img-info" src="../images/newyear/happiness.png" />
                                </div>
                            </div>
                        </div>
                        <div class="arrow_l" id="btn_before" onclick="javascript:changeCouplet(false);">
                            <span class="arrow"></span>
                        </div>
                        <div class="arrow_r" id="btn_next" onclick="javascript:changeCouplet(true);">
                             <span class="arrow"></span>
                        </div>                        
                    </div>
                    <div class="write box">
                        <div class="house">
                            <div class="couplet-box">
                                <div class="horizontal-red couplet-red">
                                    <div class="csswordw" id="hengpi">
                                    </div>
                                </div>
                                <div class="left-red couplet-red">
                                    <div class="csswordw">
                                    </div>
                                </div>
                                <div class="right-red couplet-red">
                                    <div class="csswordw">
                                    </div>
                                </div>
                                <div class="happniess-img">
                                    <img class="happniess-img-info" src="../images/newyear/happiness.png" />
                                </div>
                            </div>
                        </div>
                        <div class="write-couplet">
                            <p>上联：<input type="text" name="" placeholder="输入长度不可超过9个字" id="txt_up_w" /></p>
                            <p>下联：<input type="text" name="" placeholder="输入长度不可超过9个字" id="txt_down_w" /></p>
                            <p>横批：<input type="text" name="" placeholder="" id="txt_hiz_w" /></p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="happiness-box">
                <ul class="happiness-img_box">
                    <li id="li_fu"></li>
                </ul>
                <div style="clear: both;"></div>
                <button class="left-arrow small-arrow" id="pre-icon" onclick="javascript:changeImage(false);"></button>
                <button class="right-arrow small-arrow" id="next-icon" onclick="javascript:changeImage(true);"></button>
            </div>
            <div class="share-btn btns">
                <a href="javascript:" class="share" style="display: block;" id="share" onclick="javascript:shareMyCouplet();">生成</a>
                <a href="javascript:" class="share" id="preview" onclick="javascript:previewMyCouplet();">生成</a>            
            </div>
            <div style="clear:both;"></div>
            <div class="instruction">
                <div class="instruction-text">
                    <p style="text-align: center; font-size: 16px;">活动细则</p>
                    <p>1、春节期间（2月1日——2月15日），在活动页面定制您个人专属春联，分享给朋友，送上您的祝福，让朋友们即使在归家的途中也能感受到满屏的暖意和喜气。</p>
                    <p>2、选春联：通过左右键滑动，在热门备选春联中挑选喜欢的春联内容，并配合一副福字，搭配出您个人专属风格页面，点击晒一下分享到朋友圈、微博等平台。</p>
                    <p>3、写春联：饭是家乡好，月是故乡明，春联自己写的才够年味。利用写春联功能，创造一副最展现自己文采的春联分享给朋友，享受被大家点赞的乐趣吧。</p>
                </div>
            </div>
            <div class="copy">
                <p><a href="http://www.kjrili.com/app/index.html">活动之外更精彩，请下载开卷日历APP</a></p>
            </div>
        </div>
        <figure class="juanzhou">
            <img src="../images/newyear/juanzhou-top.png" alt="" />
        </figure>
    </div>
        <div class="share-lead">
            <img src="../images/newyear/share-lead.png" />
        </div>
    <input type="hidden" id="hid_coupletlist" runat="server" />
    <input type="hidden" id="hid_fuImagelist" runat="server" />
    <input type="hidden" id="hid_coupletid" runat="server" value="1" />
    <input type="hidden" id="hid_imgpage" value="1" />
    <input type="hidden" id="hid_imgid" runat="server" />

    <input type="hidden" id="hid_ts" value="<%=ts %>" />
    <input type="hidden" id="hid_ns" value="<%=ns %>" />
    <input type="hidden" id="hid_sign" value="<%=sign %>" />
    <input type="hidden" id="hid_domain" value="<%=domain %>" />
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <OpenBook:OBScript runat="server" ID="common_js" Src="~/js/common.js" ScriptType="Javascript" />
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
    <script src="../js/newyearindex.js" type="text/javascript"></script>
</body>
</html>
