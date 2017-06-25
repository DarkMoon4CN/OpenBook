<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriesEdit.aspx.cs" Inherits="Mars.Server.WebApp.Categories.CategoriesEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
     <OpenBook:OBScript ID="OBScript1" runat="server" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript ID="OBScript2" runat="server" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">分类菜单维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
     <h1>分类菜单维护 <small><i class="icon-double-angle-right"></i>编辑分类菜单</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
     <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑分类菜单</h4>
                    <div class="widget-toolbar">
                        <label>
                        </label>
                    </div>
                </div> 
                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row-fluid">
                            <div class="row-fluid position-relative">
                                <div class="step-pane">
                                    <form runat="server" class="form-horizontal" id="validationf">

                                        <div class="control-group">
                                            <label class="control-label" for="content_selCategoriesLevel">上级分类</label>

                                            <div class="controls">
                                                <select id="selCategoriesLevel" name="selCategoriesLevel" runat="server">
                                                    <option value="0" selected="selected">一级分类</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtCategoriesName">分类名称</label>
                                            <div class="controls">
                                                <input type="text" name="txtCategoriesName" id="txtCategoriesName" runat="server" />
                                            </div>
                                        </div>  
                                        <input type="hidden" id="hidPid" runat="server" /> 
                                    </form>
                                </div>
                            </div>

                            <hr />

                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" onclick="javascript:saveadmin();">保存</button>
                                <button class="btn btn-success" onclick="javascript:backlist();">返回</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <%--<script src="~/js/plugin/jquery.validate.min.js" type="text/Javascript" ></script>--%>
    <script> 
        
        function saveadmin() { 
            if (!$("#validationf").valid()) { 
                return false;
            }
            else { 
                document.getElementById("validationf").submit();
            }
        }
        function backlist() {
            window.history.back();
        }
        $(function () {
            var validate = $("#validationf").validate({
                debug: true, //调试模式取消submit的默认提交功能   
                errorEvent: 'span',
                errorClass: 'help-inline',   //errorClass: "label.error", //默认为错误的样式类为：error   
                focusInvalid: false, //当为false时，验证无效时，没有焦点响应    
                rules: {
                    ctl00$content$txtCategoriesName: {
                        required: true,
                        rangelength: [2,100],
                        remote:
                            {
                            type: "POST",
                            url: _root + "Handlers/CategoriesController/IsUseableName.ashx", 
                            data: { 
                                fname: function () { return $("#content_txtCategoriesName").val(); },
                                pid: function () { return $("#content_selCategoriesLevel").val(); }, 
                                id: function () { return $("#content_hidPid").val(); }
                            }
                        }
                    } 
                },  
                messages: {
                    ctl00$content$txtCategoriesName: {
                        required: "必填",
                        rangelength: "分类名称长度在2-100个字符串之间",
                        remote: jQuery.format("分类名称系统中已存在") 
                    }
                },
                highlight:function (e)
                {
                    $(e).closest('.control-group').removeClass('info').addClass('error');
                },
                success:function(e)
                {
                    $(e).closest('.control-group').removeClass('error').addClass('info');
                    $(e).remove();
                },
                submitHandler: function (form) { 
                }
            });

        });

   </script>
</asp:Content>
