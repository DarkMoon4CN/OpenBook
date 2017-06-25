<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsoleActivityTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ConsoleActivityTemplate" %>
<%/*后台使用表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span1">序号</th>
        <th class="center">活动名称</th>
        <th class="center span2">开始时间</th>
        <th class="center span2">结束时间</th>
        <th class="center span3">活动位置</th>
        <th class="center span2">主板单位</th>
        <th class="center span3">活动简介</th>
        <th class="center span2">嘉宾</th>
        <th class="center span1">子活动</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span1">{$T.row.rowId}</td>
        <td class="left">{$T.row.ActivityTitle}</td>
        <td class="center span2">{$T.row.FormatActivityStartTime}</td>
        <td class="center span2">{$T.row.FormatActivityEndTime}</td>
        <td class="center span3">{$T.row.ActivityLocation}</td>
        <td class="center span2">{$T.row.ActivityHostUnit}</td>
        <td class="center span3">{$T.row.ActivityAbstract}</td>
        <td class="center span2">{$T.row.ActivityGuest}</td>
        <td class="center span1">
            {#if $T.row.Cnt>0}
             有
            {#else}
               无
            {#/if}
        </td>
        <td class="center span2">
            <div class="inline position-relative">
                <button class="btn btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                    <li><a title="" class="tooltip-default" href="javascript:" data-rel="tooltip" onclick="javascript:openChildren({$T.row.ActivityID},'{$T.row.ActivityTitle}');"><span class="green">子活动</span></a></li>
                    <li><a title="" class="tooltip-success" href="javascript:" data-rel="tooltip" onclick="javascript:modActivity({$T.row.ActivityID});"><span class="green">编辑</span></a></li>
                    <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:delActivity({$T.row.ActivityID});"><span class="red">删除</span></a></li>
                </ul>
            </div>
        </td>
    </tr>
    {#/foreach}
</table>
