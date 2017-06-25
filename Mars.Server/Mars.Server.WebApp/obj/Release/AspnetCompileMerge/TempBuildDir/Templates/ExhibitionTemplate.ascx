<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExhibitionTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ExhibitionTemplate" %>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <td class="center">展会名称</td>
        <td class="center span2">开始时间</td>
        <td class="center span2">结束时间</td>
        <td class="center span2">操作</td>
    </tr>
    {#foreach $T.list as row}   
    <tr>
        <td class="left">{$T.row.ExhibitionTitle}</td>
        <td class="center span2">{dateFormat($T.row.ExhibitionStartTime,1)}</td>
        <td class="center span2">{dateFormat($T.row.ExhibitionEndTime,1)}</td>
        <td class="center">
            <button class="btn btn-success btn-mini" style="float:left;margin-left:10px;" onclick="javascript:modExhibition({$T.row.ExhibitionID});">编辑</button>
            <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delExhibition({$T.row.ExhibitionID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>
