<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_ActivityImport.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ActivityImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <form id="frmImport" runat="server">
     <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group">
            <label class="control-label" for="content_sel_exhibition">所属展场:</label>
            <div class="controls">
                <select class="span10" id="sel_exhibition" runat="server">
                    <option value="">==请选择==</option>
                </select>
            </div>
        </div>
        
        <div class="control-group">
            <label class="control-label" for="content_file_activity">活动Excel:</label>
            <div class="controls">
                <asp:FileUpload runat="server" ID="file_activity" />
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <asp:Button CssClass="btn btn-purple" OnClientClick="return checkForm();" Text="导入" runat="server" OnClick="btn_submit_Click" ID="btn_submit"/>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
        </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
     <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("activityimport");
        }

        var checkForm = function () {
            var error = "";
            if ($.trim($("#content_sel_exhibition").val()) == "")
            {
                error += "请选择所属展场\n\r";
            }
            if ($.trim($("#content_file_activity").val()) == "") {
                error += "请选择上传的Excel\n\r";
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
