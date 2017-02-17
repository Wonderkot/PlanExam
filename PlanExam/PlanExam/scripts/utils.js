$.ajax({
    url: 'ZoomIn',
    type: 'GET',
    cache: false,
    data: { 'ratio': 555 },
    success: function (results) {
        $("#pic").attr("src", results);
    },
    error: function () {
        alert('Error occured');
    }
});