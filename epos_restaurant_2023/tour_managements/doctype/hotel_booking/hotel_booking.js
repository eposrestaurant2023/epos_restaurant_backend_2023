// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Hotel Booking", {
	refresh(frm){
     
        set_indicator(frm);

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
    discount_type(frm){
        updateDiscount(frm); 
    },
    discount(frm){
        updateDiscount(frm); 
    },
    discount_amount(frm){
        updateDiscount(frm); 
    },
    total_rooms(frm){
        updateTotalRoomNight(frm);
    },
    total_nights(frm){
        updateTotalRoomNight(frm);
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
        updateTotalRoom(frm, locals[cdt][cdn] )
    },
    double_room:function (frm,cdt, cdn) {
        updateRoomTypeRow(frm, locals[cdt][cdn] )
        updateTotalRoom(frm, locals[cdt][cdn] )
    },
    twin_room:function (frm,cdt, cdn) {
        updateRoomTypeRow(frm, locals[cdt][cdn] )
        updateTotalRoom(frm, locals[cdt][cdn] )
    }
})

frappe.ui.form.on('Tour Booking Payments', {
 
    payment_amount:function (frm,cdt, cdn) {
        const payments=  frm.doc.payments;
        frm.set_value('total_payment', payments.reduce((n, d) => n + d.payment_amount,0));

    },
    payments_remove:function(frm){
        frm.set_value('total_payment', frm.doc.payments.reduce((n, d) => n + (d.payment_amount || 0),0));   
    }
     
})
function updateTotalRoom(frm,doc){
     frm.set_value('total_rooms', frm.doc.room_types.reduce((n,d) => n + d.number_of_room, 0));
     frm.refresh_field('total_rooms');
    
}
function updateTotalRoomNight(frm){
    if(frm.doc.total_rooms && frm.doc.total_nights){
        frm.doc.total_room_night = (frm.doc.total_rooms || 0) * (frm.doc.total_nights || 0)
        frm.refresh_field('total_room_night');
    }
}

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

function updateDiscount(frm){
    if (frm.doc.discount_type=="Percent" && frm.doc.discount){
        frm.doc.discount_amount = (frm.doc.total_amount || 0) * (frm.doc.discount || 0)/100;  
        frm.refresh_field('discount_amount')
    }
    else{
        frm.doc.discount_amount = (frm.doc.discount||0)
        frm.refresh_field('discount_amount')
    }
}
function set_indicator(frm){
    if(frm.doc.__islocal)
			return;

    if (frm.doc.total_rooms > 0 )  frm.dashboard.add_indicator(__("Total Room: {0}", [frm.doc.total_rooms]),"blue");
    if (frm.doc.total_room_night > 0 )  frm.dashboard.add_indicator(__("Total Room Night: {0}",[frm.doc.total_room_night]) ,"blue");        
    if (frm.doc.total_amount > 0 )  frm.dashboard.add_indicator(__("Total Hotel Booking: {0}",[format_currency(frm.doc.total_amount)]) ,"blue");
    if (frm.doc.discount_amount > 0 )  frm.dashboard.add_indicator(__("Discount: {0}",[format_currency(frm.doc.discount_amount)]) ,"blue");
    if (frm.doc.total_payment > 0 )  frm.dashboard.add_indicator(__("Paid: {0}",[format_currency(frm.doc.total_payment)]) ,"blue");
    if (frm.doc.balance > 0 )  frm.dashboard.add_indicator(__("Balance: {0}",[format_currency(frm.doc.balance)]) ,"blue");
}
