// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Inventory Transaction", {
	refresh(frm) {
        frm.add_custom_button('Product', () => {
            frappe.set_route('product', frm.doc.product_code);
        })
	},
});
