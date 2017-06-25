<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AppUpdateEdit.aspx.cs" Inherits="Mars.Server.WebApp.OtherManager.AppUpdateEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <script src="../js/plugin/bootbox.min.js" type="text/javascript"></script>
    <script src="../js/plugin/jquery.validate.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
     <li class="active">系统升级菜单维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
      <h1>系统升级维护<small><i class="icon-double-angle-right"></i>维护系统升级菜单</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑系统升级菜单</h4>
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
                                            <label class="control-label" for="content_txtapptype">系统</label> 
                                            <div class="controls"> 
                                                <select  id="txtapptype" name="txtapptype" runat="server">
                                                    <option value="">--请选择--</option>
                                                    <option value="1">Android</option>
                                                    <option value="2">IOS</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtVersion">版本号</label>
                                            <div class="controls">
                                                <input type="text" name="txtVersion" id="txtVersion" runat="server" /><font color="red">*</font>
                                            </div>
                                        </div>  
                                        <div class="control-group">
                                            <label class="control-label" for="content_txtdownloadUrl">下载链接</label>
                                            <div class="controls">
                                                <input type="text" name="txtdownloadUrl" id="txtdownloadUrl" runat="server" /><font color="red">*</font>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label" for="content_txtforcedUpdate">强制升级</label>
                                            <div class="controls">
                                                <select runat="server" id="txtforcedUpdate" name="txtforcedUpdate">
                                                    <option value="">--请选择--</option>
                                                    <option value="1">是</option>
                                                    <option value="0">否</option>
                                                    </select>
                                            </div>
                                        </div> 
                                        <div class="control-group">
                                            <label class="control-label" for="content_txtSize">大小</label>
                                            <div class="controls">
                                                <input type="text" name="txtSize" id="txtSize" runat="server" />（MB）<font color="red">*</font>
                                            </div> 
                                        </div>
                                         <div class="control-group">
                                            <label class="control-label" for="content_txt_updateProfile">简介</label>
                                            <div class="controls">
                                                <textarea name="txt_updateProfile" id="txt_updateProfile" style="width: 70%; height: 100px" runat="server"></textarea>
                                                <%--<input type="text" name="txt_updateProfile" id="txt_updateProfile" runat="server" />--%><font color="red">*</font>
                                            </div>
                                        </div>
                                         <div class="control-group" id="addtime" >
                                            <label class="control-label" for="content_txtcreateTime">更新时间</label>
                                            <div class="controls"> 
                                                 <label  id="txtcreateTime" runat="server" name="txtcreateTime"></label>
                                            </div>
                                        </div>
                                        <input type="hidden" id="hidPid" runat="server" /> 
                                        <input type="hidden" id="Hidtime" runat="server" /> 
                                    </form>
                                </div>
                            </div>

                            <hr />

                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" onclick="javascript:EditApptype();" id="editInfo">修改</button>
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
    <script type="text/javascript">
        function backlist() { 
            window.history.back();
        }
        function EditApptype() {
            if (!$("#validationf").valid())
            {
                return false;  
            }
            else
            {
                var version = $("#content_txtVersion").val();
                var downurl = encodeURI($("#content_txtdownloadUrl").val());
                var forcedupdate = $("#content_txtforcedUpdate").val();
                var size = $("#content_txtSize").val();
                var profile = encodeURI($("#content_txt_updateProfile").val());
                var apptype = $("#content_txtapptype").val();
                var appid=$("#content_hidPid").val();
                var pas = { "version": version, "downurl": downurl, "forcedupdate": forcedupdate, "size": size, "profile": profile, "apptype": apptype,"appid":appid }; 
                $("#editInfo").attr("disabled", true); 
                $.post(_root + "handlers/CommonDictController/UpdateAppVersion.ashx", pas, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.status == "1")
                    {
                        bootbox.dialog("保存成功！", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                window.location.href = "AppUpdateManager.aspx?fun=<%=fun%>"
                            }
                        }]);
                    }
                    else if (json.status == "2") {
                        bootbox.alert("保存失败！");
                    }
                    else { bootbox.alert("数据传输失败！"); }
                }); 
                $("#editInfo").attr("disabled", false); 
            }
        }
        
        $(function () {  
            if(<%=id%>==0)
            {$("#addtime").empty();
            $("#editInfo").text("保存");
            }else{
            $("#content_txtcreateTime").text($("#content_Hidtime").val());}
            $("#validationf").validate({    
                errorEvent: 'span',
                errorClass: 'help-inline',   //errorClass: "label.error", //默认为错误的样式类为：error    
                rules: {
                    ctl00$content$txtapptype: { required: true },
                    ctl00$content$txtVersion: {
                        required: true,
                        rangelength: [1, 50]
                    }, ctl00$content$txtdownloadUrl: {
                        required: true, rangelength: [5, 500]
                    },ctl00$content$txtforcedUpdate:{
                    required:true
                    }, ctl00$content$txtSize: {
                        required: true, digits: true
                    }
                    , ctl00$content$txt_updateProfile: {
                        required: true, rangelength: [5, 500]
                    }
                },
                messages: {
                    ctl00$content$txtapptype: { required: "必填" },
                    ctl00$content$txtVersion: {
                        required: "必填", rangelength: "长度在1-50个字符串之间"
                    }, ctl00$content$txtdownloadUrl: {
                        required: "必填", rangelength: "长度在5-500个字符串之间"
                    },ctl00$content$txtforcedUpdate:{
                        required: "必填"
                    }, ctl00$content$txtSize: { required: "必填", digits: "输入合法整数" }
                    , ctl00$content$txt_updateProfile: {
                        required: "必填", rangelength: "长度在5-500个字符串之间"
                    }
                }, highlight: function (e) {
                    $(e).closest('.control-group').removeClass('info').addClass('error');
                },
                success: function (e) {
                    $(e).closest('.control-group').removeClass('error').addClass('info');
                    $(e).remove();
                },
                submitHandler: function (form) {
                }
            })
        })
    </script>
</asp:Content>
