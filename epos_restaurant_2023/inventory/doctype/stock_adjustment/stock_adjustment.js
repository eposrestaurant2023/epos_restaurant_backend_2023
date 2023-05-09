// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Stock Adjustment", {
	refresh(frm) {
		frm.set_query("product_code","products", function() {
            return {
                filters: [
                    ["Product","is_inventory_product", "=", 1]
                ]
            }
        });
	},

    stock_location(frm){ 
        if(frm.doc.products.length > 0){
            $.each(frm.doc.products, function(i, d) {
                if(d.product_code)
                    get_location_product(frm,d)
            });
        }
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
									add_product_child(frm,r.message);
								}
								frm.refresh_field("stock_take_products");
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
});
frappe.ui.form.on('Stock Adjustment Product', {
	product_code(frm,cdt, cdn) {
        let doc = locals[cdt][cdn];
        get_location_product(frm, doc)
        
	},
    quantity(frm,cdt, cdn){
        update_product_amount(frm,cdt, cdn);
    },
    cost(frm,cdt, cdn){
        update_product_amount(frm,cdt, cdn);
    }
})

function get_location_product(frm, doc){

    frappe.call({
        method: "epos_restaurant_2023.inventory.doctype.stock_location_product.stock_location_product.get_stock_location_product",
        args: {
            stock_location:frm.doc.stock_location,
            product_code:doc.product_code,
        },
        callback: function(r){
            if(r.message!=undefined){
                doc.current_quantity = doc.quantity = r.message.quantity;
                doc.current_cost = doc.cost = r.message.cost;
                doc.difference_quantity = doc.quantity - doc.current_quantity;
                doc.difference_cost = doc.cost - doc.current_cost;
                doc.total_cost=doc.quantity * doc.cost;           
                doc.total_current_cost=doc.current_quantity * doc.current_cost;           
                updateSumTotal(frm)
            }
            else { 
                frappe.show_alert({message:"Cannot get data from stock location product", indicator:"orange"});
            }
        },
        error: function(r) {
            frappe.show_alert("load data fail");
        },
    });	
}

function update_product_amount(frm,cdt, cdn)  {
    let doc = locals[cdt][cdn];
    doc.difference_quantity = doc.quantity - doc.current_quantity;
    doc.difference_cost = doc.cost - doc.current_cost;
    doc.total_cost=doc.quantity * doc.cost;
    updateSumTotal(frm);
}

function updateSumTotal(frm) {
    
    let sum_total = 0;
	let total_qty = 0;
    let current_sum_total = 0;
	let current_total_qty = 0;
  
    $.each(frm.doc.products, function(i, d) {
        sum_total += flt(d.total_cost);
		total_qty +=flt(d.quantity);
        current_sum_total += flt(d.total_current_cost);
		current_total_qty +=flt(d.current_quantity);
    });
    let difference_quantity = total_qty - current_total_qty; 
    let difference_cost = sum_total - current_sum_total; 
    frm.set_value('total_current_cost', current_sum_total);
    frm.set_value('total_current_quantity', current_total_qty);
    frm.set_value('total_cost', sum_total);
    frm.set_value('total_quantity', total_qty);
    frm.set_value('difference_quantity', difference_quantity);
    frm.set_value('difference_cost', difference_cost);
    frm.refresh_field('products');
	frm.refresh_field("total_current_cost");
	frm.refresh_field("total_current_quantity");
    frm.refresh_field("total_cost");
	frm.refresh_field("total_quantity");
	frm.refresh_field("difference_quantity");
	frm.refresh_field("difference_cost");
    
}
function check_row_exist(frm, barcode){
	
	var row = frm.fields_dict["products"].grid.grid_rows.filter(function(d)
			{ return (d.doc.product_code==undefined?"":d.doc.product_code).toLowerCase() ===barcode.toLowerCase() })[0];
	return row;
}
function update_product_quantity(frm, row){
	if(row!=undefined){
		row.doc.quantity = row.doc.quantity + 1;
		row.doc.total_cost = row.doc.quantity * row.doc.cost;
		frm.refresh_field('products');
		updateSumTotal(frm);
	}
}
function add_product_child(frm,p){
	let all_rows = frm.fields_dict["products"].grid.grid_rows.filter(function(d) { return  d.doc.product_code==undefined});
	let row =undefined;
	
	if (all_rows.length>0){
		if ( all_rows[0].doc.product_code == undefined){ 
			row = all_rows[0];
		}
	}
	let doc = undefined;

	if(row==undefined){
		 doc = frm.add_child("products");
	}else {
		doc = row.doc;
	}
	if(doc!=undefined){
        doc.product_code = p.product_code;
		doc.product_name = p.product_name_en;
		doc.unit = p.unit;
		get_location_product(frm,doc)
	} 
}