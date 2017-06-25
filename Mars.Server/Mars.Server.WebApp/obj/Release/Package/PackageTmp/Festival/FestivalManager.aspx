<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FestivalManager.aspx.cs" Inherits="Mars.Server.WebApp.Festival.FestivalManager" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
     <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
     <script src="../js/plugin/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
     <li class="active">日历菜单维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>日历维护 <small><i class="icon-double-angle-right"></i>维护后台日历菜单</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
            <label class="control-label" for="txtStartTime">开始时间</label>
            <div class="controls">
                <input type="text" id="txtStartTime" class="Wdate searchpart"  key="txtStartTime" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({ onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}',dateFmt:'yyyy-MM-dd'})" placeholder="请输入有效开始时间" />
            </div>
        </div> 
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtEndTime">结束时间</label>
            <div class="controls">
                <input type="text" id="txtEndTime" class="Wdate searchpart"  key="txtEndTime"onfocus="WdatePicker({ dateFmt:'yyyy-MM-dd',minDate: '#F{$dp.$D(\'txtStartTime\')}' })" placeholder="请输入有效结束时间" />
            </div>
        </div>  
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="searchbut">查询</button>
            <button class="btn btn-primary" onclick="javascript:add();">新增</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
        <OpenBook:TemplateWrapper ID="tmpFestivalList" runat="server" TemplateSrc="~/Templates/FestivalTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" SearchControlID="searchbut" UseRequestCache="false"/>  
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        function add() { 
            window.location.href = "FestivalEdit.aspx?fun=<%=fun%>";
        } 
        function delFestival(id) { 
            bootbox.confirm("您确认要删除当前菜单项吗?(删除后将不可恢复)", function (result) {
                if (result) {
                    $.post(_root + "handlers/FestivalController/delFestival.ashx", { "Festivalid": id, "ts": new Date().getTime() }, function (data)
                    {
                        var json = eval("("+data+")");
                        if (json.status == "1") {
                            bootbox.alert("删除成功！");
                            TObj("tmpFestivalList").loadData();  

                        } else if (json.status == "0") {
                            bootbox.alert("删除失败！");
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