// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Sale Quotation", {
	// refresh(frm){
        
	// },
	setup(frm) {
		set_query(frm,"product_code",{  allow_sale: 1});
		set_query(frm,"stock_location",[["Stock Location","business_branch","=",frm.doc.business_branch]]);
		set_query(frm,"outlet",[["Outlet","business_branch","=",frm.doc.business_branch]]);

    },
	business_branch(frm){
		set_query(frm,"stock_location",[["Stock Location","business_branch","=",frm.doc.business_branch]]);
		set_query(frm,"outlet",[["Outlet","business_branch","=",frm.doc.business_branch]]);
		if (frm.doc.business_branch){ 
			update_product_price(frm);
		}
	},
	price_rule(frm){
		if (frm.doc.price_rule){ 
			update_product_price(frm);
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
						price_rule: frm.doc.price_rule,
						allow_sale:1,
						
						
					},
					callback: function(r){
						 
						if(r.message!=undefined){
							if(r.message.status ==0){ 
								let row_exist = check_row_exist(frm,barcode,r.message.unit);
				
								if(row_exist!=undefined && frm.doc.append_quantity == 1){
									row_exist.doc.quantity = row_exist.doc.quantity + 1;
									update_sale_product_amount(frm,row_exist.doc);
								 
								}else {
									add_product_to_sale_product(frm,r.message);
								}
								frm.refresh_field("products");
						
								
							}else {
								frappe.show_alert({message:r.message.message, indicator:"orange"});
								
							}

						}
						else {
							frappe.throw(_("Load data fail."))
						}
					},
					error: function(r) {
						frappe.throw(_("Load data fail."))
					},
				});	
					
					
		}
		frm.doc.scan_barcode = "";
		frm.refresh_field('scan_barcode'); 
	
		 
	},
	discount_type(frm){
		update_sale_discount_to_sale_product(self);
		frm.refresh_field('products');
		updateSumTotal(frm);
	},
	discount(frm){
		if(frm.doc.discount_type=="Percent" && frm.doc.discount>100){ 
			
			frappe.throw(__("Discount percent cannot greater than 100%"));
		}
		update_sale_discount_to_sale_product(frm);
		frm.refresh_field('products')
		updateSumTotal(frm);
		
	},
	tax_rule(frm){
		if(frm.doc.tax_rule){ 
			frappe.model.with_doc('Tax Rule', frm.doc.tax_rule, function () {
				let tax_rule = frappe.model.get_doc('Tax Rule', frm.doc.tax_rule);
				$.each(frm.doc.products,  function(i, d)  {
					if(d.product_tax_rule==undefined){ 
						set_product_tax(d,tax_rule);
						update_sale_product_amount(frm,d);
					}
				});

				frm.refresh_field('products');
				
				updateSumTotal(frm);
			});
		}else {
		 
			$.each(frm.doc.products,  function(i, d)  {
				if(d.product_tax_rule==undefined){ 
					set_product_tax(d,null);
					update_sale_product_amount(frm,d);
				}
			});

			frm.refresh_field('products');
			
			updateSumTotal(frm);
		}
		
	},
	customer(frm){
		frappe.model.with_doc('Customer', frm.doc.customer, function () {
			let customer = frappe.model.get_doc('Customer', frm.doc.customer);
			 
				frm.set_value('discount_type', "Percent");
				frm.set_value('discount', customer.default_discount);
				update_sale_discount_to_sale_product(frm);
				updateSumTotal(frm);
			 
        });
	}
	
});

frappe.ui.form.on('Sale Quotation Product', {
	products_remove: function(frm) {
		updateSumTotal(frm);
    },
	product_code(frm,cdt, cdn) {
		let doc=   locals[cdt][cdn];
		product_code(frm,doc);
	},
	price(frm,cdt, cdn) {
	
		const row = locals[cdt][cdn];
		if (row.allow_change_price==0 && row.price != row.base_price){
			frappe.msgprint(__("This is not allow to change price"));
			row.price = row.base_price;
		}
		update_sale_product_amount(frm,row);
		
	},
	quantity(frm,cdt, cdn) {
		let row = locals[cdt][cdn];
		update_sale_product_amount(frm,row);
	},

	unit(frm,cdt, cdn) {
			let row = locals[cdt][cdn];
			if(row.product_code){ 
				
				get_product_price(frm,row).then((v)=>{
					row.price = v;
					update_sale_product_amount(frm,row)
				});
			}
	},
	discount_type(frm,cdt, cdn) {
		let row = locals[cdt][cdn];
		update_sale_product_amount(frm,row);
	},
	discount(frm,cdt, cdn) {
		let row = locals[cdt][cdn];
		update_sale_product_amount(frm,row);
		
	},

})

function add_product_to_sale_product(frm,p){
	let all_rows = frm.fields_dict["products"].grid.grid_rows.filter(function(d)
	{ return  d.doc.product_code==undefined});
	
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
		doc.base_price = p.price,
		
		doc.price = p.price;
		doc.quantity = 1;
		doc.product_name_kh = p.product_name_kh;
		doc.unit = p.unit;
		doc.allow_change_price = p.allow_change_price;
		doc.allow_free = p.allow_free ;
		doc.allow_discount = p.allow_discount;
		doc.product_tax_rule = p.tax_rule;
		doc.tax_rule = p.tax_rule;
		if(p.tax_rule_doc){
			set_product_tax(doc,p.tax_rule_doc);
		}
		update_sale_product_amount(frm,doc);
	}
	
}

function check_row_exist(frm, barcode,unit){
	
	var row = frm.fields_dict["products"].grid.grid_rows.filter(function(d)
			{ 
				return (
					(d.doc.product_code==undefined?"":d.doc.product_code).toLowerCase() ===barcode.toLowerCase()  && 
					d.doc.unit ===unit)
			})[0];
	return row;
}

function set_query(frm,field_name, filters){ 
	 
		frm.set_query(field_name, function() {
			return {
				filters: filters
			}
		});
	 
}

async function update_product_price(frm){

	let rows = frm.fields_dict["products"].grid.grid_rows;
	const promises = [];
	 $.each(rows, async function(i, d)  {
	 
	
		if(d.doc.product_code!=undefined)
		{ 
			const promise = frappe.call({
				method: "epos_restaurant_2023.inventory.doctype.product.product.get_product_price",
				
				args: {
					barcode:d.doc.product_code,
					business_branch:frm.doc.business_branch,
					price_rule: frm.doc.price_rule,
					unit:d.doc.unit,
					portion:d.portion
				},
				callback: function(r){
					d.doc.price = r.message.price ;
					d.doc.base_price = r.message.price ;

					d.doc.sub_total = d.doc.price * d.doc.quantity;
					d.doc.amount = d.doc.sub_total - d.doc.discount_amount;
				},
				error: function(r) {
					frappe.throw(_("Load data fail."))
				},
			});	
			promises.push(promise);
		}
	});

	Promise.all(promises).then(()=>{
		updateSumTotal(frm);
		frm.refresh();

	})

}


function updateSumTotal(frm) {
		const products =  frm.doc.products;
		if(products==undefined){
			return false;
		}
 
		frm.set_value('sub_total', products.reduce((n, d) => n + d.sub_total,0));
		frm.set_value('total_quantity', products.reduce((n, d) => n + d.quantity,0));
		frm.set_value('product_discount', products.reduce((n, d) => n + d.discount_amount,0));
		frm.set_value('sale_discountable_amount', products.reduce((n, d) => n + (d.allow_discount==0 || d.discount_amount>0?0:d.sub_total),0));
		
		let discount = 0;
		if (frm.doc.discount_type=="Percent"){
			discount = frm.doc.sale_discountable_amount * frm.doc.discount /100;
		}else {
			discount = frm.doc.discount;
		}
		//UPDATE TAX FROM SALE PRODUCT
		frm.set_value('taxable_amount_1', products.reduce((n, d) => n + d.taxable_amount_1,0));
		frm.set_value('taxable_amount_2', products.reduce((n, d) => n + d.taxable_amount_2,0));
		frm.set_value('taxable_amount_3', products.reduce((n, d) => n + d.taxable_amount_3,0));
		frm.set_value('tax_1_amount', products.reduce((n, d) => n + d.tax_1_amount,0));
		frm.set_value('tax_2_amount', products.reduce((n, d) => n + d.tax_2_amount,0));
		frm.set_value('tax_3_amount', products.reduce((n, d) => n + d.tax_3_amount,0));
		frm.set_value('total_tax', products.reduce((n, d) => n + d.total_tax,0));
		frm.set_value('sale_discount', discount);
		frm.set_value('total_discount', discount + frm.doc.product_discount);
		frm.set_value('grand_total',  (frm.doc.sub_total - frm.doc.total_discount) + frm.doc.total_tax);
		frm.refresh_field('grand_total'); 
	
		
	
	
}

function calculate_sale_product_tax(doc){
		if (doc.tax_rule){
		
			if(doc.calculate_tax_1_after_discount==true){
				doc.taxable_amount_1 =   doc.sub_total - doc.total_discount
			}else {
				doc.taxable_amount_1 = doc.sub_total ;
			}

			doc.tax_1_amount =  doc.taxable_amount_1 * doc.tax_1_rate/100;

			//tax 2
			if( doc.calculate_tax_2_after_discount==true){
				doc.taxable_amount_2 = doc.sub_total - doc.total_discount;
			}else {
				doc.taxable_amount_2 = doc.sub_total  ;
			}
			if(doc.calculate_tax_2_after_adding_tax_1==true){
				 doc.taxable_amount_2 = doc.taxable_amount_2 +  doc.tax_1_amount;
			}
			doc.tax_2_amount =  doc.taxable_amount_2 *  doc.tax_2_rate/100;

			//tax 3
			if(doc.calculate_tax_3_after_discount==true){
				doc.taxable_amount_3 = doc.sub_total - doc.total_discount ;
			}else {
				doc.taxable_amount_3 =  doc.sub_total;
			}
			if(doc.calculate_tax_3_after_adding_tax_1==true){
				doc.taxable_amount_3 =   doc.taxable_amount_3 +  doc.tax_1_amount ;
			}
			if(doc.calculate_tax_3_after_adding_tax_2==true){
				doc.taxable_amount_3 = doc.taxable_amount_3 +  doc.tax_2_amount ;
			}
			doc.tax_3_amount =  doc.taxable_amount_3 * doc.tax_3_rate/100;
		 
			//total tax
			doc.total_tax = doc.tax_1_amount + doc.tax_2_amount + doc.tax_3_amount;
		}else {
			doc.taxable_amount_1 =0;
			doc.tax_1_amount=0;
			doc.taxable_amount_2 =0;
			doc.tax_2_amount=0;
			doc.taxable_amount_3 =0;
			doc.tax_3_amount=0;
			doc.total_tax =0;
		}


		//end tax calculateion
}


function product_code(frm,doc){
	if(doc.product_tax_rule){ 
		frappe.model.with_doc('Tax Rule', doc.product_tax_rule, function () {
			let tax_rule = frappe.model.get_doc('Tax Rule', doc.product_tax_rule);
			
			set_product_tax(doc,tax_rule)

			get_product_price(frm,doc).then((v)=>{
				doc.price = v;
				update_sale_product_amount(frm,doc)
				
			});
        });
	}

}

function set_product_tax(doc, tax_rule){
	if(tax_rule){ 
		doc.tax_rule = tax_rule.name;
		doc.tax_1_rate = tax_rule.tax_1_rate;
		doc.calculate_tax_1_after_discount = tax_rule.calculate_tax_1_after_discount;
		
		doc.tax_2_rate = tax_rule.tax_2_rate;
		doc.calculate_tax_2_after_discount = tax_rule.calculate_tax_2_after_discount;
		doc.calculate_tax_2_after_adding_tax_1= tax_rule.calculate_tax_2_after_adding_tax_1;
		
		doc.tax_3_rate = tax_rule.tax_3_rate;
		doc.calculate_tax_3_after_discount = tax_rule.calculate_tax_3_after_discount;
		doc.calculate_tax_3_after_adding_tax_1= tax_rule.calculate_tax_3_after_adding_tax_1;
		doc.calculate_tax_3_after_adding_tax_2= tax_rule.calculate_tax_3_after_adding_tax_2;
	}else {
		doc.tax_rule = "";
		doc.tax_1_rate = 0;
		doc.calculate_tax_1_after_discount =0;
		
		doc.tax_2_rate = 0;
		doc.calculate_tax_2_after_discount =0;
		doc.calculate_tax_2_after_adding_tax_1=0;
		
		doc.tax_3_rate = 0;
		doc.calculate_tax_3_after_discount =0;
		doc.calculate_tax_3_after_adding_tax_1=0;
		doc.calculate_tax_3_after_adding_tax_2= 0;
	}
}

function update_sale_product_amount(frm,doc){
 
	doc.sub_total = doc.price * doc.quantity;

	if(doc.discount){ 
		if (doc.discount_type=="Percent"){
			doc.discount_amount = (doc.sub_total * doc.discount/100); 
		}else {
			doc.discount_amount = doc.discount;
		}
		doc.sale_discount_percent = 0;
		doc.sale_discount_amount = 0;
	}else {
		doc.discount_amount = 0;
		//check if sale have discount then add discount to sale
		
	}
	if(doc.sale_discount_percent){
		doc.sale_discount_amount = (doc.sub_total * doc.sale_discount_percent/100); 
		 
	}
	doc.total_discount = doc.discount_amount + doc.sale_discount_amount;

	calculate_sale_product_tax(doc);
	 
	doc.amount = (doc.sub_total - doc.discount_amount) + doc.total_tax ;
	
	frm.refresh_field('products');
	updateSumTotal(frm);

}
function update_sale_discount_to_sale_product(frm){
	let products = frm.doc.products
	if(products==undefined){
		return false;
	}
	let sale_discount = frm.doc.discount
 

	if (sale_discount>0) { 
		if (frm.doc.discount_type=="Amount"){ 
			let discountable_amount = products.reduce((n, d) => n + (d.allow_discount==0 || d.discount_amount>0?0:d.sub_total),0)
			sale_discount=(sale_discount / discountable_amount ) * 100
		}
	}
	$.each(products,  function(i, d)  {
		// check if sale has discount
		if (sale_discount>0 && d.allow_discount && d.discount==0){ 
			
			d.sale_discount_percent = sale_discount  
			d.sale_discount_amount = (sale_discount/100) * d.sub_total
		}
		else { 
			d.sale_discount_percent = 0  
			d.sale_discount_amount = 0
		}
		d.total_discount = d.sale_discount_amount  + d.discount_amount ;
	
		calculate_sale_product_tax(d);
	});
}

let get_product_price = function (frm,doc) {
	
	return new Promise(function(resolve, reject) {
		
		frappe.call({
			method: "epos_restaurant_2023.inventory.doctype.product.product.get_product_price",
			args: {
				barcode:doc.product_code,
				business_branch:frm.doc.business_branch,
				price_rule: frm.doc.price_rule,
				unit:doc.unit,
				portion:doc.portion
			},
			callback: function(r){
		
				resolve(r.message.price)
			},
			error: function(r) {
				reject("error")
			},
		});	
	});
}



 