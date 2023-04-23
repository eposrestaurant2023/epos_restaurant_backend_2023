// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Ticket Booking", {
	refresh(frm) {

	},
    ticket_type(frm){
        
        frm.set_query("ticket_code","ticket_booking_item", function() {
            return {
                filters: [
                    ["Ticket","ticket_category", "=", frm.doc.ticket_type]
                ]
            }
        });
    }
});

