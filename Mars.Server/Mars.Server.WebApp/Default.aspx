<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Mars.Server.WebApp.Default" %>

<asp:Content ID="concss" runat="server" ContentPlaceHolderID="css">
</asp:Content>

<asp:Content ID="conscript" runat="server" ContentPlaceHolderID="script">
     <!--[if lt IE 9]>
    <OpenBook:OBScript runat="server" ID="excanvasjs" Src="~/js/plugin/excanvas.min.js" ScriptType="Javascript" />
	<![endif]-->
 <%--   <OpenBook:OBScript runat="server" ID="custom_js" Src="~/js/plugin/jquery-ui-1.10.2.custom.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="touch_js" Src="~/js/plugin/jquery.ui.touch-punch.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="slimscroll_js" Src="~/js/plugin/jquery.slimscroll.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="easy_pie_chart_js" Src="~/js/plugin/jquery.easy-pie-chart.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="sparkline_js" Src="~/js/plugin/jquery.sparkline.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="flot_js" Src="~/js/plugin/jquery.flot.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="flot_pie_js" Src="~/js/plugin/jquery.flot.pie.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="flot_resize_js" Src="~/js/plugin/jquery.flot.resize.min.js" ScriptType="Javascript" />--%>

    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="connavigation" runat="server" ContentPlaceHolderID="navigation">
</asp:Content>

<asp:Content ID="conpageheader" runat="server" ContentPlaceHolderID="pageheader">
</asp:Content>

<asp:Content ID="condata" runat="server" ContentPlaceHolderID="content">
</asp:Content>

<asp:Content ID="coninlinescript" runat="server" ContentPlaceHolderID="inlinescripts">
</asp:Content>
