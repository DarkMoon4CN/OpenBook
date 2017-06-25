<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoriesTemplate.ascx.cs" Inherits="Mars.Server.WebApp.Templates.CategoriesTemplate" %>
      <table class="table table-striped table-bordered table-hover" id="table_bug_report">
         <tr> <td style="text-align:center; width:15%">分类ID</td> <td style="text-align:center; width:70%">分类名称</td> <td style="text-align:center; width:15%">操作</td></tr> 
           {#foreach $T.list as row}
          <tr><td style="text-align:center; width:15%">{$T.row.CalendarTypeID}</td> <td style="text-align:center; width:70%">{$T.row.CalendarTypeName}</td> <td style="text-align:center; width:15%">
               <div class="inline position-relative">
                        <button class="btn btn-minier btn-primary dropdown-toggle" data-toggle="dropdown">操作</button>
                        <ul class="dropdown-menu dropdown-icon-only dropdown-light pull-right dropdown-caret dropdown-close">
                            <li><a title="" class="tooltip-success" href="CategoriesEdit.aspx?fun={$T.fun}&id={$T.row.CalendarTypeID}&pid={$T.row.ParentCalendarTypeID}" data-original-title="Edit" data-placement="left" data-rel="tooltip"><span class="green">编辑菜单</span></a></li>
                            <li><a title="" class="tooltip-warning" href="javascript:" data-rel="tooltip" onclick="javascript:deleteCategories({$T.row.CalendarTypeID});"><span class="red">删除</span></a></li>
                        </ul>
                    </div>
           </td></tr> 
          {#/foreach}
         </table> 
