// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Expense Payment", {
	refresh(frm) {

	},
    setup(frm) {
        frm.set_query("expense", function() {
            return {
                filters: [
                    ["Expense","docstatus", "=", 1]
                ]
            }
        });
    }
});
