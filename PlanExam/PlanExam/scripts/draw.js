var container = document.getElementById("container");
var mapPoly = document.getElementById("ImageMapmapAreas");

container.addEventListener('mousedown', mouseDown, false);
container.addEventListener('mouseup', mouseUp, false);
container.addEventListener('mousemove', mouseMove, false);

var jcrop;

function init() {
    $('#pic').Jcrop({
        onChange: WriteCoord,
        onSelect: WriteCoord,
        onRelease: AddArea
    }, function () {
        jcrop = this;
    });
}

//$('.map').maphilight({
//    alwaysOn: true,
//    shadow: true,
//    strokeColor: 'red',
//    strokeOpacity: 1,
//    strokeWidth: 1,
//    fade: false
//});



function WriteCoord(c) {
    $('#x').val(c.x);
    $('#y').val(c.y);
    $('#x2').val(c.x2);
    $('#y2').val(c.y2);
}

function AddArea() {
    var element = document.createElement("area");
    element.id = "poly";
    element.shape = "poly";
    element.coords = $('#x').val() + "," + $('#y').val() + "," + $('#x2').val() + "," + $('#y2').val();
    element.alt = "text";
    element.title = "text";
    element.href = "#";
    mapPoly.appendChild(element);
    jcrop.destroy();
}

function mouseDown() {

}

function mouseUp() {

}

function mouseMove() {

}