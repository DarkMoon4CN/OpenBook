$(function () {
    var curIndex = 0;
    var timeInterval = 3000;
    var arr = new Array();
    arr[0] = "img/one.png";
    arr[1] = "img/two.png";
    //arr[2] = "img/three.png";
    setInterval(changeImg, timeInterval);
    function changeImg() {
        var obj = document.getElementById("img");
        if (curIndex == arr.length - 1) {
            curIndex = 0;
            direction = "top";
        } else {
            curIndex += 1;
        }
        obj.src = arr[curIndex];
    }

    var QRCode = function (searchtype) {
        if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
            if (searchtype == 1) {
                window.location.href = "exhibitorWebToSearchView";
            } else if (searchtype == 2) {
                window.location.href = "activityWebToSearchView";
            }
        }
        else if (browser.versions.android) {
            window.search_obj.onClickSearch();
        }
    }

})