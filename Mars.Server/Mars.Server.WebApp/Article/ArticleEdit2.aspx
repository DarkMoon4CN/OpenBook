<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ArticleEdit2.aspx.cs" Inherits="Mars.Server.WebApp.Article.ArticleEdit2" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="chosen" Src="~/css/chosen.css" ScriptType="StyleCss" />
  <%--  <OpenBook:OBScript runat="server" ID="autocomplete_css" Src="~/js/plugin/Autocomplete/styles.css" ScriptType="StyleCss" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="fuelux_wizard_js" Src="~/js/plugin/fuelux.wizard.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="validate_js" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="maskedinput_js" Src="~/js/plugin/jquery.maskedinput.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="chosen_js" Src="~/js/plugin/chosen.jquery.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="kindeditor_js" Src="~/js/plugin/kindeditor/kindeditor-min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="kindeditor_zh_js" Src="~/js/plugin/kindeditor/lang/zh_CN.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="swfupload_js" Src="~/js/plugin/SWFUpload/swfupload.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="layer_js" Src="~/js/layer/layer.js" ScriptType="Javascript" />
 <%--   <OpenBook:OBScript runat="server" ID="autocomplete_js" Src="~/js/plugin/Autocomplete/jquery.autocomplete.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="mockjax_js" Src="~/js/plugin/Autocomplete/jquery.mockjax.js" ScriptType="Javascript" />--%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">文章平台发布管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>文章平台发布管理 <small><i class="icon-double-angle-right"></i>发布文章</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑平台文章</h4>
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

                                    <form class="form-horizontal" id="formvalid" method="get">
                                        <div class="control-group">
                                            <label class="control-label" for="content_chkSubpage">是否子页面:</label>
                                            <div class="controls">
                                                <div class="span3">
                                                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkSubpage" runat="server" /><span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="chkSubpage">是否单篇成组:</label>
                                            <div class="controls">
                                                <div class="span3">
                                                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkSingleGroup" runat="server" /><span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_selFirstType">一级分类:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <select class="span3" id="selFirstType" name="selFirstType" runat="server">
                                                        <option value="">--请选择分类--</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_selSecondType">二级分类:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <select class="span3" id="selSecondType" name="selSecondType" runat="server">
                                                        <option value="">--请选择分类--</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="control-group">
                                            <label class="control-label" for="content_selPublishZone">发布地域:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <select class="span5 chzn-select" id="selPublishZone" data-placeholder="请选择地域" multiple="multiple" name="selPublishZone">
                                                        <option value="0">全国</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtActivePlace">活动地点:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" class="span6" runat="server" id="txtActivePlace" name="txtActivePlace" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_lblUrl">URL地址:</label>
                                            <div class="controls">
                                                <div class="span12" style="text-align: left;">
                                                    <%--  <label class="control-label span12" id="lblUrl" runat="server"></label>--%>
                                                    <span id="lblUrl" runat="server"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="hr hr-dotted"></div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtTitle">标题:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" name="txtTitle" id="txtTitle" class="span6" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtContent">摘要:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <textarea name="txtContent" runat="server" id="txtContent" style="width: 70%; height: 100px">
                                                        </textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="imgLogo">封面图片：</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <img id="imgLogo" src="" style="max-width: 70%; max-height: 250px;" runat="server" />
                                                    <br />
                                                    <input id="hidImgurl" type="hidden" runat="server" />
                                                    <input type="button" id="btnImage" datatype="add" value="选择图片" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="imgThemeLogo">主题图片：</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <img id="imgThemeLogo" src="" style="max-width: 70%; max-height: 250px;" runat="server" />
                                                    <br />
                                                    <input id="hidThemeImgurl" type="hidden" runat="server" />
                                                    <input type="button" id="btnThemeImage" datatype="add" value="选择图片" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="imgThemeLogo">发现轮播图片：</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <img id="imgCarouselLogo" src="" style="max-width: 70%; max-height: 250px;" runat="server" />
                                                    <br />
                                                    <input id="hidCarouselImgurl" type="hidden" runat="server" />
                                                    <input type="button" id="btnCarouselImage" datatype="add" value="选择图片" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                         <div class="control-group">
                                            <label class="control-label" for="content_selSecondType">移动端编辑:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="scene" class="ace-switch ace-switch-2" type="checkbox" id="scene" runat="server" />
                                                    <span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtHtml">文章正文:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <textarea id="txtHtml" name="txtHtml" runat="server" class="span12" style="width: 70%; height: 450px">
                                                         </textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtHtml">添加书单:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <span id="spanButtonPlaceholder1"></span>
                                                    <span style="vertical-align: top;">（最大文件大小<% = Mars.Server.Utils.WebMaster.UploadBookListSize %>，多个文件，请打包上传）</span>

                                                    <dl class="regForm-item">
                                                        <dt class="regForm-item-tit"></dt>
                                                        <dd class="regForm-item-ct">
                                                            <div style="text-align: left; float: left; width: 350px;" id="progress1">
                                                                <div style="background-color: #015c93; width: 0%; height: 22px;" id="progress_inner1"></div>
                                                            </div>
                                                            <label style="text-align: left; float: left;" id="uploadstate1"></label>
                                                        </dd>
                                                    </dl>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="hr hr-dotted"></div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtPublishTime">发布日期:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" id="txtPublishTime" name="txtPublishTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtPublishSource">内容来源:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" id="txtPublishSource" name="txtPublishSource" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="chkRecommend">是否推荐:</label>
                                            <div class="controls">
                                                <div class="span3">
                                                    <input name="switch-field-1" runat="server" class="ace-switch ace-switch-2" type="checkbox" id="chkRecommend" /><span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="chkAdvert">首页轮播:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="switch-field-1" runat="server" class="ace-switch ace-switch-2" type="checkbox" id="chkAdvert" /><span class="lbl"></span>
                                                    首页轮播结束时间：<input type="text" id="txtAdsEndTime" name="txtAdsEndTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" runat="server"/>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="chkDiscoverAdvert">发现轮播:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="switch-field-1" runat="server" class="ace-switch ace-switch-2" type="checkbox" id="chkDiscoverAdvert" /><span class="lbl"></span>
                                                    发现轮播结束时间：<input type="text" id="txtDiscoverAdsEndTime" name="txtDiscoverAdsEndTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" runat="server"/>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtAdvertOrder">轮播顺序:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input id="txtAdvertOrder" type="text" runat="server" value="1000" name="txtAdvertOrder" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label" for="chkAllday">全天 :</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkAllday" /><span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>
                                       


                                        <div class="control-group">
                                            <label class="control-label" for="txtStartTime">开始日期:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkStartTime" /><span class="lbl"></span>
                                                   <%--  <input type="text" name="txtStartTime" id="txtStartTime" class="Wdate" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss', onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}'})" disabled="disabled"  value="4000-01-01"/>--%>
                                                    <input type="text" name="txtStartTime" id="txtStartTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"  value="4000-01-01 00:00:00" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="txtEndTime">结束日期:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" id="chkEndTime" /><span class="lbl"></span>
                                                   <%-- <input type="text" name="txtEndTime" id="txtEndTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'txtStartTime\')}' })" disabled="disabled"  value="4000-01-01"/>--%>
                                                    <input type="text" name="txtEndTime" id="txtEndTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm' })"  value="4000-01-01 00:00:00" />
                                                </div>
                                            </div>
                                        </div>

                                        

                                        <div class="hr hr-dotted"></div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_chkFestArticle">是否节日文章:</label>
                                            <div class="controls">
                                                <div class="span3">
                                                    <input name="switch-field-1" runat="server" class="ace-switch ace-switch-2" type="checkbox" id="chkFestArticle" /><span class="lbl"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_selFestival">选择节日:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <select class="chzn-select" id="selFestival" data-placeholder="请选择节日" name="selFestival" disabled="disabled">
                                                        <option value="">--请选择节日--</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>

                                  <%--      <div class="control-group">
                                            <label class="control-label" for="content_txtTitle2">节日标题:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" name="country" id="autocomplete" />
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtTitle2">节日标题:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" name="txtTitle2" id="txtTitle2" runat="server" class="span6" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="control-group">
                                            <label class="control-label" for="content_txtStartTime2">节日开始日期:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" name="txtStartTime2" id="txtStartTime2" runat="server" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="control-group">
                                            <label class="control-label" for="content_txtEndTime2">节日结束日期:</label>
                                            <div class="controls">
                                                <div class="span12">
                                                    <input type="text" name="txtEndTime2" id="txtEndTime2" runat="server" disabled="disabled" />
                                                </div>
                                            </div>
                                        </div>

                                        <input type="hidden" id="hidStartTime" runat="server" />
                                        <input type="hidden" id="hidEndTime" runat="server" />
                                        <input type="hidden" id="hidPublishTime" runat="server" />
                                        <input type="hidden" id="hidPid" value="0" runat="server" />
                                        <input type="hidden" id="hidFun" runat="server" />
                                        <input type="hidden" id="hidallday" runat="server" />
                                        <input type="hidden" id="hidPublichAreaID" runat="server" />
                                        <input type="hidden" id="hidBookListPath" runat="server" />
                                        <input type="hidden" id="hidFestivalID" runat="server" />
                                    </form>
                                </div>
                            </div>
                            <hr />

                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" id="btnSave" onclick="javascript:saveobj();">保存</button>
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


        function timeStamp2String(time) {
            var datetime = new Date();
            datetime.setTime(time);
            var year = datetime.getFullYear();
            var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
            var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
            var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
            var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
            var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
            return year + "-" + month + "-" + date + " " + hour + ":" + minute  ;
        }
        $(function () {
            InitUploadModule();
            InitUploadFile();

            $(document).on("click", "#delBookFile", function () {
                bootbox.confirm("确定要删除这个文件吗?", function (result) {
                    if (result) {
                        var fn = $("#delBookFile").attr("fullpath");
                        if (fn) {
                            var pid = $("#content_hidPid").val();
                            $.post(_root + "handlers/ArticleController/DestroyFiles.ashx", { "filename": fn, "pid": pid, "ts": new Date().getTime() }, function (data) {
                                var json = eval("(" + data + ")");

                                if (json.state == "1") {
                                    bootbox.dialog("删除成功", [{
                                        "label": "OK",
                                        "class": "btn-small btn-primary",
                                        callback: function () {
                                            var stats = swfu.getStats();
                                            stats.successful_uploads--;
                                            swfu.setStats(stats);
                                            //swfu.cancelUpload(file.id);

                                            $("#uploadstate1").text("");
                                            $("#delBookFile").parent().html("<div style=\"background-color:#015c93; width:0%; height:22px;\" id=\"progress_inner1\"></div>");
                                        }
                                    }]);
                                }
                                else if (json.state == "-2") {
                                    parent.layer.msg("文件不存在或已删除！");
                                }
                                else if (json.state == "0") {
                                    parent.layer.msg("删除失败！");
                                }
                                else {
                                     parent.layer.msg("数据传输错误！");
                                }
                            });
                        }
                    }
                });
            });
          
        });
        var InitUploadFile = function () {
            var hidPath = $("#content_hidBookListPath").val();
            if (hidPath) {
                var filename = hidPath.replace(/(\/\w+)+\//g, "")
                var createEle = "<span><a id='aBookFile' href=\"" + hidPath + "\" target=\"_blank\">" + filename + "</a></span>";
                createEle += "<a href=\"javascript:void(0)\" id=\"delBookFile\" fullpath='" + hidPath + "' style=\"margin-left:15px;\">删除</a>";
                $("#progress1").html(createEle);
            }
        }

        var swfu = null;
        var InitUploadModule = function () {
            swfu = new SWFUpload({
                // Backend Settings
                upload_url: _root + "handlers/ArticleController/ReceiveFile.ashx",
                post_params: {
                    "ASPSESSID": "<%=Session.SessionID %>"
                },

                // File Upload Settings
                file_size_limit: "<% = Mars.Server.Utils.WebMaster.UploadBookListSize%>",
                file_types: "*.xls;*.xlsx;*.doc;*.docx;*.rar;*.zip;",
                file_types_description: "Excel文件|Word文件|压缩包文件",
                file_upload_limit: "1",    // Zero means unlimited

                file_dialog_complete_handler: fileDialogComplete,
                file_queue_error_handler: fileQueueErrorHandler,
                upload_progress_handler: uploadProgress,
                upload_error_handler: uploadError,
                upload_success_handler: uploadSuccess,
                swfupload_loaded_handler: loadedHandler,

                // Button settings
                button_image_url: "../images/swfu_bgimg.png",
                button_placeholder_id: "spanButtonPlaceholder1",
                button_width: 78,
                button_height: 27,
                button_text: '<span class="button">选择文件</span>',
                button_text_style: '.button { font-family: Helvetica, Arial, sans-serif; font-size: 12pt;color: #666666; }',
                button_text_top_padding: 3,
                button_text_left_padding: 14,

                // Flash Settings
                flash_url: "../js/plugin/SWFUpload/swfupload.swf", // Relative to this file

                custom_settings: {
                    upload_target: "divFileProgressContainer"
                },
                // Debug Settings
                debug: false
            });
        }

        function loadedHandler() {
            var hidPath = $("#content_hidBookListPath").val();
            if (hidPath) {
                var stats = this.getStats();

                if (stats.successful_uploads == 0) {
                    stats.successful_uploads++;
                    this.setStats(stats);
                }
            }
        }

        function fileDialogComplete(numFilesSelected, numFilesQueued) {
            try {
                if (numFilesQueued > 0) {
                    this.startUpload();
                }
            } catch (ex) {
                bootbox.alert(ex.message);
               
                this.debug(ex);
            }
        }

        function uploadProgress(file, bytesLoaded) {
            try {
                var percent = Math.ceil((bytesLoaded / file.size) * 100);
                $("#progress_inner1").css("width", percent + "%");
                $("#uploadstate1").text("上传中..(" + percent + "%)");
            } catch (ex) {
                this.debug(ex);
            }
        }

        function uploadError(file, errorCode, message) {
            bootbox.alert(message);
        }

        function uploadSuccess(file, serverData) {
            try {
                var json = eval("(" + serverData + ")");
                $("#uploadstate1").text("上传成功");

                var createEle = "<span><a id='aBookFile' href=\"" + json["fullpath"] + "\" target=\"_blank\">" + json["filename"] + "</a></span>";
                createEle += "<a href=\"javascript:void(0)\" id=\"delBookFile\" fullpath='" + json["fullpath"] + "' style=\"margin-left:15px;\">删除</a>";
                $("#progress1").html(createEle);

            } catch (ex) {
                this.debug(ex);
            }
        }

        function fileQueueErrorHandler(fileObject, errorCode, message) {

            if (errorCode == SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED) {
                parent.layer.msg("超过上传文件数量限制");
            }
            else if (errorCode == SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT) {
                parent.layer.msg("超过上传文件大小限制");
            }
            else if (errorCode == SWFUpload.QUEUE_ERROR.INVALID_FILETYPE) {
                parent.layer.msg("上传文件类型错误");
            }
            else {
                parent.layer.msg("上传书单出现未知错误");
            }
        }

        var validformcontent = function () {

            if ($("#content_chkSingleGroup")[0].checked) {
                var hidImgurl = $.trim($("#content_hidImgurl").val());
                var imgLogo = $.trim($("#content_imgLogo").attr("src"));
                if (hidImgurl == "" || imgLogo == "") {
                    parent.layer.msg("单篇成组文章必须上传封面图片！");
                    return false;
                }
            }

            var txthtml = $.trim(editor.html());
            if (txthtml == "") {
                bparent.layer.msg("请填写文章正文！");
                return false;
            }

            return true;
        }

        function setCookie(name, value) {
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        }

        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

            if (arr = document.cookie.match(reg)) {
                return unescape(arr[2]);
            }
            else {
                return null;
            }
        }
        $(function () {
            $("#content_scene").click(function () {
                if ($(this)[0].checked != true) {
                    setCookie('autoHeightMode', false);
                }
                else
                {
                    setCookie('autoHeightMode', true);
                }
                window.location.reload();
            });


            $("#content_chkSubpage").click(function () {
                if ($(this)[0].checked) {
                    $("#content_selFirstType").val("");
                    $("#content_selFirstType").attr("disabled", true);

                    $("#content_selSecondType").val("");
                    $("#content_selSecondType").attr("disabled", true);

                    $("#content_chkSingleGroup")[0].checked = false;
                    $("#content_chkSingleGroup").attr("disabled", true);
                }
                else {
                    $("#content_selFirstType").attr("disabled", false);
                    $("#content_selSecondType").attr("disabled", false);
                    $("#content_chkSingleGroup").attr("disabled", false);
                }
            });

            $("#chkStartTime").click(function () {
                var mdate = new Date();
                if ($(this)[0].checked == true) {
                    mdate.setHours(0);
                    mdate.setMinutes(0);
                    mdate.setSeconds(0);
                    var mstime = timeStamp2String(mdate);
                    $("#txtStartTime").val(mstime);
                    $("#txtStartTime").attr("disabled", false);
                }
                else {
                    $("#txtStartTime").val("4000-01-01 00:00:00");
                    $("#txtStartTime").attr("disabled", true);
                    if ($("#chkEndTime")[0].checked == false) {
                        $("#chkAllday")[0].checked = false;
                    }
                }
            });
            
            $("#chkEndTime").click(function () {
                var mdate = new Date();
                if ($(this)[0].checked == true) {
                    mdate.setHours(17);
                    mdate.setMinutes(0);
                    mdate.setSeconds(0);
                    var mstime = timeStamp2String(mdate);
                    $("#txtEndTime").val(mstime);
                    $("#txtEndTime").attr("disabled", false);
                }
                else {
                    $("#txtEndTime").val("4000-01-01 00:00:00");
                    $("#txtEndTime").attr("disabled", true);
                    if ($("#chkStartTime")[0].checked == false) {
                        $("#chkAllday")[0].checked = false;
                    }
                }
            });

            $("#chkAllday").click(function () {
                var mdate = new Date();
                if ($(this)[0].checked == true) {
                    mdate.setHours(9);
                    mdate.setMinutes(0);
                    mdate.setSeconds(0);
                    var mstime = timeStamp2String(mdate);
                    $("#txtStartTime").val(mstime);
                    mdate.setHours(17);
                    var metime = timeStamp2String(mdate);
                    $("#txtEndTime").val(metime);
                   
                    $("#txtEndTime").attr("disabled", false);
                    $("#txtStartTime").attr("disabled", false);
                    
                   
                    $("#chkStartTime")[0].checked = true;
                    $("#chkEndTime")[0].checked = true;
                }
                else
                {
                    mdate.setHours(0);
                    mdate.setMinutes(0);
                    mdate.setSeconds(0);
                    var mstime = timeStamp2String(mdate);

                    $("#txtStartTime").val(mstime);
                    mdate.setHours(23);
                    mdate.setMinutes(59);
                    var metime = timeStamp2String(mdate);
                    $("#txtEndTime").val(metime);
                }
            });

            $("#content_chkFestArticle").click(function () {
                if ($(this)[0].checked) {
                    $("#selFestival").attr("disabled", false).trigger("liszt:updated");
                }
                else {
                    $("#content_txtTitle2").val("");
                    $("#content_txtStartTime2").val("");
                    $("#content_txtEndTime2").val("");
                    $("#selFestival").attr("disabled", true).val("").trigger("liszt:updated");
                }
            });

            $("#txtPublishTime").val($("#content_hidPublishTime").val());

            //判定是否是当天 start
            var hidStartTime = $("#content_hidStartTime").val();
            var hidEndTime = $("#content_hidEndTime").val();
            var hidStartDate = new Date(hidStartTime);
            var hidEndDate = new Date(hidEndTime);
            $("#txtStartTime").val(hidStartTime);
            $("#txtEndTime").val(hidEndTime);
            
            if (hidStartTime == "4000-01-01 00:00:00" || hidEndTime == "4000-01-01 00:00:00") {
                $("#txtStartTime").attr("disabled", true);
                $("#txtEndTime").attr("disabled", true);
                $("#chkStartTime")[0].checked == false;
                $("#chkEndTime")[0].checked == false;
                $("#chkAllday")[0].checked == false;
            }
            else
            {
                $("#chkStartTime")[0].checked == false;
                $("#chkEndTime")[0].checked == false;
            }
            if (hidStartTime.split(" ")[0] == hidEndTime.split(" ")[0])
            {
                $("#chkAllday").attr("checked", "checked");
                $("#chkAllday")[0].checked == true;
            }
            else
            {
                $("#chkAllday")[0].checked == false;
            }
            //判定是否是当天 end

        });

        $(function () {
            $('#formvalid').validate({
                errorElement: 'span',
                errorClass: 'help-inline',
                focusInvalid: false,
                rules: {
                    ctl00$content$selFirstType: { required: true, digits: true },
                    ctl00$content$selSecondType: { digits: true },
                    selPublishZone: { required: true },
                    ctl00$content$txtTitle: {
                        required: true, rangelength: [2, 100], remote: {
                            type: "POST",
                            url: _root + "handlers/ArticleController/IsUseableTitle.ashx",
                            data: {
                                title: function () { return $("#content_txtTitle").val() },
                                pid: function () { return $("#content_hidPid").val() }
                            }
                        }
                    },
                    ctl00$content$txtContent: { required: true, rangelength: [2, 150] },
                    ctl00$content$txtHtml: { required: true },
                    txtPublishTime: { required: true },
                    ctl00$content$txtAdvertOrder: {
                        required: true, digits: true,
                        remote: {
                            type: "POST",
                            url: _root + "handlers/ArticleController/IsCanSetOrderNum.ashx",
                            data: {
                                "pid": function () { return $("#content_hidPid").val() },
                                "advertorder": function () { return $("#content_txtAdvertOrder").val(); }
                            }
                        }
                    },
                    txtStartTime: { required: true },
                    txtEndTime: { required: true },
                    ctl00$content$txtActivePlace: { maxlength: 100 },
                    ctl00$content$txtPublishSource: { required: true, maxlength: 100 },
                    txtAllDay: { required: true },
                    selFestival: { required: true }
                },
                messages: {
                    ctl00$content$selFirstType: { required: "必填项", digits: "请选择一级分类" },
                    ctl00$content$selSecondType: { digits: "请选择二级分类" },
                    selPublishZone: { required: "必选项" },
                    ctl00$content$txtTitle: { required: "必填项", rangelength: "文本长度必须介于2和100个字符之间", remote: jQuery.format("文章标题系统中已存在") },
                    ctl00$content$txtContent: { required: "必填项", rangelength: "文本长度必须介于2和150个字符之间" },
                    ctl00$content$txtHtml: { required: "必填项" },
                    txtPublishTime: { required: "必填项" },
                    ctl00$content$txtAdvertOrder: { required: "必填项", digits: "请输入整数数字", remote: "轮播顺序号已被占用，请重新设置" },
                    txtStartTime: { required: "必填项" },
                    txtEndTime: { required: "必填项" },
                    ctl00$content$txtActivePlace: { maxlength: "输入长度最多不能超过100个字符" },
                    ctl00$content$txtPublishSource: { required: "必填项", maxlength: "输入长度最多不能超过100个字符" },
                    txtAllDay: { required: "必填项" },
                    selFestival: { required: "必填项" }
                },
                invalidHandler: function (event, validator) { //display error alert on form submit   
                    $('.alert-error', $('.login-form')).show();
                },
                highlight: function (e) {
                    $(e).closest('.control-group').removeClass('info').addClass('error');
                },
                success: function (e) {
                    $(e).closest('.control-group').removeClass('error').addClass('info');
                    $(e).remove();
                },
                errorPlacement: function (error, element) {
                    if (element.is(':checkbox') || element.is(':radio')) {
                        var controls = element.closest('.controls');
                        if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                        else error.insertAfter(element.nextAll('.lbl').eq(0));
                    }
                    else if (element.is('.chzn-select')) {
                        error.insertAfter(element.nextAll('[class*="chzn-container"]').eq(0));
                    }
                    else error.insertAfter(element);
                },
                submitHandler: function (form) {
                },
                invalidHandler: function (form) {
                }
            });

            // documentation: http://docs.jquery.com/Plugins/Validation/validate

        });//end        

        $(function () {
            $("#content_chkAdvert").change(function () {
                var originstatus = !$(this)[0].checked;

                if ($(this)[0].checked) {
                    var pid = $("#content_hidPid").val();
                    $.post(_root + "handlers/ArticleController/IsCanSetAdvert.ashx", { "pid": pid }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {

                        }
                        else if (json.status == "0") {
                            $("#content_chkAdvert")[0].checked = originstatus;
                            parent.layer.msg("首页轮播数量不能超过" + json.num + "个", { time: 1000 });
                            return false;
                        }
                        else {
                            $("#content_chkAdvert")[0].checked = originstatus;
                            parent.layer.msg("系统判断出现异常，不可设置轮播！", { time: 1000 });
                            return false;
                        }

                        $("#content_txtAdvertOrder").attr("disabled", false);
                    });
                }
                else {
                    $("#content_txtAdvertOrder").attr("disabled", true);
                }
            });

            $("#content_chkDiscoverAdvert").change(function () {

                var originstatus = !$(this)[0].checked;

                if ($(this)[0].checked) {
                    $.post(_root + "handlers/ArticleController/IsCanSetCarousel.ashx", { "pid": 0 }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            if ($(this)[0].checked && $.trim($("#content_hidCarouselImgurl").val()) == "") {
                                parent.layer.msg("请返回“上一步”上传”发现轮播图片!", { time: 1000 });
                                $(this)[0].checked = false;
                                return;
                            }
                        }
                        else if (json.status == "0") {
                            $("#content_chkDiscoverAdvert")[0].checked = originstatus;
                            parent.layer.msg("发现轮播数量不能超过" + json.num + "个", { time: 1000 });
                            return false;
                        }
                        else {
                            $("#content_chkDiscoverAdvert")[0].checked = originstatus;
                            parent.layer.msg("系统判断出现异常，不可设置轮播！", { time: 1000 });
                            return false;
                        }
                    });
                }
            });

            $("#content_selFirstType").change(function () {
                var selvalue = $(this).val();

                if (selvalue == -1) {
                    $("#content_selSecondType").val(-1);
                    $("#content_selSecondType option:not(:eq(0))").remove();
                }
                else {
                    $("#content_selSecondType").val(-1);
                    $("#content_selSecondType option:not(:eq(0))").remove();

                    $.post(_root + "handlers/CommonDictController/GetSecondCalendarType.ashx", { pid: selvalue }, function (data) {
                        var json = eval("(" + data + ")");

                        $.each(json, function (index, item) {
                            $("#content_selSecondType").append("<option value='" + item.CalendarTypeID + "'>" + item.CalendarTypeName + "</option>");
                        });
                    });
                }
            });

            $.post(_root + "handlers/CommonDictController/GetProvinceToGroup.ashx", function (data) {
                var json = eval("(" + data + ")");

                $.each(json, function (index, item) {
                    var optgroup = "<optgroup label='" + item.ZoneName + "'>";

                    $.each(item.ZoneList, function (index, item) {
                        optgroup += "<option value='" + item.ZoneID + "'>" + item.ZoneName + "</option>";
                    });
                    optgroup += "</optgroup>";
                    $("#selPublishZone").append(optgroup);
                });

                chose_mult_set_ini("selPublishZone", $("#content_hidPublichAreaID").val());
                $("#selPublishZone").chosen({ no_results_text: "未找此选项" });
            });

            $.post(_root + "handlers/CommonDictController/GetFestivalList.ashx", function (data) {
                var json = eval("(" + data + ")");
               
                //var option = "";
                //$.each(json, function (index, item) {
                //    option += "<option stime='" + item.StartTime + "' etime='" + item.EndTime + "' value='" + item.FestivalID + "'>" + item.FestivalName + "</option>";
                //});
                //$("#selFestival").append(option);

                $.each(json, function (index, item0) {
                    var optgroup = "<optgroup label='" + item0.FestivalName + "'>";                  

                    $.each(item0.FestivalList, function (index, item) {                      
                        optgroup += "<option stime='" + item.StartTime + "' etime='" + item.EndTime + "' value='" + item.FestivalID + "'>" + item.FestivalName + "</option>";
                    });
                    optgroup += "</optgroup>";
                    $("#selFestival").append(optgroup);
                });              

                $("#selFestival").chosen({ no_results_text: "未找到此选项" }).change(function () {                  
                    //var fesobj = $(this).children('option:selected')
                    var fesobj = $(this).find("option:selected");
                    if (fesobj.val()) {
                        $("#content_txtTitle2").val(fesobj.text());
                        $("#content_txtStartTime2").val(fesobj.attr("stime").substr(0, 10));
                        $("#content_txtEndTime2").val(fesobj.attr("etime").substr(0, 10));
                    }
                    else {
                        $("#content_txtTitle2").val("");
                        $("#content_txtStartTime2").val("");
                        $("#content_txtEndTime2").val("");
                    }
                });

                if ($("#content_hidFestivalID").val()) {
                    $("#selFestival").attr("disabled", false);
                    $("#selFestival").val($("#content_hidFestivalID").val()).trigger("liszt:updated");
                }
            });
        });//end

        function chose_mult_set_ini(select, values) {
            var arr = values.split(',');
            var length = arr.length;
            var value = '';
            for (i = 0; i < length; i++) {
                value = arr[i];
                $("#" + select + " [value='" + value + "']").attr('selected', 'selected');
            }
            $("#" + select).trigger("liszt:updated");
        }

        var editor;
        KindEditor.ready(function (K) {
            var heightMode = getCookie('autoHeightMode');
            var hMode = false;
            if (heightMode == "true") {
                $("#content_scene")[0].checked = true;
                hMode = true;
            } else
            {
                $("#content_scene")[0].checked = false;
            }
            editor = K.create('#content_txtHtml', {
                cssData: 'body {font-family: "宋体"; font-size: 16px; text-align:justify; line-height:1.75;color:#333333;}',
                uploadJson: _root + 'js/plugin/kindeditor/upload_json.ashx',
                showRemote: false,
                filterMode: false,
                afterUpload: function (url, data) {
                    //alert(data.imgname);                 
                },
                items: ['source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                        'superscript', 'clearhtml', 'quickformat',
                        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', 'image', 'multiimage', 'flash', 'media',
                        'table', 'hr', 'emoticons', 'baidumap', 'pagebreak', 'anchor', 'link', 'unlink', '/',
                        'selectall', '|', 'fullscreen'],
                autoHeightMode: hMode,
                afterCreate: function () {
                    this.loadPlugin('autoheight');
                }
            });
            K('#content_btnImage').click(function () {
                if ($(this).attr("datatype") == "add") {
                    editor.loadPlugin('image', function () {
                        editor.plugin.imageDialog({
                            imageUrl: K('#content_imgLogo').attr("src"),
                            showRemote: false,
                            afterUpload: function (url, data) {
                                //alert(11);
                                //alert(data.imgname);
                            },
                            clickFn: function (url, title, width, height, border, align) {
                                K('#content_imgLogo').attr("src", url).css({ "max-width": "70%", "max-height": "250px", "display": "inline" });
                                $('#content_hidImgurl').val(url);
                                $("#content_btnImage").val("删除图片").attr("datatype", "del");

                                editor.hideDialog();
                            }
                        });
                    });
                }
                else {
                    bootbox.confirm("您确认要删除当前图片吗?(删除后将不可恢复)", function (result) {
                        if (result) {
                            var imgurl = $('#content_hidImgurl').val();
                            var pid = $("#content_hidPid").val();
                            $.post(_root + "handlers/ArticleController/DelConverPic.ashx", { "pid": pid, "imgurl": imgurl }, function (data) {
                                var json = eval("(" + data + ")");

                                if (json.status != "-1") {
                                    $("#content_imgLogo").attr("src", "").css("display", "none");
                                    $("#content_hidImgurl").val("");
                                    $("#content_btnImage").val("选择图片").attr("datatype", "add");
                                 
                                    parent.layer.msg("删除成功!", { time: 1000 });
                                }
                                  
                                else {
                                   
                                    parent.layer.msg("数据传输错误!", { time: 1000 });
                                }
                            });
                        }
                    });
                }
            });

            K('#content_btnThemeImage').click(function () {
                if ($(this).attr("datatype") == "add") {
                    editor.loadPlugin('image', function () {
                        editor.plugin.imageDialog({
                            imageUrl: K('#content_imgThemeLogo').attr("src"),
                            showRemote: false,
                            afterUpload: function (url, data) {
                                //alert(11);
                                //alert(data.imgname);
                            },
                            clickFn: function (url, title, width, height, border, align) {
                                K('#content_imgThemeLogo').attr("src", url).css({ "max-width": "70%", "max-height": "250px", "display": "inline" });
                                $('#content_hidThemeImgurl').val(url);
                                $("#content_btnThemeImage").val("删除图片").attr("datatype", "del");

                                editor.hideDialog();
                            }
                        });
                    });
                }
                else {
                    bootbox.confirm("您确认要删除当前图片吗?(删除后将不可恢复)", function (result) {
                        if (result) {
                            var imgurl = $('#content_hidThemeImgurl').val();
                            var pid = $("#content_hidPid").val();
                            $.post(_root + "handlers/ArticleController/DelThemePic.ashx", { "pid": pid, "imgurl": imgurl }, function (data) {
                                var json = eval("(" + data + ")");

                                if (json.status != "-1") {
                                    $("#content_imgThemeLogo").attr("src", "").css("display", "none");
                                    $("#content_hidThemeImgurl").val("");
                                    $("#content_btnThemeImage").val("选择图片").attr("datatype", "add");
                                    parent.layer.msg("删除成功!", { time: 1000 });
                                }
                                    //else if (json.status == "0") {
                                    //    bootbox.alert("删除失败!");
                                    //    return false;
                                    //}
                                else {
                                
                                    parent.layer.msg("数据传输错误!", { time: 1000 });
                                    return false;
                                }
                            });
                        }
                    });
                }
            });

            K('#content_btnCarouselImage').click(function () {
                if ($(this).attr("datatype") == "add") {
                    editor.loadPlugin('image', function () {
                        editor.plugin.imageDialog({
                            imageUrl: K('#content_imgCarouselLogo').attr("src"),
                            showRemote: false,
                            afterUpload: function (url, data) {
                                //alert(11);
                                //alert(data.imgname);
                            },
                            clickFn: function (url, title, width, height, border, align) {
                                K('#content_imgCarouselLogo').attr("src", url).css({ "max-width": "70%", "max-height": "250px", "display": "inline" });
                                $('#content_hidCarouselImgurl').val(url);
                                $("#content_btnCarouselImage").val("删除图片").attr("datatype", "del");

                                editor.hideDialog();
                            }
                        });
                    });
                }
                else {
                    bootbox.confirm("您确认要删除当前图片吗?(删除后将不可恢复)", function (result) {
                        if (result) {
                            var imgurl = $('#content_hidCarouselImgurl').val();
                            var pid = $("#content_hidPid").val();
                            $.post(_root + "handlers/ArticleController/DelCarouselPic.ashx", { "pid": pid, "imgurl": imgurl }, function (data) {
                                var json = eval("(" + data + ")");

                                if (json.status != "-1") {
                                    $("#content_imgCarouselLogo").attr("src", "").css("display", "none");
                                    $("#content_hidCarouselImgurl").val("");
                                    $("#content_btnCarouselImage").val("选择图片").attr("datatype", "add");
                                    parent.layer.msg("删除成功!", { time: 1000 });
                                }
                                    //else if (json.status == "0") {
                                    //    bootbox.alert("删除失败!");
                                    //    return false;
                                    //}
                                else {
                                   
                                    parent.layer.msg("数据传输错误！", { time: 1000 });
                                    return false;
                                }
                            });
                        }
                    });
                }
            });
          
        });

        var saveobj = function () {
            parent.layer.msg("正在保存修改....", { time: 500 });
            if (!$("#formvalid").valid()) {
                return false;
            }
            else {
                if (!validformcontent()) {
                    return false;
                }

                //提交保存
                //构建json对象
                var selFirstType = $("#content_selFirstType").val();
                var selSecondType = $("#content_selSecondType").val();
                var selPublishZone = encodeURI($("#selPublishZone").val());
                var txtActivePlace = $("#content_txtActivePlace").val();

                var txtTitle = encodeURIComponent($.trim($("#content_txtTitle").val()));
                var txtContent = encodeURIComponent($.trim($("#content_txtContent").val()));
                var hidImgurl = encodeURI($("#content_hidImgurl").val());
                var hidThemeImgurl = encodeURI($("#content_hidThemeImgurl").val())
                var txtHtml = encodeURIComponent(editor.html());

                var txtPublishTime = $("#txtPublishTime").val();
                var txtPublishSource = $("#content_txtPublishSource").val();
                var chkRecommend = $("#content_chkRecommend")[0].checked ? 1 : 0;
                var chkAdvert = $("#content_chkAdvert")[0].checked ? 1 : 0;
                var adsendtime = $("#content_txtAdsEndTime").val();
                var txtAdvertOrder = $("#content_txtAdvertOrder").val();
                var txtStartTime = $("#txtStartTime").val();
                var txtEndTime = $("#txtEndTime").val();
                var aBookFile = $("#aBookFile").attr("href") ? encodeURI($("#aBookFile").attr("href")) : "";

                var hidPid = $("#content_hidPid").val();

                var allday = "";
                if ($("#chkAllday")[0].checked && $("#txtAllDay").val() != "") {
                    allday = $("#txtAllDay").val();
                }

                var eventItemFlag = $("#content_chkFestArticle")[0].checked ? 1 : 0;
                var txtTitle2 = $("#content_txtTitle2").val();
                var txtStartTime2 = $("#content_txtStartTime2").val();
                var txtEndTime2 = $("#content_txtEndTime2").val();
                var hidCarouselImgurl = $("#content_hidCarouselImgurl").val();
                var discoveradvert = $("#content_chkDiscoverAdvert")[0].checked ? 2 : 0;
                var discoveradsendtime = $("#content_txtDiscoverAdsEndTime").val();
                var chkSingleGroup = $("#content_chkSingleGroup")[0].checked ? 1 : 0;
                var selFestival = $("#selFestival").val();

                var dataparas = {
                    "firsttype": selFirstType, "secondtype": selSecondType, "publishzone": selPublishZone, "activeplace": txtActivePlace, "title": txtTitle, "content": txtContent,
                    "imgurl": hidImgurl, "imgthemeurl": hidThemeImgurl, "imgcarouselurl": hidCarouselImgurl, "html": txtHtml, "publishtime": txtPublishTime, "publishsource": txtPublishSource, "recommend": chkRecommend, "advert": chkAdvert, "advertorder": txtAdvertOrder,
                    "starttime": txtStartTime, "endtime": txtEndTime, "pid": hidPid, "allday": allday, "booklistpath": aBookFile, "singlegroup": chkSingleGroup, "festivalid": selFestival, "eventItemFlag": eventItemFlag, "title2": txtTitle2, "starttime2": txtStartTime2,
                    "endtime2": txtEndTime2, "discoveradvert": discoveradvert, "ts": new Date().getTime(), "adsendtime": adsendtime, "discoveradsendtime": discoveradsendtime
                };

                $("#btnSave").text("保存中...").attr("disabled", true);

                $.post(_root + "handlers/ArticleController/Update.ashx", dataparas, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.status == "0") {
                        $("#btnSave").text("保存").attr("disabled", false);
                        parent.layer.msg("保存失败！");
                        return false;
                    }
                    else if (json.status == "1") {
                        parent.layer.msg("保存成功！", { time: 1000 }, function () {
                            window.location.href = "ArticleManager.aspx?fun=" + $("#content_hidFun").val();
                        });
                    }
                    else if (json.status == "2") {
                        $("#btnSave").text("保存").attr("disabled", false);
                        parent.layer.msg("系统中已存在该文章标题！", { time: 1500 });
                        return false;
                    }
                    else {
                        $("#btnSave").text("保存").attr("disabled", false);
                        parent.layer.msg("数据传输错误！", { time: 1500 });
                        return false;
                    }
                });
            }
        };
    </script>
</asp:Content>
