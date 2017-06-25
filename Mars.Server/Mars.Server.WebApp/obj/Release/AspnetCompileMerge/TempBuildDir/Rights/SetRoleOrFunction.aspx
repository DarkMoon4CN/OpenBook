<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="SetRoleOrFunction.aspx.cs" Inherits="Mars.Server.WebApp.Rights.SetRoleOrFunction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <h3 class="lighter block green">管理员权限账户维护</h3>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>

    <div class="tabbable">
        <ul class="nav nav-tabs" id="myTab">
            <li class="active"><a href="#role" data-toggle="tab">角色列表 </a></li>
            <li><a href="#profile" data-toggle="tab">自定义权限列表 </a></li>
        </ul>
        <div class="tab-content">
            <div class="tab-pane active" id="role">
                <div class="span6">
                    <div class="control-group">
                        <div class="controls">
                            <asp:Repeater ID="rptRole" runat="server">
                                <ItemTemplate>
                                    <label>
                                        <input name="rbrole" type="radio" value="<%#DataBinder.Eval(Container.DataItem,"Role_ID") %>" /><span class="lbl"> <%#DataBinder.Eval(Container.DataItem,"Role_Name") %></span>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="hr hr-dotted"></div>
                    <div class="span4" style="float: left;">
                        <button class="btn btn-success" onclick="javascript:saverole();">保存</button>
                        <button class="btn btn-danger" onclick="javascript:closeset();">关闭</button>
                    </div>
                </div>

            </div>
            <div class="tab-pane" id="profile" style="height: 400px;">
                <div id="function_treeview" runat="server">
                </div>
                <div class="hr hr-dotted"></div>
                <div class="span4" style="float: left;">
                    <button class="btn btn-success" onclick="javascript:savefunction();">保存</button>
                    <button class="btn btn-danger" onclick="javascript:closeset();">关闭</button>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hidrole" value="" runat="server" />
    <input type="hidden" value="" runat="server" id="userfun" />
    <input type="hidden" value="<%= userId %>" id="hiduserid" />
    <input type="hidden" id="hidOriginRole" value="" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="inlinescripts" ID="coninlinescript" runat="server">
    <OpenBook:OBScript runat="server" Src="~/js/plugin/jquery.treeview.js" ID="treeview_js" ScriptType="Javascript" />
    <script type="text/javascript">
        var closeset = function () {
            parent.asyncbox.close("setroleorfunction");
            // parent.location.href = parent.location.href;
        }

        var saverole = function () {
            if ($("#content_hidrole").val() == "" || $("#content_hidrole").val() == "-1") {
                bootbox.alert("请选择角色");
            } else {
                //更新角色
                //构建json对象
                var userid, roleid;

                userid = $("#hiduserid").val();
                roleid = $("#content_hidrole").val();

                var data = { "userid": userid, "roleid": roleid, "ts": new Date().getTime() };
                $.post(_root + "handlers/AdminController/SetRole.ashx", data, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "0") {
                        alert("设置失败失败！");
                        return false;
                    }
                    else if (json.state == "1") {
                        bootbox.dialog("设置成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                parent.refresh(true);
                                parent.asyncbox.close("setroleorfunction");
                            }
                        }]
                        );
                    }
                });
            }
        }

        var savefunction = function () {
            if ($("#content_hidOriginRole").val() == "-1") {
                bootbox.alert("请先设置角色");
                return false;
            }
            else if (getReturnValue() == "") {
                bootbox.alert("请选择权限");
                return false;
            }
            else {
                //更新权限
                //更新角色
                //构建json对象
                var userid, functionsid;

                userid = $("#hiduserid").val();
                functionsid = getReturnValue();

                var data = { "userid": userid, "functionsid": functionsid, "ts": new Date().getTime() };
                $.post(_root + "handlers/AdminController/SetFunctions.ashx", data, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "0") {
                        alert("设置失败失败！");
                        return false;
                    }
                    else if (json.state == "1") {
                        bootbox.dialog("设置成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                parent.refresh(true);
                                parent.asyncbox.close("setroleorfunction");
                                //parent.location.href = parent.location.href;
                            }
                        }]
                        );
                    }
                });
            }
        }

        function getReturnValue() {
            var newfunctions = "";
            //标准分类
            $("[checktype='public']:checked").each(function () {
                //if ($(this).attr("checked")=="checked") {
                newfunctions += $(this).attr("value") + ',';
                //}
            });          
            return newfunctions;
        }

        var checkchange = function (obj) {
            if ($(obj).attr("level") == "1") {
                if ($(obj).is(":checked")) {
                    $(obj).closest("li").find("[level=2]").prop("checked", true);
                }
                else {
                    $(obj).closest("li").find("[level=2]").prop("checked", false);
                }
            }
            else {
                var selnums = $(obj).closest("ul").find("[level=2]:checked").length;

                if ($(obj).is(":checked")) {
                    $(obj).closest("li.open.expandable").find("[level=1]").prop("checked", true);
                }
                else if (selnums == 0) {
                    $(obj).closest("li.open.expandable").find("[level=1]").prop("checked", false);
                }
            }
        }

        $(function () {
            $("#content_function_treeview").treeview({
                persist: "location",
                collapsed: true,
                unique: false,
                animated: "fast"
            });

            $("input[name='rbrole']").click(function () {
                $("#content_hidrole").val($(this).val());
            });

            var funarray = $("#content_userfun").val().split(",");
            $("input[type=checkbox]").each(function () {
                for (var i = 0; i <= funarray.length; i++) {
                    if ($(this).val() == funarray[i]) {
                        $(this).attr("checked", true);
                    }
                }
            });

            var roleid = $("#content_hidrole").val();
            $("input[name='rbrole']").each(function () {
                if ($(this).val() == roleid) {
                    $(this).attr("checked", "checked");
                }
            });
        });
    </script>
</asp:Content>
