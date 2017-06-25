<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppBoxTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.AppBoxTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="AppBoxList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thMessageID" CssClass="center span2" Text="序号" BindingFiled="MessageID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thStartTime" CssClass="center span2"  Text="开始时间" BindingFiled="StartTime" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thArticleLink" CssClass="center span2"  Text="文章链接" BindingFiled="ArticleLink" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thImageLink" CssClass="center span2"   Text="图片链接" BindingFiled="ImageLink" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thRemarks" CssClass="center span2"   Text="状态" BindingFiled="Remarks" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thType" CssClass="center span2"   Text="类型" BindingFiled="Type" OutPutOrder="6" />
                <th class="center span3">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.SignID}">
                <td class="center">{$T.row.MessageID}</td>
                <td class="center" style="width:270px">{$T.row.StartTime}</td>
                <td class="center" style="width:270px"><a href="{$T.row.ArticleLink}" target="_blank">{$T.row.ALink}</a></td>
                <td class="center" style="width:270px"><a href="{$T.row.ImageLink}" target="_blank">{$T.row.ILink}</a></td>
                <td class="center">{$T.row.Remarks}</td>
                <td class="center" style="width:270px">{$T.row.Type}</td>
                  <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:updatesign({$T.row.MessageID});"><span class="red">编辑</span></a></li>              
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deletesign({$T.row.MessageID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>