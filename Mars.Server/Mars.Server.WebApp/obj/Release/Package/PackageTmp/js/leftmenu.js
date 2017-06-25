$(function () {
    var para = getquerystring("fun");
    var pid = "";

    $("#li_" + para).attr("class", "active");
    if ($("#li_" + para).attr("pid") != undefined) {
        pid = $("#li_" + para).attr("pid");
    }

    if(pid!="")
    {
        $("#" + pid).attr("class", "active open");
    }
});
