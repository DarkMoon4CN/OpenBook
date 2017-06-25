<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EventItemGroupEdit.aspx.cs" Inherits="Mars.Server.WebApp.Article.EventItemGroupEdit" %>


<asp:content id="Content1" contentplaceholderid="css" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="script" runat="server">
    <OpenBook:OBScript runat="server" ID="bootbox_js" Src="~/js/plugin/bootbox.min.js" ScriptType="Javascript" />
    <OpenBook:OBScript runat="server" ID="asyncbox_js" Src="~/js/plugin/asyncbox/asyncbox.v1.5.beta.min.js" ScriptType="Javascript" />
</asp:content>
<asp:content id="Content3" contentplaceholderid="navigation" runat="server">
    <li class="active">编辑专题分组文章</li>
</asp:content>
<asp:content id="Content4" contentplaceholderid="pageheader" runat="server">
    <h1>专题分组管理 <small><i class="icon-double-angle-right"></i>编辑专题分组文章</small></h1>
</asp:content>
<asp:content id="Content5" contentplaceholderid="content" runat="server">
    <div class="row-fluid">
        <!-- PAGE CONTENT BEGINS HERE -->
        <div class="row-fluid">
            <div class="span12">
                <div class="widget-box">
                    <div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">
                        <h4 class="lighter">编辑专题分组文章</h4>
                        <div class="widget-toolbar">
                            <button class="btn btn-small btn-primary" id="btn_selArticle">选择文章</button>
                        </div>
                </div>
                <div class="widget-body">

                    <div class="form-horizontal" id="selectbooks">
                        <OpenBook:TemplateWrapper ID="tmpArticleGroupRel" runat="server" TemplateSrc="~/Templates/ArticleGroupRelTemplate.ascx"
                            DebugMode="false" PaginationType="Scrolling" HttpMethod="Get" PageSize="50" />
                    </div>
                    <!--/widget-main-->
                </div>                   
                <!--/widget-body-->
                </div>
                 <div class="row-fluid wizard-actions">
                     <button class="btn btn-success" onclick="javascript:goback();">返回 </button>
                </div>
            </div>
        </div>
        <!--PAGE PARAS-->
        <!-- PAGE CONTENT ENDS HERE -->
    </div>
    <!--/row-->
    <input type="hidden" value="" id="hidGroupID" key="groupid" runat="server" class="searchpart"/>
    <input type="hidden" value="" id="hidFun" runat="server" />
    <input type="hidden" id="hidArticleIds" value="" key="ids" class="searchpart" />   
    
</asp:content>
<asp:content id="Content6" contentplaceholderid="inlinescripts" runat="server">
    <script type ="text/javascript">
        $(function () {
            $("#btn_selArticle").click(function () {
                asyncbox.open({
                    modal: true,
                    id: "articleselect",
                    title: "选择专题文章",
                    url: _root + "Article/SelectGroupArticle.aspx",
                    width: 950,
                    height: 550,
                    buttons: [{
                        value: '确定',
                        result: 'sure'
                    }],
                    callback: function (btnRes, cntWin, reVal) {
                        if (btnRes == "sure") {                          
                            if (typeof (reVal) != "undefined" && reVal != "")
                            {
                                setGroupArticle(reVal);
                            }
                        }
                    }
                });
            });
        });

        var setGroupArticle = function (reval) {          

            bootbox.confirm("您确定要添加所选文章吗？", function (result) {
                if (result) {
                    var groupid = $("#content_hidGroupID").val();                  
                    $.post(_root + "handlers/ArticleController/GroupRelUpdate.ashx", { "groupid":groupid , "ids": reval, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("添加成功", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    TObj("tmpArticleGroupRel").S();
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("添加失败");
                            return false;
                        }
                        else if (json.status == "2")
                        {
                            bootbox.alert("当前专题分组文章中都没有封面图片！请保证至少有一篇文章有封面图！");
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

        var delitem = function (itemid) {
            bootbox.confirm("您确定要从专题分组移除该文章吗？", function (result) {
                if (result) {
                    var groupid = $("#content_hidGroupID").val();                  
                    $.post(_root + "handlers/ArticleController/GroupRelDelete.ashx", { "groupid": groupid, "itemid": itemid, "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1") {
                            bootbox.dialog("移除成功", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    TObj("tmpArticleGroupRel").S();
                                }
                            }]);
                        }
                        else if (json.status == "0") {
                            bootbox.alert("移除失败");
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

        var setorder = function(groupid,itemid)
        {
            asyncbox.open({
                modal: true,
                id: "setgroupatricleorder",
                title: "设置专题文章序号",
                url: _root + "Article/SetGroupArticleOrder.aspx?groupid=" + groupid + "&itemid=" + itemid,
                width: 950,
                height: 400,
                buttons: [{
                    value: '确定',
                    result: 'sure'
                }],
                callback: function (btnRes, cntWin, reVal) {
                    if (btnRes == "sure") {
                        
                    }
                }
            });
        }

        var goback = function () {
            window.location.href = "ArticleGroupManager.aspx?fun=" + $("#content_hidFun").val();
        }

        var refresh = function (reload) {
            TObj("tmpArticleGroupRel").S();
        }
    </script>
</asp:content>
