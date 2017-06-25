<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarouselTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.CarouselTemplate" %>

<OpenBook:MyPlaceHolder runat="server" ID="mphCarousel" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thRowID" CssClass="center span1" Text="序号" BindingFiled="rowId" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thTitle" CssClass="center span4" Text="标题" BindingFiled="Title" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thFirstTypName" CssClass="center" Text="一级分类" BindingFiled="FirstTypName" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thSecondTypeName" CssClass="center" Text="二级分类" BindingFiled="SecondTypeName" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thRecommend" CssClass="center" Text="是否推荐" BindingFiled="Recommend" OutPutOrder="5" />             
                <OpenBook:Th runat="server" ID="thAdvert" CssClass="center" Text="发布时间" BindingFiled="PublishTime" OutPutOrder="7" />
                <OpenBook:Th runat="server" ID="thDiscoverAdvert" CssClass="center" Text="结束时间" BindingFiled="AdsEndTime" OutPutOrder="8" />
                <OpenBook:Th runat="server" ID="thAdvertOrder" CssClass="center spqn1" Text="轮播顺序" BindingFiled="AdvertOrder" OutPutOrder="9" />
                <OpenBook:Th runat="server" ID="thBrowseCnt" CssClass="center" Text="浏览次数" BindingFiled="BrowseCnt" OutPutOrder="10" />                
                <th class="center span2">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.EventItemID}">
                <td class="center">{$T.row.rowId}</td>
                <td><a target="_blank" href="{$T.row.Url}" >{$T.row.Title}</a></td>
                <td>{$T.row.FirstTypName}</td>
                <td>{$T.row.SecondTypeName}</td>
                  <td class="center">
                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chksd_{$T.row.EventItemID}" uid="{$T.row.EventItemID}" {getCheckState($T.row.Recommend,'1',true)} 
                        onchange="javascript:changedrecommend({$T.row.EventItemID},this);"/><span class="lbl"></span>
                </td>
             <%--   <td class="center">
                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chksd2_{$T.row.EventItemID}" uid="{$T.row.EventItemID}" {getCheckState($T.row.ActiveApply,true,true)} 
                        onchange="javascript:changeactiveapply({$T.row.EventItemID},this)" /><span class="lbl"></span>              
                </td>--%>
               <td class="center">
                    {$T.row.PublishTime}
                </td>
                <td class="center">
                    {$T.row.DiscoverAdsEndTime}
                </td>
                <td class="center">{$T.row.AdvertOrder}
                </td>
                <td class="center">{$T.row.BrowseCnt}</td>
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">                          
                            <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:setadvert({$T.row.EventItemID});"><span class="red">设置发现轮播</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:canceladvert({$T.row.EventItemID});"><span class="red">取消发现轮播</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>
