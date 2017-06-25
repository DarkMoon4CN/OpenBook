//jquery-1.9.1.min.js
$(function () {
    poling.init();
});
var poling = {};
var testCount;
poling.init = function ()
{
    testCount = setInterval(waiting, 5000);
}

var waiting = function ()
{
    $.post(_root + "handlers/TestController/QueryTitleById.ashx", { "id": 25, "ts": new Date().getTime() }, function (data) {
        var testAlert = $.parseJSON(data);
        if (data == null)
        {
            alert("系统正在执行更新！请及时保存数据以防丢失");
            clearInterval(iCount);
        }
    });
}