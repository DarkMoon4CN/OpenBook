<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionsTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.FunctionsTemplate" %>
<OpenBook:MyPlaceHolder runat="server" ID="mphFunctionsList" Tag="1" Visible="false">
    <table class="table table-striped table-bordered table-hover" id="table_bug_report">
        <thead>
            <tr>
                <%--  <OpenBook:Th runat="server" ID="thFunction_ID" CssClass="center span2" Text="菜单ID" BindingFiled="Function_ID" OutPutOrder="1" />--%>
                <OpenBook:Th runat="server" ID="thFunction_Name" CssClass="center" Text="菜单名称" BindingFiled="Function_Name" OutPutOrder="2" />
                <OpenBook:Th runat="server" ID="thFunctio_URL_New" CssClass="center " Text="菜单路径" BindingFiled="Function_URL_New" OutPutOrder="3" />
                <OpenBook:Th runat="server" ID="thCreateTime" CssClass="center span3" Text="创建日期" BindingFiled="CreateDate" OutPutOrder="4" />
                <th class="center span3">操作</th>
            </tr>
        </thead>
        <tbody>
            {#foreach $T.list as row}
            <tr id="tr{$T.row.Function_ID}">
                <%--  <td class="center">{$T.row.Function_ID}</td>--%>
                <td colspan="3" style="font-weight: bold;">{$T.row.Function_Name}</td>
                <td class="center" style="border-left: 0px;">
                    <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="FunctionEdit.aspx?fun={$T.fun}&pid={$T.row.Function_ID}" data-original-title="Edit" data-placement="left" data-rel="tooltip"><span class="green">编辑菜单</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:deletefunction({$T.row.Function_ID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            {#foreach $T.row.ChildFunctionList as row2}
              <tr id="tr{$T.row2.Function_ID}">
                  <%--   <td class="center">{$T.row2.Function_ID}</td>--%>
                  <td><a href="javascript:" onclick="javascript:openview({$T.row2.Function_ID});">{$T.row2.Function_Name}</a></td>
                  <td>{$T.row2.Function_URL_New}</td>
                  <td>{$T.row2.CreateDate}</td>
                  <td class="center">
                      <div class="inline position-relative">
                          <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                          <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                              <li><a title="" class="tooltip-success" href="FunctionEdit.aspx?pid={$T.row2.Function_ID}" data-original-title="Edit" data-placement="left" data-rel="tooltip"><span class="green">编辑菜单</span></a></li>
                              <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:deletefunction({$T.row2.Function_ID});"><span class="red">删除</span></a></li>
                          </ul>
                      </div>
                  </td>
              </tr>
            {#/foreach}       
            {#/foreach}
        </tbody>
    </table>    
</OpenBook:MyPlaceHolder>
