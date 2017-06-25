<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FunctionEdit.aspx.cs" Inherits="Mars.Server.WebApp.Rights.FunctionEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="concss" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="navigation" ID="connav" runat="server">
    <li class="active">权限菜单维护</li>
</asp:Content>

<asp:Content ContentPlaceHolderID="pageheader" ID="conheader" runat="server">
    <h1>权限菜单维护 <small><i class="icon-double-angle-right"></i>编辑权限菜单</small></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="conContent" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑权限菜单</h4>
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
                                            <label class="control-label" for="content_selFunctionLevel">上级菜单</label>

                                            <div class="controls">
                                                <select id="selFunctionLevel" name="selFunctionLevel" runat="server">
                                                    <option value="0" selected="selected">一级菜单</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtFunctionName">菜单名称</label>
                                            <div class="controls">
                                                <input type="text" name="txtFunctionName" id="txtFunctionName" runat="server" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtFunctionUrl">菜单路径</label>
                                            <div class="controls">
                                                <input type="text" name="txtFunctionUrl" id="txtFunctionUrl" runat="server" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtFunctionOrder">显示顺序</label>
                                            <div class="controls">
                                                <input type="text" name="txtFunctionOrder" id="txtFunctionOrder" runat="server" />
                                            </div>
                                        </div>


                                        <input type="hidden" id="hidPid" runat="server" />
                                        <input type="hidden"  id="hidfun" runat="server"/>
                                    </form>
                                </div>
                            </div>

                            <hr />

                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" onclick="javascript:saveadmin();">保存</button>
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
    <script type="text/javascript">

        $(function () {
            $("#content_selFunctionLevel").change(function () {
                if ($(this).val() == "0")
                {
                    $("#content_txtFunctionUrl").val("");
                    $("#content_txtFunctionUrl").attr("disabled","disabled");
                }
                else
                {
                    $("#content_txtFunctionUrl").attr("disabled",false);
                }
            });
        });

        var saveadmin = function () {
            if (!$("#validationf").valid()) {
                return false;
            }
            else {
                document.getElementById("validationf").submit();
            }
        }

        $(function () {
            $("#validationf").validate({
                errorEvent: 'span',
                errorClass: 'help-inline',
                focusInvalid: false,
                rules: {
                    ctl00$content$txtFunctionName: {
                        required: true,
                        rangelength: [2, 20],
                        remote: {
                            type: "POST",
                            url: _root + "handlers/RightController/IsUseableFunctionName.ashx",
                            data: {
                                fname: function () { return $("#content_txtFunctionName").val(); },
                                pid: function () { return $("#content_hidPid").val(); }
                            }
                        }
                    },
                    ctl00$content$txtFunctionOrder: { required:true, rangelength: [1, 5], digits: true },
                    ctl00$content$txtFunctionUrl: { required: true, rangelength: [5, 200] }
                },
                messages: {
                    ctl00$content$txtFunctionName: {
                        required: "必填项",
                        remote: jQuery.format("菜单名称系统中已存在"),
                        rangelength: "菜单名称长度2-20个字符之间"
                    },
                    ctl00$content$txtFunctionOrder: { required: "必填项", rangelength: "顺序位数1-5位", digits: "请输入数字" },
                    ctl00$content$txtFunctionUrl: { required: "必填项", rangelength: "菜单路径长度介于5-200之间的字符串" }
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

        var backlist = function () {
           history.back();
        }
    </script>
</asp:Content>
