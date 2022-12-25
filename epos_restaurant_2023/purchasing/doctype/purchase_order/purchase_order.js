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
	stock_location(frm){
		update_stock(frm)
    },
});
frappe.ui.form.on('Purchase Order Products', {
	product_code(frm,cdt, cdn) {
		let doc=   locals[cdt][cdn];
        frm.set_query("unit","purchase_order_products", function() {
            return {
                filters: [
                    ["Unit Of Measurement","unit_category", "=", doc.unit_category]
                ]
            }
        });
        product_code(frm,cdt,cdn);
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
    }
})

function update_purchase_order_products_amount(frm,cdt, cdn)  {
    let doc = locals[cdt][cdn];
		if(doc.quantity < 1) doc.quantity = 1;
		doc.amount=doc.quantity * doc.cost;
	    frm.refresh_field('purchase_order_products');
		updateSumTotal(frm);
}

function updateSumTotal(frm) {
    
    let sum_total = 0;
	let total_qty = 0;
  
    $.each(frm.doc.purchase_order_products, function(i, d) {
        sum_total += flt(d.amount);
		total_qty +=flt(d.quantity);
		 
    });
	
    
    frm.set_value('sub_total', sum_total);
    frm.set_value('total_quantity', total_qty);
   
	frm.refresh_field("sub_total");
	frm.refresh_field("total_quantity");
}

function check_row_exist(frm, barcode){
	
	var row = frm.fields_dict["purchase_order_products"].grid.grid_rows.filter(function(d)
			{ return (d.doc.product_code==undefined?"":d.doc.product_code).toLowerCase() ===barcode.toLowerCase() })[0];
	return row;
}
function update_product_quantity(frm, row){
	if(row!=undefined){
		row.doc.quantity = row.doc.quantity + 1;
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
		doc.amount = doc.quantity * doc.cost;
		doc.unit = p.unit;
		doc.product_name = p.product_name_en;
		product_by_scan(frm,doc)
	}
}

function product_by_scan(frm,doc){
	get_product_cost(frm,doc).then((v)=>{
		doc.cost = v;
		doc.amount=doc.quantity * doc.cost;
		frm.refresh_field('purchase_order_products');
		updateSumTotal(frm);
	});
}

function product_code(frm,cdt,cdn){
	let doc = locals[cdt][cdn]
	get_product_cost(frm,doc).then((v)=>{
		doc.cost = v;
		
		frm.refresh_field('purchase_order_products');
		update_purchase_order_products_amount(frm,cdt,cdn)
	});
}

let get_product_cost = function (frm,doc) {
	return new Promise(function(resolve, reject) {
		frappe.call({
			method: "epos_restaurant_2023.inventory.doctype.product.product.get_product_cost_by_stock",
			args: {
				//barcode:d.doc.product_code,
				stock_location:frm.doc.stock_location,
				product_code: doc.product_code
				// product: d.doc.unit,
				// unit: d.doc.unit
			},
			callback: function(r){
				resolve(r.message.cost)
			},
			error: function(r) {
				reject("error")
			},
		});
	});
}
async function update_stock(frm){
	let rows = frm.fields_dict["purchase_order_products"].grid.grid_rows;
	 $.each(rows, async function(i, d)  {
		
		if(d.doc.product_code!=undefined)
		{
			get_product_cost(frm,d.doc).then((v)=>{
				d.doc.price = v;
				d.doc.amount = d.doc.price * d.doc.quantity;
				frm.refresh_field('purchase_order_products');
				updateSumTotal(frm);
			});
		}
	});
	

}