var setCommentParms = function (u) {
    if (u != undefined) {
        if (u != "-1") {
            $("#hiduid").val(u);
            setOverPostion(true, false);
        }
    }
}

var setOverPostion = function (reload,isgoto) {
    if (reload) {
        var obj = TObj("tmpEventItemCommentList");
        obj._isLoadDataNow = true;
        obj._prmsData.ts = new Date().getTime();
        obj.S();
    }
    if (isgoto) {
        goToComment();
    }
}

var goToComment = function () {
    $("html,body").animate({ scrollTop: $("#commentlist").offset().top-50 }, 0)
}

var openComment = function () {
    if ($("#hidshare").val() != "1") {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            window.location.href = "openbook://openCommentBar";
        }
        else if (browser.versions.android) {
            window.add_obj.addComments();
        } else {
            window.location.href = "http://www.kjrili.com/app/index.html";
        }
    } else {
        window.location.href = "http://www.kjrili.com/app/index.html";
    }
    
}

$(function () {
    //$("#hiduid").val(288);
    getUserID();
    $("#commentlist").hide();
});

var textAll = function (objId,obj) {
    if ($("#" + objId) != undefined){
        $("#" + objId).removeClass('comment-detail');
        $(obj).html("");
    }
}

var setUserID = function (uid) {
    $("#hiduid").val(uid);
}

var openReply = function (c, rc) {
    if ($("#hidshare").val() != "1") {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            window.location.href = "openbook://openReplyListid_rc_?" + c + "&" + rc;
        }
        else if (browser.versions.android) {
            window.add_obj.addReply(c, rc);
        }
    }
}

var deleteComment = function () {
    $.post(_root + "handlers/CommentController/DeleteComment.ashx", { "_id": $(".sure-a").attr("cid"), "_uid": $("#hiduid").val(), "ts": new Date().getTime() }, function (data) {
        var json = eval("(" + data + ")");
        if (json.state == "1") {
            setOverPostion(true, true);
            if ($("#hidshare").val() != "1") {
                if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                    window.location.href = "openbook://deleteComment";
                }
                else if (browser.versions.android) {
                    window.add_obj.refreshComments();
                }
            }
            closeDialog();

        } else {
            alert(json.msg);
        }
    });
}

var openDialog = function (cid)
{
    $(".dialog-box").addClass("is-visable");
    $(".sure-a").attr("cid", cid);
}

var closeDialog = function () {
    $(".dialog-box").removeClass("is-visable");
    $(".sure-a").attr("cid", 0);
}

var getUserID = function () {
    try{
        if ($("#hidshare").val() != "1") {
            if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
                window.location.href = "openbook://getUserId";
            }
            else if (browser.versions.android) {
                setUserID(window.add_obj.addUseId());
            } 
        }
    }catch(e)
    {

    }
}

var rl = function () {
    setOverPostion(true, false);
}