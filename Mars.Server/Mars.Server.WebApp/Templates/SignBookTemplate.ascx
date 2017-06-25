<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignBookTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.SignBookTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="SignBookList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thSignID" CssClass="center span2" Text="报名序号" BindingFiled="SignID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thCustomer" CssClass="center span2"  Text="姓名" BindingFiled="Customer" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thMoblie" CssClass="center span2" Text="手机" BindingFiled="Moblie" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thCompany" CssClass="center span2"  Text="单位" BindingFiled="Company" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thDepartment" CssClass="center span2"   Text="部门" BindingFiled="Department" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thPosition" CssClass="center span2"  Text="职位" BindingFiled="Position" OutPutOrder="6" />
                <OpenBook:Th runat="server" ID="thEmail" CssClass="center span2"  Text="邮箱" BindingFiled="Email" OutPutOrder="7" />
                <OpenBook:Th runat="server" ID="thSalesName" CssClass="center span2"  Text="业务对接人" BindingFiled="SalesName" OutPutOrder="8" />
                <OpenBook:Th runat="server" ID="thLuckyNumber" CssClass="center span2"  Text="参会编号" BindingFiled="LuckyNumber" OutPutOrder="9" />
                <OpenBook:Th runat="server" ID="thIsSign" CssClass="center span2"  Text="签到状态" BindingFiled="IsSign" OutPutOrder="10" />
                <OpenBook:Th runat="server" ID="thIsRegister" CssClass="center span2"  Text="是否注册" BindingFiled="IsRegister" OutPutOrder="11" />
                <th class="center span3">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.SignID}">
                <td class="center">{$T.row.SignID}</td>
                <td class="center"><a href="/SignBook/Details.aspx?sid={$T.row.SignID}" target="_blank">{$T.row.Customer}</a></td>
                <td class="center">{$T.row.Moblie}</td>
                <td class="center">{$T.row.Company}</td>
                <td class="center">{$T.row.Department}</td>
                <td class="center">{$T.row.Position}</td>
                <td class="center">{$T.row.Email}</td>
                <td class="center">{$T.row.SalesName}</td>
                <td class="center">{$T.row.LuckyNumber}</td>
                <td class="center">
                    {#if $T.row.IsSign == 0}
                       <span class="badge badge-warning">未签到</span> 
                    {#else}
                       <span class="badge badge-success">已签</span> 
                    {#/if}
                </td>
                 <td class="center">
                    {#if $T.row.IsRegister == 0}
                       <span class="badge badge-warning">未注册</span> 
                    {#else}
                       <span class="badge badge-success">已注册</span> 
                    {#/if}
                </td>
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:setright({$T.row.SignID});"><span class="red">更改签到状态</span></a></li>  
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:update({$T.row.SignID});"><span class="red">编辑</span></a></li>             
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deletesign({$T.row.SignID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>