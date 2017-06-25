<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExhibitorTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ExhibitorTemplate" %>
<%/*后台使用表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span1">序号</th>
        <th class="center">展商名称</th>
        <th class="center span3">展位位置</th>
        <th class="center span1">书目</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span1">{$T.row.rowId}</td>
        <td class="left">{$T.row.ExhibitorName}</td>
        <td class="center span3">
            {#foreach $T.row.ExhibitorLocationList as locrow}
                {$T.locrow.ExhibitorLocation}<br />
            {#/foreach}
        </td>
        <td class="center span1">
            <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chksd_{$T.row.ExhibitorID}" uid="{$T.row.ExhibitorID}" {getCheckStateByTrueFalse($T.row.IsHadBookList)} 
                        onchange="javascript:changedBookListState({$T.row.ExhibitorID},this);"/><span class="lbl"></span>
        </td>
        <td class="center span2">
            <button class="btn btn-success btn-mini" style="float:left;margin-left:10px;" onclick="javascript:modExhibitor({$T.row.ExhibitorID});">编辑</button>
            <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delExhibitor({$T.row.ExhibitorID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>