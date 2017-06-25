<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticleGroupManager.aspx.cs" Inherits="Mars.Server.WebApp.Article.ArticleGroupManager" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">平台文章专题分组维护</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>平台文章专题分组维护 <small><i class="icon-double-angle-right"></i>平台文章专题分组管理</small></h1>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtGroupEventName">分组名称</label>
            <div class="controls">
                <input type="text" id="txtGroupEventName" placeholder="请输入文章专题分组名称" />
            </div>
        </div>

        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
            <button class="btn btn-primary" onclick="javascript:add();">新增</button>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpArticleGroup" runat="server" TemplateSrc="~/Templates/ArticleGroupTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />

    <input type="hidden" id="hidGroupState" key="groupstate" value="1" class="searchpart"/>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var add = function () {
            window.location.href = "ArticleGroupEdit.aspx?fun=<%=fun %>";
        }

        var deleteobj = function (pid) {
            bootbox.confirm("您确认要删除该专题吗?(删除后将不可恢复)", function (result) {
                if (result) {
                    $.post(_root + "handlers/ArticleController/GroupDelete.ashx", { "groupid": pid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("删除成功!", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    search(true);
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("删除失败!");
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

        var refresh = function (reload) {
            search(reload);
        }

        var search = function (reload) {

            TObj("tmpArticleGroup")._prmsData.groupname = $("#txtGroupEventName").val();           

            if (reload) {
                TObj("tmpArticleGroup")._prmsData.ts = new Date().getTime();
            }

            TObj("tmpArticleGroup")._prmsData._pageIndex = 0;
            TObj("tmpArticleGroup").loadData();
        }

        var editgroup = function (groupid) {
            asyncbox.open({
                modal: true,
                id: "editgroup",
                title: "编辑专题分组",
                url: _root + "Article/ArticleGroupEdit2.aspx?pid=" + groupid,
                width: 700,
                height: 300,
                callback: function (btnRes, cntWin, reVal) {
                    if (btnRes == "sure") {
                        //searchtopic();
                    }
                }
            });
        }

        var refresh = function (reload) {
            search(reload);
        }

    </script>
</asp:Content>

