 <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExhibitorsTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.ExhibitorsTemplate" %>

{#if $T.cnt==0 && $T.pindex==1}
<div class="reault-1">
    <figure class="no-result">
        <img src="../images/none.png" />
    </figure>
    <p class="sorry">对不起，全宇宙<span>没有找到</span>你要搜索的展商结果！</p>
</div>
{#else}
{#foreach $T.list as row}
<div class="main-exhibitors" repeater="item">
	<div class="main-exhibitors_infor">
		    <p class="con-main exhibitionName">
                {$T.row.ExhibitorName}
                {#if $T.row.IsHadBookList}
                <span>电子书目</span>
                {#/if}
		    </p>
        {#foreach $T.row.ExhibitorLocationList as elrow}
        <div class="con-main">
            <span class="exhibitionAddress_icon icon infor-icon"></span>
            <p class="activity-infor">
                {$T.elrow.ExhibitorLocation}
		    </p>
             <div style="clear:both;"></div>
        </div>
		
        {#/for}
	</div>
</div>
{#/for}

{#/if}
       
