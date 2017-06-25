<%@ Page Title="" Language="C#" MasterPageFile="~/Exhibition/Exhibition.Master" AutoEventWireup="true" CodeBehind="ActivityList.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition.ActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="autocomplete_css" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="autocomplete_js" Src="~/js/plugin/Autocomplete/jquery-autocomplete-1.10.4.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="activity_js" Src="~/js/exhibitionactivity.js" ScriptType="Javascript" />
    <%--<script type="text/javascript">
        $(function () {
            $("#search_activity").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: _root + "handlers/ExhibitionController/GetActivityList.ashx",
                        dataType: "jsonp",
                        data: {
                            top: 5,
                            key: request.term,
                            type: 1,
                            exhibitionid: $("#hid_exhibitionid").val()
                        },

                        success: function (data) {
                            response($.map(data.activitylist, function (item) {
                                return {
                                    label: item.name, value: item.name, key: item.atitle
                                }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    $("#search_activity").val(ui.item.key);
                    searchActivity();
                }
            });

            $("#search_activity").keydown(function () {
                if (event.keyCode == "13") {//keyCode=13是回车键
                    searchActivity();
                }
            });
        });

        var searchActivity = function () {
            var obj = TObj("holder");
            obj._prmsData._searchname = escape($("#search_activity").val());
            obj._prmsData._pageIndex = 0;
            if (obj.paginationType < 2) {
                obj.loadData();
            }
            else {
                obj.InitscrollPagination();
            }
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <!--main-->
    <!--search-->
    <div class="con-main">
        <div class="search-box">
             <div class="search" id="search_activity" onclick="javascript: openSearch(2);">输入活动名称</div>
        </div>
    </div>
    <!--search-->
    <!--activePage-->
    <section class="main-content">
        <OpenBook:TemplateWrapper ID="holder" runat="server"
            TemplateSrc="~/Templates/ActivityTemplate.ascx"
            PageSize="10" EnablePagination="true" PaginationType="Scrolling" AutoLoadData="true"
            HttpMethod="Get" DebugMode="true"></OpenBook:TemplateWrapper>
    </section>
    <!--main-->
    <input class="searchpart" type="hidden" id="hid_exhibitionid" value="1" key="_exhibitionid" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
</asp:Content>
