<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_StartPicList.aspx.cs" Inherits="Mars.Server.WebApp.AppConfigure._StartPicList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">App配置</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>App配置<small><i class="icon-double-angle-right"></i>配置开屏图片</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">

        </div>
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_add" onclick="javascript:addPic();">添加</button>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpStartPicList" runat="server" TemplateSrc="~/Templates/StartPicListTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" UseRequestCache="false" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var changedDefault = function (id, obj) {
            var isdefault = 0;
            var originStatus = !obj.checked;
            if (obj.checked) {
                isdefault = 1;
            }
            $.post(_root + "handlers/StartPicController/ChangeDefault.ashx", { "_id": id, "_isdefault": isdefault, "ts": new Date().getTime() }, function (data) {
                var json = eval("(" + data + ")");
                if (json.state != "1") {
                    bootbox.alert(json.msg);
                    obj.checked = originStatus;
                }
            });
        }

        var delPicture = function (id) {
            bootbox.confirm("您确认要删除项吗?", function (result) {
                if (result) {
                    $.post(_root + "handlers/StartPicController/DeleteStartPic.ashx", { "_id": id, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpStartPicList")._prmsData.ts = new Date().getTime();
                            TObj("tmpStartPicList").S();
                        }
                        else if (json.state == "0") {
                            bootbox.alert(json.msg);
                            return false;
                        }
                        else {
                            bootbox.alert("数据传输错误");
                            return false;
                        }
                    });
                }
            });
        }

        var addPic = function () {
            asyncbox.open({
                modal: true,
                id: "addstartpic",
                title: "添加开屏图片",
                url: _root + "AppConfigure/_StartPicEdit.aspx",
                width: 950,
                height: 650
            });
        }
    </script>
</asp:Content>
