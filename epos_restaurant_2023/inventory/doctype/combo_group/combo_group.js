// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Combo Group", {
	refresh(frm) {
        frm.set_query("product","products", function() {
            return {
                filters: [
                    ["Product","is_combo_menu", "=", 0],
                    ["Product","disabled", "=", 0],
                ]
            }
        });
	},
});
