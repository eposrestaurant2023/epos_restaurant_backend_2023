// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Purchase Order", {
    setup(frm){
        frm.set_query("product","purchase_order_products", function() {
            return {
                filters: {
                    allow_purchase: 1
                }
            }
        });
    },
    scan_barcode(frm){
		if(frm.doc.scan_barcode!=undefined){
				let barcode = frm.doc.scan_barcode;
				frappe.call({
					method: "epos_restaurant_2023.inventory.doctype.product.product.get_product",
					args: {
						barcode:frm.doc.scan_barcode,
						business_branch:frm.doc.business_branch,
					},
					callback: function(r){
						if(r.message!=undefined){
							if(r.message.status ==0){ 
								let row_exist = check_row_exist(frm,barcode);
								if(row_exist!=undefined && frm.doc.append_quantity == 1){
									update_product_quantity(frm,row_exist);
								}else {
									add_product_to_po_product(frm,r.message);
								}
								frm.refresh_field("purchase_order_products");
							}else {
								frappe.show_alert({message:r.message.message, indicator:"orange"});
							}
						}
						else {
							alert("faile")
						}
					},
					error: function(r) {
						alert("load data fail");
					},
				});	
					
					
		}
		frm.doc.scan_barcode = "";
		frm.refresh_field('scan_barcode'); 
	
		 
	},
	discount_type(frm){
		update_po_discount_to_po_product(frm);
	},
	discount(frm){
		if(frm.doc.discount_type=="Percent" && frm.doc.discount>100){ 
			
			frappe.throw(__("Discount percent cannot greater than 100%"));
		}
		update_po_discount_to_po_product(frm);
	}
});
frappe.ui.form.on('Purchase Order Products', {
	product_code(frm,cdt, cdn) {
		let doc=   locals[cdt][cdn];
    
        update_po_product_amount(frm,doc);
        frm.refresh_field('purchase_order_products');
	},
    quantity(frm,cdt, cdn) {
		update_purchase_order_products_amount(frm,cdt,cdn);
	},
    cost(frm,cdt, cdn) {
		update_purchase_order_products_amount(frm,cdt,cdn);
	},
    business_branch(frm,cdt, cdn){
        let doc = locals[cdt][cdn];
      
        frm.set_query("stock_location", function() {
            return {
                filters: [
                    ["Stock Location","business_branch", "=", doc.business_branch]
                ]
            }
        });
        
        frm.refresh_field('stock_location');
    },
	discount_type(frm, cdt, cdn){
		const row = locals[cdt][cdn];
		update_po_product_amount(frm,row);
	},
	discount(frm, cdt, cdn){
		const row = locals[cdt][cdn];
		if(frm.doc.discount_type=="Percent" && frm.doc.discount>100){ 
			frappe.throw(__("Discount percent cannot greater than 100%"));
		}
		update_po_product_amount(frm,row);
		frm.refresh_field('sale_products')		
	}
})

function update_purchase_order_products_amount(frm,cdt, cdn)  {
    let doc = locals[cdt][cdn];
	if(doc.quantity <= 0) doc.quantity = 1;
	update_po_product_amount(frm, doc)
}

function updateSumTotal(frm) {
    const products =  frm.doc.purchase_order_products; 
    if(products==undefined){
		return false;
	}

	frm.set_value('sub_total', products.reduce((n, d) => n + d.sub_total,0));
	frm.set_value('total_quantity', products.reduce((n, d) => n + d.quantity,0));
	frm.set_value('po_discountable_amount', products.reduce((n, d) => n + (d.discount_amount>0?0:d.sub_total),0));
	
	let discount = 0;
	if (frm.doc.discount_type=="Percent"){
		discount = frm.doc.po_discountable_amount * frm.doc.discount / 100;
	}else {
		discount = frm.doc.discount;
	}
    
	frm.set_value('product_discount', products.reduce((n, d) => n + d.discount_amount,0));
	frm.set_value('po_discount', discount);
	frm.set_value('total_discount', discount + frm.doc.product_discount);
	frm.set_value('grand_total',  frm.doc.sub_total - frm.doc.total_discount);
	frm.set_value('balance',  frm.doc.grand_total - frm.doc.total_paid);

	frm.refresh_field("sub_total");
	frm.refresh_field("total_quantity");
	frm.refresh_field("product_discount");
	frm.refresh_field("total_discount");
	frm.refresh_field("grand_total");
	frm.refresh_field("po_discount");
	frm.refresh_field("po_discountable_amount");
	frm.refresh_field("balance");
}

function check_row_exist(frm, barcode){
	
	var row = frm.fields_dict["purchase_order_products"].grid.grid_rows.filter(function(d)
			{ return (d.doc.product_code==undefined?"":d.doc.product_code).toLowerCase() ===barcode.toLowerCase() })[0];
	return row;
}
function update_product_quantity(frm, row){
	if(row!=undefined){
		row.doc.quantity = row.doc.quantity + 1;
		update_po_product_amount(frm, row.doc)
	}
}
function add_product_to_po_product(frm,p){
	let all_rows = frm.fields_dict["purchase_order_products"].grid.grid_rows.filter(function(d) { return  d.doc.product_code==undefined});
	let row =undefined;
	
	if (all_rows.length>0){
		if ( all_rows[0].doc.product_code == undefined){ 
			row = all_rows[0];
		}
	}
	let doc = undefined;
	if(row==undefined){
		 doc = frm.add_child("purchase_order_products");
	}else {
		doc = row.doc;
	}
	if(doc!=undefined){ 
		doc.product_code = p.product_code;
		doc.product_name = p.product_name_en;
		doc.cost = p.cost;
		doc.quantity = 1;
		doc.sub_total = doc.quantity * doc.cost;
		doc.unit = p.unit;
		doc.product_name = p.product_name_en;
		update_po_product_amount(frm,doc)
	}
}

function update_po_product_amount(frm,doc){
	doc.sub_total = doc.cost * doc.quantity;
	
	if(doc.discount){
		if (doc.discount_type=="Percent"){
			doc.discount_amount = (doc.sub_total * doc.discount/100);
			doc.po_discount_percent = doc.discount;

		}else {
			doc.discount_amount = doc.discount;
		}
	}else {
		doc.discount_amount = 0;
		//check if sale have discount then add discount to sale
	}
	if(doc.po_discount_percent){
		doc.po_discount_amount = (doc.sub_total * doc.po_discount_percent/100); 
		 
	}
	doc.total_discount = doc.discount_amount + doc.po_discount_amount;

	doc.amount = (doc.sub_total - doc.discount_amount);

	frm.refresh_field('purchase_order_products');
	updateSumTotal(frm);

}

function update_po_discount_to_po_product(frm){
	let products = frm.doc.purchase_order_products
	if(products==undefined){
		return false;
	}
	let po_discount = frm.doc.discount
	

	if (po_discount>0) { 
		if (frm.doc.discount_type=="Amount"){ 
			let discountable_amount = products.reduce((n, d) => n + (d.discount_amount>0?0:d.sub_total),0)
			po_discount=(po_discount / discountable_amount ) * 100
		}
	}
	$.each(products,  function(i, d)  {
		// check if sale has discount
		if (po_discount>0 && d.discount==0){ 
			
			d.po_discount_percent = po_discount  
			d.po_discount_amount = (po_discount/100) * d.sub_total
		}
		else { 
			d.po_discount_percent = 0  
			d.po_discount_amount = 0
		}
		d.total_discount = d.po_discount_amount  + d.discount_amount;
	});

	updateSumTotal(frm);
}