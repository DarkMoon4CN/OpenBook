<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleSelectTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ArticleSelectTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphArticleSelect" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                {#if $T.ShowFlag != 1}
                <th class="center span1">
                    <input type="checkbox" id="chkall" value="all" onclick="javascript: selectall(this);" style="opacity: inherit; position: inherit;" /></th>
                {#/if}
                <OpenBook:Th runat="server" ID="thRowID" CssClass="center span1" Text="序号" BindingFiled="rowId" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thTitle" CssClass="center span4" Text="标题" BindingFiled="Title" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thFirstTypName" CssClass="center" Text="一级分类" BindingFiled="FirstTypName" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thSecondTypeName" CssClass="center" Text="二级分类" BindingFiled="SecondTypeName" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thIsSingleGroupState" CssClass="center" Text="单篇成组" BindingFiled="IsSingleGroupState" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thPictureID" CssClass="center" Text="封面图片" BindingFiled="PictureID" OutPutOrder="5" />
                {#if $T.ShowFlag == 1}
                 <th class="center">操作</th>
                {#/if}                                          
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            {#if $T.row.PictureID > 0}
                <tr id="tr{$T.row.EventItemID}" pid="{$T.row.EventItemID}" converpic="{$T.row.PictureID}">
            {#else}
                <tr id="tr{$T.row.EventItemID}" pid="{$T.row.EventItemID}" >
            {#/if}
           
                {#if $T.ShowFlag != 1}
                <td class="center">
                    <input type="checkbox" id="chk{$T.row.EventItemID}" chktype="info" style="opacity: inherit; position: inherit;" uid="{$T.row.EventItemID}" /></td>
                {#/if}
                <td class="center">{$T.row.rowId}</td>
                <td><a target="_blank" href="{$T.row.Url}">{$T.row.Title}</a></td>
                <td>{$T.row.FirstTypName}</td>
                <td>{$T.row.SecondTypeName}</td>
                <td class="center">{#if $T.row.IsSingleGroupState == 0}
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
                {#if $T.ShowFlag == 1}
                  <td>
                      <div class="inline position-relative">
                          <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown" onclick="javascript:delitem('{$T.row.EventItemID}');">删除</button>
                      </div>
                  </td>
                {#/if}                        
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>
