<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Header.ascx.cs" Inherits="Mars.Server.WebApp.UCControl.UC_Header" %>
<div class="navbar navbar-inverse">
    <div class="navbar-inner">
        <div class="container-fluid">
            <a class="brand" href="/Default.aspx"><small><i class="icon-leaf"></i>信息发布平台--<%=title %></small> </a>
            <ul class="nav ace-nav pull-right">
                <li class="grey" style="display: none;">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                        <i class="icon-tasks"></i>
                        <span class="badge">4</span>
                    </a>
                    <ul class="pull-right dropdown-navbar dropdown-menu dropdown-caret dropdown-closer">
                        <li class="nav-header">
                            <i class="icon-ok"></i>4 Tasks to complete
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left">Software Update</span>
                                    <span class="pull-right">65%</span>
                                </div>
                                <div class="progress progress-mini">
                                    <div class="bar" style="width: 65%"></div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left">Hardware Upgrade</span>
                                    <span class="pull-right">35%</span>
                                </div>
                                <div class="progress progress-mini progress-danger">
                                    <div class="bar" style="width: 35%"></div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left">Unit Testing</span>
                                    <span class="pull-right">15%</span>
                                </div>
                                <div class="progress progress-mini progress-warning">
                                    <div class="bar" style="width: 15%"></div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left">Bug Fixes</span>
                                    <span class="pull-right">90%</span>
                                </div>
                                <div class="progress progress-mini progress-success progress-striped active">
                                    <div class="bar" style="width: 90%"></div>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">See tasks with details
									<i class="icon-arrow-right"></i>
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="purple" style="display: none;">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                        <i class="icon-bell-alt icon-animated-bell icon-only"></i>
                        <span class="badge badge-important">8</span>
                    </a>
                    <ul class="pull-right dropdown-navbar navbar-pink dropdown-menu dropdown-caret dropdown-closer">
                        <li class="nav-header">
                            <i class="icon-warning-sign"></i>8 Notifications
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left"><i class="icon-comment btn btn-mini btn-pink"></i>New comments</span>
                                    <span class="pull-right badge badge-info">+12</span>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <i class="icon-user btn btn-mini btn-primary"></i>Bob just signed up as an editor ...
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left"><i class="icon-shopping-cart btn btn-mini btn-success"></i>New orders</span>
                                    <span class="pull-right badge badge-success">+8</span>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <div class="clearfix">
                                    <span class="pull-left"><i class="icon-twitter btn btn-mini btn-info"></i>Followers</span>
                                    <span class="pull-right badge badge-info">+4</span>
                                </div>
                            </a>
                        </li>

                        <li>
                            <a href="#">See all notifications
									<i class="icon-arrow-right"></i>
                            </a>
                        </li>
                    </ul>
                </li>
                <li class="green" style="display: none;">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                        <i class="icon-envelope-alt icon-animated-vertical icon-only"></i>
                        <span class="badge badge-success">5</span>
                    </a>
                    <ul class="pull-right dropdown-navbar dropdown-menu dropdown-caret dropdown-closer">
                        <li class="nav-header">
                            <i class="icon-envelope"></i>5 Messages
                        </li>

                        <li>
                            <a href="#">
                                <img alt="Alex's Avatar" class="msg-photo" src="avatars/avatar.png" />
                                <span class="msg-body">
                                    <span class="msg-title">
                                        <span class="blue">Alex:</span>
                                        Ciao sociis natoque penatibus et auctor ...
                                    </span>
                                    <span class="msg-time">
                                        <i class="icon-time"></i><span>a moment ago</span>
                                    </span>
                                </span>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <img alt="Susan's Avatar" class="msg-photo" src="avatars/avatar3.png" />
                                <span class="msg-body">
                                    <span class="msg-title">
                                        <span class="blue">Susan:</span>
                                        Vestibulum id ligula porta felis euismod ...
                                    </span>
                                    <span class="msg-time">
                                        <i class="icon-time"></i><span>20 minutes ago</span>
                                    </span>
                                </span>
                            </a>
                        </li>

                        <li>
                            <a href="#">
                                <img alt="Bob's Avatar" class="msg-photo" src="avatars/avatar4.png" />
                                <span class="msg-body">
                                    <span class="msg-title">
                                        <span class="blue">Bob:</span>
                                        Nullam quis risus eget urna mollis ornare ...
                                    </span>
                                    <span class="msg-time">
                                        <i class="icon-time"></i><span>3:15 pm</span>
                                    </span>
                                </span>
                            </a>
                        </li>

                        <li>
                            <a href="#">See all messages
									<i class="icon-arrow-right"></i>
                            </a>
                        </li>

                    </ul>
                </li>
                <li class="light-blue user-profile">
                    <a class="user-menu dropdown-toggle" href="#" data-toggle="dropdown">
                        <img alt="Jason's Photo" src="<%=VirtualPathUtility.ToAbsolute("~/images/avatars/user.jpg") %>" class="nav-user-photo" />
                        <span id="user_info">
                            <small>欢迎,</small> <%=this.CurrentAdmin.Sys_DisplayName %>
                        </span>
                        <i class="icon-caret-down"></i>
                    </a>
                    <ul id="user_menu" class="pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-closer">
                        <%--  <li><a href="javascript:"><i class="icon-cog"></i>设置 [暂未开放]</a></li>
                        <li><a href="javascript:"><i class="icon-user"></i>我的信息 [暂未开放]</a></li>--%>
                        <%--   <li class="divider"></li>--%>

                         <li><a id="changepwdstate" title="修改密码" href="javascript:"><i class="icon-user"></i>修改密码</a></li>                

                        <% if (CurrentAdmin.Sys_RoleID == 100)
                           { %>
                        <li>
                            <a title="初始化系统后台数据，将删除所有系统用户及角色" id="initsystem" href="javascript:"><i class="icon-cog"></i>初始化系统</a>
                        </li>
                        <% } %>
                        <li class="divider"></li>
                        <li>
                            <a title="退出当前系统" href="javascript:" onclick="javascript:logout()"><i class="icon-off"></i>退出</a>
                        </li>
                    </ul>
                </li>
            </ul>
            <!--/.ace-nav-->
        </div>
        <!--/.container-fluid-->
    </div>
    <!--/.navbar-inner-->
</div>
<script type="text/javascript">
    $(function () {
        $("#initsystem").click(function () {
            bootbox.confirm("您确认要初始化系统吗？<br/>此操作将删除所有系统用户及角色", function (result) {
                if (result) {
                    $.post("<% = Mars.Server.Utils.WebMaster.WebRoot %>handlers/AdminController/InitData.ashx", { "ts": new Date().getTime() }, function (data) {
                        var json = eval("(" + data + ")");

                        if (json.status == "1")
                        {
                            bootbox.dialog("操作成功!", [{
                                "label": "OK",
                                "class": "btn-small btn-primary",
                                callback: function () {
                                    refresh2();
                                }
                            }]);
                        }
                        else if (json.status == "0")
                        {
                            bootbox.alert("操作失败!");
                        }
                        else if (json.status == "2")
                        {
                            bootbox.alert("您的权限不足!");
                        }                    
                    });                 
                }
            });
        });

        $("#changepwdstate").click(function () {
            asyncbox.open({
                modal: true,
                id: "changepwd2",
                title: "修改管理员密码",
                url: "<% = Mars.Server.Utils.WebMaster.WebRoot %>Admin/ChangePwd.aspx?flag=2&pid=<% = CurrentAdmin.Sys_UserID %>&uname=<%= CurrentAdmin.Sys_LoginName%>" + "&ts=" + new Date().getTime(),
                width: 800,
                height: 300,
                callback: function (btnRes, cntWin, reVal) {               
                    if (btnRes == "sure") {
                        searchuser(true);
                    }
                }
            });
        });
    });

    var logout = function () {
        $.post("<% = Mars.Server.Utils.WebMaster.WebRoot %>handlers/AdminController/Logout.ashx", { "ts": new Date().getTime() }, function (data) {
            var json = eval("(" + data + ")");

            if (json.status == "1") {
                window.location.href = "../Logins/Login.aspx";
            }
            else {
                bootbox.alert("系统异常!");
                return false;
            }
        });
    }

    var refresh2 = function () {
        window.location.reload();
    }
</script>
