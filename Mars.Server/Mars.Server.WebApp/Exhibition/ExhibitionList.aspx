<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExhibitionList.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition.ExhibitionList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="autocomplete_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">展会地图</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>展会地图 <small><i class="icon-double-angle-right"></i>展会信息</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
            <label class="control-label" for="txt_exhibitiontitle">展会名称</label>
            <div class="controls">
                <input type="text" id="txt_exhibitiontitle" class="searchpart"  key="_exhibitiontitle"  placeholder="展会名称" />
            </div>
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_search">查询</button>
            <button class="btn btn-primary" onclick="javascript:addExhibition();">新增</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpExhibitionList" runat="server" TemplateSrc="~/Templates/ExhibitionTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" SearchControlID="btn_search" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var delExhibition = function (eid)
        {
            bootbox.confirm("您确认要删除此条展会信息吗？", function (result) {
                if (result) {
                    $.post(_root + "handlers/ExhibitionController/DeleteExhibition.ashx", { "exhibitionid": eid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");
                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpExhibitionList")._prmsData.ts = new Date().getTime();
                            TObj("tmpExhibitionList").loadData();
                        } else if (json.state == "0") {
                            bootbox.alert(json.msg);
                            return false;
                        }
                        else {
                            bootbox.alert("数据传输失败！")
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
