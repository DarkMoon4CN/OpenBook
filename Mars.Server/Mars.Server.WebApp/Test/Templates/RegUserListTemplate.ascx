<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegUserListTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Test.Templates.RegUserListTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphRegUserList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thUserID" CssClass="center span2" Text="用户ID" BindingFiled="User_ID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thLoginName" CssClass="center span4"  Text="登录名" BindingFiled="LoginName" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thUserName" CssClass="center span2"  Text="姓名" BindingFiled="UserName" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thCompanyName" CssClass="center"  Text="单位名称" BindingFiled="CompanyName" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thUserIdentityDesc" CssClass="center span2"  Text="用户身份" BindingFiled="UserIdentityDesc" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thMobile" CssClass="center span2"  Text="手机" BindingFiled="Mobile" OutPutOrder="6" />
                <OpenBook:Th runat="server" ID="thPhone" CssClass="center span3"  Text="座机" BindingFiled="Phone" OutPutOrder="7" />
                <OpenBook:Th runat="server" ID="thRegisterDate" CssClass="center span2"  Text="注册时间" BindingFiled="RegisterDate" OutPutOrder="8" />
                <th class="center span2">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.User_ID}">
                <td class="center">{$T.row.User_ID}</td>
                <td ><a href="javascript:" onclick="javascript:openview({$T.row.UserID});">{$T.row.LoginName}</a></td>
                <td class="center">{$T.row.UserName}</td>
                <td>{$T.row.CompanyName}</td>
                <td class="center">{$T.row.UserIdentityDesc}</td>
                <td>{isnull($T.row.Mobile,'')}</td>
                <td>{isnull($T.row.Phone,'')}</td>
                <td class="center">{$T.row.FormatRegisterDate}</td>
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="javascript:" data-original-title="Edit" data-placement="left" data-rel="tooltip" onclick="javascript:openview({$T.row.UserID});"><span class="green">查看详细</span></a></li>
                            <li><a title="" class="tooltip-success" href="javascript:" data-original-title="Edit" data-placement="left" data-rel="tooltip" onclick="javascript:pass({$T.row.UserID});"><span class="green">审核通过</span></a></li>
                            <li><a title="" class="tooltip-error" href="javascript:" data-original-title="Delete" data-placement="left" data-rel="tooltip" onclick="javascript:fail({$T.row.UserID});"><span class="red">审核不通过</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deleteuser({$T.row.UserID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>