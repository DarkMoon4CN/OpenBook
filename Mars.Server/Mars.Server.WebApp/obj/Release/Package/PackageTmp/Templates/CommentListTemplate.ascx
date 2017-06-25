<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.CommentListTemplate" %>
<%/*后台使用文章评论信息表格控件*/%>
<table class="table table-striped table-bordered table-hover">
    <tr>
        <th class="center span2">用户ID</th>
        <th class="center span2">用户昵称</th>
        <th class="center span2">匿名</th>
        <th class="center span2">源于文章</th>
        <th class="center span2">评论内容</th>
        <th class="center span2">评论时间</th>
        <th class="center span2">点赞数</th>
        <th class="center span2">回复数</th>
        <th class="center span2">审核状态</th>
        <th class="center span2">敏感词</th>
        <th class="center span2">操作</th>
    </tr>
    {#foreach $T.list as row}   
    <tr class="">
        <td class="center span2">{$T.row.UserID}</td>
        <td class="center span2">{$T.row.NickName}</td>
        <td class="center span2">{$T.row.IsAnonymous}</td>
        <td class="center span2">
            <a href="/news/{$T.row.EventItemGUID}.html" target="_blank">{$T.row.Title}</a></td>
        <td class="left" style="width:400px;word-break:break-all;">{$T.row.CommentContent}</td>
        <td class="center span2">{$T.row.FormatCommentTime}</td>
        <td class="center span2">{$T.row.LikeCnt}</td>
        <td class="center span2">{$T.row.ReplyCnt}</td>
        <td class="center span2">{$T.row.CheckTypeDesc}</td>
        <td class="left" style="width:200px;word-break:break-all;">{$T.row.WordsInfo}</td>
        <td class="center span2">
            <select class="" style="width:128px;" id="sel_{$T.row.CommentID}" onchange="javascript:changeViewState(this);" cid="{$T.row.CommentID}">
                {#if $T.row.ViewStateID==0}
                <option value="0" selected="selected">前台隐藏</option>
                <option value="1">前台显示</option>
                {#else}
                <option value="0">前台隐藏</option>
                <option value="1"selected="selected">前台显示</option>
                {#/if}
            </select>
        </td>
    </tr>
    {#/foreach}
</table>
