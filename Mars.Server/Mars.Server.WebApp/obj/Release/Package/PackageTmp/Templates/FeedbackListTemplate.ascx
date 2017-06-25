<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.FeedbackListTemplate" %>
<%/*后台使用用户反馈信息表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span1">序号</th>
        <th class="center">反馈内容</th>
        <th class="center span2">用户名</th>
        <th class="center span2">提交时间</th>
        <th class="center span2">联系方式</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span1">{$T.row.rowId}</td>
        <td class="left">{$T.row.Content}</td>
        <td class="center span2">{$T.row.UserName}</td>
        <td class="center span2">{$T.row.FormatCreateTime}</td>
        <td class="center span2">{$T.row.ContactMethod}</td>
        <td class="center span2">
            <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delFeedback({$T.row.FeedbackID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>
