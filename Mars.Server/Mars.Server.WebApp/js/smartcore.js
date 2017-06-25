function smart() {
    //property
    var This = this;
    this._dataUrl;
    this._prmsData = {};
    this._holderID;
    this._hiddenCols = [];
    this.colsID = "idx";
    this.debug = false;
    this.enableColunmFilter = false;
    this.colunmSettingKey = "__col_settings";
    this.menu_flag = 0;
    this.asc_Class = "sortBg aoHorn";
    this.desc_Class = "sortBg aoHorn";
    this.r_regex = /(.*?):(.*)/;
    this._menuID;
    this.loadingHTML = "<div class=\"dataloading\"></div>";
    this.scrollLoadingHTML = "<div class=\"scrollingloading\"></div>";
    this.scrollNodataHTML = "<div class=\"scrollnodata\">已全部加载</div>";
    this.menuItemHTML = "<li><label class=\"labChe\"><input type=\"checkbox\" {0}  index=\"{1}\">{2}</label></li>" //"<li><input type=\"checkbox\" {0}  index=\"{1}\"><span>{2}</span></li>";
    this.menuHTML = " <ul class=\"ulMenu disNone\" id=\"{0}\">";
    this.headerItemHTML = "<a class=\"SecMenu\" href=\"javascript:void(0)\"></a>";
    this.paginationType = 1;
    this.httpType = 0;
    this.useCache = true;
    this.tmpHolderID;
    this._isLoadDataNow = false;

    //methord
    function Constructor() { };

    this.SetHolder = function (h) {
        This._holderID = h;
        This.tmpHolderID = "#__tmp_holder" + This._holderID;
        if (This.enableColunmFilter) {
            This._menuID = "__menu_" + This._holderID;
            if ($("#" + This._holderID).find("#" + This._menuID).length == 0) {
                $(document.body).append(This.format(This.menuHTML, This._menuID));
            }
            $("#" + This._menuID).mouseover(function () { This.menu_flag = 1; }).mouseout(function () { This.menu_flag = 0; });
            $("#" + This._holderID).get(0).onmousedown = function () {
                if (This.menu_flag == 0) {
                    $("#" + This._menuID).hide();
                }
            };
        }
        //增加临时容器


        if ($(This.tmpHolderID).length == 0) {
            $(document.body).append("<div id=\"__tmp_holder" + This._holderID + "\" style=\"display:none\"></div>")
        }

    };
    this.test = function () {
        alert("openbook");
    };

    this.setTemplate = function (template_path) {
        $("#" + This._holderID).setTemplateURL(template_path);
        $(This.tmpHolderID).setTemplateURL(template_path);
    }

    this.innerInitP = function () {
        This._prmsData.__mode = 1;
        This._prmsData._pageIndex = 0;

        if (!this.useCache || This._prmsData.ts == undefined) {
            This._prmsData.ts = new Date().getTime();
        }

        var tmps1 = [];
        $(".searchpart").each(function () {
            var m = This.r_regex.exec($(this).attr("key"));
            if (m == null) {
                tmps1.push("This._prmsData." + $(this).attr("key") + "= escape($(\"#" + $(this).attr("id") + "\").val()) ");
            }
            else {
                tmps1.push("This._prmsData." + m[1] + "=" + m[2]);
            }
        });
        eval(tmps1.join(';'));
    }

    this.initP = function () {
        This._prmsData._orderindex = 0; //reset orderindex
        This.innerInitP();
    };

    this.S = function (isAutoload) {
        This.onSearchButtonClicking(This, isAutoload);
        This.initP();
        This.onSearchButtonClicked(This, isAutoload);
        if (This.canSearch(This)) {
            if (This.paginationType < 2) {
                This.loadData();
            }
            else {
                This.InitscrollPagination();
            }
        }
    };

    this.restoreColumnSettings = function (id) {
        var _relid = id == undefined ? This._holderID : id;
        var settings_str = $.cookie(This.colunmSettingKey);
        var settings = This._hiddenCols;
        if (settings_str != null) {
            settings = $J(settings_str); //eval("(" + settings_str + ")");
        }

        var indexes = [];
        for (var i = 0; i < settings.length; i++) {
            var idx = $("#" + _relid + " th[idx=" + settings[i] + "]").hide().index();
            indexes.push(idx);
        }

        for (var j = 0; j < indexes.length; j++) {
            $("#" + _relid + " tr").each(function () {
                $($(this).find("td").get(indexes[j])).hide();
            });
        }
    };

    this.checkColumnEnable = function () {
        var cnt = $("#" + This._menuID).find("input:checked").length;
        if (cnt > 1) {
            $("#" + This._menuID).find("input:checked").removeAttr("disabled");
        }
        else {
            $("#" + This._menuID).find("input:checked").attr("disabled", "disabled");
        }
    };

    this.findOrderCols = function (index) {
        //var index = This._prmsData._orderindex;
        var ordertype = This._prmsData._ordertype;

        var target_th = null;

        $("#" + This._holderID).find("th").each(function () {
            var a_th = $(this);
            a_th.removeClass(This.asc_Class).removeClass(This.desc_Class);
            if ($(this).attr("ox") == index) {
                target_th = this;
            }
        });
        if (target_th != null) {
            $(target_th).addClass(ordertype == 0 ? This.desc_Class : This.asc_Class);
        }
    };

    this.InitOrderControl = function () {
        $("#" + This._holderID).find("a[oc=1]").click(function () {
            var index = $(this).parent().attr("ox");
            if (index == This._prmsData._orderindex) {
                This._prmsData._ordertype = This._prmsData._ordertype == 0 ? 1 : 0;
            }
            else {
                This._prmsData._orderindex = index;
                This._prmsData._ordertype = 0;
            }
            This._prmsData._pageIndex = 0;
            if (This.paginationType < 2) {
                This.loadData();
            }
            else {
                This.InitscrollPagination();
            }

        });
    };

    this.InitColumnFilter = function (id) {
        var _relid = id == undefined ? This._holderID : id;
        $("#" + _relid).find("th").each(function () {
            $(this).mouseover(function () {
                $(this).addClass("thHover");
                $(this).children(".SecMenu").show();
            }).mouseout(function () {
                $(this).removeClass("thHover");
                $(this).children(".SecMenu").hide();
            });

            var h = $(This.headerItemHTML).click(function () {
                var maxleft = document.documentElement.clientWidth - 100;
                var pos = $(this).offset();
                var _left = pos.left;
                var width = $(this).width();
                if (_left + width > maxleft) {
                    _left = maxleft - width - 20;
                }
                var height = $(this).height();
                $("#" + This._menuID).css("top", pos.top + height - 5).css("left", _left).slideDown();
            });
            $(this).append(h);
        });

        if ($("#" + This._menuID).find("li").length == 0) {
            var i = 0;
            $("#" + This._holderID + " th").each(function () {
                This.log($(this).text() + "__" + $(this).is(":visible"));
                var mi = $(This.format(This.menuItemHTML, ($(this).is(":visible") ? " checked=\"checked\" " : ""), $(this).attr("idx"), $(this).text()));
                //mi.mouseover(function(){$(this).addClass("hLi")}).mouseout(function(){$(this).removeClass("hLi")});
                mi.find("input").bind("change", function () {
                    This.checkColumnEnable();
                    var index = $(this).attr("index");
                    if ($(this).attr("checked")) {
                        This.remove_col_settingID(index);
                        var idx = $("#" + This._holderID + " th[idx=" + index + "]").show().index();
                        $("#" + This._holderID + " tr").each(function () {
                            $($(this).find("td").get(idx)).show();
                        });
                    }
                    else {
                        This.add_cos_settingID(index);
                        var idx = $("#" + This._holderID + " th[idx=" + index + "]").hide().index();
                        $("#" + This._holderID + " tr").each(function () {
                            $($(this).find("td").get(idx)).hide();
                        });
                    }
                });
                $("#" + This._menuID).append(mi);
                i++;
            });

            This.checkColumnEnable();

        }
    };

    this.loadData = function () {
        This.log("Request::" + This._dataUrl + "?" + j2U(This._prmsData));
        $("#" + This._holderID).html(This.loadingHTML);

        if (!this.useCache) {
            This._prmsData.ts = new Date().getTime();
        }

        $.ajax({
            type: (This.httpType == 0 ? 'GET' : 'POST'),
            url: This._dataUrl, //+ (This.httpType == 0 ? "" : "?__mode="+This._prmsData.__mode)
            data: This._prmsData,
            success: function (data) {
                var json = $J(data); //eval("(" + data + ")");
                //$("#" + This._holderID).processTemplate(json);
                This.handleRetData(This, json);
                //列筛选开始
                if (This.enableColunmFilter) {
                    This.restoreColumnSettings();
                    This.InitColumnFilter();
                }
                //列筛选结束

                //order function
                This.findOrderCols(json.orderindex);
                This.InitOrderControl();


                if (This._prmsData._pageIndex == 0 && This.paginationType > 0) {
                    initPagination(json.cnt);
                }

                This.onDataLoaded(This, json);

            },
            dataType: 'html'
        });
    };

    this.stopScrollPagination = function () {
        $("#" + This._holderID).stopScrollPagination();
    }

    this.InitscrollPagination = function () {
        This.log("Request::" + This._dataUrl + "?" + j2U(This._prmsData));
        $("#" + This._holderID).html(This.loadingHTML);
        $("#" + This._holderID).scrollPagination({
            'isLoadDataNow': This._isLoadDataNow,
            'contentPage': This._dataUrl, // the url you are fetching the results
            'contentData': This._prmsData, // these are the variables you can pass to the request, for example: children().size() to know which page you are
            'scrollTarget': $(window), // who gonna scroll? in this example, the full window
            'heightOffset': 100, // it gonna request when scroll is 100 pixels before the page ends
            'beforeLoad': function () { // before load function, you can display a preloader div
                //$('#loading').fadeIn();
                if (This._prmsData._pageIndex > 0) {
                    $("#" + This._holderID).append(This.scrollLoadingHTML);
                    $("#" + This._holderID).find(".scrollnodata").remove();
                }
            },
            'httpType': This.httpType,
            'afterLoad': function (data) { // after loading content, you can use this function to animate your new elements
                //$('#loading').fadeOut();
                $("#" + This._holderID).find(".scrollingloading").remove();
                //alertify.success("Success notification");
                //var i = 0;

                var json = $J(data); //eval("(" + data + ")");

                if (json.list == undefined || json.list.length == 0) {
                    $("#" + This._holderID).scrollNoData();
                    var nodata = $("*[class='searchnodata']");
                    if (nodata == undefined || nodata.length==0) {
                        $("#" + This._holderID).append(This.scrollNodataHTML);
                    }
                }

                if (This._prmsData._pageIndex == 0) {
                    $("#" + This._holderID).processTemplate(json);
                    //列筛选开始
                    if (This.enableColunmFilter) {
                        This.restoreColumnSettings();
                        This.InitColumnFilter();
                    }
                    //列筛选结束
                    //order function
                    This.findOrderCols(json.orderindex);
                    This.InitOrderControl();
                }
                else {
                    $(This.tmpHolderID).processTemplate(json);

                    //列筛选开始
                    if (This.enableColunmFilter) {
                        This.restoreColumnSettings("__tmp_holder" + This._holderID);
                        This.InitColumnFilter("__tmp_holder" + This._holderID);
                    }

                    if (json.showstyle == "2") {
                        //记录条目显示  暂时写死
                        var rows = $(This.tmpHolderID).find("*[Repeater='item']");
                        var holdertarget = $("#" + This._holderID );

                        if (rows.length > 0) {
                            for (var i = 0; i < rows.length; i++) {
                                holdertarget.append(rows[i]);
                            }
                        }
                    } else {
                        var rows = $(This.tmpHolderID).find("tr[class]");
                        var holdertarget = $("#" + This._holderID + " tbody");

                        if (rows.length > 0) {
                            for (var i = 0; i < rows.length; i++) {
                                holdertarget.append(rows[i]);
                            }
                        }
                    }
                }
                This.onDataLoaded(This, json);
                This._prmsData._pageIndex++;



                //$(elementsLoaded).fadeInWithDelay();

                //                if ($('#content').children().size() > 100) { // if more than 100 results already loaded, then stop pagination (only for testing)
                //                    $('#nomoreresults').fadeIn();
                //                    $('#content').stopScrollPagination();
                //                }
            }
        });
    }
    /******************/


    // code for fade in element by element
    $.fn.fadeInWithDelay = function () {
        var delay = 0;
        return this.each(function () {
            $(this).delay(delay).animate({ opacity: 1 }, 200);
            delay += 100;
        });
    };
    /*****************/


    //翻页处理
    function sp(page_id) {
        This._prmsData._pageIndex = page_id;
        This.loadData();
    }

    this.hidePagination = function () {
        $("#dp_" + This._holderID).hide();
    }

    //生成分页
    function initPagination(cnt) {
        $("#dp_" + This._holderID).show().pagination(cnt, {
            callback: sp,
            prev_text: '< 上一页',
            next_text: '下一页 >',
            items_per_page: This._prmsData._pageSize,
            num_display_entries: 6,
            current_page: This._prmsData._pageIndex,
            num_edge_entries: 2,
            link_to: "javascript:void(0)"
        });
    }

    this.download = function () {
        This._prmsData.__mode = 2;
        var fd = "";
        $("#" + This._holderID + " th").each(function () {
            if ($(this).is(":visible")) {
                fd += $(this).attr("idx") + ",";
            }
        });
        var url = This._dataUrl + "&" + j2U(This._prmsData) + "&_fd=" + fd;
        This.log("Download::" + url);
        This._prmsData.__mode = 1;
        $("#__exframe").attr("src", url);
    };

    this.downloadAll = function () {
        This._prmsData.__mode = 2;
        var url = This._dataUrl + "&" + j2U(This._prmsData);
        This.log("Download::" + url);
        This._prmsData.__mode = 1;
        s_alert("下载中.....请耐心等待...", 10);
        $("#__exframe").attr("src", url);
    };

    this.setD = function (controlid) {
        $("#" + controlid).click(function () {
            s_alert("下载中...请耐心等待...", 10);
            This.download();
        });
    };

    this.setDA = function (controlid) {
        $("#" + controlid).click(function () {
            This.downloadAll();
        });
    };

    this.setS = function (controlid) {
        $("#" + controlid).click(function () {
            This.S();
        });
    };

    this.log = function (msg) {
        if (This.debug) {
            if (!$("#__log").get(0)) {
                $(document.body).prepend("<div id=\"__log\" style=\"height:100px; background-color:Black;color:White; overflow:scroll;\"></div>");
            }
            $("#__log").prepend(msg + "<br/>");
        }
    };

    this.format = function () {
        if (arguments.length == 0)
            return null;

        var str = arguments[0];
        for (var i = 1; i < arguments.length; i++) {
            var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
            str = str.replace(re, arguments[i]);
        }
        return str;
    };
    this.exsitID = function (s, v) {
        for (var i = 0; i < s.length; i++) {
            if (s[i] == v) {
                return true;
            }
        }
        return false;
    };
    this.remove_col_settingID = function (id) {
        var settings = $J($.cookie(This.colunmSettingKey)); //eval("("+$.cookie(This.colunmSettingKey)+")");
        if (settings == null) {
            settings = This._hiddenCols;
        }
        var d = [];
        if (settings != null) {
            for (var i = 0; i < settings.length; i++) {
                if (settings[i] != id) {
                    d.push(settings[i]);
                }
            }
        }

        $.cookie(This.colunmSettingKey, (d.length > 0 ? "[\"" + d.join("\",\"") + "\"]" : d), { expires: 365 });
    };
    this.add_cos_settingID = function (id) {
        var settings = $J($.cookie(This.colunmSettingKey)); //eval("("+$.cookie(This.colunmSettingKey)+")");
        if (settings == null) {
            settings = This._hiddenCols;
        }
        if (!This.exsitID(settings, id)) {
            settings.push(id);
        }

        $.cookie(This.colunmSettingKey, (settings.length > 0 ? "[\"" + settings.join("\",\"") + "\"]" : settings), { expires: 365 });
    };

    this.onDataLoaded = function (obj, data) { };
    this.onSearchButtonClicked = function (obj, isAutoload) { };
    this.onSearchButtonClicking = function (obj, isAutoload) { };
    this.canSearch = function (obj) { return true; }
    this.handleRetData = function (obj, json) {
        $("#" + obj._holderID).processTemplate(json);
    }

    Constructor();
}