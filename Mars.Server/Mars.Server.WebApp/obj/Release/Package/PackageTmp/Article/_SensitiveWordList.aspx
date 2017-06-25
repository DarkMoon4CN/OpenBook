<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_SensitiveWordList.aspx.cs" Inherits="Mars.Server.WebApp.Article._SensitiveWordList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">信息发布管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>信息发布管理<small><i class="icon-double-angle-right"></i>敏感词库</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
            
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_add"  onclick="javascript:addSensitiveWord();">添加敏感词</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpSensitiveWordList" runat="server" TemplateSrc="~/Templates/SensitiveWordListTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
        <script type="text/javascript">
            var delSensitiveWord = function (id) {
                bootbox.confirm("您确认要删除此敏感词吗？", function (result) {
                    if (result) {
                        $.post(_root + "handlers/SensitiveWordController/ChangeStateType.ashx", { "_id": id, "_st": "0", "ts": new Date().getTime() }, function (data) {
                            var json = eval("(" + data + ")");
                            if (json.state == "1") {
                                TObj("tmpSensitiveWordList")._prmsData.ts = new Date().getTime();
                                TObj("tmpSensitiveWordList").loadData();
                                return true;
                            } else {
                                bootbox.alert(json.msg);
                                return false;
                            }
                        });
                    }
                });
            }

            var addSensitiveWord = function () {
                asyncbox.open({
                    modal: true,
                    id: "sensitiveword",
                    title: "添加敏感词",
                    url: _root + "Article/_SensitiveWordEdit.aspx",
                    width: 650,
                    height: 450
                });
            }
    </script>
</asp:Content>
