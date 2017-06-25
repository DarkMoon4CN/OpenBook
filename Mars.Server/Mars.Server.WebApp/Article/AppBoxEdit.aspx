<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AppBoxEdit.aspx.cs" Inherits="Mars.Server.WebApp.Article.AppBoxEdit" %>
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
    <li class="active">信息发布管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>信息发布管理<small><i class="icon-double-angle-right"></i>首页弹框编辑</small></h1>
</asp:Content>


<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
            <div class="control-group" >
                    <label class="control-label" for="ckExcel">是否预约:</label>
                    <div class="controls">
                       <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="isTiming" /><span class="lbl"></span>
                    </div>
            </div>
            
            <div class="control-group" >
                    <label class="control-label" for="startTime">发布时间:</label>
                    <div class="controls">
                       <input type="text" name="txtTime" id="txtTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" placeholder="设置发布时间" value="<%=startTime %>"  />
            </div>
            </div>
             <div class="control-group" >
                <label class="control-label" for="StartType">弹框次数:</label>
                <div class="controls">
                      <select id="startType" class="searchpart"  style="width:221px">
                           <option  value="-1">==请选择==</option>
                           <option selected value="1">当天弹出一次</option>
                           <option value="2">当天启动时弹出</option>
                       </select>
                </div>
            </div>
            <div class="control-group" >
                    <label class="control-label" for="updateImage">图片地址:</label>
                     <div class="controls">
                         <div style="float:left;">
                             <input type="text" name="imageUrl" id="imgeUrl"  placeholder="图片链接地址" class="searchpart" value="<%=entity.ImageLink %>"/> 
                         </div>
                     <div style="float:left;">
                            <span id="btn_upload" ></span>
                           <a href="javascript:void(0);" id="import" class="btn btn-purple import">选择</a>
                     </div>
                     <div style="clear:both;"></div>
                    </div>
            </div>

            <div class="control-group" >
                <label class="control-label" for="textArticleLink">文章链接:</label>
                <div class="controls">
                    <input type="text" name="textArticleLink" id="textArticleLink"   placeholder="文章链接地址" class="searchpart" value="<%=entity.ArticleLink %>"/>
                    <a href="javascript:void(0);" id="addArticleLink" class="btn btn-purple">选择</a>
              </div>
           </div>
              
            <div class="control-group" >
                <label class="control-label" for="textButtonText">按钮描述:</label>
                <div class="controls">
                    <input type="text" name="textButtonText" id="textButtonText"  placeholder="按钮描述" class="searchpart"  value="<%=entity.ButtonText %>" />
                </div>
            </div>

            <div class="control-group" >
                <label class="control-label" for="textContent">主体内容:</label>
                <div class="controls">
                    <input type="text" name="textContent" id="textContent"   placeholder="主体内容" class="searchpart" value="<%=entity.Contents %>"/>
                    <span class="lbl">选填</span>
                </div>
            </div>

            <div class="control-group">
                 <label class="control-label" for="submit"></label>
                 <div class="controls">   
                     <a href="javascript:void(0);" id="submit" class="btn btn-purple" >确定</a>
                     <a href="javascript:void(0);" onclick="goback()" id="goBack" class="btn btn-purple" >返回</a>
                 </div>
            </div>
        <input  type="hidden" id="hidStartType" value="<%=entity.StartType %>"/>
        <input  type="hidden" id="hidIsTiming" value="<%=isTiming %>"/>
        <input  type="hidden" id="hidMid" value="<%=entity.MessageID %>"/>
    </div>
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        $(function () {
            var url = _root + "handlers/AppBoxController/Import.ashx";
            $('#import').uploadify({
                uploader: url,                          // 服务器处理地址
                swf: '/js/plugin/uploadify3.2.1/uploadify.swf',
                buttonText: "选择",                //按钮文字
                height: 30,                             //按钮高度
                width: 75,                              //按钮宽度
                fileTypeExts: "*.gif;*.jpg;*.jpeg;*.png",                //允许的文件类型
                fileTypeDesc: "上传图片",           //文件说明   
                formData: { "imgType": "normal" }, //提交给服务器端的参数
                onUploadSuccess: function (file, data, response) {   //一个文件上传成功后的响应事件处理
                    if (data == "false") {
                        bootbox.alert("图片服务器无法连接,请稍后再试！");
                    }
                    else {
                        $("#imgeUrl").val(data);
                    }
                }
            });
            var type = $("#hidStartType").val();
            $("#startType").find("option[value='" + type + "']").attr("selected", true);

            var isTiming = $("#hidIsTiming").val();
            if (isTiming == 0) {
                $("#txtTime").attr("disabled", true);
                $("#isTiming")[0].checked == false;
            } else
            {
                $("#txtTime").attr("disabled", false);
                $("#isTiming")[0].checked == true;
            }
        });
        var appBox = {};
        appBox.init = function () {
            $("#addArticleLink").click(function () {
                asyncbox.open({
                    modal: true,
                    title: "选择专题文章",
                    url: _root + "Article/SelectGroupArticle.aspx?sf=3",
                    width: 950,
                    height: 550,
                    buttons: [{
                        value: '确定',
                        result: 'sure'
                    }],
                    callback: function (btnRes, cntWin, reVal) {
                        if (btnRes == "sure") {
                            if (typeof (reVal) != "undefined" && reVal != "")
                                setGroupArticle(reVal);
                        }
                    }
                });
            });
            $("#isTiming").click(function () {
                if ($(this)[0].checked == true) {
                    var mdate = new Date();
                    mdate.setDate(mdate.getDate() + 1);
                    var strDate = convertTime(mdate);
                    $("#txtTime").val(strDate);
                    $("#txtTime").attr("disabled", false);
                }
                else {
                    getMaxTime();
                }
            });
            $("#submit").click(function () {
                var stime = $("#txtTime").val();
                var ilink = $("#imgeUrl").val();
                var alink = $("#textArticleLink").val();
                var btnText = $("#textButtonText").val();
                var contents = $("#textContent").val();
                var startType = $("#startType").val();
                var mid = $("#hidMid").val();
                $.post(_root + "handlers/AppBoxController/Update.ashx", {
                    "alink": alink,
                    "ilink": ilink,
                    "btnText": btnText,
                    "contents": contents,
                    "stime": stime,
                    "startType": startType,
                    "mid":mid,
                    "ts": new Date().getTime()
                }, function (data) {
                    if (data.indexOf("_") != -1) {
                        var result = data.split('_');
                        bootbox.alert(result[1]);
                    }
                    else {
                        bootbox.alert("已更新完成！", function () {
                            window.location.href = "/Article/AppBoxManager.aspx";
                        });
                      
                    }
                });
            });
        }
        appBox.init();
        var setGroupArticle = function (reval) {
            $.post(_root + "handlers/AppBoxController/GetArticleUrl.ashx", { "aid": reval, "ts": new Date().getTime() }, function (data) {
                $("#textArticleLink").val(data);
            });
        }
        var getMaxTime = function () {
            $.post(_root + "handlers/AppBoxController/GetMaxTime.ashx", { "ts": new Date().getTime() }, function (data) {
                if (data != "null") {
                    if (data.indexOf("_") != -1)//大于今天的改成预约
                    {
                        var result = data.split('_');
                        $("#txtTime").val(result[1]);
                        $("#txtTime").attr("disabled", false);
                        $("#isTiming")[0].checked == true;
                    } else {
                        $("#txtTime").val(data);
                        $("#txtTime").attr("disabled", true);
                    }
                }
            });
        }
        var convertTime = function timeStamp2String(time) {
            var datetime = new Date();
            datetime.setTime(time);
            var year = datetime.getFullYear();
            var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
            var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
            var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
            var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
            var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
            return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
        }
        var goback = function ()
        {
           window.location.href="/Article/AppBoxManager.aspx";
        }
    </script>
</asp:Content>