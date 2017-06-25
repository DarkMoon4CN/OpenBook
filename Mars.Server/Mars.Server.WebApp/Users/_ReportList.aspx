<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_ReportList.aspx.cs" Inherits="Mars.Server.WebApp.Users._ReportList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
     <li class="active">用户管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>用户管理<small><i class="icon-double-angle-right"></i>用户举报</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
        <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
            <div class="control-group" style="float: left;">
            <label class="control-label" for="txtStartTime">开始时间</label>
            <div class="controls">
                <input type="text" id="txtStartTime" class="Wdate searchpart" key="stime" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({ onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}'})" placeholder="请输入有效开始时间" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtEndTime">结束时间</label>
            <div class="controls">
                <input type="text" id="txtEndTime" class="Wdate searchpart" key="etime" onfocus="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' })" placeholder="请输入有效结束时间" />
            </div>
        </div>
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_search">查询</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpReportList" runat="server" TemplateSrc="~/Templates/ReportListTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" SearchControlID="btn_search" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
        <script type="text/javascript">
            var delReport = function (id) {
            bootbox.confirm("您确认要删除此条举报吗？", function (result) {
                if (result) {
                    $.post(_root + "handlers/UserController/DeleteReport.ashx", { "_id": id, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpReportList")._prmsData.ts = new Date().getTime();
                            TObj("tmpReportList").S();
                        }
                        else if (json.state == "0") {
                            bootbox.alert(json.msg);
                            return false;
                        }
                        else {
                            bootbox.alert("数据传输错误");
                            return false;
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
