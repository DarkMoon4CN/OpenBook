//添加进日历操作
var addTOSchedule = function (id, title, stime, etime) {
    ///alert(id);
    if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
        window.location.href = "webAddRichengByTitle_bTime_eTime_?" + title + "&" + stime + "&" + etime + "";
    }
    else if (browser.versions.android) {
        window.add_schedule.onAddScheduleClick(title, stime, etime);
    }
}

//打开子活动或者主办方，简介操作
var openDetail = function (obj, aid) {
    $("#div_child_"+aid).slideToggle();
    $(obj).find('.btm_arrow').toggleClass('top_arrow');
}