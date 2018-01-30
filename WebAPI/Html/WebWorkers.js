var i = 0;

function timedCount() {
    i = i + 1;
    postMessage(i);

    console.log("i=" + i);

    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/', true);

    xhr.onload = function () {
        // 请求结束后,在此处写处理代码
        console.log("xhr GET");
    };

    xhr.send(null);

    setTimeout("timedCount()", 15000);
}

timedCount();