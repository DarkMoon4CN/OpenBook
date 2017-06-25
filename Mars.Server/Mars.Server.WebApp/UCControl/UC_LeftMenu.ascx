<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_LeftMenu.ascx.cs" Inherits="Mars.Server.WebApp.UCControl.UC_LeftMenu" %>
<asp:Literal ID="literalMenu" runat="server"></asp:Literal>
<OpenBook:OBScript Src="~/js/leftmenu.js" runat="server" ID="OBScript" ScriptType="Javascript" />