<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AppUpdateManager.aspx.cs" Inherits="Mars.Server.WebApp.OtherManager.AppUpdateManager" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <script src="../js/plugin/bootbox.min.js" type="text/javascript"></script>
    <script src="../js/plugin/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
      <li class="active">系统升级菜单维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
     <h1>系统升级维护<small><i class="icon-double-angle-right"></i>维护系统升级菜单</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server"> 
        <div class="alert alert-block alert-success form-horizontal"> 
                <div class="control-group" style="float: left;">
            <label class="control-label" for="txtAppType">系统</label>
            <div class="controls">
               <select id="selAppType" runat="server" class="searchpart" key="selAppType">
                   <option value="">--请选择--</option>
                   <option value="1">Android</option>
                   <option value="2">IOS</option>
               </select>
            </div>
        </div>    
          <div class="control-group" style="float: left;">
            <label class="control-label" for="txtVersion">版本号</label>
            <div class="controls">
               <input id="txtVersion" runat="server" type="text" placeholder="请输入版本号" class="searchpart" key="txtVersion"/>
            </div>
        </div>  
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
            <button class="btn btn-primary" onclick="javascript:NewAppUpdate();">新增</button>
        </div> 
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div> 
    <OpenBook:TemplateWrapper ID="tmpAppUpdate" runat="server" TemplateSrc="~/Templates/AppUpdateTemplate.ascx" PageSize="20"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" UseRequestCache="false" SearchControlID="searchbut" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script>
        function NewAppUpdate() {
            window.location.href = "AppUpdateEdit.aspx?fun=<%=fun%>"
        }
        function deleteAppUpdate(appid) {
            bootbox.confirm("您确认要删除当前菜单项吗?(删除后将不可恢复)", function (result) {
                if (result) {
                    $.post(_root + "handlers/CommonDictController/delAppUpdate.ashx", { "appid": appid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");
                        if (json.status == "1") {
                            bootbox.alert("删除成功！");
                            TObj("tmpAppUpdate").loadData();

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
