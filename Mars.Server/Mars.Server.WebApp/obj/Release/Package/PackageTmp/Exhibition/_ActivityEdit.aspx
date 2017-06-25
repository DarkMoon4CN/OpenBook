<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_ActivityEdit.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ActivityEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="wdatepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group childnotneed">
            <label class="control-label" for="content_sel_exhibition">所属展场:</label>
            <div class="controls">
                <select class="span10" id="sel_exhibition" runat="server" onchange="javascript:getExhibitorSelect(this);">
                    <option value="">==请选择==</option>
                </select>
            </div>
        </div>

        <div class="control-group childnotneed">
            <label class="control-label" for="content_sel_exhibitor">所属展商:</label>
            <div class="controls">
                <select class="span10" id="sel_exhibitor" runat="server">
                    <option value="">==请选择==</option>
                </select>
            </div>
        </div>
        
        <div class="control-group">
            <label class="control-label" for="content_txt_activitytitle">活动名称:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_activitytitle" id="txt_activitytitle" placeholder="活动名称" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">活动时间:</label>
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
            <label class="control-label" for="content_txt_activitylocation">展位:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_activitylocation" id="txt_activitylocation" placeholder="展位" runat="server" />
            </div>
        </div>

        <div class="control-group childnotneed">
            <label class="control-label" for="content_txt_activityhostunit">主办方:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_activityhostunit" id="txt_activityhostunit" placeholder="主办方" runat="server" />
            </div>
        </div>

         <div class="control-group childnotneed">
            <label class="control-label" for="content_txt_activityabstract">简介:</label>
            <div class="controls">
                <textarea class="span10" name="txt_activityabstract" id="txt_activityabstract" placeholder="简介" rows="4" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_activityguest">嘉宾:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_activityguest" id="txt_activityguest" placeholder="嘉宾" runat="server" />
            </div>
        </div>

        <div class="control-group childnotneed">
            <label class="control-label" for="content_txt_activityorder">活动排序:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_activityorder" id="txt_activityorder" placeholder="活动排序" runat="server" value="1000" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_chk_statetype">是否启用:</label>
            <div class="controls">
                <input name="chk_statetype" class="ace-switch ace-switch-2" type="checkbox" id="chk_statetype" runat="server" /><span class="lbl"></span>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <a href="javascript:" id="submit" class="btn btn-purple" onclick="javascript:addActivity();">保存</a>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_aid" runat="server" />
    <input type="hidden" id="hid_pid" runat="server" value="0"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("activity");
        }

        $(function () {
            if ($("#content_hid_pid").val() > 0)
            {
                $(".childnotneed").hide();
            }
        });

        var getExhibitorSelect = function (obj) {
            $("#content_sel_exhibitor").empty();
            $("#content_sel_exhibitor").append("<option value=''>==请选择==</option>");

            $.get(_root + "handlers/ExhibitionController/ExhibitorList.ashx", { "eid": $(obj).val(), "ts": new Date().getTime() }, function (data) {
                var json = eval("(" + data + ")");
                if (json.state == "1") {
                    $.each(json.list, function (index, item) {
                        $("#content_sel_exhibitor").append("<option value='" + item.ExhibitorID + "'>" + item.ExhibitorName + "</option>");
                    });
                } else {
                    bootbox.alert(json.msg);
                }
            });
        }

        var addActivity = function () {
            if (checkForm()){
                var pms = {
                    "emid": $("#content_sel_exhibition").val(),
                    "id": $("#content_hid_aid").val(),
                    "pid": $("#content_hid_pid").val(),
                    "aname": escape($("#content_txt_activitytitle").val()),
                    "eid": $("#content_sel_exhibitor").val(),
                    "astime": $("#content_txt_starttime").val(),
                    "aetime": $("#content_txt_endTime").val(),
                    "aloc": escape($("#content_txt_activitylocation").val()),
                    "ahost": escape($("#content_txt_activityhostunit").val()),
                    "aabstract": escape($("#content_txt_activityabstract").val()),
                    "aguest": escape($("#content_txt_activityguest").val()),
                    "aorder": escape($("#content_txt_activityorder").val()),
                    "estate": $("#content_chk_statetype").get(0).checked,
                    "ts": new Date().getTime()
                };

                $.post(_root + "handlers/ExhibitionController/ActivityEdit.ashx", pms, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.state == "1") {
                        bootbox.alert(json.msg, function () {
                            if ($("#content_hid_pid").val() > 0) {
                                if (window.parent != undefined) {
                                    window.parent.TObj("tmpActivityChildrenList")._prmsData.ts = new Date().getTime();
                                    window.parent.TObj("tmpActivityChildrenList").S();
                                    window.parent.asyncbox.close("activitychild");
                                }
                            } else {
                                if (window.parent != undefined) {
                                    window.parent.TObj("tmpActivityList")._prmsData.ts = new Date().getTime();
                                    window.parent.TObj("tmpActivityList").S();
                                    window.parent.asyncbox.close("activity");
                                }
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
            if ($.trim($("#content_sel_exhibition").val()) == "" && $("#content_hid_pid").val() == 0)
            {
                error += "请选择所属展场\n\r";
            }
            if ($.trim($("#content_txt_activitytitle").val()) == "") {
                error += "请填写活动名称\n\r";
            }
            if ($.trim($("#content_txt_starttime").val()) == "") {
                error += "请填写活动开始时间\n\r";
            }
            if ($.trim($("#content_txt_endTime").val()) == "") {
                error += "请填写活动结束时间\n\r";
            }
            if ($.trim($("#content_txt_activitylocation").val()) == "") {
                error += "请填写活动位置\n\r";
            }
            if ($.trim($("#content_txt_activityhostunit").val()) == "" && $("#content_hid_pid").val()==0) {
                error += "请填写活动主办方\n\r";
            }
            
            if (error != "")
            {
                bootbox.alert(error);
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
