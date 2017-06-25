<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ActivityTemplate" %>
{#if $T.cnt==0 && $T.pindex==1}
<div class="reault-1">
    <figure class="no-result">
        <img src="../images/none.png" />
    </figure>
    <p class="sorry">对不起，全宇宙<span>没有找到</span>你要搜索的活动结果！</p>
</div>
{#else}
{#foreach $T.list as row}
<div class="main-active-box" repeater="item">
    <div class="main-active con-main" onclick="javascript:openDetail(this,{$T.row.ActivityID});">
        <p class="active-name">{$T.row.ActivityTitle}</p>
        <div>
            <span class="active-time_icon icon infor-icon"></span>
            <p class="activity-infor">{$T.row.FormatActivityTime}</p>
            <div style="clear:both;"></div>
        </div>
        <div>
            <span class="active-publish_icon icon infor-icon"></span>
            <p class="activity-infor">{$T.row.ActivityHostUnit}</p>
            <div style="clear:both;"></div>
        </div>
        <div>
            <span class="exhibitionAddress_icon icon infor-icon"></span>
            <p class="activity-infor" style="margin-bottom:3rem;">{$T.row.ActivityLocation}</p>
            <div style="clear:both;"></div>
        </div>      
        {#if $T.row.ActivityAbstract!="" || $T.row.ActivityGuest!="" || $T.row.SubactivityList.length>0}
            <input type="image" src="../images/btm-arrow.png" class="btm_arrow" />
        {#/if}
        {#if $T.row.ActivityIsEnd==1}
        <button class="join-btn" disabled="disabled">已结束</button>
        {#else}
        <button class="join-btn" id="btn_add_{$T.row.ActivityID}" onclick="javascript:addTOSchedule({$T.row.ActivityID},'{$T.row.ActivityTitle}','{$T.row.FormatActivityStartTime}','{$T.row.FormatActivityEndTiem}');">加入日程</button>
        {#/if}
    </div>
    {#if $T.row.ActivityAbstract!="" || $T.row.ActivityGuest!="" || $T.row.SubactivityList.length>0}
	<div class="main-active_child" id="div_child_{$T.row.ActivityID}">
        {#if $T.row.ActivityGuest!=""}
        <div class="con-main main-active_child_infor">
            <p class="active-intro">
                <span>嘉宾：</span>
                {$T.row.ActivityGuest}
            </p>
        </div>
        {#/if}
        {#if $T.row.ActivityAbstract!=""}
		<div class="con-main main-active_child_infor">
            <p class="active-intro">
                <span>活动简介：</span>
                {$T.row.ActivityAbstract}
            </p>
        </div>
        {#/if}
        {#if $T.row.SubactivityList.length>0}
        {#foreach $T.row.SubactivityList as subrow}
		<div class="con-main main-active_child_infor">
            <p class="active-name">{$T.subrow.ActivityTitle}</p>
            <p class="guest">{$T.subrow.ActivityGuest}</p>
            <div>
                <span class="active-time_icon icon infor-icon"></span>
                <p class="activity-infor">{$T.subrow.FormatActivityTime}</p>
                <div style="clear:both;"></div>
            </div>
            <div>
                <span class="exhibitionAddress_icon icon infor-icon"></span>
                <p class="activity-infor" style="margin-bottom:3rem;">{$T.subrow.ActivityLocation}</p>
                <div style="clear:both;"></div>
            </div>
            {#if $T.subrow.ActivityIsEnd==1}
                    <button class="join-btn" disabled="disabled">已结束</button>
            {#else}
                    <button class="join-btn" id="btn_add_{$T.subrow.ActivityID}" onclick="javascript:addTOSchedule({$T.subrow.ActivityID},'{$T.subrow.ActivityTitle}','{$T.subrow.FormatActivityStartTime}','{$T.subrow.FormatActivityEndTiem}');">加入日程</button>
            {#/if}
        </div>
        {#/for}
            {#/if}
    </div>
    {#/if}
</div>
{#/for}
{#/if}