// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Happy Hours Promotion", {

	refresh(frm) {
        frm.set_query("product_code","products", function() {
            return {
                filters: [
                    ["Product","allow_discount", "=", 1]
                ]
            }
        });
	},
});
