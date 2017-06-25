<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RoleEdit.aspx.cs" Inherits="Mars.Server.WebApp.Rights.RoleEdit" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="concss" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="navigation" ID="connav" runat="server">
    <li class="active">管理员角色维护</li>
</asp:Content>

<asp:Content ContentPlaceHolderID="pageheader" ID="conheader" runat="server">
    <h1>管理员角色维护 <small><i class="icon-double-angle-right"></i>编辑管理员角色</small></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="conContent" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑管理员角色</h4>
                    <div class="widget-toolbar">
                        <label>
                        </label>
                    </div>
                </div>

                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row-fluid">
                            <div class="row-fluid position-relative">
                                <div class="step-pane">
                                    <form runat="server" class="form-horizontal" id="validationf">

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtRoleName">角色名称</label>
                                            <div class="controls">
                                                <input type="text" name="txtRoleName" id="txtRoleName" runat="server" maxlength="20" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="ulfunction">权限</label>
                                            <div class="controls">
                                                <div id="function_treeview" runat="server">
                                                </div>
                                            </div>
                                        </div>

                                        <input type="hidden" id="hidpid" value="0" runat="server" />
                                        <input type="hidden" value="" runat="server" id="userfun" />
                                        <input type="hidden" runat="server" id="hidfun" />
                                    </form>
                                </div>
                            </div>

                            <hr />
                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" onclick="javascript:save();">保存</button>
                                <button class="btn btn-success" onclick="javascript:backlist();">返回</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="inlinescripts" ID="conLineScript" runat="server">
    <OpenBook:OBScript runat="server" Src="~/js/plugin/jquery.treeview.js" ID="treeview_js" ScriptType="Javascript" />
    <script type="text/javascript">

        $(function () {
            $("#content_function_treeview").treeview({
                persist: "location",
                collapsed: true,
                unique: false,
                animated: "fast"
            });

            var funarray = $("#content_userfun").val().split(",");
            $("input[type=checkbox]").each(function () {
                for (var i = 0; i <= funarray.length; i++) {
                    if ($(this).val() == funarray[i]) {
                        $(this).attr("checked", true);
                    }
                }
            });
        });

        $(function () {
            $("#validationf").validate({
                errorEvent: 'span',
                errorClass: 'help-inline',
                focusInvalid: false,
                rules: {
                    ctl00$content$txtRoleName: {
                        required: true,
                        remote: {
                            type: "POST",
                            url: _root + "handlers/RightController/IsUseableRoleName.ashx",
                            data: {
                                rname: function () { return $("#content_txtRoleName").val(); },
                                pid: function () { return $("#content_hidpid").val(); }
                            }
                        }
                    }
                },
                messages: {
                    ctl00$content$txtRoleName: {
                        required: "必填项",
                        remote: jQuery.format("角色名称已存在")
                    }
                },
                invalidHandler: function (event, validator) { //display error alert on form submit   
                    $('.alert-error', $('.login-form')).show();
                },
                highlight: function (e) {
                    $(e).closest('.control-group').removeClass('info').addClass('error');
                },
                success: function (e) {
                    $(e).closest('.control-group').removeClass('error').addClass('info');
                    $(e).remove();
                },
                errorPlacement: function (error, element) {
                    if (element.is(':checkbox') || element.is(':radio')) {
                        var controls = element.closest('.controls');
                        if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                        else error.insertAfter(element.nextAll('.lbl').eq(0));
                    }
                    else if (element.is('.chzn-select')) {
                        error.insertAfter(element.nextAll('[class*="chzn-container"]').eq(0));
                    }
                    else error.insertAfter(element);
                },
                submitHandler: function (form) {
                },
                invalidHandler: function (form) {
                }
            });
        });

        var save = function () {
            if (!$("#validationf").valid()) {
                return false;
            }
            else {
                saveRolefunctionRel();
            }
        }

        var saveRolefunctionRel = function () {
            var selfunctions = getReturnValue();

            if (selfunctions == "") {
                bootbox.alert("请选择权限");
                return false;
            }
            else {
                var pid, rname;
                pid = $("#content_hidpid").val();
                rname = $("#content_txtRoleName").val();

                var data = { "pid": pid, "functionsid": selfunctions, "rname": rname, "ts": new Date().getTime() };
                $.post(_root + "handlers/RightController/SaveRoleFunRel.ashx", data, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.status == "0") {
                        bootbox.alert("保存失败！");
                        return false;
                    }
                    else if (json.status == "1") {
                        bootbox.dialog("保存成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                window.location.href = "RoleManager.aspx?fun=" + $("#content_hidfun").val();
                                //parent.location.href = parent.location.href;
                            }
                        }]
                        );
                    }
                    else {
                        bootbox.alert("数据传输错误");
                        return false;
                    }
                });
            }
        }

        function getReturnValue() {
            var newfunctions = "";
            //标准分类
            $("[checktype='public']:checked").each(function () {
                newfunctions += $(this).attr("value") + ',';
            });         
            return newfunctions;
        }

        var checkchange = function (obj) {
            //$("input[type='checkbox'][level='2']:eq(0)").parents("li.open.collapsable").find("[level='1']").attr("checked", "checked")
            //$("input[type='checkbox'][level='2']:eq(0)").parents("li.open.collapsable") .find("[level='1']").is(":checked")
            //$("#content_function_treeview input[type='checkbox']:eq(1)").closest("li.open.collapsable").find("[level=1]").attr("checked", 'false')
            
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

                if ($(obj).is(":checked"))
                {
                    $(obj).closest("li.open.collapsable").find("[level=1]").prop("checked", true);
                }
                else if(selnums == 0)
                {
                    $(obj).closest("li.open.collapsable").find("[level=1]").prop("checked", false);
                }             
            }
        }

        var backlist = function () {
            history.back();
        }
    </script>
</asp:Content>


