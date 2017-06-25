<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master" CodeBehind="SelectGroupArticle.aspx.cs" Inherits="Mars.Server.WebApp.Article.SelectGroupArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="OBScript3" Src="~/css/jquery-ui.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" Src="~/js/plugin/jquery.treeview.js" ID="OBScript5" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
        <h4 class="lighter">选择专题文章</h4>
        <div class="widget-toolbar">
            <label>
            </label>
        </div>
    </div>
    <div class="widget-body">
        <div class="widget-main">

            <div class="row-fluid">
        
                <div class="control-group form-horizontal">
                    <label class="control-label" for="txtsearch">文章标题查询:</label> 
                    <div class="controls">
                        <div class="span12">
                            <input type="text" class="span4" id="txtsearch" name="txtsearch" />
                            <button class="btn btn-small btn-primary" id="btn_searchbooks" onclick="javascript:searchreport(true);">查询</button>
                        </div>
                    </div>           
                </div>
            </div>
            <hr />
            <OpenBook:TemplateWrapper ID="tmpArticleList" runat="server" TemplateSrc="~/Templates/ArticleSelectTemplate.ascx"
                DebugMode="true" PaginationType="Classic" HttpMethod="Get" AutoLoadData="true" />
        </div>
        <!--/widget-main-->     
        <input type="hidden" id="articleids" name="articleids" value="" />    
        <input type="hidden" id="hidShowFlag" key="showflag" value="<%=showFlag %>" class="searchpart" />     
        <input type="hidden" id="hidSingleGroup" key="singlegroup" value="-1" class="searchpart"/>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var dialog = frameElement.api;

        $(function () {
            $(document).on("click", "input[chktype='info']", function () {
                var ids = $("#articleids").val();
                if (ids != "")
                {
                    var tempids = uniqueitem(ids + "," + $(this).attr("uid"));
                    $("#articleids").val(tempids);
                }
                else
                {
                    var tempids = uniqueitem($(this).attr("uid"));
                    $("#articleids").val(tempids);
                }             
                dialog.returnValue = $("#articleids").val();
            });
        });

        var uniqueitem = function (item) {
            if (item)
            {
                var temparray = $.unique(item.split(","));
                temparray = $.map(temparray, function (item) { return item != "" ? item : null; })
                return temparray.join(",");
            }
            return "";
        }

        var selectall = function (obj)
        {
            if (obj.checked)
            {
                $("input[chktype='info']").prop("checked", true);
            }
            else
            {
                $("input[chktype='info']").prop("checked", false);
            }
            dialog.returnValue = getselect();
        }

        var getselect = function()
        {
            var ids = $("#articleids").val();
            $("input[chktype='info']:checked").each(function () {               
                if (ids != "")
                {
                   ids += "," + $(this).attr("uid");
                }
                else
                {
                    ids += $(this).attr("uid");
                }
            });
           
            $("#articleids").val(uniqueitem(ids));          
            return ids;
        }       

        var searchreport = function (reload) {
            $("#articleids").val("");
            var keyword = $("#txtsearch").val();
            var showflag = $("#hidShowFlag").val();
            TObj("tmpArticleList")._prmsData.title = keyword;          
            TObj("tmpArticleList")._prmsData.showflag = showflag;
            if (reload) {
                TObj("tmpArticleList")._prmsData.ts = new Date().getTime();
            }
            TObj("tmpArticleList")._prmsData._pageIndex = 0;
            TObj("tmpArticleList").loadData();
        }
    </script>
</asp:Content>