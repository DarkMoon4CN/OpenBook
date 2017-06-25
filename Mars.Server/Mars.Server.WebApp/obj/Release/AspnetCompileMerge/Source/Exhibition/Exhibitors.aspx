<%@ Page Title="" Language="C#" MasterPageFile="~/Exhibition/Exhibition.Master" AutoEventWireup="true" CodeBehind="Exhibitors.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition.Exhibitors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="swiper_css" Src="~/css/swiper.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="autocomplete_css" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.css" ScriptType="StyleCss" />
    <style type="text/css">
        body{position:relative;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="autocomplete_js" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="jqueryjs" Src="~/js/plugin/underscore.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="pinchzoom_js" Src="~/js/plugin/pinchzoom.js" ScriptType="Javascript" />
    <%--    <script type="text/javascript">
        $(function () {
            $("#search_exhibitor").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: _root + "handlers/ExhibitionController/GetExhibitorList.ashx",
                        dataType: "jsonp",
                        data: {
                            top: 5,
                            key: request.term,
                            type: 1,
                            exhibitionid: $("#hid_exhibitionid").val()
                        },

                        success: function (data) {
                            response($.map(data.exhibitors, function (item) {
                                return {
                                    label: item.name, value: item.name
                                }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    $("#search_exhibitor").val(ui.item.value);
                    searchExhibitors();
                }
            });

            $("#search_exhibitor").keydown(function () {
                if (event.keyCode == "13") {//keyCode=13是回车键
                    searchExhibitors();
                }
            });
        });

        var searchExhibitors = function () {
            var obj = TObj("holder");
            obj._prmsData._exhibitorname = escape($("#search_exhibitor").val());
            obj._prmsData._pageIndex = 0;
            if (obj.paginationType < 2) {
                obj.loadData();
            }
            else {
                obj.InitscrollPagination();
            }
        }
    </script>--%>
    <script type="text/javascript">
        $(function () {
            $('.distribution a').click(function () {
                $(this).addClass('blue_text').siblings().removeClass('blue_text');
                $('body').addClass('opacityBg');
                $('.face').css({ 'display': 'none' });
                $('.closeBtn').show();
                $('.mapBox').show();
                $('.lazyLoad-img').css({ 'display': 'block' });
                $(".pinch-zoom").show();
                $(".tmpimg").get(0).src = $(this).attr("imgsrc");
            });
            $('.closeBtn').click(function () {
                $('body').removeClass('opacityBg');
                $('.face').css({ 'display': 'block' });
                $('.mapBox').hide();
                $('.closeBtn').hide();
                $('.lazyLoad-img').css({ 'display': 'none' });
                $(".tmpimg").get(0).src = $(this).attr("");
                
            });
            $('.pinch-zoom').each(function () {
                new RTP.PinchZoom($(this), {});
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <p class="closeBtn">×</p>
    <div class="lazyLoad-img">
        <img src="http://7xkwie.com2.z0.glb.qiniucdn.com/20151224/4c35c79f-883d-4678-916d-3ac68e4bbe7d.jpg" />
    </div>
    <div class="mapBox">
        <div class="pinch-zoom" id="imgdiv1">
            <img class="tmpimg" src="" alt="" />
        </div>
    </div>
    <!--search-->
    <div class="con-main face">
         <div class="search" id="search_exhibitor" onclick="javascript: openSearch(1);">输入展商名称</div>
    </div>
    <div class="distributionBox face">
        <h1>2016北京图书订货会展馆分布图</h1>
        <div class="distribution">
            <a href="javaScript:void(0)" id="d_0" class="blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/58f3c5b7-b60e-43a1-ab23-6f47d4604be3.jpg">1号馆一层</a>
            <a href="javaScript:void(0)" id="d_1" class="blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/53496d9c-995e-4f22-8ed5-735401c7c2b5.jpg">1号馆二层</a>
            <a href="javaScript:void(0)" id="d_2" class="no-border blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/05f783cf-c21b-4bc2-a5b3-0745447a334c.jpg">2、3号馆</a>
            <a href="javaScript:void(0)" id="d_3" class="blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/f15f6258-c14e-4097-aa9b-9401584a5c07.jpg">4、5号馆</a>
            <a href="javaScript:void(0)" id="d_4" class="blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/685f1c98-04aa-4d96-98cb-347e5aa31e18.jpg">6、7号馆</a>
            <a href="javaScript:void(0)" id="d_5" class="no-border blue click-btn" imgsrc="http://7xkwie.com2.z0.glb.qiniucdn.com/20151231/985217aa-9360-4e3e-bedd-6b97a5bafa21.jpg">8号馆</a>
            <div style="clear: both;"></div>
        </div>
    </div>
    <!--search-->
    <section class="main-body face">
        <!--zhanshangPage-->
        <div class="main-sort">
            <%--<div class="latter-box">
				<h1 class="con-main latter">A</h1>
			</div>--%>
            <OpenBook:TemplateWrapper ID="holder" runat="server"
                TemplateSrc="~/Templates/ExhibitorsTemplate.ascx"
                PageSize="10" EnablePagination="true" PaginationType="Scrolling" AutoLoadData="true"
                HttpMethod="Get" DebugMode="true"></OpenBook:TemplateWrapper>
        </div>
    </section>
    <!--main-->
    <input class="searchpart" type="hidden" id="hid_exhibitionid" value="1" key="_exhibitionid" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
</asp:Content>
