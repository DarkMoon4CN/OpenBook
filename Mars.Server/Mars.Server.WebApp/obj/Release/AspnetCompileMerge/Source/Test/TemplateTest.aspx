<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TemplateTest.aspx.cs" Inherits="Mars.Server.WebApp.Test.TemplateTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">注册用户审批</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>用户注册审批 <small><i class="icon-double-angle-right"></i>网站注册用户审批</small></h1>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="btn-group" style="float: right;">
            <button class="btn  btn-primary" onclick="javascript:searchuser(false);">查询</button>
            <button class="btn  btn-primary dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a href="javascript:" onclick="javascript:searchuser(false);">查询</a></li>
                <li><a href="javascript:" id="btndownload">导出</a></li>
            </ul>
        </div>
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>

    <OpenBook:TemplateWrapper ID="tmpRegUserList" runat="server" TemplateSrc="~/Test/Templates/RegUserListTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="10"  />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        function dataloaded(obj, data) {
            if (obj._prmsData._pageIndex == 0) {
                changesearchtitle(data.message);
            }
        }

        var changesearchtitle = function (strmessage) {
            $("#lblSTitle")[0].innerHTML = strmessage;
        }
    </script>
</asp:Content>
