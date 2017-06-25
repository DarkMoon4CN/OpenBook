<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ArticleGroupEdit.aspx.cs" Inherits="Mars.Server.WebApp.Article.ArticleGroupEdit" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
    <OpenBook:OBScript runat="server" ID="chosen" Src="~/css/chosen.css" ScriptType="StyleCss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="fuelux_wizard_js" Src="~/js/plugin/fuelux.wizard.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="validate_js" Src="~/js/plugin/jquery.validate.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="chosen_js" Src="~/js/plugin/chosen.jquery.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">专题分组发布管理</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>专题分组发布管理 <small><i class="icon-double-angle-right"></i>发布专题分组</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="row-fluid">
        <!-- PAGE CONTENT BEGINS HERE -->
        <div class="row-fluid">
            <div class="span12">
                <div class="widget-box">
                    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                        <h4 class="lighter">发布专题分组</h4>
                        <div class="widget-toolbar">
                            <label>
                                <%--<small class="green"><b>Validation</b></small>
                                <input id="skip-validation" type="checkbox" class="ace-switch ace-switch-4" /><span class="lbl"></span>--%>
                            </label>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">

                            <div class="row-fluid">
                                <div id="fuelux-wizard" class="row-fluid hidden">
                                    <ul class="wizard-steps" id="ulstep">
                                        <li data-target="#step1" class="active"><span class="step">1</span> <span class="title">设置专题</span></li>
                                        <li data-target="#step2"><span class="step">2</span> <span class="title">选择文章</span></li>
                                        <li data-target="#step3"><span class="step">3</span> <span class="title">设置发布日期</span></li>
                                    </ul>
                                </div>
                                <hr />
                                <div class="step-content row-fluid position-relative">
                                    <div class="step-pane active" id="step1">
                                        <div class="span11">
                                            <h3 class="lighter block green">选择专题分组</h3>
                                        </div>

                                        <form class="form-horizontal" id="formcategory" method="get">

                                            <div class="control-group">
                                                <label class="control-label" for="txtGroupEventName">专题分组名称:</label>
                                                <div class="controls">
                                                    <div class="span12">
                                                        <input type="text" class="span6" id="txtGroupEventName" name="txtGroupEventName" />
                                                        <%--  <select class="span5 chzn-select" id="selGroupEventName" data-placeholder="请选择文章专题" name="selGroupEventName">
                                                            <option value="">请选择</option>
                                                        </select>--%>
                                                    </div>
                                                </div>
                                            </div>

                                        </form>
                                    </div>

                                    <div class="step-pane" id="step2">
                                        <div class="span10">
                                            <h3 class="lighter block green">选择专题文章</h3>
                                        </div>
                                        <%--   <form class="form-horizontal" id="formcontent" method="get">
                                         </form>--%>
                                        <div class="span2">
                                            <button class="btn btn-info" id="btn_selArticle">选择文章</button>
                                        </div>
                                        <div class="form-horizontal span11" id="selectarticle">
                                            <OpenBook:TemplateWrapper ID="tmpArticleList" runat="server" TemplateSrc="~/Templates/ArticleSelectTemplate.ascx"
                                                DebugMode="false" PaginationType="Classic" HttpMethod="Get" AutoLoadData="true" />
                                        </div>

                                    </div>

                                    <div class="step-pane" id="step3">
                                        <h3 class="lighter block green">设置发布日期</h3>
                                        <form class="form-horizontal" id="formstate" method="get">

                                            <div class="control-group">
                                                <label class="control-label" for="txtPublishTime">发布日期:</label>
                                                <div class="controls">
                                                    <div class="span12">
                                                        <input type="text" id="txtPublishTime" name="txtPublishTime" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});" />
                                                    </div>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>

                                <hr />

                                <div class="row-fluid wizard-actions">
                                    <button class="btn btn-prev"><i class="icon-arrow-left"></i>上一步</button>
                                    <button id="btnSave" class="btn btn-success btn-next" data-last="保存 ">下一步 <i class="icon-arrow-right icon-on-right"></i></button>
                                </div>
                            </div>
                        </div>
                        <!--/widget-main-->
                    </div>
                    <!--/widget-body-->
                </div>
            </div>
        </div>
        <!--PAGE PARAS-->
        <input type="hidden" id="hidArticleIds" key="ids" value="" class="searchpart" />
        <input type="hidden" id="hidShowFlag" key="showflag" value="1" class="searchpart" />
        <input type="hidden" id="hidFun" runat="server" />
        <!-- PAGE CONTENT ENDS HERE -->
    </div>
    <!--/row-->
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#fuelux-wizard').ace_wizard().on('change', function (e, info) {
                if (info.step == 1) {
                    if (!$('#formcategory').valid()) {
                        return false;
                    }
                }

                if (info.step == 2 && info.direction == "next") {
                    if ($("div[id='selectarticle'] tr").length > 1 && $("div[id='selectarticle'] tr[converpic]").length == 0) {

                        bootbox.alert("当前专题分组文章中都没有封面图片！请保证至少有一篇文章有封面图！");
                        return false;
                    }
                }

                if (info.step == 3 && info.direction == "next") {
                    //最后一步，并没有执行验证
                    if (!$('#formstate').valid()) {
                        return false;
                    }
                }
            }).on('finished', function (e) {
                if (!$('#formstate').valid()) {
                    return false;
                }

                $("#btnSave").text("保存中...").attr("disabled", true);

                //提交保存
                //构建json对象
                var txtGroupEventName = $("#txtGroupEventName").val();
                var ids = "";
                $("tr[id^='tr']").each(function () {
                    ids += "," + $(this).attr("pid");
                });

                var hascarouselIds = "";
                $("div[id='selectarticle'] tr[converpic]").each(function () {
                    hascarouselIds += "," + $(this).attr("pid");
                });

                var txtPublishTime = $("#txtPublishTime").val();

                var dataparas = { "groupname": txtGroupEventName, "ids": ids, "hascarouselIds": hascarouselIds, "publishtime": txtPublishTime, "ts": new Date().getTime() };

                $.post(_root + "handlers/ArticleController/GroupRelInsert.ashx", dataparas, function (data) {
                    var json = eval("(" + data + ")");

                    if (json.status == "0") {
                        $("#btnSave").text("保存").attr("disabled", false);
                        bootbox.alert("创建失败！");
                        return false;
                    }
                    else if (json.status == "1") {
                        bootbox.dialog("创建成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                window.location.href = "ArticleGroupManager.aspx?fun=" + $("#content_hidFun").val();
                            }
                        }]
                        );
                    }
                    else if (json.status == "2") {
                        $("#btnSave").text("保存").attr("disabled", false);
                        bootbox.alert("当前专题分组系统中已存在");
                        return false;
                    }
                    else {
                        $("#btnSave").text("保存").attr("disabled", false);
                        bootbox.alert("数据传输错误！");
                        return false;
                    }
                });
            });
        });//end

        $(function () {

            $('#formcategory').validate({
                errorElement: 'span',
                errorClass: 'help-inline',
                focusInvalid: false,
                rules: {
                    txtGroupEventName: {
                        required: true,
                        maxlength: 50,
                        remote: {
                            type: "POST",
                            url: _root + "handlers/ArticleController/IsUseableGroupName.ashx",
                            data: {
                                "pid": 0,
                                "groupeventname": function () { return $("#txtGroupEventName").val(); }
                            }
                        }
                    }
                },
                messages: {
                    txtGroupEventName: { required: "必填项", maxlength: "长度最多不能超过50个字符", remote: "系统中已存在该专题分组名称" }
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

            //documentation: http://docs.jquery.com/Plugins/Validation/validate             

            $('#formstate').validate({
                errorElement: 'span',
                errorClass: 'help-inline',
                focusInvalid: false,
                rules: {
                    txtPublishTime: { required: true }
                },
                messages: {
                    txtPublishTime: { required: "必填项" }
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
        });//end     

        $(function () {

            $("#btn_selArticle").click(function () {
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
        });//end

        var setGroupArticle = function (reval) {
            var ids = $("#hidArticleIds").val() + "," + reval;
            $("#hidArticleIds").val(uniqueitem(ids));
            TObj("tmpArticleList").S();
        }

        var uniqueitem = function (item) {
            if (item != "") {
                var temparray = $.unique(item.split(","));
                temparray = $.map(temparray, function (item) { return item != "" ? item : null; })
                return temparray.join(",");
            }
            return "";
        }

        var delitem = function (pid) {
            var ids = $("#hidArticleIds").val();
            var temparray = ids.split(",");
            temparray.splice($.inArray(pid, temparray), 1);
            $("#hidArticleIds").val(temparray.join(","));

            TObj("tmpArticleList").S();
        }
    </script>
</asp:Content>
