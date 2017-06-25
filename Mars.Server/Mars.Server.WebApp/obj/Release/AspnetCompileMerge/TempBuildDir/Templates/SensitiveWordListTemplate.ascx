<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SensitiveWordListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.SensitiveWordListTemplate" %>
<%/*后台使用敏感词库信息表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span2">敏感词ID</th>
        <th class="center">敏感词</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span2">{$T.row.SWID}</td>
        <td class="center">{$T.row.SensitiveWords}</td>
        <td class="center span2">
            <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delSensitiveWord({$T.row.SWID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>
