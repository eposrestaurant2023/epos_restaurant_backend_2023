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
    }

});

function setTotalNights(frm){
    if (frm.doc.arrival_date && frm.doc.departure_date){
        frm.doc.total_nights = frappe.datetime.get_diff( frm.doc.departure_date , frm.doc.arrival_date )
        frm.refresh_field('total_nights');
    }
}