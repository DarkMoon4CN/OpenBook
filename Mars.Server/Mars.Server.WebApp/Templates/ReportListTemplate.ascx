<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ReportListTemplate" %>
<%/*后台使用用户举报信息表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span1">序号</th>
        <th class="center span2">举报用户</th>
        <th class="center span2">被举报用户</th>
        <th class="center span2">举报文章类型</th>
        <th class="center span2">文章/回复内容</th>
        <th class="center span2">提交时间</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span1">{$T.row.rowId}</td>
        <td class="center span2">{$T.row.FormUserName}</td>
        <td class="center span2">{$T.row.ToUserName}</td>
        <td class="center span2">{$T.row.ReportInfoTypeDesc}</td>
        <td class="center span2">{$T.row.ReportInfo}</td>
        <td class="center span2">{$T.row.FormatCreateTime}</td>
        <td class="center span2">
            <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delReport({$T.row.ReportID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>
