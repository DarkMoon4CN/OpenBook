<%@ Page Title="" Language="C#" MasterPageFile="~/Exhibition/Exhibition.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="autocomplete_css" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="autocomplete_js" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="activity_js" Src="~/js/exhibitionactivity.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="search_js" Src="~/js/exhibitionsearch.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <section class="main-body">
        <div class="head-search">
            <div class="con-main search-input">
                <input type="image" name="" id="" value="" src="../images/search.png" />
                <input type="text" name="" style="width:82%;padding-right:17px;font-size:1.4rem;" id="search-keyword" placeholder="输入展商/活动名称" />
                <a href="javascript:" class="cancle" id="acontroller">取消</a>
            </div>
        </div>

        <div class="wrap">
            <div class="tabs">
                <a href="javascript:" class="active" searchtype="1">展商</a>
                <a href="javascript:" searchtype="2">活动</a>
                <div style="clear:both;"></div>
            </div>
            <div class="result-con">
                <div class="content-slide" id="div_exhibitor" style="display: block;">
                    <OpenBook:TemplateWrapper ID="exhibitorholder" runat="server"
                        TemplateSrc="~/Templates/ExhibitorsTemplate.ascx"
                        PageSize="10" EnablePagination="true" PaginationType="Scrolling" AutoLoadData="false"
                        HttpMethod="Get" DebugMode="true"></OpenBook:TemplateWrapper>
                </div>
                
                <div class="content-slide"  id="div_activity">
                    <OpenBook:TemplateWrapper ID="activityholder" runat="server"
                        TemplateSrc="~/Templates/ActivityTemplate.ascx"
                        PageSize="10" EnablePagination="true" PaginationType="Scrolling" AutoLoadData="false"
                        HttpMethod="Get" DebugMode="true"></OpenBook:TemplateWrapper>
                </div>
            </div>
        </div>
    </section>
    <input class="searchpart" type="hidden" id="hid_exhibitionid" value="1" key="_exhibitionid" />
    <input type="hidden" id="hid_searchtype" value="<%=_searchtype %>" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
</asp:Content>
