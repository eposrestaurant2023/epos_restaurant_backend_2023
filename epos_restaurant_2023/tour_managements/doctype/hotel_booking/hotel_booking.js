// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Hotel Booking", {
	refresh(frm) {

	},
    arrival_date(frm){   
        setTotalNights(frm);  
        $.each(frm.doc.room_types,  function(i, d)  {
            updateRoomTypeRow(frm,d)
        });
    },
    departure_date(frm){    
        setTotalNights(frm);
        $.each(frm.doc.room_types,  function(i, d)  {
            updateRoomTypeRow(frm,d)
        });
    },
    total_rooms(frm){
        setTotalRoomNight(frm);
    },
    total_nights(frm){
        setTotalRoomNight(frm);
    }

});

frappe.ui.form.on('Hotel Booking Room Type', {
    room_type:function (frm,cdt, cdn) {
        let doc = locals[cdt][cdn];
        frm.call({
            method: 'get_rate',
            args:{
                hotel_name:frm.doc.hotel_name,
                room_type:doc.room_type,
            },
            callback:function(r){
                if(r.message){
                   doc.rate = r.message;
                   updateRoomTypeRow(doc);
                }
                
            },
            async: true,
        });
         

    },
    single_room:function (frm,cdt, cdn) {
        updateRoomTypeRow(frm, locals[cdt][cdn] )

    },
    double_room:function (frm,cdt, cdn) {
        updateRoomTypeRow(frm, locals[cdt][cdn] )
    },
    twin_room:function (frm,cdt, cdn) {
        updateRoomTypeRow(frm, locals[cdt][cdn] )
    },
})



function updateRoomTypeRow(frm,doc){
    
    doc.total_amount =doc.rate * frm.doc.total_nights * (doc.number_of_room || 1);
    doc.number_of_room = (doc.single_room || 0) + (doc.double_room || 0) + (doc.twin_room || 0)
    doc.number_of_room = doc.number_of_room || 1;
    doc.total_amount =(doc.rate || 0) * (frm.doc.total_nights || 1) * (doc.number_of_room || 1);
    refresh_field('room_types');  
}
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