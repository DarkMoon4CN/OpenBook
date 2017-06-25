<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AdminManager.aspx.cs" Inherits="Mars.Server.WebApp.Admin.AdminManager" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">管理员账户</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>管理员账户 <small><i class="icon-double-angle-right"></i>维护后台管理员用户的权限</small></h1>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtloginname">登录名</label>
            <div class="controls">
                <input type="text" id="txtloginname" placeholder="登录名" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtloginname">姓名</label>
            <div class="controls">
                <input type="text" id="txtUsername" placeholder="姓名" />
            </div>
        </div>

        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" onclick="javascript:searchuser(false);">查询</button>
            <button class="btn btn-primary" onclick="javascript:addnewadmin();">新增</button>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpAdminList" runat="server" TemplateSrc="~/Templates/AdminsTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var changepwd = function (pid, uname) {
            asyncbox.open({
                modal: true,
                id: "changepwd",
                title: "修改管理员密码",
                url: _root + "Admin/ChangePwd.aspx?flag=1&pid=" + pid + "&uname=" + uname + "&ts=" + new Date().getTime(),
                width: 800,
                height: 300,
                callback: function (btnRes, cntWin, reVal) {
                    if (btnRes == "sure") {
                        searchuser(true);
                    }
                }
            });
        }

        function refresh(reload) {
            searchuser(reload);
        }

        var addnewadmin = function () {
            window.location.href = "AdminEdit.aspx?fun=<%= fun%>";
        }

            var deleteuser = function (pid) {
                bootbox.confirm("您确认要删除当前管理员吗?(删除后将不可恢复)", function (result) {
                    if (result) {
                        $.post(_root + "handlers/AdminController/DeleteAdmin.ashx", { "pid": pid, "ts": new Date().getTime() }, function (data) {
                            var json = eval("(" + data + ")");

                            if (json.state = "1") {
                                bootbox.dialog("删除成功!", [{
                                    "label": "OK",
                                    "class": "btn-small btn-primary",
                                    callback: function () {
                                        searchuser(true);
                                    }
                                }]);
                            }
                            else if (json.state = "0") {
                                bootbox.alert("删除失败!");
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

            var searchuser = function (reload) {
                var txtloginname = $("#txtloginname").val();
                var txtUsername = $("#txtUsername").val();

                TObj("tmpAdminList")._prmsData.loginname = txtloginname;
                TObj("tmpAdminList")._prmsData.username = txtUsername;

                if (reload) {
                    TObj("tmpAdminList")._prmsData.ts = new Date().getTime();
                }

                TObj("tmpAdminList")._prmsData._pageIndex = 0;
                TObj("tmpAdminList").loadData();
            }

            var setright = function (pid) {
                asyncbox.open({
                    modal: true,
                    id: "setroleorfunction",
                    title: "设置角色权限",
                    url: _root + "Rights/SetRoleOrFunction.aspx?uid=" + pid + "&ts=" + new Date().getTime(),
                    width: 950,
                    height: 600,
                    callback: function (btnRes, cntWin, reVal) {
                        if (btnRes == "sure") {

                        }
                    }
                });
            }
    </script>
</asp:Content>
