function PhotoCamara(id) {
    Webcam.reset('#video');
    Webcam.set({
        width: 540,
        height: 420,
        dest_width: 540,
        dest_height: 420,
        fps: 45,
        image_format: 'jpeg',
        jpeg_quality: 100
    });
    Webcam.attach('#video');
}
function CapturePhoto() {
    return new Promise((resolve) => {
        Webcam.snap(async (data_uri) => {
            resolve(data_uri);
        });
    });
}




    $('.helpicon').on('click', function () {
        $('.truncate').addClass('truncated');
    });

    $('span').on('click', function () {
        $('.truncated').removeClass('truncated');
    });
