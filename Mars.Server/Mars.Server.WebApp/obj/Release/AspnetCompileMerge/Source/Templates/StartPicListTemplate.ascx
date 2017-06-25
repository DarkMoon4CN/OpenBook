<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StartPicListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.StartPicListTemplate" %>
<%/*后台使用开屏信息表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span2">图片ID</th>
        <th class="center span2">URL</th>
        <th class="center span2">图片</th>
        <th class="center span2">开始时间</th>
        <th class="center span2">结束时间</th>
        <th class="center span2">默认图片</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span2">{$T.row.PictureID}</td>
        <td class="center span2">{$T.row.Domain}/{$T.row.PicturePath}</td>
        <td class="center span2"><img alt="" src="{$T.row.Domain}/{$T.row.PicturePath}?imageView2/0/w/100/h/160/interlace/1/format/jpg" /></td>
        <td class="center span2">{$T.row.FormatStartTime}</td>
        <td class="center span2">{$T.row.FormatEndTime}</td>
        <td class="center span2">
            <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chksd_{$T.row.PictureID}" uid="{$T.row.PictureID}" {getCheckState($T.row.IsDefault,'1',true)} 
                        onchange="javascript:changedDefault({$T.row.PictureID},this);"/><span class="lbl"></span></td>
        <td class="center span2">
           <button class="btn btn-warning btn-mini" style="float:left;margin-left:10px;" onclick="javascript:delPicture({$T.row.PictureID});">删除</button>
        </td>
    </tr>
    {#/foreach}
</table>
