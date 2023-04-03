// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

// frappe.ui.form.on("Tour Booking", {
// 	refresh(frm) {

// 	},
// });

// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Tour Booking", {
    refresh(frm){
     
        set_indicator(frm);

        
    },
    setup(frm){
       
    },    
});



function set_indicator(frm){
    if(frm.doc.__islocal)
			return;
  
    frm.dashboard.add_indicator(__("Tour Package Price: {0}",[format_currency(frm.doc.price)]) ,"blue");
    frm.dashboard.add_indicator(__("Additional Charge: {0}",[format_currency(frm.doc.total_additional_charge)]) ,"blue");
    frm.dashboard.add_indicator(__("Total Charge: {0}",[format_currency(frm.doc.total_additional_charge + frm.doc.price)]) ,"blue");
    frm.dashboard.add_indicator(__("Additional Expense: {0}",[format_currency(frm.doc.total_expense)]) ,"red");
    frm.dashboard.add_indicator(__("Total Paid: {0}",[format_currency(frm.doc.total_paid)]) ,"green");
    frm.dashboard.add_indicator(__("Balance: {0}",[format_currency(frm.doc.balance)]) ,"red");
}
