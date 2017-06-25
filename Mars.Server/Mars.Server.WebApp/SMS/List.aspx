<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="List.aspx.cs" Inherits="Mars.Server.WebApp.SMS.List" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="Content1" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="OBScript4" Src="~/js/plugin/uploadify3.2.1/jquery.uploadify.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="OBScript5" Src="~/js/plugin/uploadify3.2.1/uploadify.css" ScriptType="StyleCss" />
       <OpenBook:OBScript runat="server" ID="bootstrap" Src="~/css/bootstrap.min.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">短信管理</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>短信管理<small><i class="icon-double-angle-right"></i>短信检索</small></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
   
        <div class="alert alert-block alert-success form-horizontal">
            <div class="control-group">
                     <div class="controls">
                      <select id="selType" class="searchpart"  style="width:120px">
                           <option selected value="-1">==请选择==</option>
                           <option value="1">姓名</option>
                           <option value="2">手机号</option>
                           <option value="3">内容</option>
                           <option value="4">时间</option>
                       </select>
               <input id="txtValue"  type="text" placeholder="" class="searchpart" />

               <input type="text" name="txtTime" id="txtTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"  />
             <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
            </div>
             </div>
        </div>
    
        <OpenBook:TemplateWrapper ID="SignSMS" runat="server" TemplateSrc="~/Templates/SignSMSTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
<script type="text/javascript">
var refresh = function (reload) {
    search(reload);
}
var search = function (reload) {
    var txtkey = $("#selType").val();
    var txtValue = $("#txtValue").val();
    
    if (txtkey == 4)
    {
        txtValue = $("#txtTime").val();
    }
    TObj("SignSMS")._prmsData.key = txtkey;
    TObj("SignSMS")._prmsData.value = txtValue;
    if (reload)
    {
        TObj("SignSMS")._prmsData.ts = new Date().getTime();
    }
    TObj("SignSMS")._prmsData._pageIndex = 0;
    TObj("SignSMS").loadData();
}
var sms = {};
sms.init = function () {
    $("#selType").val("-1");
    $("#txtValue").show();
    $("#txtTime").hide();
    $('#selType').change(function ()
    {
        var selType= $(this).val();
        if (selType == 4) {
            $("#txtValue").hide();
            $("#txtTime").show();
        } else
        {
            $("#txtValue").show();
            $("#txtTime").hide();
        }
    });
}
sms.init();
var deletesign = function (sid) {
    bootbox.confirm("您确认要删除该短信吗?(删除后将不可恢复)", function (result) {
        if (result) {
            $.post(_root + "handlers/SMSController/Delete.ashx", { "smsID": sid }, function (data) {
                if (data.indexOf("_") != -1) {
                    var msg = data.split('_');
                    bootbox.alert("删除失败，" + msg[1]);
                }
                else if (data == "false") {
                    bootbox.alert("服务器访问失败，请查看网络是否畅通！");
                }
                else {
                    bootbox.alert("删除成功！");
                    $("#tr" + sid).hide();
                }
            });
        }
    });
}

</script>
</asp:Content>