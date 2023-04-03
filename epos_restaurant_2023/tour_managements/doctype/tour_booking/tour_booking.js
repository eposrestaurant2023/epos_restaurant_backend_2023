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
        frm.set_query("document_type", "guides_and_drivers", function(doc, cdt, cdn) {
            return {
                filters: {
                    name: ["in",["Tour Guides","Drivers"]]
                    
                }
            };
        });
    },    
});



function set_indicator(frm){
    if(frm.doc.__islocal)
			return;
  
    if (frm.doc.price > 0 ) frm.dashboard.add_indicator(__("Tour Package Price: {0}",[format_currency(frm.doc.price)]) ,"blue");

    if (frm.doc.total_additional_charge > 0 )  frm.dashboard.add_indicator(__("Additional Charge: {0}",[format_currency(frm.doc.total_additional_charge)]) ,"blue");
    if (frm.doc.total_additional_charge>0  &&  frm.doc.price > 0 )  frm.dashboard.add_indicator(__("Total Charge: {0}",[format_currency(frm.doc.total_additional_charge + frm.doc.price)]) ,"blue");
    if (frm.doc.total_expense > 0 )  frm.dashboard.add_indicator(__("Additional Expense: {0}",[format_currency(frm.doc.total_expense)]) ,"red");
    if (frm.doc.total_paid > 0 )  frm.dashboard.add_indicator(__("Total Paid: {0}",[format_currency(frm.doc.total_paid)]) ,"green");
    if (frm.doc.balance > 0 )  frm.dashboard.add_indicator(__("Balance: {0}",[format_currency(frm.doc.balance)]) ,"red");
}

function set_query(frm,field_name, filters){ 
	 
    frm.set_query(field_name, function() {
        return {
            filters: filters
        }
    });
 
}