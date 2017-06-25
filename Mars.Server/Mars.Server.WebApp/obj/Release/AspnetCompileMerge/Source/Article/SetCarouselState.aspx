<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="SetCarouselState.aspx.cs" Inherits="Mars.Server.WebApp.Article.SetCarouselState" %>

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
        <h4 class="lighter">设置发现频道轮播</h4>
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="row-fluid">
                <form runat="server" class="form-horizontal" id="validationf">

                    <div class="control-group">
                        <label class="control-label" for="chkAdvert">是否发现频道轮播:</label>
                        <div class="controls">
                            <div class="span3">
                                <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkAdvert" runat="server" /><span class="lbl"></span>
                            </div>
                        </div>
                    </div>

                    <div class="control-group">
                        <label class="control-label" for="txtAdvertOrder">轮播结束时间:</label>
                        <div class="controls">
                            <div class="span12">
                                <input type="text" id="txtAdsEndTime" name="txtAdsEndTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" runat="server" />
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
            //$("#content_chkAdvert").click(function () {
            //    if ($(this)[0].checked) {
            //        $("#content_txtAdvertOrder").attr({ "disabled": false, "value": "" });
            //    }
            //    else {
            //        $("#content_txtAdvertOrder").attr({ "disabled": true, "value": 1000 });
            //    }
            //});

            $("#validationf").validate({
                errorEvent: "span",
                errorClass: "help-inline",
                foucusInvalid: false,
                rules: {
                    //ctl00$content$txtAdvertOrder: {
                    //    required: true, digits: true, remote: {
                    //        type: "POST",
                    //        url: _root + "handlers/ArticleController/IsCanSetOrderNum.ashx",
                    //        data: {
                    //            "pid": function () { return $("#content_hidPid").val() },
                    //            "advertorder": function () { return $("#content_txtAdvertOrder").val(); }
                    //        }
                    //    }
                    //}
                },
                messages: {
                    //ctl00$content$txtAdvertOrder: { required: "必填项", digits: "请输入整数数字", remote: "轮播顺序号已被占用，请重新设置" }
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
                var chkAdvert = $("#content_chkAdvert")[0].checked ? 2 : 0;
                //var txtAdvertOrder = $("#content_txtAdvertOrder").val();

                var data = { "pid": pid, "status": chkAdvert, "adsendtime": $("#content_txtAdsEndTime").val(), "ts": new Date().getTime() };
                $.post(_root + "handlers/ArticleController/ChangeCarouselState.ashx", data, function (data) {
                    var json = eval("(" + data + ")");

                    if (json.status == "1") {
                        bootbox.dialog("操作成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                parent.refresh(true);
                                parent.asyncbox.close("changecarousel");
                            }
                        }]);
                    }
                    else if (json.status == "0") {
                        bootbox.alert("操作失败!");
                        return false;
                    }
                    else if (json.status == "2") {
                        bootbox.alert("发现轮播数量不能超过" + json.num + "个");
                        return false;
                    }
                    //else if (json.status == "3") {
                    //    bootbox.alert("轮播顺序号已被占用，请重新设置");
                    //    return false;
                    //}
                    else {
                        bootbox.alert("数据传输错误!");
                        return false;
                    }
                });
            }
        }

        var closeset = function () {
            parent.asyncbox.close("changeadvert");
        }
    </script>
</asp:Content>
