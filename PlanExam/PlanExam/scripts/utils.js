var elem = document.getElementById('container');

if (elem.addEventListener) {
    if ('onwheel' in document) {
        // IE9+, FF17+
        elem.addEventListener("wheel", onWheel);
    } else if ('onmousewheel' in document) {
        // устаревший вариант события
        elem.addEventListener("mousewheel", onWheel);
    } else {
        // Firefox < 17
        elem.addEventListener("MozMousePixelScroll", onWheel);
    }
} else { // IE8-
    elem.attachEvent("onmousewheel", onWheel);
}


function onWheel(e) {
    e = e || window.event;

    // wheelDelta не дает возможность узнать количество пикселей
    var delta = e.deltaY || e.detail || e.wheelDelta;
    var direction = true;
    if (delta > 0) {
        //прокрутка вниз
        direction = false;
    }
    $.ajax({
        url: 'GetScaledImage',
        type: 'GET',
        cache: false,
        data: { 'direction': direction },
        success: function (results) {
            //$("#pic").attr("src", results);
            var canvas = document.getElementById("myCanvas"),
                ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            // Контекст
            var pic = new Image();
            pic.src = results;
            pic.onload = function () {
                ctx.drawImage(pic, 0, 0);
            }
        },
        error: function () {
            alert('Error occured');
        }
    });

    e.preventDefault ? e.preventDefault() : (e.returnValue = false);
}

function getClientScreenSize() {
    $(document).ready(function () {
        var width = screen.width;
        var height = screen.height;

        $.ajax({
            url: 'Home/GetClientScreenSize',
            type: 'POST',
            cache: false,
            data: { 'width': width, 'height': height },
            success: function () {
                Console.log('screen size sended');
            },
            error: function () {
                //пару раз были проблемы, поэтому страхуемся
                $.ajax({
                    url: 'GetClientScreenSize',
                    type: 'POST',
                    cache: false,
                    data: { 'width': width, 'height': height },
                    success: function () {
                        Console.log('screen size sended');
                    },
                    error: function () {
                        alert('Error while sending client screen size');
                    }
                });
            }
        });
    });
}

function draw() {

}
