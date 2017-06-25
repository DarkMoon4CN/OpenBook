<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriesSecondManager.aspx.cs" Inherits="Mars.Server.WebApp.Categories.CategoriesSecondManager" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript Src="~/js/plugin/bootbox.min.js" ScriptType="javascript" runat="server" /> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
      <li class="active">二级分类维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
     <h1>二级分类维护 <small><i class="icon-double-angle-right"></i>维护后台二级分类</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtCateSecondName">一级分类</label>
            <div class="controls">
               <select id="txtCategoriesName" name="txtCategoriesName" runat="server" class="searchpart" key="ParentCalendarID">  
                   </select>
            </div>
        </div>    
        <div class="control-group" style="float: left;">
            <label class="control-label" for="textCateSecondName">二级分类</label>
            <div class="controls">
               <input type="text" id="textCateSecondName" name="textCateSecondName" placeholder="请输入二级分类"  /> 
            </div>
        </div>  
        <div class="btn-group" style="float: right;">
             
            <button class="btn btn-purple" onclick="javascript:search();" id="btsearch">查询</button>
           <%-- <button class="btn btn-primary" onclick="javascript:add();">新增</button>   --%>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
  <OpenBook:TemplateWrapper ID="tmpCategoriesList" runat="server" TemplateSrc="~/Templates/CategoriesTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20"  /> <%--SearchControlID="btsearch"--%>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        function add() {
            window.location.href = "CategoriesEdit.aspx?fun=<%=fun%>";
        }
        $(function () {
            $("#content_txtCategoriesName").change(function () { 
                var txtCategoriesvalue = $("#content_txtCategoriesName").val();
                TObj("tmpCategoriesList")._prmsData.ParentCalendarID = txtCategoriesvalue; 
                TObj("tmpCategoriesList")._prmsData._pageIndex = 0;
                TObj("tmpCategoriesList").loadData();
            });
        });
            function search() {
                var txtCategoriesvalue = $("#content_txtCategoriesName").val();
                var txtCategoriesName = $("#textCateSecondName").val();
                TObj("tmpCategoriesList")._prmsData.ParentCalendarID = txtCategoriesvalue;
                TObj("tmpCategoriesList")._prmsData.Categoriesname = txtCategoriesName;
                TObj("tmpCategoriesList")._prmsData.ts = new Date().getTime();
            TObj("tmpCategoriesList")._prmsData._pageIndex = 0;
            TObj("tmpCategoriesList").loadData();
            } 
            function deleteCategories(id) {
                bootbox.confirm("您确认要删除当前菜单项吗?(删除后将不可恢复)", function (result) {
                    if (result) {
                        $.ajax({
                            type: "post",
                            url: _root + "handlers/CategoriesController/DeleteCategories.ashx",
                            data: "id=" + id,
                            dataType: "json",
                            error: function () { alert("删除失败！") },
                            success: function (data) {
                                if (data.status == "1") {
                                    bootbox.dialog("删除成功!", [{
                                        "label": "OK",
                                        "class": "btn-small btn-primary",
                                        callback: function () {
                                            search();
                                        }
                                    }]);
                                }
                                else if (data.status == "0") {
                                    bootbox.dialog("删除失败!", [{
                                        "label": "OK",
                                        "class": "btn-small btn-primary"
                                    }])
                                }
                                else {
                                    bootbox.dialog("数据加载失败!", [{
                                        "label": "OK",
                                        "class": "btn-small btn-primary"
                                    }])
                                }
                            }
                        });
                    }
                });
            }
    </script>
</asp:Content>
