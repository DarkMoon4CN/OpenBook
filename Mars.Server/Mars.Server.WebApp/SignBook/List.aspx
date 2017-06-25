<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="List.aspx.cs" Inherits="Mars.Server.WebApp.SignBook.List" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
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
    <h1>客户签到<small><i class="icon-double-angle-right"></i>签到统计</small></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
       <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left; width:auto" >
            <div class="controls">
              <select id="selType" class="searchpart"  style="width:120px">
                   <option selected value="-1">==请选择==</option>
                   <option value="1">姓名</option>
                   <option value="2">单位</option>
                   <option value="3">手机号</option>
                   <option value="4">业务对接人</option>
                   <option value="5">已签到</option>
                   <option value="6">未签到</option>
               </select>
             <input id="txtValue"  type="text" placeholder="" class="searchpart" />
             <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
             <a href="javascript:void(0);" class="btn btn-purple export">导出</a>
            <span id="btn_upload" ></span>
             <a href="javascript:void(0);" id="import" class="btn btn-purple import" >导入</a>
            </div>
        </div>

<%--        <div class="control-group" style="float: left;">
            <div class="controls">   
                <span id="btn_upload" ></span>
                <a href="javascript:void(0);" id="import" class="btn btn-purple import" >导入</a>
            </div>
        </div>--%>


        <div class="btn-group" style="float: right; margin-right:50px">
            <label class="control-label" for="selAdvert" style="margin-top: 110px;" id="showCount">正在加载信息...</label>
        </div>
        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
        <OpenBook:TemplateWrapper ID="tmpSignBook" runat="server" TemplateSrc="~/Templates/SignBookTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <style>
        #import{
            float: right;
            margin: 7px 0px 0px !important;
        }
    </style>
<script type="text/javascript">        
    $(function () {
        var url = _root + "handlers/SignBookController/Import.ashx";
        $('#import').uploadify({
            uploader: url,                          // 服务器处理地址
            swf: '/js/plugin/uploadify3.2.1/uploadify.swf',
            buttonText: "导入Excel",                //按钮文字
            height: 30,                             //按钮高度
            width: 75,                              //按钮宽度
            fileTypeExts: "*.xlsx;*.xls;",                //允许的文件类型
            fileTypeDesc: "Excel 工作表",           //文件说明   
            formData: { "imgType": "normal" }, //提交给服务器端的参数
            onUploadSuccess: function (file, data, response) {   //一个文件上传成功后的响应事件处理
              
                if (data.indexOf("_") != -1)
                {
                    var existMoblie = data.split('_');
                    bootbox.alert("已存在相同客户手机：" + existMoblie[1]);
                }
                else if (data == "false") {
                    bootbox.alert("服务器访问失败，请查看网络是否畅通！");
                }
                else
                {
                    bootbox.alert(" Excel 工作表 上传成功！");
                    search(false);
                }
            }
        });
        GetCount();
      
    });
var refresh = function (reload) {
    search(reload);
    GetCount();
}
var search = function (reload) {
    var selType = $("#selType").val();
    var txtValue = $("#txtValue").val();
    TObj("tmpSignBook")._prmsData.key = selType;
    TObj("tmpSignBook")._prmsData.value = txtValue;
    if (reload) {
        TObj("tmpSignBook")._prmsData.ts = new Date().getTime();
    }
    TObj("tmpSignBook")._prmsData._pageIndex = 0;
    TObj("tmpSignBook").loadData();
}
var setright = function (sid)
{
    $.post(_root + "handlers/SignBookController/UpdateState.ashx", { "signID": sid, "ts": new Date().getTime() }, function (data) {
        if (data.indexOf("_") != -1 && data.indexOf("false") !=-1) {
            var existMoblie = data.split('_');
            bootbox.alert("设置失败，" + existMoblie[1]);
        }
        else if (data == "false") {
            bootbox.alert("服务器访问失败，请查看网络是否畅通！");
        }
        else {
            var dataSplit = data.split('_');
            bootbox.alert(dataSplit[1]);
            search(true);
        }
    });
    GetCount();
}
var deletesign = function (sid)
{
    bootbox.confirm("您确认要删除该客户吗?(删除后将不可恢复)", function (result) {
        if (result) {
            $.post(_root + "handlers/SignBookController/Delete.ashx", { "signID": sid }, function (data) {
                if (data.indexOf("_") != -1) {
                    var existMoblie = data.split('_');
                    bootbox.alert("删除失败，" + existMoblie[1]);
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
    GetCount();
}
$(".export").click(function () {
    var selType = $("#selType").val();
    var selValue = $("#txtValue").val();
    $.post(_root + "handlers/SignBookController/Export.ashx", { "selType": selType, "selValue": selValue, "ts": new Date().getTime() }, function (data) {
        if (data == "false")
        {
            bootbox.alert("所有客户都没有签到");
        }
        else
        {
            var res = data.split('_');
            window.location.href = "/UploadFile/SignBook/"+res[1];
        }
    });
});
var GetCount = function ()
{
    //加载总页数
    $.post(_root + "handlers/SignBookController/GetCount.ashx", { "ts": new Date().getTime() }, function (data) {
        $("#showCount").html(data);
    });
}
var update = function (sid)
{
    asyncbox.open({
        modal: true,
        title: "编辑签到信息",
        url: _root + "SignBook/Edit.aspx?sid=" + sid,
        width: 750,
        height: 360,
        id: "signBook"
    });
}
</script>
</asp:Content>