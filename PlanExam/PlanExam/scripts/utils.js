var elem = document.getElementById('container');

var notes = [];

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
            $("#pic").attr("src", results);
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
    $('#pic').imgAreaSelect({ enable: true, onSelectChange: showaddnote, x1: 120, y1: 90, x2: 280, y2: 210 });
    return false;
}

function showaddnote(img, area) {
    var imgOffset = $(img).offset();
    var formLeft = parseInt(imgOffset.left) + parseInt(area.x1);
    var formTop = parseInt(imgOffset.top) + parseInt(area.y1) + parseInt(area.height) + 5;

    $('#noteform').css({ left: formLeft + 'px', top: formTop + 'px' });

    $('#noteform').show();

    $('#noteform').css("z-index", 10000);
    $('#NoteX1').val(area.x1);
    $('#NoteY1').val(area.y1);
    $('#NoteHeight').val(area.height);
    $('#NoteWidth').val(area.width);
}

function saveNote() {
    var noteX1 = $('#NoteX1').val();
    var noteY1 = $('#NoteY1').val();
    var noteHeight = $('#NoteHeight').val();
    var noteWidth = $('#NoteWidth').val();
    var noteNote = $('#NoteNote').val();
    var id = notes.length + 1;

    notes.push({ "x1": noteX1, "y1": noteY1, "height": noteHeight, "width": noteWidth, "note": noteNote, "id": id });

    //$('#pic').imgNotes({ notes: notes, isMobile: false });
    //cancel();
}

function cancel() {
    $('#pic').imgAreaSelect({ hide: true, disable: true });
    $('#noteform').hide();
}