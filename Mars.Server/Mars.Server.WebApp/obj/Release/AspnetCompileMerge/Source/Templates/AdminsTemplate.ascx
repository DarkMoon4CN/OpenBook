<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminsTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.AdminsTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphRegUserList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thUserID" CssClass="center span2" Text="用户ID" BindingFiled="User_ID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thLoginName" CssClass="center span2"  Text="登录名" BindingFiled="User_Name" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thUserName" CssClass="center span2"  Text="姓名" BindingFiled="TrueName" OutPutOrder="3" />                          
                <OpenBook:Th runat="server" ID="thUserTel" CssClass="center span2"  Text="分机号" BindingFiled="User_Tel" OutPutOrder="4" />
                <OpenBook:Th runat="server" ID="thDept" CssClass="center span2"  Text="部门" BindingFiled="Dept_Name" OutPutOrder="5" />
                <OpenBook:Th runat="server" ID="thRoleName" CssClass="center"  Text="角色" BindingFiled="Role_Name" OutPutOrder="6" />  
                <OpenBook:Th runat="server" ID="thRegisterDate" CssClass="center"  Text="注册日期" BindingFiled="RegisterDate" OutPutOrder="6" />       
                <th class="center span3">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.User_ID}">
                <td class="center">{$T.row.User_ID}</td>
                <td ><a href="javascript:" onclick="javascript:openview({$T.row.UserID});">{$T.row.User_Name}</a></td>
                <td class="center">{$T.row.TrueName}</td>             
                <td>{$T.row.User_Tel}</td>
                <td>{$T.row.Dept_Name}</td>   
                <td>{$T.row.Role_Name}</td>       
                <td>{$T.row.RegisterDate}</td>        
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="AdminEdit.aspx?fun={$T.fun}&pid={$T.row.User_ID}" data-original-title="Edit" data-placement="left" data-rel="tooltip" ><span class="green">编辑</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:changepwd({$T.row.User_ID},'{$T.row.User_Name}');"><span class="red">修改密码</span></a></li>                 
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:setright({$T.row.User_ID});"><span class="red">设置角色权限</span></a></li>               
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deleteuser({$T.row.User_ID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>