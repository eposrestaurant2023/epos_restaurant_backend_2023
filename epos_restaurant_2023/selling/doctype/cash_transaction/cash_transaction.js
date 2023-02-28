// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Cash Transaction", {
	setup(frm) {
        frm.set_query('payment_type', () => {
            return {
                filters: {
                    payment_type_group: ['=', 'Cash']
                }
            }
        }) 
    },
    input_amount(frm){ 
        update_amount(frm)
    },
    payment_type(frm){ 
        update_amount(frm)
    },
});

function update_amount(frm) {
    if(frm.doc.exchange_currency){
        let amount = frm.doc.input_amount / frm.doc.exchange_currency
        frm.set_value('amount', amount);
    }
    
}