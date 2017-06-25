<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AdminEdit.aspx.cs" Inherits="Mars.Server.WebApp.Admin.AdminEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="concss" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="navigation" ID="connav" runat="server">
    <li class="active">系统管理员维护</li>
</asp:Content>

<asp:Content ContentPlaceHolderID="pageheader" ID="conheader" runat="server">
    <h1>系统管理员维护 <small><i class="icon-double-angle-right"></i>编辑系统管理员</small></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="conContent" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑系统管理员</h4>
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
                                        <input type="hidden" id="hidPid" runat="server"/>
                                        <div class="control-group" >
                                            <label class="control-label" for="content_txtLoginname">登录名</label>
                                            <div class="controls">
                                                <input type="text" name="txtLoginname" id="txtLoginname" runat="server" />
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtTruename">姓名</label>
                                            <div class="controls">
                                                <input type="text" name="txtTruename" id="txtTruename" runat="server" />
                                            </div>
                                        </div>                                        
 
                                        <div class="control-group" id="divUserpwd" runat="server">
                                            <label class="control-label" for="content_txtUserpwd">密码</label>
                                            <div class="controls">
                                                <input type="password" name="txtUserpwd" id="txtUserpwd" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group" id="divConfirmuserpwd" runat="server">
                                            <label class="control-label" for="content_txtConfirmuserpwd">确认密码</label>
                                            <div class="controls">
                                                <input type="password" name="txtConfirmuserpwd" id="txtConfirmuserpwd" runat="server" />
                                            </div>
                                        </div>                                      

                                          <div class="control-group" >
                                            <label class="control-label" for="content_selsex">性别</label>
                                            <div class="controls">
                                                <select id="selsex" name="selsex" runat="server">
                                                    <option value="0" selected="selected">男</option>
                                                    <option value="1">女</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="control-group" >
                                            <label class="control-label" for="content_txtUser_Tel">分机号</label>
                                            <div class="controls">
                                                <input type="text" name="txtUser_Tel" id="txtUser_Tel" runat="server" />
                                            </div>
                                        </div>                                      

                                           <div class="control-group" >
                                            <label class="control-label" for="content_txtUser_Mobile">手机号</label>
                                            <div class="controls">
                                                <input type="text" name="txtUser_Mobile" id="txtUser_Mobile" runat="server" />
                                            </div>
                                        </div>
                                        <div class="control-group" >
                                            <label class="control-label" for="content_txtUser_Mail">电子邮箱</label>
                                            <div class="controls">
                                                <input type="text" name="txtUser_Mail" id="txtUser_Mail" runat="server" />
                                            </div>
                                        </div>                                     

                                         <div class="control-group">
                                            <label class="control-label" for="content_selDepartMent">公司部门</label>
                                            <div class="controls">
                                                <select id="selDepartMent" name="selDepartMent" runat="server">
                                                    <option value="-1" selected="selected">请选择</option>
                                                </select>
                                            </div>
                                        </div>                                      
                                    </form>
                                </div>
                            </div>

                            <input type="hidden" id="hidfun" runat="server" />
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
            jQuery.validator.addMethod("checkUsername", function (value, element) {
                return this.optional(element) || /^[a-zA-Z]+\w{3,14}$/.test(value);
            }, "用户名长度为4-15以字母开头字母、数字和下划线");
        });

        var saveadmin = function () {
            if (!$("#validationf").valid())
            {
                return false;
            }
            else
            {
                document.getElementById("validationf").submit();
            }
        }

        $(function () {
            $("#validationf").validate({
                errorEvent:'span',
                errorClass:'help-inline',
                focusInvalid:false,
                rules: {
                    ctl00$content$txtLoginname: {
                        required: true,
                        checkUsername:true,
                        remote: {
                            type: "POST",
                            url: _root + "handlers/AdminController/IsUseableByUsername.ashx",
                            data: {
                                username: function () { return $("#content_txtLoginname").val(); }
                            }
                        }
                    },
                    ctl00$content$txtTruename: { required: true },
                    ctl00$content$selDepartMent: { required: true, min: 1 },
                    ctl00$content$txtUserpwd: { required: true, rangelength:[6,15] },
                    ctl00$content$txtConfirmuserpwd: { equalTo: "#content_txtUserpwd" },
                    ctl00$content$txtUser_Tel: { required: true, rangelength: [3,5],digits:true },
                    ctl00$content$txtUser_Mail: {
                        email: true,
                        remote: {
                            type: "POST",
                            url: _root + "handlers/AdminController/IsUseableByEmail.ashx",
                            data: {
                                email: function () { return $("#content_txtUser_Mail").val(); },
                                pid: function () { return $("#content_hidPid").val(); }
                            }
                        }
                    },                   
                },
                messages: {
                    ctl00$content$txtLoginname: {
                        required: "必填项",
                        remote: jQuery.format("用户名已经被注册")
                    },
                    ctl00$content$txtTruename: { required: "必填项" },
                    ctl00$content$selDepartMent: { required: "必填项", min: "必填项" },
                    ctl00$content$txtUserpwd: { required: "必填项", rangelength: "密码长度6-15位(以字母开头6-15位字母、数字和下划线)" },
                    ctl00$content$txtConfirmuserpwd: { equalTo: "两次密码输入不一致" },
                    ctl00$content$txtUser_Tel: { required: "必填项",rangelength:"分机号长度3-5位数字", digits:"格式错误" },
                    ctl00$content$txtUser_Mail: {
                        email: "邮箱格式错误",
                        remote: jQuery.format("该邮箱在系统中已存在")                      
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


        var backlist = function () {
            history.back();
        }
    </script>
</asp:Content>
