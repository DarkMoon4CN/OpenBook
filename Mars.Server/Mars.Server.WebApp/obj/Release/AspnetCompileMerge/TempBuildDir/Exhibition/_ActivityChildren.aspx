<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_ActivityChildren.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ActivityChildren" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
           
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-primary" onclick="javascript:addActivity();">添加活动</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpActivityChildrenList" runat="server" TemplateSrc="~/Templates/ConsoleActivityChildrenTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" UseRequestCache="false"/> 

    <input type="hidden" id="pid" class="searchpart" key="pid" value="<%= parentid%>" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var addActivity = function () {
            asyncbox.open({
                modal: true,
                id: "activitychild",
                title: "添加活动",
                url: _root + "Exhibition/_ActivityEdit.aspx?pid="+$("#pid").val(),
                width: 950,
                height: 650
            });
        }

        var delActivity = function (id) {
            bootbox.confirm("您确认要删除此项活动吗?", function (result) {
                if (result) {
                    $.post(_root + "handlers/ExhibitionController/DeleteActivity.ashx", { "_id": id, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpActivityChildrenList")._prmsData.ts = new Date().getTime();
                            TObj("tmpActivityChildrenList").S();
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

        var modActivity = function (id) {
            asyncbox.open({
                modal: true,
                id: "activitychild",
                title: "编辑活动",
                url: _root + "Exhibition/_ActivityEdit.aspx?id=" + id+"&pid="+$("#pid").val(),
                width: 950,
                height: 550
            });
        }
    </script>
</asp:Content>
