<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_StartPicEdit.aspx.cs" Inherits="Mars.Server.WebApp.AppConfigure._StartPicEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="wdatepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="kindeditor_js" Src="~/js/plugin/kindeditor/kindeditor-min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="kindeditor_zh_js" Src="~/js/plugin/kindeditor/lang/zh_CN.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group childnotneed">
            <label class="control-label" for="content_sel_exhibitor">上传图片:</label>
            <div class="controls">
                <div class="span12">
                    <img id="imgPic" src="" style="display: none;" />
                    <br />
                    <input id="hidImgurl" type="hidden" />
                    <input type="button" id="btnImage" datatype="add" value="选择图片" />
                </div>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">显示时间:</label>
            <div class="controls">
                <input type="text" id="txt_starttime" name="txt_starttime" class="Wdate" readonly="readonly" 
                    onfocus="var content_txt_endTime=$dp.$('content_txt_endTime');
                    WdatePicker({ onpicked:function(){content_txt_endTime.focus();},
                    maxDate:'#F{$dp.$D(\'content_txt_endTime\')}',dateFmt:'yyyy-MM-dd HH:mm:ss'})" 
                    placeholder="请输入有效开始时间" runat="server" />
                到
                <input type="text" id="txt_endTime" name="txt_endTime" class="Wdate" readonly="readonly" 
                    onfocus="WdatePicker({ dateFmt:'yyyy-MM-dd HH:mm:ss',minDate: '#F{$dp.$D(\'content_txt_starttime\')}' })" 
                    placeholder="请输入有效结束时间" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_chk_isdefault">是否默认:</label>
            <div class="controls">
                <input name="chk_isdefault" class="ace-switch ace-switch-2" type="checkbox" id="chk_isdefault" runat="server" /><span class="lbl"></span>
            </div>
            <div style="display:none;">
                <textarea id="txtHtml" name="txtHtml" class="span12" style="display:none;"></textarea>
            </div>
        </div>
        
        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <a href="javascript:" id="submit" class="btn btn-purple" onclick="javascript:addstartpic();">保存</a>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("addstartpic");
        }

        var addstartpic = function () {
            if (checkForm()){
                var pms = {
                    "pid": escape($("#hidImgurl").val()),
                    "stime": $("#content_txt_starttime").val(),
                    "etime": $("#content_txt_endTime").val(),
                    "defalut": $("#content_chk_isdefault").get(0).checked,
                    "ts": new Date().getTime()
                };

                $.post(_root + "handlers/StartPicController/StartPicEdit.ashx", pms, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "1") {
                        bootbox.alert(json.msg, function () {
                            if (window.parent != undefined) {
                                window.parent.TObj("tmpStartPicList")._prmsData.ts = new Date().getTime();
                                window.parent.TObj("tmpStartPicList").S();
                                window.parent.asyncbox.close("addstartpic");
                            }
                        });
                    } else {
                        bootbox.alert(json.msg);
                    }
                });
                
            }
        }

        var checkForm = function () {
            var error = "";

            if ($.trim($("#hidImgurl").val()) == "") {
                error += "请上传图片\n\r";
            }
            
            if (error != "")
            {
                bootbox.alert(error);
                return false;
            }

            return true;
        }

        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#txtHtml', {
                cssData: 'body {font-family: "宋体"; font-size: 16px; text-align:justify; }',
                uploadJson: _root + 'js/plugin/kindeditor/upload_json.ashx',
                showRemote: false,
                filterMode: false,
                afterUpload: function (url, data) {
                },
                items: ['source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                        'superscript', 'clearhtml', 'quickformat',
                        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', 'image', 'multiimage', 'flash', 'media',
                        'table', 'hr', 'emoticons', 'baidumap', 'pagebreak', 'anchor', 'link', 'unlink', '/',
                        'selectall', '|', 'fullscreen']
            });


            K('#btnImage').click(function () {
                if ($(this).attr("datatype") == "add") {
                    editor.loadPlugin('image', function () {
                        editor.plugin.imageDialog({
                            imageUrl: K('#imgPic').attr("src"),
                            showRemote: false,
                            afterUpload: function (url, data) {
                            },
                            clickFn: function (url, title, width, height, border, align,pid) {
                                K('#imgPic').attr("src", url).css({ "max-width": "70%", "max-height": "250px", "display": "inline" });
                                $('#hidImgurl').val(url);
                                $("#btnImage").val("删除图片").attr("datatype", "del");
                                editor.hideDialog();
                            }
                        });
                    });
                }
                else {
                    bootbox.confirm("您确认要删除当前图片吗?(删除后将不可恢复)", function (result) {
                        if (result) {
                            var imgurl = $('#hidImgurl').val();
                            $.post(_root + "handlers/ArticleController/DeleteImage.ashx", { "imgurl": imgurl }, function (data) {
                                var json = eval("(" + data + ")");

                                if (json.status != "-1") {
                                    $("#imgPic").attr("src", "").css("display", "none");
                                    $("#hidImgurl").val("");
                                    $("#btnImage").val("选择图片").attr("datatype", "add");
                                    bootbox.alert("删除成功!");
                                }
                                else {
                                    bootbox.alert("数据传输错误!");
                                }
                            });
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>
