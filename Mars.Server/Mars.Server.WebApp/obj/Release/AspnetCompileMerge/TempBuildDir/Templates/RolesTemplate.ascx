<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolesTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.RolesTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphRoleList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <OpenBook:Th runat="server" ID="thRole_ID" CssClass="center span2" Text="角色ID" BindingFiled="Role_ID" OutPutOrder="1" />
                <OpenBook:Th runat="server" ID="thRole_Name" CssClass="center"  Text="角色名称" BindingFiled="Role_Name" OutPutOrder="2" />                
                <OpenBook:Th runat="server" ID="thCreateTime" CssClass="center span2"  Text="创建日期" BindingFiled="CreateTime" OutPutOrder="3" />       
                <th class="center span2">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.Role_ID}">
                <td class="center">{$T.row.Role_ID}</td>
                <td ><a href="javascript:" onclick="javascript:openview({$T.row.Role_ID});">{$T.row.Role_Name}</a></td>           
                <td>{$T.row.CreateTime}</td>        
                <td class="center">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="RoleEdit.aspx?fun={$T.fun}&pid={$T.row.Role_ID}" data-original-title="Edit" data-placement="left" data-rel="tooltip" ><span class="green">编辑</span></a></li>                          
                            <li><a title="" class="tooltip-warning" href="javascript:"  data-rel="tooltip" onclick="javascript:deleterole({$T.row.Role_ID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div> 
                </td>
            </tr>
            {#/foreach}
        </tbody>
    </table>
</OpenBook:MyPlaceHolder>