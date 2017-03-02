var container = document.getElementById("container");
var mapPoly = document.getElementById("ImageMapmapAreas");

container.addEventListener('mousedown', mouseDown, false);
container.addEventListener('mouseup', mouseUp, false);
container.addEventListener('mousemove', mouseMove, false);

$(document).ready(function () {
    $('img#pic').imgAreaSelect({
        handles: true,
        onSelectEnd: WriteCoord
    });
});

function init() {
    
}

$('.map').maphilight({
    alwaysOn: true,
    shadow: true
});



function WriteCoord(img, selection) {

    $('#x').val(selection.x1);
    $('#y').val(selection.y1);
    $('#x2').val(selection.x2);
    $('#y2').val(selection.y2);
    AddArea();
}

function AddArea() {
    var element = document.createElement("area");
    element.id = "poly";
    element.shape = "rect";
    element.coords = $('#x').val() + "," + $('#y').val() + "," + $('#x2').val() + "," + $('#y2').val();
    element.alt = "text";
    element.title = "text";
    element.href = "#";
    mapPoly.appendChild(element);
}

function mouseDown() {

}

function mouseUp() {

}

function mouseMove() {

}