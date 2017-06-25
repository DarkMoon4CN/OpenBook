<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_ExhibitorEdit.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ExhibitorEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group">
            <label class="control-label" for="sel_exhibition">所属展场:</label>
            <div class="controls">
                <select class="span10" id="sel_exhibition" runat="server">
                    <option value="">==请选择==</option>
                </select>
            </div>
        </div>
        
        <div class="control-group">
            <label class="control-label" for="txt_exhibitorname">展商名称:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_exhibitorname" id="txt_exhibitorname" placeholder="展商名称" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="txt_exhibitorlocations">展位:</label>
            <div class="controls">
                <textarea class="span10" name="txt_exhibitorlocations" id="txt_exhibitorlocations" placeholder="展位" rows="4" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="chk_ishadbooklist">是否有书单:</label>
            <div class="controls">
                <input name="chk_ishadbooklist" class="ace-switch ace-switch-2" type="checkbox" id="chk_ishadbooklist" runat="server" /><span class="lbl"></span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="chk_statetype">是否启用:</label>
            <div class="controls">
                <input name="chk_statetype" class="ace-switch ace-switch-2" type="checkbox" id="chk_statetype" runat="server" /><span class="lbl"></span>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <a href="javascript:" id="submit" class="btn btn-purple" onclick="javascript:addExhibitor();">保存</a>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_eid" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("exhibitor");
        }

        var addExhibitor = function () {
            if (checkForm()){
                var pms = {
                    "id": $("#content_hid_eid").val(),
                    "ename": escape($("#content_txt_exhibitorname").val()),
                    "eid": $("#content_sel_exhibition").val(),
                    "eloc": escape($("#content_txt_exhibitorlocations").val()),
                    "ebook": $("#content_chk_ishadbooklist").get(0).checked,
                    "estate": $("#content_chk_statetype").get(0).checked,
                    "ts":new Date().getTime()
                };
                
                $.post(_root + "handlers/ExhibitionController/ExhibitorEdit.ashx", pms, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "1") {
                        bootbox.alert(json.msg, function () {
                            if (window.parent != undefined) {
                                window.parent.TObj("tmpExhibitorList")._prmsData.ts = new Date().getTime();
                                window.parent.TObj("tmpExhibitorList").S();
                                window.parent.asyncbox.close("exhibitor");
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
            if ($.trim($("#content_sel_exhibition").val()) == "")
            {
                error += "请选择所属展场\n\r";
            }
            if ($.trim($("#content_txt_exhibitorname").val()) == "") {
                error += "请填写展商名称\n\r";
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
