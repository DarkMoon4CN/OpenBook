<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_ReplyList.aspx.cs" Inherits="Mars.Server.WebApp.Article._ReplyList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">信息发布管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>信息发布管理<small><i class="icon-double-angle-right"></i>回复列表</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
            <div class="control-group" style="float: left;">
            <label class="control-label" for="txtStartTime">文章名称</label>
            <div class="controls">
                <input type="text" id="txt_title" class="searchpart" key="_title" placeholder="输入文章名称" />
            </div>
        </div>
           <div class="control-group" style="float: left;">
                <label class="control-label" for="sel_chkType">审核状态</label>
                <div class="controls">
                    <select id="sel_chkType" class="searchpart" key="_ct">
                        <option value="">全部</option>
                        <option value="1">审核通过</option>
                        <option value="0">未审核</option>
                        <option value="-1">审核未通过</option>
                    </select>
                </div>
            </div>
            <div class="control-group" style="float: left;">
                <label class="control-label" for="sel_viewType">操作状态</label>
                <div class="controls">
                    <select id="sel_viewType" class="searchpart" key="_vt">
                        <option value="">全部</option>
                        <option value="1">前台显示</option>
                        <option value="0">前台隐藏</option>
                    </select>
                </div>
            </div>
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_search">查询</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpReplyList" runat="server" TemplateSrc="~/Templates/ReplyListTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" SearchControlID="btn_search" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var changeViewState = function (obj) {
            $.post(_root + "handlers/ReplyController/ChangeViewState.ashx", { "_id": $(obj).attr("rid"), "_vs": $(obj).val(), "ts": new Date().getTime() }, function (data) {
                var json = eval("(" + data + ")");
                if (json.state != "1") {
                    bootbox.alert(json.msg);
                    return false;
                }
            });
        }
    </script>
</asp:Content>
