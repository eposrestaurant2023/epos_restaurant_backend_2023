// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Product", {
   
    refresh(frm){
        frm.set_query("product","product_recipe", function() {
            return {
                filters: [
                    ["Product","is_recipe", "=", 1]
                ]
            }
        });
        frm.set_query("product","product_combo_menus", function() {
            return {
                filters: [
                    ["Product","is_combo_menu", "=", 0]
                ]
            }
        });

        print_barcode_button(frm);

        set_product_indicator(frm);

            frm.set_df_property('naming_series', 'reqd', 0)

    },
    setup(frm){
        frm.set_query('product_category', () => {
            return {
                filters: {
                    is_group: 0
                }
            }
        });
    },
    generate_variant(frm){
        frm.call({
            method: 'generate_variant',
            doc:frm.doc,
            callback:function(r){
                if(r.message){
                    frm.set_value('product_variants',r.message);
                }

            },
            async: true,
        });
    }  ,
});

function print_barcode_button(frm) {
    frappe.db.get_list('Print Barcode', {
        fields: ['title','barcode_url'],
    }).then(res => {
        $.each(res, function(i, d) {
            frm.add_custom_button(__(d.title), function() {
                let msg = frappe.msgprint('<iframe src="' +  d.barcode_url + '&rs:Command=Render&rc:Zoom=Page%20Width&barcode='+ frm.doc.name +'&price='+ frm.doc.price +'&product_name_kh=' + encodeURIComponent(frm.doc.product_name_kh) + '&product_name=' + encodeURIComponent(frm.doc.product_name_en) + '&cost=' + frm.doc.cost + '" frameBorder="0" width="100%" height="650" title="Print Barcode"></iframe>', 'Print Barcode')
                msg.$wrapper.find('.modal-dialog').css("max-width", "80%");

            }, __("Print Barcode"));
        });
    });

}

function set_product_indicator(frm){
    if(frm.doc.__islocal)
			return;

    frm.call({
        method: 'get_product_summary_information',
        doc:frm.doc,
        callback:function(r){

            if(r.message){
                let total_total_quantity = 0;
                $.each(r.message.stock_information, function(i, d) {
                    let indicator = "blue";
                    if(d.quantity<0){
                        indicator = "red";
                    }
                    total_total_quantity = total_total_quantity + d.quantity;
                    frm.dashboard.add_indicator(d.stock_location + ": " + d.quantity.toFixed(r.message.precision) + " " +d.unit , indicator);
                });
                if (r.message.stock_information.length>1){
                    frm.dashboard.add_indicator(__("Total Quantity: {0}",[total_total_quantity.toFixed(r.message.precision)]) ,total_total_quantity>0?"blue":"red");
                }

                if (r.message.total_annual_sale>0){
                    frm.dashboard.add_indicator(__("Annual Sale: {0}",[format_currency(r.message.total_annual_sale)]) ,"green");
                }

            }

        },
        async: true,
    });
}