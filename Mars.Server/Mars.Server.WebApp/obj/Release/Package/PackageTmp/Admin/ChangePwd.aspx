<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="ChangePwd.aspx.cs" Inherits="Mars.Server.WebApp.Admin.ChangePwd" %>

<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="asyncbox_js" runat="server" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
        <h4 class="lighter">修改管理员密码</h4>      
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="row-fluid">
                <form runat="server" class="form-horizontal" id="validationf">
                    <div class="control-group">
                        <label class="control-label">登录名:</label>
                        <div class="controls">                          
                              <label class="control-label" id="lblLoginName" style="text-align:left;" runat="server"></label>                            
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label" for="txtUserPwd">新密码:</label>
                        <div class="controls">                           
                                <input type="password" id="txtUserPwd" name="txtUserPwd" />                          
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label" for="txtConfirmUserPwd">确认密码:</label>
                        <div class="controls">                          
                                <input type="password" id="txtConfirmUserPwd" name="txtConfirmUserPwd" />                           
                        </div>
                    </div>

                    <div class="hr hr-dotted"></div>
                    <div style="text-align: center;">
                        <button class="btn btn-success" onclick="javascript:savecompany();">保存</button>
                        <button class="btn btn-danger" onclick="javascript:closeset();">关闭</button>
                    </div>

                    <input type="hidden" id="hidPid" runat="server" />                  
                </form>
            </div>
        </div>
        <!--/widget-main-->
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="inlinescripts" ID="coninlinescript" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#validationf").validate({               
                errorEvent: "span",
                errorClass: "help-inline",
                foucusInvalid: false,
                rules: {
                    txtUserPwd: { required: true, rangelength: [6, 15] },
                    txtConfirmUserPwd: { equalTo: "#txtUserPwd" }
                },
                messages: {
                    txtUserPwd: { required: "必填项", rangelength: "密码长度6-15位(以字母开头6-15位字母、数字和下划线)" },
                    txtConfirmUserPwd: { equalTo: "两次密码输入不一致" }
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

        var savecompany = function () {
            if (!$("#validationf").valid())
            {
                return false;
            }
            else
            {
                var pid = $("#content_hidPid").val();
                var pwd = $("#txtUserPwd").val();

                var data = { "pwd": pwd, "pid": pid };
                $.post(_root + "handlers/AdminController/ChangeAdminPwd.ashx", data, function (data) {
                    var json = eval("(" + data + ")");

                    if (json.status == "1")
                    {
                        bootbox.dialog("密码修改成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                if ("<% = flag%>" == "2")
                                {
                                    parent.refresh2(true);
                                    parent.asyncbox.close("changepwd2");
                                }
                                else
                                {
                                    parent.refresh(true);
                                    parent.asyncbox.close("changepwd");
                                }                                                            
                            }
                        }]);
                    }
                    else if (json.status == "0")
                    {
                        bootbox.alert("密码修改失败");
                        return false;
                    }
                    else
                    {
                        bootbox.alert("数据传输错误");
                        return false;
                    }
                });
            }
        }

        var closeset = function () {
            if ("<% = flag%>" == "2")
            {
                parent.asyncbox.close("changepwd2");
            }
            else
            {
                parent.asyncbox.close("changepwd");
            }          
        }
    </script>
</asp:Content>
