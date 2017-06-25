<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppUpdateTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.AppUpdateTemplate" %>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <td class="center span1">序号</td>
        <td class="center span2">系统</td>
        <td class="center span2">版本号</td>
        <td class="center span2">下载链接</td>
        <td class="center span2">强制升级</td>
        <td class="center span2">更新时间</td>
        <td class="center span2">大小(MB)</td> 
        <td class="center span2">操作</td>
    </tr>
     {#foreach $T.list as row}   
    <tr>
        <td class="center span1">{$T.row.rowId}</td>
        <td class="center span2">{#if $T.row.appType==1}Android{#else}IOS{#/if}</td>
        <td class="center span2">{$T.row.version}</td>
        <td class="center span2">{$T.row.downloadUrl}</td>
        <td class="center span2">{#if $T.row.forcedUpdate==0}否{#else}是{#/if}</td>
        <td class="center span2">{$T.row.createTime}</td>
        <td class="center span2">{$T.row.appSize}</td> 
         <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="AppUpdateEdit.aspx?fun={$T.fun}&id={$T.row.appId}&flg=1" data-original-title="Edit" data-placement="left" data-rel="tooltip"><span class="green">编辑系统</span></a></li> 
                            <li><a title="" class="tooltip-success" href="javascript:void()" data-original-title="Edit" data-placement="left" data-rel="tooltip" onclick="javascript:deleteAppUpdate({$T.row.appId});"><span class="red">删除</span></a></li> 
                        </ul>
                    </div>
                </td> 
    </tr>
    {#/foreach}
</table>
