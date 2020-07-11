var domain = window.location.protocol + "//" + window.location.host;

function showloading(t) {
    if (t) {//如果是true则显示loading
        console.log(t);
        loading = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
    } else {//如果是false则关闭loading
        console.log("关闭loading层:" + t);
        layer.closeAll('loading');
    }
}



function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return null;
}
