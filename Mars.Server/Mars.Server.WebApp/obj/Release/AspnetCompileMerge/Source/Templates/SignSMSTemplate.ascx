<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignSMSTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.SignSMSTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="SignSMSList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thSignID" CssClass="center span2" Text="短信序号" BindingFiled="SmsID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thCustomer" CssClass="center span2"  Text="姓名" BindingFiled="Customer" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thMoblie" CssClass="center span2" Text="手机" BindingFiled="Moblie" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thContent" CssClass="center span2" Text="内容" BindingFiled="Content" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thSendTime" CssClass="center span2"  Text="发送时间" BindingFiled="SendTime" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thIsSend" CssClass="center span2"  Text="发送状态" BindingFiled="IsSend" OutPutOrder="6" />
                <th class="center span3">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.SmsID}">
                <td class="center">{$T.row.SmsID}</td>
                <td class="center">{$T.row.Customer}</td>
                <td class="center">{$T.row.Moblie}</td>
                <td class="center" style="width:270px">
                    {#if $T.row.Content ==null}
                         
                    {#else}
                       {$T.row.Content}
                    {#/if}
                </td>
                <td class="center">
                    {#if $T.row.SendTime ==null}
                         
                    {#else}
                        {$T.row.SendTime}
                    {#/if}
                </td>
                <td class="center">
                    {#if $T.row.IsSend == 0}
                       <span class="badge badge-warning">未发送</span> 
                    {#else}
                       <span class="badge badge-warning">已发送</span> 
                    {#/if}
                </td>
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deletesign({$T.row.SmsID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>