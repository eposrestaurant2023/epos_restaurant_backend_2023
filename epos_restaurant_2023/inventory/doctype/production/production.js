// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Production", {
    product(frm,cdt, cdn) {
        const row = locals[cdt][cdn];
        frappe.model.with_doc("Product", row.product, function() {
            var doc = frappe.model.get_doc("Product", row.product);
            frm.doc.produce_products = []
            frm.refresh_field('produce_products')
            doc.produce_products.forEach(r => {
                frm.add_child('produce_products', {
                    product_code: r.product_code,
                    product_name: r.product_name,
                    product_category: r.product_category,
                    product_group: r.product_group,
                    base_unit: r.unit,
                    unit: r.unit,
                    is_inventory_product: r.is_inventory_product
                });
            });
            frm.refresh_field('produce_products'); 
        });
    },
});

function update_stock_take_product_amount(frm,cdt, cdn)  {
    let doc = locals[cdt][cdn];
		if(doc.quantity <= 0) doc.quantity = 1;
		doc.amount=doc.quantity * doc.cost;
	    frm.refresh_field('produce_products');
		updateSumTotal(frm);
}

function updateSumTotal(frm) {
    
    let sum_total = 0;
	let total_qty = 0;
  
    $.each(frm.doc.produce_products, function(i, d) {
        sum_total += flt(d.amount);
		total_qty +=flt(d.quantity);
		 
    });
	
    
    frm.set_value('total_amount', sum_total);
    frm.set_value('total_quantity', total_qty);
   
	frm.refresh_field("total_amount");
	frm.refresh_field("total_quantity");
}