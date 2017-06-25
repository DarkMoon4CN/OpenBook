<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="AddList.aspx.cs" Inherits="Mars.Server.WebApp.SMS.AddList" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="script" ID="Content1" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="uploadifyMin_js" Src="~/js/plugin/uploadify3.2.1/jquery.uploadify.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="uploadify_css" Src="~/js/plugin/uploadify3.2.1/uploadify.css" ScriptType="StyleCss" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">客户签到</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>客户签到<small><i class="icon-double-angle-right"></i>发送短信</small></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    
    <div class="alert alert-block alert-success form-horizontal">
            <div class="control-group" >
                    <label class="control-label" for="ckExcel">是否导入Excel :</label>
                    <div class="controls">
                       <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="isExcel" /><span class="lbl"></span>
                    </div>
             </div>


             <div class="control-group" style="float: left;">
                 <label class="control-label" for="txtGroupEventName">短信模板</label>
                 <div class="controls">
                      <select id="selModel"  class="searchpart"  style="width:220px">
                           <option selected value="-1">==请选择==</option>
                           <%=signSMSModel %>
                       </select>
                </div>
            </div>
             
            <div class="control-group" >
                 <label class="control-label" for="txtGroupEventName">模板原文&nbsp;&nbsp; </label>
                 <div class="controls">
                       <textarea name="lblContentModel" id="lblContentModel"  disabled="disabled" style="width:400px; height: 150px;overflow-x:hidden;overflow-y:hidden;resize: none;"></textarea>
                </div>
            </div>

            <div class="control-group" id="AddSingle" >

                 <label class="control-label" for="txtGroupEventName">客户姓名</label>
                 <div class="controls">   
                         <input id="txtCustomer"  type="text" placeholder="" class="searchpart" />
                 </div>
                <br />
                 <label class="control-label" for="txtGroupEventName">客户手机号</label>
                 <div class="controls">   
                         <input id="txtPhone"  type="text" placeholder="" class="searchpart" />
                 </div>
           </div>

            

            <div class="control-group" id="AddList">
                 <label class="control-label" for="txtGroupEventName">文件位置</label>
                <div class="controls">   
                    <span id="btn_upload" ></span>
                    <a href="javascript:void(0);" id="import" class="btn btn-purple import" >导入</a>
                </div>
            </div>



            <div class="control-group" style="float: left;">
                 <label class="control-label" for="txtGroupEventName">发送内容</label>
                 <div class="controls">
                    <textarea name="txtContent" id="txtContent" style="width:205px; height: 150px;overflow-x:hidden;overflow-y:hidden;resize: none;"></textarea>
                </div>
            </div>
            <div class="control-group" >
                 <label class="control-label" for="txtGroupEventName">短信预览&nbsp;&nbsp;</label>
                 <div class="controls">
                    <textarea name="lblContent" id="lblContent" disabled="disabled" style="width:400px; height: 150px;overflow-x:hidden;overflow-y:hidden;resize: none;"></textarea>
                </div>
            </div>


            <div class="control-group">
                 <label class="control-label" for="txtGroupEventName"></label>
                 <div class="controls">   
                     <a href="javascript:void(0);" id="submit" class="btn btn-purple import" >发送</a>
                 </div>
            </div>
        </div>
         <input  id="hidType" type="hidden" value="1"/> 
         <input  id="hiduploadCustomer" type="hidden" /> 
         <input  id="sUserID" type="hidden" value="<%=sUserID %>"/> 
        <OpenBook:TemplateWrapper ID="SignSMS" runat="server" TemplateSrc="~/Templates/SignSMSTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
<script type="text/javascript">
var refresh = function (reload) {
    $("#hidType").val(2);
    search(reload);
}
var search = function (reload) {
    var hidType = $("#hidType").val();
    TObj("SignSMS")._prmsData.hidType = hidType;
    if (reload) {
        TObj("SignSMS")._prmsData.ts = new Date().getTime();
    }
    TObj("SignSMS")._prmsData._pageIndex = 0;
    TObj("SignSMS").loadData();
}
$(function () {
    $("#hidType").val(2);
    var sUserID = $("#sUserID").val();
    var url = _root + "handlers/SMSController/Import.ashx";
    $('#import').uploadify({
        uploader: url,                          // 服务器处理地址
        swf: '/js/plugin/uploadify3.2.1/uploadify.swf',
        buttonText: "导入Excel",                //按钮文字
        height: 30,                             //按钮高度
        width: 75,                              //按钮宽度
        fileTypeExts: "*.xlsx;*.xls;",          //允许的文件类型
        fileTypeDesc: "Excel 工作表",           //文件说明   
        formData: { "sUserID": sUserID },      //提交给服务器端的参数
        onUploadSuccess: function (file, data, response) {   //一个文件上传成功后的响应事件处理
            if (data == "false") {
                bootbox.alert("服务器访问失败，请查看网络是否畅通！");
            }
            else {
                bootbox.alert(" Excel 工作表 上传成功！");
                $("#hiduploadCustomer").val(data.split('_')[1]);
                search(false);
                sms.changeShow();
            }
        }
    });
    
});
var sms = {};
sms.init = function ()
{
    $("#AddSingle").show();
    $("#AddList").hide();
    $("#isExcel").click(function () {
        var isExcel = $(this)[0].checked;
        if (isExcel) {
            $("#AddSingle").hide();
            $("#AddList").show();
        } else {
            $("#AddSingle").show();
            $("#AddList").hide();
        }
        $("#lblContent").val("");
        $("#lblContentModel").val("");
        $("#selModel").val("-1");
    });
    $("#submit").click(function () {
        var isExcel = $("#isExcel")[0].checked;
        var modelKey = $("#selModel").val();
        if (modelKey == "-1")
        {
            bootbox.alert(" 请选择短信模板！ ");
            return false;
        }
        if (isExcel == true) {
            isExcel = 1;
        }
        else
        {
            isExcel = 0;
        }
        var txtPhone = $("#txtPhone").val();
        var txtCustomer = $("#txtCustomer").val();
        var txtContent = $("#txtContent").val();
        var sUserID = $("#sUserID").val();
        if (txtContent == null || txtContent.length == 0)
        {
            bootbox.alert(" 信息内容不可为空！ ");
            return false;
        }
        $.post(_root + "handlers/SMSController/Add.ashx", {
            "sUserID": sUserID,
            "isExcel": isExcel,
            "txtPhone": txtPhone,
            "txtCustomer": txtCustomer,
            "txtContent": txtContent,
            "modelKey":modelKey,
            "ts": new Date().getTime()
        }, function (data) {
            if (data != "false") {
                bootbox.alert(" 已将信息发布至客户手机！ ");
                window.location.href = "/SMS//List.aspx";
            }
            else
            {
                bootbox.alert(" 短信发送失败,请查看网络是否畅通！ ");
            }
        });
    });
    $("#selModel").change(function () {
        var modelKey = $(this).val();
        if (modelKey == "-1")
        {
            $("#lblContent").val("");
            $("#lblContentModel").val("");
            return false;
        }
        $.post(_root + "handlers/SMSController/GetModelContent.ashx", { "modelKey": modelKey, "ts": new Date().getTime() }, function (data) {
            if (data != "null") {
                $("#lblContentModel").val(data);
                sms.changeShow();
            }
            else {
                bootbox.alert(" 无法获取短信模板内容,请查看网络是否畅通！ ");
            }
        });
        if (modelKey == "52237") {
            $("#txtContent").val("http://www.kjrili.com/app/default.html?chn=1003");
        } else
        {
            $("#txtContent").val("http://www.kjrili.com/app/index.html?channel=1001");
        }
        $("#txtContent").keyup();
    });
    $("#txtCustomer").keyup(function () {
        sms.changeShow();
    });
    $("#txtContent").keyup(function () {
        sms.changeShow();
    });
}
sms.init();
sms.changeShow = function () {
    var contentModel= $("#lblContentModel").val();
    var txtCustomer = $("#txtCustomer").val();
    var txtContent = $("#txtContent").val();
    var isExcel = $("#isExcel")[0].checked;
    if (isExcel == false) {
        contentModel = contentModel.replace("{1}", " " + txtCustomer +" ");
    }
    else
    {
        var hiduploadCustomer = $("#hiduploadCustomer").val();
        contentModel = contentModel.replace("{1}", " " + hiduploadCustomer + " ");
    }
    contentModel = contentModel.replace("{2}", " " + txtContent + " ");
    $("#lblContent").val(contentModel);
}
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