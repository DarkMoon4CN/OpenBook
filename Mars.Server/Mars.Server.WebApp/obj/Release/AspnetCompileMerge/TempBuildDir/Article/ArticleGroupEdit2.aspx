<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="ArticleGroupEdit2.aspx.cs" Inherits="Mars.Server.WebApp.Article.ArticleGroupEdit2" %>

<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="asyncbox_js" runat="server" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
        <h4 class="lighter">编辑专题分组</h4>
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="row-fluid">
                <form runat="server" class="form-horizontal" id="validationf">

                    <div class="control-group">
                        <label class="control-label" for="txtGroupEventName">专题分组名称:</label>
                        <div class="controls">
                            <div class="span12">
                                <input type="text"  id="txtGroupEventName" runat="server" name="txtGroupEventName" />
                            </div>
                        </div>
                    </div>


                    <div class="control-group">
                        <label class="control-label" for="txtPublishTime">发布日期:</label>
                        <div class="controls">
                            <div class="span12">
                                <input type="text" id="txtPublishTime" name="txtPublishTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" />
                            </div>
                        </div>
                    </div>

                    <div class="hr hr-dotted"></div>
                    <div style="text-align: center;">
                        <button class="btn btn-success" onclick="javascript:save();">保存</button>
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
                    ctl00$content$txtGroupEventName: {
                        required: true, maxlength:50, remote: {
                            type: "POST",
                            url: _root + "handlers/ArticleController/IsUseableGroupName.ashx",
                            data: {
                                "pid": function () { return $("#content_hidPid").val() },
                                "groupeventname": function () { return $("#content_txtGroupEventName").val(); }
                            }
                        }
                    },
                    ctl00$content$txtPublishTime:{ required: true }
                },
                messages: {
                    ctl00$content$txtGroupEventName: { required: "必填项", maxlength: "长度最多不能超过50个字符", remote: "系统中已存在该专题分组名称" },
                    ctl00$content$txtPublishTime: { required: "必填项" }
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
                var pid = $("#content_hidPid").val();
                var txtGroupEventName = $("#content_txtGroupEventName").val();
                var txtPublisTime = $("#content_txtPublishTime").val();             

                var data = { "groupid": pid, "groupname": txtGroupEventName, "publishtime": txtPublisTime, "ts": new Date().getTime() };
                $.post(_root + "handlers/ArticleController/GroupUpdate.ashx", data, function (data) {
                    var json = eval("(" + data + ")");

                    if (json.status == "1") {
                        bootbox.dialog("操作成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                parent.refresh(true);
                                parent.asyncbox.close("editgroup");
                            }
                        }]);
                    }
                    else if (json.status == "0") {
                        bootbox.alert("操作失败!");
                        return false;
                    }
                    else if (json.status == "2") {
                        bootbox.alert("当前专题分组不存在或已被删除");
                        return false;
                    }                 
                    else {
                        bootbox.alert("数据传输错误!");
                        return false;
                    }
                });
            }
        }

        var closeset = function () {
            parent.asyncbox.close("editgroup");
        }
    </script>
</asp:Content>

