<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ArticleManager.aspx.cs" Inherits="Mars.Server.WebApp.Article.ArticleManager" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">平台文章维护</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>平台文章维护 <small><i class="icon-double-angle-right"></i>平台文章管理</small></h1>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtTitle">文章名称</label>
            <div class="controls">
                <input type="text" id="txtTitle" placeholder="请输入文章名称" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selRecommend">是否推荐</label>
            <div class="controls">
                <select class="span12" id="selRecommend">
                    <option value="-1">--请选择--</option>
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selAdvert">是否首页轮播</label>
            <div class="controls">
                <select class="span12" id="selAdvert">
                    <option value="-1">--请选择--</option>
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtStartTime">开始时间</label>
            <div class="controls">
                <input type="text" id="txtStartTime" class="Wdate" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({ onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}'})" placeholder="请输入有效开始时间" />
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="txtEndTime">结束时间</label>
            <div class="controls">
                <input type="text" id="txtEndTime" class="Wdate" onfocus="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' })" placeholder="请输入有效结束时间" />
            </div>
        </div>

        
        <div class="control-group" style="float: left;">
            <label class="control-label" for="selRecommend">是否子页面</label>
            <div class="controls">
                <select class="span12" id="selSubArticle">
                    <option value="-1">--请选择--</option>
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selFirstType">一级分类</label>
            <div class="controls">
                <select class="span12" id="selFirstType">
                    <option value="-1">--请选择--</option>
                </select>
            </div>
        </div>

        <div class="control-group" style="float: left;">
            <label class="control-label" for="selSecondType">二级分类</label>
            <div class="controls">
                <select class="span12" id="selSecondType">
                    <option value="-1">--请选择--</option>
                </select>
            </div>
        </div>

          <div class="control-group" style="float: left;">
            <label class="control-label" for="selSingleGroup">是否单篇成组</label>
            <div class="controls">
                <select class="span12" id="selSingleGroup">
                    <option value="-1">--请选择--</option>
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
        </div>

        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
            <button class="btn btn-primary" onclick="javascript:add();">新增</button>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpArticleList" runat="server" TemplateSrc="~/Templates/ArticlesTemplate.ascx"
        DebugMode="true" PaginationType="Classic" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var add = function () {
            window.location.href = "ArticleEdit.aspx?fun=<%= fun%>";
        }

        var deleteobj = function (pid) {
            bootbox.confirm("您确认要删除项吗?(删除后将不可恢复)", function (result) {
                if (result) {
                    $.post(_root + "handlers/ArticleController/DeleteEventItem.ashx", { "pid": pid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("删除成功!", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    search(true);
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("删除失败!");
                            return false;
                        }
                        else {
                            bootbox.alert("数据传输错误");
                            return false;
                        }
                    });
                }
            });
        }     

        var refresh = function (reload) {
            search(reload);
        }

        var search = function (reload) {
            var txtTitle = $("#txtTitle").val();
            var selRecommend = $("#selRecommend").val();
            var selAdvert = $("#selAdvert").val();
            var selFirstType = $("#selFirstType").val();
            var selSecondType = $("#selSecondType").val();
            var txtStartTime = $("#txtStartTime").val();
            var txtEndTime = $("#txtEndTime").val();
            var selSubArticle = $("#selSubArticle").val();
            var selSingleGroup = $("#selSingleGroup").val();

            TObj("tmpArticleList")._prmsData.title = txtTitle;
            TObj("tmpArticleList")._prmsData.recommend = selRecommend;
            TObj("tmpArticleList")._prmsData.advert = selAdvert;
            TObj("tmpArticleList")._prmsData.type1 = selFirstType;
            TObj("tmpArticleList")._prmsData.type2 = selSecondType;
            TObj("tmpArticleList")._prmsData.stime = txtStartTime;
            TObj("tmpArticleList")._prmsData.etime = txtEndTime;
            TObj("tmpArticleList")._prmsData.subarticle = selSubArticle;
            TObj("tmpArticleList")._prmsData.singlegroup = selSingleGroup;

            if (reload) {
                TObj("tmpArticleList")._prmsData.ts = new Date().getTime();
            }

            TObj("tmpArticleList")._prmsData._pageIndex = 0;
            TObj("tmpArticleList").loadData();
        }

        var changedrecommend = function (pid, obj) {
            var status = 0;
            var originStatus = !obj.checked;
            bootbox.confirm("您确认进行此项操作吗?", function (result) {
                if (result) {
                    if (obj.checked) {
                        status = 1;
                    }

                    var prms = { "pid": pid, "status": status, "ts": new Date().getTime() };
                    $.post(_root + "handlers/ArticleController/ChangeRecommendState.ashx", prms, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("操作成功！", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    TObj("tmpArticleList").S();
                                }
                            }
                            ]);
                        }
                        else if (json.status == "0") {
                            obj.checked = originStatus;
                            bootbox.alert("操作失败！");
                            return false;
                        }
                        else {
                            obj.checked = originStatus;
                            bootbox.alert("数据传输错误！");
                            return false;
                        }
                    });
                }
                else {
                    obj.checked = originStatus;
                }
            });
        }

        var changeactiveapply = function (pid, obj)
        {
            var isApply = false;
            var originStatus = !obj.checked;       

            bootbox.confirm("您确认进行此项操作吗?", function (result) {
                if (result) {
                    if (obj.checked)
                    {
                        isApply = true;
                    }

                    var prms = {"pid": pid, "activeApply":isApply, "ts":new Date().getTime() };
                    $.post(_root + "handlers/ArticleController/SetActiveApply.ashx", prms, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.state == "1") {
                            bootbox.dialog("操作成功！", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    TObj("tmpArticleList").S();
                                }
                            }]);
                        }
                        else if (json.state == "0") {
                            obj.checked = originStatus;
                            bootbox.alert("操作失败！");
                            return false;
                        }
                        else {
                            obj.checked = originStatus;
                            bootbox.alert("数据传输错误！");
                            return false;
                        }
                    });
                }
                else
                {
                    obj.checked = originStatus;
                }
            });
         }

        var setadvert = function (pid) {
            $.post(_root + "handlers/ArticleController/IsCanSetAdvert.ashx", { "pid": pid }, function (data) {
                var json = eval("(" + data + ")");

                if (json.status == "1") {
                    asyncbox.open({
                        modal: true,
                        id: "changeadvert",
                        title: "设置首页轮播",
                        url: _root + "Article/SetAdvertState.aspx?pid=" + pid,
                        width: 700,
                        height: 300,
                        callback: function (btnRes, cntWin, reVal) {
                            if (btnRes == "sure") {
                                //searchtopic();
                            }
                        }
                    });
                }
                else if (json.status == "0") {
                    bootbox.alert("首页轮播数量不能超过" + json.num + "个");
                    return false;
                }
                else {
                    bootbox.alert("系统判断出现异常，不可设置轮播！");
                    return false;
                }

               // $("#txtAdvertOrder").attr("disabled", false);
            });
        }

        var setcarousel = function (pid) {
            $.post(_root + "handlers/ArticleController/IsCanSetCarousel.ashx", { "pid": pid }, function (data) {
                var json = eval("(" + data + ")");

                if (json.status == "1") {
                    asyncbox.open({
                        modal: true,
                        id: "changecarousel",
                        title: "设置发现轮播",
                        url: _root + "Article/SetCarouselState.aspx?pid=" + pid,
                        width: 700,
                        height: 300,
                        callback: function (btnRes, cntWin, reVal) {
                            if (btnRes == "sure") {
                                //searchtopic();
                            }
                        }
                    });
                }
                else if (json.status == "0") {
                    bootbox.alert("发现轮播数量不能超过" + json.num + "个");
                    return false;
                }
                else {
                    bootbox.alert("系统判断出现异常，不可设置轮播！");
                    return false;
                }

                //$("#txtAdvertOrder").attr("disabled", false);
            });
        }        

        $(function () {

            $.post(_root + "handlers/CommonDictController/GetFirstCalendarType.ashx", {}, function (data) {
                var json = eval("(" + data + ")");

                $.each(json, function (index, item) {
                    //alert(index + "  " + item.CalendarTypeID)
                    $("#selFirstType").append("<option value='" + item.CalendarTypeID + "'>" + item.CalendarTypeName + "</option>");
                });
            });

            $("#selFirstType").change(function () {
                var selvalue = $(this).val();

                if (selvalue == -1) {
                    $("#selSecondType").val(-1);
                    $("#selSecondType option:not(:eq(0))").remove();
                }
                else {
                    $("#selSecondType").val(-1);
                    $("#selSecondType option:not(:eq(0))").remove();

                    $.post(_root + "handlers/CommonDictController/GetSecondCalendarType.ashx", { pid: selvalue }, function (data) {
                        var json = eval("(" + data + ")");

                        $.each(json, function (index, item) {
                            $("#selSecondType").append("<option value='" + item.CalendarTypeID + "'>" + item.CalendarTypeName + "</option>");
                        });
                    });
                }
            });
        });

    </script>
</asp:Content>


