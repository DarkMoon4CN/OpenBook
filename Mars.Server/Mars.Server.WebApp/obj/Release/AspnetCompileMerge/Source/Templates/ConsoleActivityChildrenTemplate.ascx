<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsoleActivityChildrenTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ConsoleActivityChildrenTemplate" %>
<%/*后台使用表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span1">序号</th>
        <th class="center">子活动名称</th>
        <th class="center span2">开始时间</th>
        <th class="center span2">结束时间</th>
        <th class="center span3">子活动位置</th>
        <th class="center span2">子活动嘉宾</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span1">{$T.row.rowId}</td>
        <td class="left">{$T.row.ActivityTitle}</td>
        <td class="center span2">{$T.row.FormatActivityStartTime}</td>
        <td class="center span2">{$T.row.FormatActivityEndTime}</td>
        <td class="center span3">{$T.row.ActivityLocation}</td>
        <td class="center span2">{$T.row.ActivityGuest}</td>
        <td class="center span2">
            <div class="inline position-relative">
                <button class="btn btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                    <li><a title="" class="tooltip-success" href="javascript:" data-rel="tooltip" onclick="javascript:modActivity({$T.row.ActivityID});"><span class="green">编辑</span></a></li>
                    <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:delActivity({$T.row.ActivityID});"><span class="red">删除</span></a></li>
                </ul>
            </div>
        </td>
    </tr>
    {#/foreach}
</table>
