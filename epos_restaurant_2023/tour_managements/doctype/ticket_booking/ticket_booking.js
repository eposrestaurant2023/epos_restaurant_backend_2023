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
    },
    total_discount(frm){
        updateDiscount(frm);
    },
    discount_type(frm){
        updateDiscount(frm); 
    },
    discount(frm){
        updateDiscount(frm); 
    },
    adult(frm){
        totalPax(frm);
    },
    child(frm){
        totalPax(frm);
    },
    total_payment(frm){
        if(frm.doc.total_payment && frm.doc.total_amount){
            frm.doc.balance = (frm.doc.total_amount || 0) - (frm.doc.total_payment || 0);
            frm.refresh_field('balance')
        }
    },
    total_amount(frm){
        if(frm.doc.total_payment && frm.doc.total_amount){
            frm.doc.balance = (frm.doc.total_amount || 0) - (frm.doc.total_payment || 0);
            frm.refresh_field('balance')
        }
    },
});

frappe.ui.form.on('Tour Booking Payments', {
    payment_amount(frm,cdt, cdn) {
       
       const payments = frm.doc.payments
       frm.set_value('total_payment', payments.reduce((n, d) => n + d.payment_amount,0));
       frm.refresh_field('total_payment'); 
    },
    payments_remove:function(frm){
        frm.set_value('total_payment', frm.doc.payments.reduce((n, d) => n + (d.payment_amount || 0),0));
        frm.refresh_field('total_payment'); 
    },
});
frappe.ui.form.on('Ticket Booking Item', {
    quantity:function (frm,cdt, cdn) {
     let doc = locals[cdt][cdn];
     if (doc.quantity && doc.price){
         doc.total_amount = (doc.quantity || 0) * (doc.price || 0)
         refresh_field('ticket_booking_item');
     }
     const ticket_booking_item = frm.doc.ticket_booking_item;
     frm.set_value('total_amount', ticket_booking_item.reduce((n,d) => n + d.total_amount, 0));
     frm.refresh_field('total_amount');
   },
   price:function (frm,cdt, cdn) {
     let doc = locals[cdt][cdn];
     if (doc.quantity && doc.price){
         doc.total_amount = (doc.quantity || 0) * (doc.price || 0)
         refresh_field('ticket_booking_item');
     }
     const ticket_booking_item = frm.doc.ticket_booking_item;
     frm.set_value('total_amount', ticket_booking_item.reduce((n,d) => n + d.total_amount, 0));
     frm.refresh_field('total_amount');
   },
 });

function updateDiscount(frm){
    if (frm.doc.discount_type=="Percent" && frm.doc.discount){
        frm.doc.total_discount = (frm.doc.total_amount || 0) * (frm.doc.discount || 0)/100;
        frm.doc.balance = (frm.doc.total_amount || 0) - (frm.doc.total_payment || 0) - (frm.doc.total_discount || 0);  
    }
    else{
        frm.doc.total_discount = (frm.doc.discount||0)
        frm.doc.balance = (frm.doc.total_amount || 0) - (frm.doc.total_payment || 0) - (frm.doc.total_discount || 0);
    }
    frm.refresh_field('total_discount')
    frm.refresh_field('balance')
}
function totalPax(frm){
    if(frm.doc.adult && frm.doc.child){
        frm.doc.total_pax = (frm.doc.adult || 0) + (frm.doc.child || 0)
        frm.refresh_field('total_pax');
    }
}