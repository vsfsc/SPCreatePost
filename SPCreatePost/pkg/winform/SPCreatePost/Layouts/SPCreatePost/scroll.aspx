<html lang="zh-cn">
<head>
    <meta charset="UTF-8">
    <title>信息无缝滚动效果</title>
    <style >
       
* {
    padding: 0;
    margin: 0;
}

body {
    font-size: 12px;
    line-height: 24px;
    text-align: center;
}

ul {
    list-style: none;
}

a img {
    border: none;
}

a {
    color: #333333;
    text-decoration: none;
}

a:hover {
    color: #ff0000;
}

#box {
    width: 335px;
    height: 144px;
    margin: 50px auto 0 auto;
    overflow: hidden; /*  这个一定要加，超出的内容部分要隐藏，免得撑高中间部分 */
}
    </style>
</head>
<body>

<div id="box" style="width:200px; " >
    <ul id="cont1">
        <li><a href="#">111111111111</a></li>
        <li><a href="#">222222222222</a></li>
        <li><a href="#">333333333333</a></li>
        <li><a href="#">444444444444</a></li>
        <li><a href="#">555555555555</a></li>
        <li><a href="#">666666666666</a></li>
        <li><a href="#">777777777777</a></li>
        <li><a href="#">888888888888</a></li>
        <li><a href="#">999999999999</a></li>
    </ul>
    <ul id="cont2"></ul>
</div>

<script>
    复制代码
    var area = document.getElementById('box');
    var cont1 = document.getElementById('cont1');
    var cont2 = document.getElementById('cont2');

    area.scrollTop = 0;
    // 克隆cont1给cont2
    cont2.innerHTML = cont1.innerHTML;

    function myScroll() {
        if (area.scrollTop >= cont1.scrollHeight) {
            area.scrollTop = 0;
        } else {
            area.scrollTop++;
        }
    }

    var time = 50;
    var interval = setInterval('myScroll()', time);

    area.onmouseover = function () {
        clearInterval(interval);
    };

    area.onmouseout = function () {
        // 继续执行之前的定时器
        interval = setInterval('myScroll()', time);
    };
</script>

</body>
</html>
