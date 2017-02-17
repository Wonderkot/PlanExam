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
  var info = document.getElementById('delta');
  info.innerHTML = +info.innerHTML + delta;
  $.ajax({
      url: 'ZoomIn',
      type: 'GET',
      cache: false,
      data: { 'step': info.innerHTML },
      success: function (results) {
          $("#pic").attr("src", results);
          var img = document.getElementById('img');
          img.innerHTML = +img.innerHTML + results;
      },
      error: function () {
          alert('Error occured');
      }
  });
  
  e.preventDefault ? e.preventDefault() : (e.returnValue = false);
}