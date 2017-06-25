<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FestivalTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.FestivalTemplate" %>
<table class="table table-striped table-bordered table-hover" id="table_bug_report">
    <tr>
        <td class="center span1">序号</td>
        <td class="center span2">节日名称</td>
        <td class="center span2">开始时间</td>
        <td class="center span2">结束时间</td>
        <td class="center span2">节日类型</td>
        <td class="center span2">关联文章</td>
        <td class="center span2">节日权重</td>
        <td class="center span2">操作</td>
    </tr>
    {#foreach $T.list as row}   
    <tr>
        <td class="center span1">{$T.row.rowId}</td>
        <td class="center span2">{$T.row.FestivalName}</td>
        <td class="center span2">{$T.row.StartTime}</td>
        <td class="center span2">{$T.row.EndTime}</td>
        <td class="center span2">{#if $T.row.FestivalType==1}班{#/if}{#if $T.row.FestivalType==2}休{#/if}{#if $T.row.FestivalType==3}节日{#/if}
        </td>
        <td class="center span2">
            {#if $T.row.EventItemGUID!=null}
            <a href="/news/{$T.row.EventItemGUID}.html" target="_blank">{$T.row.Title}</a>
            {#/if}
        </td>
        <td class="center span2">{$T.row.FestivalWeight}</td>
        <td class="center">
            <div class="inline position-relative">
                <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                    <li><a title="" class="tooltip-success" href="FestivalEdit.aspx?fun={$T.fun}&id={$T.row.FestivalID}" data-original-title="Edit" data-placement="left" data-rel="tooltip"><span class="green">编辑日历</span></a></li>
                    <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:delFestival('{$T.row.FestivalID}');"><span class="red">删除</span></a></li>
                </ul>
            </div>
        </td>
    </tr>
    {#/foreach}
</table>
