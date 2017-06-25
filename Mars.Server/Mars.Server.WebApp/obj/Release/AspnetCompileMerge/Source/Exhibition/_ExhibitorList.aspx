<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="_ExhibitorList.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ExhibitorList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">展会地图</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>展会地图 <small><i class="icon-double-angle-right"></i>展商信息</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
       <div class="control-group" style="float: left;">
           <label class="control-label" for="txt_exhibitorname">展场名称</label>
            <div class="controls">
                <select class="searchpart" id="sel_exhibition" class="searchpart"  key="_exhibitionid">
                    <option value="1">2016年北京图书订货会</option>
                </select>
            </div>
            <label class="control-label" for="txt_exhibitorname">展商名称</label>
            <div class="controls">
                <input type="text" id="txt_exhibitorname" class="searchpart"  key="_exhibitorname"  placeholder="展商名称" />
            </div>
        </div> 
        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" id="btn_search">查询</button>
            <button class="btn btn-success" id="btn_import" onclick="javascript:importExhibitor();">批量导入</button>
            <button class="btn btn-primary" onclick="javascript:addExhibitor();">添加展商</button>
        </div>
           
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
         <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpExhibitorList" runat="server" TemplateSrc="~/Templates/ExhibitorTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" EnablePagination="true" PageSize="20" SearchControlID="btn_search" UseRequestCache="false"/> 
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var changedBookListState = function (id,obj) {
            var ishadbooklist = 0;
            var originStatus = !obj.checked;
            if (obj.checked) {
                ishadbooklist = 1;
            }
            $.post(_root + "handlers/ExhibitionController/ChangeExhibitorIsHadBookList.ashx", { "_id": id, "_ishadbooklist": ishadbooklist, "ts": new Date().getTime() }, function (data) {
                var json = eval("(" + data + ")");
                if (json.state != "1") {
                    bootbox.alert(json.msg);
                    obj.checked = originStatus;
                }
            });
        }

        var delExhibitor = function (id) {
            bootbox.confirm("您确认要删除项吗?", function (result) {
                if (result) {
                    $.post(_root + "handlers/ExhibitionController/DeleteExhibitor.ashx", { "_id": id, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.alert(json.msg);
                            TObj("tmpExhibitorList")._prmsData.ts = new Date().getTime();
                            TObj("tmpExhibitorList").S();
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

        var addExhibitor = function () {
            asyncbox.open({
                modal: true,
                id:"exhibitor",
                title: "添加展商",
                url: _root + "Exhibition/_ExhibitorEdit.aspx",
                width: 950,
                height: 550
            });
        }

        var modExhibitor = function (id) {
            asyncbox.open({
                modal: true,
                id: "exhibitor",
                title: "编辑展商",
                url: _root + "Exhibition/_ExhibitorEdit.aspx?id="+id,
                width: 950,
                height: 550
            });
        }

        var importExhibitor = function () {
            asyncbox.open({
                modal: true,
                id: "exhibitorimport",
                title: "导入展商",
                url: _root + "Exhibition/_ExhibitorImport.aspx",
                width: 950,
                height: 250
            });
        }
    </script>
</asp:Content>
