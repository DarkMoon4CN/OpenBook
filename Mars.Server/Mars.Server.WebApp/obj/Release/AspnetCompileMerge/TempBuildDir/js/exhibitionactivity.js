//��ӽ���������
var addTOSchedule = function (id, title, stime, etime) {
    ///alert(id);
    if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
        window.location.href = "webAddRichengByTitle_bTime_eTime_?" + title + "&" + stime + "&" + etime + "";
    }
    else if (browser.versions.android) {
        window.add_schedule.onAddScheduleClick(title, stime, etime);
    }
}

//���ӻ�������췽��������
var openDetail = function (obj, aid) {
    $("#div_child_"+aid).slideToggle();
    $(obj).find('.btm_arrow').toggleClass('top_arrow');
}