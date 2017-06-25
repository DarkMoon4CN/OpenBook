<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FestivalEdit.aspx.cs" Inherits="Mars.Server.WebApp.Festival.FestivalEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
      <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
     <script src="../js/plugin/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
     <script src="../js/plugin/jquery.validate.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
     <li class="active">分类菜单维护</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageheader" runat="server">
       <h1>日历菜单维护 <small><i class="icon-double-angle-right"></i>编辑日历菜单</small></h1>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
     <div class="row-fluid">
        <div class="span12">
            <div class="widget-box">
                <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                    <h4 class="lighter">编辑日历菜单</h4>
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
                                    <form runat="server" class="form-horizontal" id="validationf">

                                        <div class="control-group">
                                            <label class="control-label" for="content_FestivalName">节日名称</label> 
                                            <div class="controls">
                                               <input type="text" placeholder="请输入节日名称"  runat="server" id="FestivalName"/><font color="red">*</font>
                                            </div>
                                        </div>
                                         <div class="control-group">
                                            <label class="control-label" for="content_FestivalShortName">节日简称</label> 
                                            <div class="controls">
                                               <input type="text" placeholder="请输入节日简称"  runat="server" id="FestivalShortName"/>
                                            </div>
                                        </div> 
                                        <div class="control-group">
                                            <label class="control-label" for="txtStartTime">开始时间</label>
                                            <div class="controls">
                                                <input type="text" id="txtStartTime" name="txtStartTime" value="<%=starttime%>"  readonly="readonly" class="Wdate" onfocus="var txtEndTime=$dp.$('txtEndTime');WdatePicker({ onpicked:function(){txtEndTime.focus();},maxDate:'#F{$dp.$D(\'txtEndTime\')}',dateFmt:'yyyy-MM-dd'})" placeholder="请输入有效开始时间" /><font color="red">*</font>
                                            </div>
                                        </div> 
                                        <div class="control-group">
                                            <label class="control-label" for="txtEndTime">结束时间</label>
                                            <div class="controls">
                                                <input type="text" id="txtEndTime" name="txtEndTime" class="Wdate" value="<%=Endtime%>"  readonly="readonly" onfocus="WdatePicker({ dateFmt:'yyyy-MM-dd',minDate: '#F{$dp.$D(\'txtStartTime\')}' })" placeholder="请输入有效结束时间" /><font color="red">*</font>
                                            </div>
                                        </div>  
                                        <div class="control-group">
                                            <label class="control-label" for="content_FestivalType">节日类型</label>
                                            <div class="controls"> 
                                                <select id="FestivalType" runat="server" >
                                                    <option value="">--请选择--</option>
                                                    <option value="1">班</option>
                                                    <option value="2">休</option>
                                                    <option value="3">节日</option>
                                                </select><font color="red">*</font>
                                            </div>
                                        </div>   
                                         <div class="control-group">
                                            <label class="control-label" for="content_FestivalWeight">节日权重</label> 
                                            <div class="controls">
                                               <input type="text" placeholder="请输入节日权重"  runat="server" id="FestivalWeight" value="1000"/><font color="red">*</font>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                             <input type="hidden"  id="txtid" runat="server"/>  
                            <hr />

                            <div class="row-fluid wizard-actions">
                                <button class="btn btn-success" onclick="javascript:savefestival();" id="saveall">保存</button>
                                <button class="btn btn-success" onclick="javascript:backlist();">返回</button>
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
        function backlist() {
            window.history.back();
        }
        $(function () {
            var validate = $("#validationf").validate({
                debug: true, //调试模式取消submit的默认提交功能   
                errorEvent: 'span',
                errorClass: 'help-inline',   
                rules: {
                    ctl00$content$FestivalName: {
                        required: true, rangelength: [0, 50]
                    },
                    txtStartTime: {
                        required: true, date: true
                    },
                    txtEndTime: {
                        required: true, date: true
                    },
                    ctl00$content$FestivalType: {
                        required: true
                    },
                    ctl00$content$FestivalWeight: {
                        required: true, digits: true
                    } 
                },
                messages: {
                    ctl00$content$FestivalName: {
                        required: "必填", rangelength: "输入长度小于50的字符串"
                    },
                    txtStartTime: { required: "必填", date: "请填写合法的日期" },
                    txtEndTime: { required: "必填", date: "请填写合法的日期" },
                    ctl00$content$FestivalType: { required: "必填" },
                    ctl00$content$FestivalWeight: { required: "必填", digits: "请填写合法的数字" }
                },
                highlight: function (e) {
                    $(e).closest('.control-group').removeClass('info').addClass('error');
                },
                success: function (e) {
                    $(e).closest('.control-group').removeClass('error').addClass('info');
                    $(e).remove();
                },
                submitHandler: function (form) {
                } 
            });
        });
        function savefestival() {
            if ($("#validationf").valid()) {
                var name = $("#content_FestivalName").val();
                var shortname = $("#content_FestivalShortName").val();
                var txtStartTime = $("#txtStartTime").val();
                var txtEndTime = $("#txtEndTime").val();
                var type = $("#content_FestivalType").val();
                var Weight = $("#content_FestivalWeight").val();
                var id = $("#content_txtid").val();      
                var dataparas = { "name": name, "shortname": shortname, "txtStartTime": txtStartTime, "txtEndTime": txtEndTime, "type": type, "Weight": Weight, "id": id };
                $("#saveall").attr("disabled", true);
                <%if(string.IsNullOrEmpty(id)){%>
                $.post(_root + "handlers/FestivalController/InsertFestival.ashx", dataparas, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.status == "1")
                    {
                        bootbox.dialog("添加成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                window.location.href = "FestivalManager.aspx?fun=<%=fun%>"
                            }
                        }]);
                    }
                    else if (json.status == "2")
                    {
                        bootbox.alert("已经存在该节日！");
                    }
                    else
                    {
                        bootbox.alert("数据传输错误！");
                    }

                });
                <%} else {%>
                $.post(_root + "handlers/FestivalController/UpdateFestival.ashx", dataparas, function (data) {
                    var json = eval("(" + data + ")");
                    if (json.status == "1") {
                        bootbox.dialog("修改成功!", [{
                            "label": "OK",
                            "class": "btn-small btn-primary",
                            callback: function () {
                                window.location.href = "FestivalManager.aspx?fun=<%=fun%>"
                            }
                        }]);
                    }
                    else if (json.status == "2") {
                        bootbox.alert("已经存在该节日！");
                    }
                    else {
                        bootbox.alert("数据传输错误！");
                    }

                });
                <%}%> 
                $("#saveall").attr("disabled", false);
            }
        }
    </script>
</asp:Content>
