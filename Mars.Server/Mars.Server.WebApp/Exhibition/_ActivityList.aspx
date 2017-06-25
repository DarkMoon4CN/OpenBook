<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_ActivityList.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ActivityList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">展会地图</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>展会地图 <small><i class="icon-double-angle-right"></i>活动信息</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
           <label class="control-label" for="txt_exhibitorname">展场名称</label>
            <div class="controls">
                <select class="searchpart" id="sel_exhibition" class="searchpart"  key="_exhibitionid">
                    <option value="1">2016年北京图书订货会</option>
                </select>
            </div>
            <label class="control-label" for="txt_activityname">活动名称</label>
            <div class="controls">
                <input type="text" id="txt_activityname" class="searchpart"  key="_activityname"  placeholder="活动名称" />
            </div>
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_search">查询</button>
            <button class="btn btn-success" id="btn_import" onclick="javascript:importActivity();">批量导入</button>
            <button class="btn btn-primary" onclick="javascript:addActivity();">添加活动</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpActivityList" runat="server" TemplateSrc="~/Templates/ConsoleActivityTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" SearchControlID="btn_search" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var delActivity = function (id) {
            bootbox.confirm("您确认要删除此项活动吗?(相关子活动也将被删除)", function (result) {
                if (result) {
                    $.post(_root + "handlers/ExhibitionController/DeleteActivity.ashx", { "_id": id, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpActivityList")._prmsData.ts = new Date().getTime();
                            TObj("tmpActivityList").S();
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

        var addActivity = function () {
            asyncbox.open({
                modal: true,
                id: "activity",
                title: "添加活动",
                url: _root + "Exhibition/_ActivityEdit.aspx",
                width: 950,
                height: 650
            });
        }

        var modActivity = function (id) {
            asyncbox.open({
                modal: true,
                id: "activity",
                title: "编辑活动",
                url: _root + "Exhibition/_ActivityEdit.aspx?id=" + id,
                width: 950,
                height: 550
            });
        }

        var openChildren = function (id,title) {
            asyncbox.open({
                modal: true,
                id: "activitychild",
                title: title + "的子活动",
                url: _root + "Exhibition/_ActivityChildren.aspx?id=" + id,
                width: 1250,
                height: 750
            });
        }

        var importActivity = function () {
            asyncbox.open({
                modal: true,
                id: "activityimport",
                title: "导入展商",
                url: _root + "Exhibition/_ActivityImport.aspx",
                width: 950,
                height: 250
            });
        }
    </script>
</asp:Content>
