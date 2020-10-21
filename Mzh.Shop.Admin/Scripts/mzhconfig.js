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

var popDiag = {
    layer: { 'width': 200, 'height': 100 },
    title: '提示消息',
    time: 3000,//自动关闭时间
    anims: { 'type': 'slide', 'speed': 500 },
    timer: null,
    inits: function (title, text) {
        if ($("#pop").is("div")) { return; }
        $this = this;
        $(document.body).prepend('<div id="pop" style="display:none;border:#b9c9ef 1px solid;z-index:100;width:' + this.layer.width + 'px;height:' + this.layer.height + 'px;position:absolute;background:#cfdef4; bottom:0; right:0; overflow:hidden;"><div style="border:1px solid #fff;border-bottom:none;width:100%;height:25px;font-size:12px;overflow:hidden;color:#1f336b;"><span id="pop_close" style="float:right;padding:5px 0 5px 0;width:16px;line-height:auto;color:red;font-size:12px;font-weight:bold;text-align:center;cursor:pointer;overflow:hidden;">×</span><div style="padding:5px 0 5px 5px;width:100px;line-height:18px;text-align:left;overflow:hidden;">' + title + '</div><div style="clear:both;"></div></div> <div style="padding-bottom:5px;border:1px solid #fff;border-top:none;width:100%;height:auto;font-size:12px;"><div id="pop_content" style="margin:0 5px 0 5px;border:#b9c9ef 1px solid;padding:10px 0 10px 5px;font-size:12px;width:' + (this.layer.width - 17) + 'px;height:' + (this.layer.height - 50) + 'px;color:#1f336b;text-align:left;overflow:hidden;">' + text + '</div></div></div>');
        $("#pop_close").click(function () {
            setTimeout('$this.close()', 1);
        });
        $("#pop").hover(function () {
            clearTimeout($this.timer);
            $this.timer = null;
        }, function () {
            $this.timer = setTimeout('$this.close()', $this.time);
        });
    },
    showPop: function (title, text, time) {
        if (title == 0) title = this.title;
        this.inits(title, text);
        if (time >= 0) this.time = time;
        switch (this.anims.type) {
            case 'slide': $("#pop").slideDown(this.anims.speed); break;
            case 'fade': $("#pop").fadeIn(this.anims.speed); break;
            case 'show': $("#pop").show(this.anims.speed); break;
            default: $("#pop").slideDown(this.anims.speed); break;
        };
        this.runPop(this.time);
    },
    runPop: function (time) {
        if (time > 0) {
            var tw = this;
            this.timer = setTimeout(function () { tw.close(); }, time);
        }
        //setTimeout('$("#pop").remove();',this.anims.speed);
    },
    close: function () {
        switch (this.anims.type) {
            case 'slide': $("#pop").slideUp(this.anims.speed); break;
            case 'fade': $("#pop").fadeOut(this.anims.speed); break;
            case 'show': $("#pop").hide(this.anims.speed); break;
            default: $("#pop").slideUp(this.anims.speed); break;
        };
        setTimeout('$("#pop").remove();', this.anims.speed);
        this.original();
    },
    original: function () {
        this.layer = { 'width': 200, 'height': 100 };
        this.title = '信息提示';
        this.time = 3000;
        this.anims = { 'type': 'slide', 'speed': 500 };
    }
};
