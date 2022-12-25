// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

// frappe.ui.form.on("Sale Payment", {
// 	refresh(frm) {

// 	},
// });
frappe.ui.form.on("Sale Payment", {
    setup(frm) {
        frm.set_query("sale", function() {
            return {
                filters: [
                    ["Sale","docstatus", "=", 1]
                ]
            }
        });
    },
    input_amount(frm){
        frm.set_value('payment_amount', frm.doc.input_amount/frm.doc.exchange_rate);
    }
});