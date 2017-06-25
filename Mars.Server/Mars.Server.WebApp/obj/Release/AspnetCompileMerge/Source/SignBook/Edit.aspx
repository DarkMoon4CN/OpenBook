<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common.Master"  CodeBehind="Edit.aspx.cs" Inherits="Mars.Server.WebApp.SignBook.Edit" %>
<%@ MasterType VirtualPath="~/Common.Master" %>
<asp:Content ContentPlaceHolderID="css" ID="concss" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="script" ID="conscript" runat="server">
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="datepicker_js" Src="~/js/plugin/My97DatePicker/WdatePicker.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="OBScript4" Src="~/js/plugin/uploadify3.2.1/jquery.uploadify.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="OBScript5" Src="~/js/plugin/uploadify3.2.1/uploadify.css" ScriptType="StyleCss" />
    <OpenBook:OBScript runat="server" ID="bootstrap" Src="~/css/bootstrap.min.css" ScriptType="StyleCss" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
    <div class="alert alert-block alert-success form-horizontal">
            <div class="control-group" >
                    <label class="control-label" for="lblCustomer">姓名:</label>
                    <div class="controls">
                       <input type="text" name="textCustomer" id="textCustomer"   disabled="disabled" class="searchpart"  /><span class="lbl"></span>
                    </div>
            </div>
            
            <div class="control-group" >
                    <label class="control-label" for="lblMoblie">手机号:</label>
                    <div class="controls">
                        <input type="text" name="textMoblie" id="textMoblie"   disabled="disabled" class="searchpart"  /><span class="lbl"></span>
                    </div>
            </div>

            <div class="control-group" >
                    <label class="control-label" for="lblCompany">单位:</label>
                     <div class="controls">
                             <input type="text" name="textCompany" id="textCompany"  placeholder="单位" class="searchpart" value=""/> 
                    </div>
            </div>

            <div class="control-group" >
                <label class="control-label" for="lblDepartment">部门:</label>
                <div class="controls">
                    <input type="text" name="textDepartment" id="textDepartment"   placeholder="部门" class="searchpart" value=""/>
              </div>
           </div>
              
            <div class="control-group" >
                <label class="control-label" for="lblPosition">职位:</label>
                <div class="controls">
                    <input type="text" name="textPosition" id="textPosition"  placeholder="职位" class="searchpart"  value="" />
                </div>
            </div>

            <div class="control-group" >
                <label class="control-label" for="lblEmail">邮箱:</label>
                <div class="controls">
                    <input type="text" name="textEmail" id="textEmail"   placeholder="邮箱" class="searchpart" value=""/>
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
          <input  type="hidden" id="hsid" value="<%=sid %>"/>
    </div>
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="inlinescripts" runat="server">
    <script type="text/javascript">
        $(function () {
            var hsid = $("#hsid").val();
            if (hsid == null || hsid == "") {
                return false;
            }
            $.post(_root + "handlers/SignBookController/GetInfo.ashx", { "sid": hsid, "ts": new Date().getTime() }, function (data) {
                var json = $.parseJSON(data);
                if (data != "false") {
                    $("#textCustomer").val(json.Customer);
                    $("#textMoblie").val(json.Moblie);
                    $("#textCompany").val(json.Company);
                    $("#textDepartment").val(json.Department);
                    $("#textPosition").val(json.Position);
                    $("#textEmail").val(json.Email);
                }
            });
            $("#submit").click(function () {
                var hsid = $("#hsid").val();
                if (hsid == null || hsid == "") {
                    return false;
                }
                var customer=$("#textCustomer").val();
                var moblie=$("#textMoblie").val();
                var commany=$("#textCompany").val();
                var departement=$("#textDepartment").val();
                var position=$("#textPosition").val();
                var email = $("#textEmail").val();
                $.post(_root + "handlers/SignBookController/Update.ashx", {
                    "sid": hsid,
                    "customer": customer,
                    "commany":commany,
                    "moblie": moblie,
                    "departement": departement,
                    "position": position,
                    "email": email,
                    "ts": new Date().getTime()
                }, function (data) {
                    var res = data.split('_');
                    if (res[0] == "true")
                    {
                        bootbox.alert(res[1], function () {
                            if (window.parent != undefined) {
                                window.parent.TObj("tmpSignBook")._prmsData.ts = new Date().getTime();
                                window.parent.TObj("tmpSignBook").loadData();
                                window.parent.asyncbox.close("signBook");
                            }
                        });
                    }
                });
            });
            $("#goBack").click(function () {
                window.parent.asyncbox.close("signBook");
            });
        });



    </script>
</asp:Content>