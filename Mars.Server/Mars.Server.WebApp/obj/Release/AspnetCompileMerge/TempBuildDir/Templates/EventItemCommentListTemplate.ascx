<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventItemCommentListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.EventItemCommentListTemplate" %>

{#if $T.cnt==0 && $T.pindex==1}
    <div class="comment-sofa" onclick="javascript:openComment();">
        <div class="comment-sofa_img">
            <img src="../images/sofa.png" />
            <input class="searchnodata" type="hidden" value=""/>
        </div>
        {#if $T.share>0}
        <p>暂无评论，点击进入客户端抢沙发</p>
        {#else}
        <p>暂无评论，点击抢沙发</p>
        {#/if}
    </div>
{#else}
{#foreach $T.list as row}
    <section class="comment-content" repeater="item">
        <div class="user-header">
            {#if $T.row.IsAnonymous}
                <img src="{$T.useranonymousheader}?imageView2/0/w/100/h/100/interlace/1/format/jpg" alt='' />
            {#else}
                {#if $T.row.UserPictuePath==null}
                    {#if $T.row.ThirdPictureUrl==null}
                        <img src="{$T.userdefaultheader}?imageView2/0/w/100/h/100/interlace/1/format/jpg" alt='' />
                    {#else}
                        <img src="{$T.row.ThirdPictureUrl}" alt='' />
                    {#/if}
                {#else}
                    <img src="{$T.row.UserDomain}{$T.row.UserPictuePath}?imageView2/0/w/100/h/100/interlace/1/format/jpg" alt='' />
                {#/if}       
            {#/if}
        </div>
        <div class="comment-infor_box">
            <div class="user-comment_infor">
                <div>
                    <p class="user-name">
                        {#if $T.row.IsAnonymous}
                                         匿名
                                    {#else}
                                        {$T.row.NickName}
                                    {#/if}
                    </p>
                    <p class="comment-time">{$T.row.FormatCommentDate} <span>{$T.row.FormatCommentTime}</span></p>
                </div>
                {#if $T.uid!=$T.row.UserID}
		        			    <div class="comment-text" onclick="javascript:openReply({$T.row.CommentID},{$T.row.ReplyCnt});">
                                    <p class="comment-detail" id="p_detail_{$T.row.CommentID}">{$T.row.CommentContent}</p>
                                </div>
                {#if $T.row.lenContent>50}
                                    <a href="javascript:" class="all-text" onclick="javascript:textAll('p_detail_{$T.row.CommentID}',this);">全文</a>
                {#/if}
                            {#else}
                                <div class="comment-text" onclick="javascript:openReply({$T.row.CommentID},{$T.row.ReplyCnt});">
                                    <p class="" id="p_detail_{$T.row.CommentID}">{$T.row.CommentContent}</p>
                                </div>
                <a href="javascript:" class="all-text" onclick="javascript:openDialog('{$T.row.CommentID}');">删除</a>
                {#/if}
            </div>
            <div class="comment-num_infor">
                <div class="comment-num comment-flex" onclick="javascript:openReply({$T.row.CommentID},{$T.row.ReplyCnt});">
                    <i class="comment-icon"></i>
                    <span>{$T.row.ReplyCnt}</span>
                </div>
                {#if $T.row.IsLike>0}
                <div class="comment-like comment-flex" id="good_{$T.row.CommentID}" isclicked="1" onclick="javascript:likeClick(this,{$T.row.CommentID});">
                    <i class="nice-in"></i>
                    <span class="orange">{$T.row.LikeCnt}</span>
                </div>
                {#else}
                <div class="comment-like comment-flex" id="good_{$T.row.CommentID}" onclick="javascript:likeClick(this,{$T.row.CommentID});">
                    <i class="comment-like_icon"></i>
                    <span>{$T.row.LikeCnt}</span>
                </div>
                {#/if}

            </div>
        </div>
    </section>
{#/for}
{#/if}

