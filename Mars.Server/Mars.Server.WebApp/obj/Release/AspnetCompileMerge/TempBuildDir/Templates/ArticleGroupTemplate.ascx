<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleGroupTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ArticleGroupTemplate" %>

<OpenBook:MyPlaceHolder runat="server" ID="mphArticleGroup" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thRowID" CssClass="center span1" Text="序号" BindingFiled="rowId" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thTitle" CssClass="center" Text="专题分组名称" BindingFiled="GroupEventName" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thFirstTypName" CssClass="center" Text="发布时间" BindingFiled="PublishTime" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thSecondTypeName" CssClass="center" Text="创建时间" BindingFiled="CreatedTime" OutPutOrder="4" />                              
                <th class="center span2">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.GroupEventID}">
                <td class="center">{$T.row.rowId}</td>
                <td><a href="javascript:" >{$T.row.GroupEventName}</a></td>
                <td class="center">{$T.row.PublishTime}</td>
                <td class="center">{$T.row.CreatedTime}</td>                      
              
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="javascript:" data-original-title="Edit" onclick="javascript:editgroup({$T.row.GroupEventID})" data-placement="left" data-rel="tooltip"><span class="green">修改</span></a></li>
                            <li><a title="" class="tooltip-warning" href="EventItemGroupEdit.aspx?fun={$T.fun}&groupid={$T.row.GroupEventID}" data-rel="tooltip" ><span class="green">维护专题文章</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:deleteobj({$T.row.GroupEventID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>