<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="_ExhibitionEdit.aspx.cs" Inherits="Mars.Server.WebApp.Exhibition._ExhibitionEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="css" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="wdatepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <form id="frmExhibition" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitionname">展场名称:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_exhibitionname" id="txt_exhibitionname" placeholder="展场名称" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_imageurl">图片:</label>
            <div class="controls">
                <asp:FileUpload runat="server" ID="txt_imageurl" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitionlocation">位置:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_exhibitionlocation" id="txt_exhibitionlocation" placeholder="位置" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label">时间:</label>
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
            <label class="control-label" for="content_txt_exhibitionaddress">地点:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_exhibitionaddress" id="txt_exhibitionaddress" placeholder="地点" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitiontraffic">交通:</label>
            <div class="controls">
                <textarea class="span10" name="txt_exhibitiontraffic" id="txt_exhibitiontraffic" placeholder="交通" rows="3" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitionabstract">简介:</label>
            <div class="controls">
                <textarea class="span10" name="txt_exhibitionabstract" id="txt_exhibitionabstract" placeholder="简介" rows="4" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitionabout">关于展会信息:</label>
            <div class="controls">
                <textarea class="span10" name="txt_exhibitionabout" id="txt_exhibitionabout" placeholder="关于展会信息" rows="5" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_exhibitionbooklistdesc">获取书目功能介绍:</label>
            <div class="controls">
               <textarea class="span10" name="txt_exhibitionbooklistdesc" id="txt_exhibitionbooklistdesc" placeholder="获取书目功能介绍" rows="3" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_advertisementtitle">一句广告语:</label>
            <div class="controls">
                <textarea class="span10" name="txt_advertisementtitle" id="txt_advertisementtitle" placeholder="一句广告语" rows="3" runat="server"></textarea>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_chk_statetype">是否启用:</label>
            <div class="controls">
                <input name="chk_statetype" class="ace-switch ace-switch-2" type="checkbox" id="chk_statetype" runat="server" /><span class="lbl"></span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_chk_ispublish">是否对外发布:</label>
            <div class="controls">
                <input name="chk_ispublish" class="ace-switch ace-switch-2" type="checkbox" id="chk_ispublish" runat="server" /><span class="lbl"></span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_chk_isdownloadbooklist">是否开启下载书目:</label>
            <div class="controls">
                <input name="chk_isdownloadbooklist" class="ace-switch ace-switch-2" type="checkbox" id="chk_isdownloadbooklist" runat="server" /><span class="lbl"></span>
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="content_txt_booklistdownloadurl">下载书目URL:</label>
            <div class="controls">
                <input class="span10" type="text" name="txt_booklistdownloadurl" id="txt_booklistdownloadurl" placeholder="下载书目URL" runat="server" />
            </div>
        </div>

        <div class="control-group">
            <label class="control-label" for="submit"></label>
            <div class="controls">
                <asp:Button CssClass="btn btn-purple" OnClientClick="return checkForm();" Text="保存" runat="server" OnClick="btn_submit_Click" ID="btn_submit"/>
                <a href="javascript:" onclick="javascript:closeMyself();" id="goBack" class="btn btn-purple">返回</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_eid" runat="server" />
        <input type="hidden" id="hid_imgurl" runat="server" />
    </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        var closeMyself = function () {
            if (window.parent != undefined)
                window.parent.asyncbox.close("exhibition");
        }

        var checkForm = function () {
            var error = "";
            if ($.trim($("#content_txt_exhibitionname").val()) == "") {
                error += "请填写展场名称\n\r";
            }
            if ($.trim($("#content_txt_imageurl").val()) == "" && $.trim($("#content_hid_imgurl").val()) == "") {
                error += "请选择要上传的封面\n\r";
            }
            if ($.trim($("#content_txt_exhibitionlocation").val()) == "") {
                error += "请填写展场位置\n\r";
            }
            if ($.trim($("#content_txt_starttime").val()) == "") {
                error += "请填写开始时间\n\r";
            }
            if ($.trim($("#content_txt_endTime").val()) == "") {
                error += "请填写结束时间\n\r";
            }

            if ($.trim($("#content_txt_exhibitionaddress").val()) == "") {
                error += "请填写展场地点\n\r";
            }
            if ($.trim($("#content_txt_exhibitiontraffic").val()) == "") {
                error += "请填写展场交通\n\r";
            }
            if ($.trim($("#content_txt_exhibitionabstract").val()) == "") {
                error += "请填写展场简介\n\r";
            }
            if ($.trim($("#content_txt_exhibitionabout").val()) == "") {
                error += "请填写关于展会信息\n\r";
            }
            if ($.trim($("#content_txt_exhibitionbooklistdesc").val()) == "") {
                error += "请填写获取书目功能介绍\n\r";
            }
            if ($.trim($("#content_txt_advertisementtitle").val()) == "") {
                error += "请填写一句广告语\n\r";
            }

            if ($.trim($("#content_txt_booklistdownloadurl").val()) == "" && $("#content_chk_isdownloadbooklist").get(0).checked)
            {
                error += "请填写下载书目链接\n\r";
            }
            if (error != "") {
                bootbox.alert(error);
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
