<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RoleManager.aspx.cs" Inherits="Mars.Server.WebApp.Rights.RoleManager" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <%--<OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />--%>
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">管理员角色维护</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>管理员角色维护 <small><i class="icon-double-angle-right"></i>维护后台管理员角色</small></h1>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtRolename">角色名称</label>
            <div class="controls">
                <input type="text" id="txtRolename" placeholder="角色名称" />
            </div>
        </div>    

        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
            <button class="btn btn-primary" onclick="javascript:add();">新增</button>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpRolesList" runat="server" TemplateSrc="~/Templates/RolesTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var add = function () {
            window.location.href = "RoleEdit.aspx?fun=<% = fun%>";
        }

        var deleterole = function (pid) {
            bootbox.confirm("您确认要删除当前角色吗?(删除后将不可恢复)", function (result) {
                if (result) {
                    $.post(_root + "handlers/RightController/DeleteRole.ashx", { "pid": pid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("删除成功!", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    search(true);
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("删除失败!");
                            return false;
                        }
                        else if (json.status == "2") {
                            bootbox.alert("有系统管理员占用着该角色，无法删除");
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

        var search = function (reload) {
            var txtRolename = $("#txtRolename").val();


            TObj("tmpRolesList")._prmsData.rolename = txtRolename;

            if (reload) {
                TObj("tmpRolesList")._prmsData.ts = new Date().getTime();
            }

            TObj("tmpRolesList")._prmsData._pageIndex = 0;
            TObj("tmpRolesList").loadData();
        }

    </script>
</asp:Content>

