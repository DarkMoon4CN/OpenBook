<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="SetGroupArticleOrder.aspx.cs" Inherits="Mars.Server.WebApp.Article.SetGroupArticleOrder" %>

<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript ID="bootbox_js" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="asyncbox_js" runat="server" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="validate_js" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
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
                        <label class="control-label" for="txtTitle">文章标题:</label>
                        <div class="controls">
                            <div class="span12">
                                <input type="text" id="txtTitle" runat="server" name="txtTitle" disabled="disabled" />
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label" for="txtDisplayOrder">显示顺序:</label>
                        <div class="controls">
                            <div class="span12">
                                <input type="text" id="txtDisplayOrder" name="txtDisplayOrder" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="hr hr-dotted"></div>
                    <div style="text-align: center;">
                        <button class="btn btn-success" onclick="javascript:save();">保存</button>
                        <button class="btn btn-danger" onclick="javascript:closeset();">关闭</button>
                    </div>
                </form>
            </div>
        </div>
        <!--/widget-main-->
        <input type="hidden" id="hid_GroupID" runat="server" />
        <input type="hidden" id="hid_ItemID" runat="server" />
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
                    ctl00$content$txtDisplayOrder: {
                        required: true, digits: true, remote: {
                            type: "POST",
                            url: _root + "handlers/ArticleController/IsUseableOrderByGroupArticle.ashx",
                            data: {
                                groupid: function () { return $("#content_hid_GroupID").val() },
                                itemid: function () { return $("#content_hid_ItemID").val() },
                                displayorder: function () { return $("#content_txtDisplayOrder").val() }
                            }
                        }
                    },
                },
                messages: {
                    ctl00$content$txtDisplayOrder: { required: "必填项", maxlength: "请输入数字", remote: "当前专题分组已有文章设置该显示顺序号" },
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
                var hidGroupID = $("#content_hid_GroupID").val();
                var hidItemID = $("#content_hid_ItemID").val();
                var txtDisplayOrder = $("#content_txtDisplayOrder").val();             

                var data = { "groupid": hidGroupID, "itemid": hidItemID, "displayorder": txtDisplayOrder, "ts": new Date().getTime() };
                $.post(_root + "handlers/ArticleController/UpdateGroupRelOrder.ashx", data, function (data) {
                    var json = eval("(" + data + ")");

                    if (json.status == "1") {
                        bootbox.dialog("操作成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                parent.refresh(true);
                                parent.asyncbox.close("setgroupatricleorder");
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
            parent.asyncbox.close("setgroupatricleorder");
        }
    </script>
</asp:Content>

