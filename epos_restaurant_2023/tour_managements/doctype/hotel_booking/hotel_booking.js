// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Hotel Booking", {
	refresh(frm) {

	},
    arrival_date(frm){   
        setTotalNights(frm);  
    },
    departure_date(frm){    
        setTotalNights(frm);
    },
    total_rooms(frm){
        setTotalRoomNight(frm);
    },
    total_nights(frm){
        setTotalRoomNight(frm);
    }

});

function setTotalNights(frm){
    if (frm.doc.arrival_date && frm.doc.departure_date){
        frm.doc.total_nights = frappe.datetime.get_diff( frm.doc.departure_date , frm.doc.arrival_date )
        frm.refresh_field('total_nights');
    }
    
}
function setTotalRoomNight(frm){
    if(frm.doc.total_nights && frm.doc.total_rooms){
        frm.doc.total_room_night = frm.doc.total_nights * frm.doc.total_rooms
        frm.refresh_field('total_room_night');
    }
}