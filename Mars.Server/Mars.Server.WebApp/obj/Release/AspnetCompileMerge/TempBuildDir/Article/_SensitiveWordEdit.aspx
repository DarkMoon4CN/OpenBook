<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_SensitiveWordEdit.aspx.cs" Inherits="Mars.Server.WebApp.Article._SensitiveWordEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
        <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group">
            <label class="control-label" for="content_txt_sensitiveword">敏感词:</label>
            <div class="controls">
                <textarea class="span10" type="text" name="txt_sensitiveword" id="txt_sensitiveword" placeholder="敏感词" runat="server" rows="8"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label"></label>
            <div class="controls">
                多个敏感词导入每个词之间使用半角的逗号分隔，如：“<span style="color:red;">,</span>”
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="content_chk_isneedrecheck">是否重新检查:</label>
            <div class="controls">
                <input name="chk_isneedrecheck" class="ace-switch ace-switch-2" type="checkbox" id="chk_isneedrecheck" runat="server" /><span class="lbl"></span>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <a href="javascript:" id="submit" class="btn btn-purple" onclick="javascript:addSensitiveWord();">保存</a>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_swid" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("sensitiveword");
        }

        var addSensitiveWord = function () {
            if (checkForm()){
                var pms = {
                    "id": $("#hid_swid").val(),
                    "sw": escape($("#content_txt_sensitiveword").val()),
                    "st": $("#content_chk_isneedrecheck").get(0).checked,
                    "ts": new Date().getTime()
                };

                $.post(_root + "handlers/SensitiveWordController/SensitiveWordEdit.ashx", pms, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "1") {
                        bootbox.alert(json.msg, function () {
                            if (window.parent != undefined) {
                                window.parent.TObj("tmpSensitiveWordList")._prmsData.ts = new Date().getTime();
                                window.parent.TObj("tmpSensitiveWordList").loadData();
                                window.parent.asyncbox.close("sensitiveword");
                            }
                        });
                    } else {
                        bootbox.alert(json.msg);
                    }
                });
                
            }
        }

        var checkForm = function () {
            var error = "";
            if ($.trim($("#content_txt_sensitiveword").val()) == "") {
                error += "请填写敏感词\n\r";
            }
            if (error != "")
            {
                bootbox.alert(error);
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
