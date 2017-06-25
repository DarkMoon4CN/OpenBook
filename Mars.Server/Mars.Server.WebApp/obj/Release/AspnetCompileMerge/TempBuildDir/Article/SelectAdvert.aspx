<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="SelectAdvert.aspx.cs" Inherits="Mars.Server.WebApp.Article.SelectAdvert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="OBScript3" Src="~/css/jquery-ui.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
        <h4 class="lighter">选择轮播</h4>
        <div class="widget-toolbar">
            <label>
            </label>
        </div>
    </div>

    <div class="alert-block form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtTitle">文章名称</label>
            <div class="controls">
                <input type="text" id="txtTitle" placeholder="请输入文章名称" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtStartTime">开始时间</label>
            <div class="controls">
                <input type="text" id="txtStartTime" class="Wdate" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({ onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}'})" placeholder="请输入有效开始时间" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtEndTime">结束时间</label>
            <div class="controls">
                <input type="text" id="txtEndTime" class="Wdate" onfocus="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' })" placeholder="请输入有效结束时间" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selFirstType">一级分类</label>
            <div class="controls">
                <select id="selFirstType">
                    <option value="-1">--请选择--</option>
                </select>
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selSecondType">二级分类</label>
            <div class="controls">
                <select id="selSecondType">
                    <option value="-1">--请选择--</option>
                </select>
            </div>
        </div>

        <div class="btn-group" style="float: right; margin-right: 100px;">
            <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
        </div>
    </div>
    <div style="clear: both;"></div>
    <hr />
   <%-- <OpenBook:TemplateWrapper ID="tmpBookList" runat="server" TemplateSrc="~/Templates/SelectAdvertTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" AutoLoadData="false" />--%>

      <OpenBook:TemplateWrapper ID="tmpArticleList" runat="server" TemplateSrc="~/Templates/ArticlesTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />

    <%-- <div class="widget-body">
        <div class="widget-main">
            <div class="row-fluid">

                <hr />               
            </div>          
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

    </script>
</asp:Content>
