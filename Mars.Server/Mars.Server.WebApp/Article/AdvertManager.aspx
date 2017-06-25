<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AdvertManager.aspx.cs" Inherits="Mars.Server.WebApp.Article.AdvertManager" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
    <li class="active">首页轮播维护</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
    <h1>首页轮播维护<small><i class="icon-double-angle-right"></i>首页轮播管理</small></h1>
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
            <label class="control-label" for="txtTime">指定日期</label>
            <div class="controls">
                <input type="text" id="txtTime" class="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'});" placeholder="指定日期" />
            </div>
        </div>

        <%--<div class="control-group" style="float: left;">
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
        </div>--%>

        <div class="btn-group" style="float: right;">
            <button class="btn btn-purple" onclick="javascript:search(false);">查询</button>
           <%-- <button class="btn btn-primary" onclick="javascript:select();">选择</button>--%>
        </div>

        <span class="span12">
            <label id="lblSTitle"></label>
        </span>
        <div style="width: 100%; height: 1px; font-size: 0; overflow: hidden; clear: both;"></div>
    </div>
    <OpenBook:TemplateWrapper ID="tmpAdvertList" runat="server" TemplateSrc="~/Templates/AdvertTemplate.ascx"
        DebugMode="true" PaginationType="Scrolling" HttpMethod="Get" PageSize="20" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">

        var canceladvert = function (pid) {
            bootbox.confirm("您确认要取消该文章首页轮播?", function (result) {
                if (result) {
                    $.post(_root + "handlers/ArticleController/CancelAdvert.ashx", { "pid": pid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("取消首页轮播成功!", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    search(true);
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("取消首页轮播失败!");
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
            var selFirstType = $("#selFirstType").val();
            var selSecondType = $("#selSecondType").val();
            //var txtStartTime = $("#txtStartTime").val();
            //var txtEndTime = $("#txtEndTime").val();
            var txtTime = $("#txtTime").val();
            TObj("tmpAdvertList")._prmsData.title = txtTitle;
            TObj("tmpAdvertList")._prmsData.type1 = selFirstType;
            TObj("tmpAdvertList")._prmsData.type2 = selSecondType;
            //TObj("tmpAdvertList")._prmsData.stime = txtStartTime;
            //TObj("tmpAdvertList")._prmsData.etime = txtEndTime;
            TObj("tmpAdvertList")._prmsData.dtime = txtTime;
            if (reload) {
                TObj("tmpAdvertList")._prmsData.ts = new Date().getTime();
            }

            TObj("tmpAdvertList")._prmsData._pageIndex = 0;
            TObj("tmpAdvertList").loadData();
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

                $("#txtAdvertOrder").attr("disabled", false);
            });
        }

        var select = function () {
            asyncbox.open({
                modal: true,
                id: "selectadvert",
                title: "选择首页轮播文章",
                url: _root + "Article/SelectAdvert.aspx",
                width: 900,
                height: 550,
                callback: function (btnRes, cntWin, reVal) {
                    if (btnRes == "sure") {
                        //searchtopic();
                    }
                }
            });
        }

        $(function () {

            $.post(_root + "handlers/CommonDictController/GetFirstCalendarType.ashx", {}, function (data) {
                var json = eval("(" + data + ")");

                $.each(json, function (index, item) {
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




