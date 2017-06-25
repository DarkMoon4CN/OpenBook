<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleGroupRelTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ArticleGroupRelTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphArticleGroupRel" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thRowID" CssClass="center span1" Text="序号" BindingFiled="rowId" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thTitle" CssClass="center" Text="标题" BindingFiled="Title" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thFirstTypName" CssClass="center" Text="一级分类" BindingFiled="FirstTypName" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thSecondTypeName" CssClass="center" Text="二级分类" BindingFiled="SecondTypeName" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thIsSingleGroupState" CssClass="center" Text="单篇成组" BindingFiled="IsSingleGroupState" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thPictureID" CssClass="center" Text="封面图片" BindingFiled="PictureID" OutPutOrder="6" />
                <OpenBook:Th runat="server" ID="thDisplayOrder" CssClass="center" Text="显示顺序" BindingFiled="DisplayOrder" OutPutOrder="7" />
                <th class="center">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.EventItemID}" pid="{$T.row.EventItemID}">
                <td class="center">{$T.row.rowId}</td>
                <td class="center"><a target="_blank" href="{$T.row.Url}">{$T.row.Title}</a></td>
                <td class="center">{$T.row.FirstTypName}</td>
                <td class="center">{$T.row.SecondTypeName}</td>
                <td class="center">
                    {#if $T.row.IsSingleGroupState == 0}
                       <span class="badge badge-warning">否</span>                   
                    {#else}
                        <span class="badge badge-success">是</span>
                    {#/if}
                </td>
                <td class="center">{#if $T.row.PictureID > 0 }
                         <span class="badge badge-success">有</span>
                    {#else}
                         <span class="badge badge-warning">无</span>
                    {#/if}
                </td>
                <td class="center">{$T.row.DisplayOrder}</td>
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown" onclick="javascript:delitem({$T.row.EventItemID});">删除</button>
                    </div>
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-success dropdown-toggle" data-toggle="dropdown" onclick="javascript:setorder({$T.row.EventGroupID},{$T.row.EventItemID});">设置顺序</button>
                    </div>
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>
