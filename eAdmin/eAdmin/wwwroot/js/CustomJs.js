function CustomConfirm(title, message, type) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: message,
            type: type,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Ok'
        }).then((result) => {
            if (result.value) {
                resolve(true);
            } else {
                resolve(false);
            }
        });
    });
}

function HistoryBack() {
    window.history.back();
}

function SweetAlert() {
    swal("Congratulation!", "Save Success!", "success");
}

function SetActiveMenu() {
    
    $(".uk-parent").children("a").removeClass('active');
    $(".uk-nav-sub a").closest(".uk-parent").removeClass('uk-open');

    $(".uk-nav-sub .active").closest(".uk-parent").children("a").addClass('active');
    $(".uk-nav-sub a").closest(".uk-nav-sub").attr("aria-hidden", "true").attr("hidden","hidden");

    $(".uk-nav-sub .active").closest(".uk-parent").addClass("uk-open");
    $(".uk-nav-sub .active").closest(".uk-nav-sub").attr("aria-hidden", "false").removeAttr("hidden");
    
}

function Calendar(id, controller, eventModel) {
    var calendarEl = document.getElementById(id); 
    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid', 'timeGrid', 'dayGridWeek'],
        defaultView: 'dayGridMonth',
        defaultDate: (new Date()),
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        
        nowIndicator: true,
        events: {
            url: controller,
            extraParams: {
                outlet_id: eventModel.outlet_id
            }
        },
        eventClick: async (info) => { 
            await DotNet.invokeMethodAsync('eOptical', "EventClick", parseInt(info.event.id));
        }

    });
    var h = $(window).height() - 60;
    var w = $(window).width();
    calendar.setOption('aspectRatio', w/h);
    calendar.refetchEvents();
    calendar.render();
}
 

window.JsFunctions = {
    focusElement: function (element) {
        element.focus();
        element.select();
    },
    ReloadPage: function () {
        location.reload();
    }

}

function IsHasFooter() {
    if ($(".wrp-action-form").length > 0) {
        $(".page_containter").addClass("is-has-footer");
    } else{
        $(".page_containter").removeClass("is-has-footer");
    }
} 
 
 

function IncreaseTextArea(id) {
    $("#" + id).on('input', function () {
        this.style.height = 'auto';

        this.style.height =
            (this.scrollHeight) + 'px';
    });
}

function openFullscreen(id) {
    var elem = document.getElementById(id);
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
        elem.msRequestFullscreen();
    }
}

function closeFullscreen(id) {
    var elem = document.getElementById(id);
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
    }
}
 

function ToogleMenu() {
    $(".app-layout").toggleClass("is_open");
}

//$(document).ready(function () {
//    UIkit.toggle(".app-layout", { "cls": "is_open" });
//    $('.uk-button-province').on('click', function () {
//        var _marker = markers.find(r => r.id == $(this).data('markerid'));
//        if (_marker != undefined) {
//            google.maps.event.trigger(_marker, 'click');
//            map.setZoom(8);
//            map.panTo(_marker.getPosition());
//            $('.uk-button-province').removeClass("active");
//            $(this).addClass("active");
//        }
//    });
//});

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
function CheckIfExit(selector) {
    return new Promise((resolve) => {
        alert($(selector).length);
        if ($(selector).length > 0) {
            resolve(true);
        } else {
            resolve(false);
        }
    });
}

function ClosePhotoCamara(id) {
    Webcam.reset('#video');
}
function Toast(msg, type, position) {
    $.notify(msg, { className: type, globalPosition: position });
}