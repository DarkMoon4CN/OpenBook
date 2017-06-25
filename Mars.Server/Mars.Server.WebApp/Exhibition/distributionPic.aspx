<%@ Page Title="" Language="C#" MasterPageFile="~/Exhibition/Exhibition.Master" AutoEventWireup="true" CodeBehind="distributionPic.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition.distributionPic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="swiper_css" Src="~/css/swiper.css" ScriptType="StyleCss" />
    <style type="text/css">
        img {
            width: 100%;
            margin: auto;
        }

        body {
            background-color: white !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="jqueryjs" Src="~/js/plugin/swiper.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="swiper-container">
		 <div class="swiper-wrapper">
	        <!-- Slides -->
	        <div class="swiper-slide" id="map_1">
	        	<figure>
	        		<img src="/images/11.png"/>
	        	</figure>
	        </div>
	        <div class="swiper-slide"" id="map_2">
	        	<figure>
	        		<img src="/images/12.png"/>
	        	</figure>
	        </div>
	        <div class="swiper-slide" id="map_3">
	        	<figure>
	        		<img src="/images/23.png"/>
	        	</figure>
	        </div>
	        <div class="swiper-slide"" id="map_4">
	        	<figure>
	        		<img src="/images/45.png"/>
	        	</figure>
	        </div>
	        <div class="swiper-slide"" id="map_5">
	        	<figure>
	        		<img src="/images/67.png"/>
	        	</figure>
	        </div>
	        <div class="swiper-slide"" id="map_6">
	        	<figure>
	        		<img src="/images/8.png"/>
	        	</figure>
	        </div>
	    </div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script>
        $(document).ready(function () {
            var mySwiper = new Swiper('.swiper-container', {
                direction: 'horizontal',
            });
            mySwiper.slideTo(<%=Request.QueryString["aid"]%>, 2000, false);//切换到第一个slide，速度为1秒
        });
    </script>
</asp:Content>
