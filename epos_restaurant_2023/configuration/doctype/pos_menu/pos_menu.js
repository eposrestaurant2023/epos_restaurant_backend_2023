// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("POS Menu", {
	setup(frm) {
        frm.set_query("parent_pos_menu", function() {
            return {
                filters: [
                    ["POS Menu","is_group", "=", true]
                ]
            }
        });
    }
});
