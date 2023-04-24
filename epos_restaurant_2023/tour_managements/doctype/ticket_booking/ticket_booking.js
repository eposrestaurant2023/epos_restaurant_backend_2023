// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Ticket Booking", {
	refresh(frm) {
        if(!frm.doc.__islocal){ 
 
         frm.dashboard.add_indicator(__("Total Amount: {0}",[format_currency(frm.doc.total_amount)]) ,"blue");
         frm.dashboard.add_indicator(__("Total Paid: {0}",[format_currency(frm.doc.total_payment)]) ,"green");
         frm.dashboard.add_indicator(__("Balance: {0}",[format_currency(frm.doc.balance)]) ,"orange");
    }
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

frappe.ui.form.on('Tour Booking Payments', {
    payment_amount(frm,cdt, cdn) {
       
       const payments = frm.doc.payments
       frm.set_value('total_payment', payments.reduce((n, d) => n + d.payment_amount,0));
       frm.refresh_field('total_payment'); 
       
       frm.set_value('balance', frm.doc.total_amount - payments.reduce((n, d) => n + d.payment_amount,0));
       frm.refresh_field('balance'); 
     

    }


})
 