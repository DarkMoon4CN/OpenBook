$(function () {
    $('.tabs a').click(function () {
        var i = $(this).index();
        var searchtype = $(this).attr("searchtype");
        var show = $('.result-con').children();
        $(this).addClass('active').siblings().removeClass('active');
        $(".content-slide").eq(i).show().siblings().hide();
        if (searchtype == "1") {
            //展商控制
            searchExhibitor();
        } else if (searchtype == "2") {
            //活动查询
            searchActivity();
        }
    });

    $(".tabs").hide();

    if ($("#hid_searchtype").val() == "1") {
        $("#search-keyword").attr("placeholder", "输入展商名称");
        $("#div_exhibitor").show();
        $("#div_activity").hide();
    } else if ($("#hid_searchtype").val() == "2") {
        $("#search-keyword").attr("placeholder", "输入活动名称");
        $("#div_exhibitor").hide();
        $("#div_activity").show();
    } else {

    }


});

$(function () {
    $("#search-keyword").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: _root + "handlers/ExhibitionController/GetSearchKeyWordList.ashx",
                dataType: "jsonp",
                data: {
                    top: 5,
                    key: request.term,
                    type: 1,
                    exhibitionid: $("#hid_exhibitionid").val(),
                    searchtype: $("#hid_searchtype").val()
                },

                success: function (data) {
                    response($.map(data.keywordlist, function (item) {
                        return {
                            label: item.name, value: item.searchkey, key: item.searchkey
                        }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $("#search-keyword").val(ui.item.key);
            $("#acontroller").html("取消");
            searchResults();
        },
        open: function (event, ui) {
            if ($.trim($(this).val()) == "") {
                $("#acontroller").html("取消");
            } else {
                $("#acontroller").html("查询");
            }
        }
    });

    $("#search-keyword").keyup(function () {
        if (event.keyCode == "13") {//keyCode=13是回车键
            searchResults();
        }
        if ($.trim($(this).val()) == "") {
            $("#acontroller").html("取消");
        } else {
            $("#acontroller").html("查询");
        }
    });

    $("#acontroller").click(function () {
        if ($(this).html() == "查询") {
            $(this).html("取消");
            searchResults();
        } else {
            if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                window.location.href = "kaijuanCancelBtn_cancelBtnBackToHome";
            }
            else if (browser.versions.android) {
                window.close_obj.onCloseClick();
            }
            //if ($("#hid_searchtype").val() == "") {
                
            //}
            //else if ($("#hid_searchtype").val() == "1") {
            //    window.location.href ="exhibitors.aspx";
            //}
            //else if ($("#hid_searchtype").val() == "2") {
            //    window.location.href = "activitylist.aspx";
            //}
        }
    });
});

var searchExhibitor = function () {
    if ($.trim($("#search-keyword").val()) != "") {
        var obj = TObj("exhibitorholder");
        obj._prmsData._exhibitorname = escape($("#search-keyword").val());
        obj._prmsData._exhibitionid = $("#hid_exhibitionid").val();
        obj._prmsData._pageIndex = 0;
        if (obj.paginationType < 2) {
            obj.loadData();
        }
        else {
            obj.InitscrollPagination();
        }
    }
}

var searchActivity = function () {
    if ($.trim($("#search-keyword").val()) != "") {
        var obj = TObj("activityholder");
        obj._prmsData._searchname = escape($("#search-keyword").val());
        obj._prmsData._exhibitionid = $("#hid_exhibitionid").val();
        obj._prmsData._pageIndex = 0;
        if (obj.paginationType < 2) {
            obj.loadData();
        }
        else {
            obj.InitscrollPagination();
        }
    }
}

var searchResults = function () {
    //if ($(".tabs").is(':hidden')) {
    //    if ($("#hid_searchtype").val() == "1") {
    //        searchExhibitor();
    //    } else if ($("#hid_searchtype").val() == "2") {
    //        searchActivity();
    //    }
    //} else {
    //    var activediv = $(".tabs a[class='active']").attr("searchtype");
    //    if (activediv == "1") {
    //        //展商控制
    //        searchExhibitor();
    //    } else if (activediv == "2") {
    //        //活动查询
    //        searchActivity();
    //    }
    //}

    
    if ($("#hid_searchtype").val() == "1") {
        searchExhibitor();
    } else if ($("#hid_searchtype").val() == "2") {
        searchActivity();
    }
    else {
        $(".tabs").show();
        var activediv = $(".tabs a[class='active']").attr("searchtype");
        if (activediv == "1") {
            //展商控制
            searchExhibitor();
        } else if (activediv == "2") {
            //活动查询
            searchActivity();
        }
    }
}
