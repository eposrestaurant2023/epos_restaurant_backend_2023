frappe.listview_settings['Product'] = {


    onload(me) { 
        me.page.add_action_item('Assign Menu', function() {
            let d = new frappe.ui.Dialog({
                title: 'Assign Menu',
                fields: [
                    {'fieldname': 'pos_menu', 'fieldtype': 'Link', 'options': 'POS Menu'},
                ],
                primary_action_label: 'Save',
                primary_action(values) {
                    const selected =  me.get_checked_items() ;
                    const result = selected.map(item => item.name).join(',');
                    d.freeze= true;
                    frappe.call({
                        method: "epos_restaurant_2023.inventory.doctype.product.product.assign_menu",
                        args: {
                            "products": result,
                            "menu": values.pos_menu
                        },
                        callback: function(r) {
                            frappe.msgprint("Products were assigned to menu successfully")                            
                        }
                    });
                    d.freeze= false;
                    d.hide();
                },              
            })
            d.show();          
        });

        //assign Printer
        me.page.add_action_item('Assign Printer', function() {
            let d = new frappe.ui.Dialog({
                title: 'Assign Printer',
                fields: [
                    {'fieldname': 'printer', 'fieldtype': 'Link', 'options': 'Printer'},
                ],
                primary_action_label: 'Save',
                primary_action(values) {
                    const selected =  me.get_checked_items() ;
                    const result = selected.map(item => item.name).join(',');
                    d.freeze= true;
                    frappe.call({
                        method: "epos_restaurant_2023.inventory.doctype.product.product.assign_printer",
                        args: {
                            "products": result,
                            "printer": values.printer
                        },
                        callback: function(r) {
                            frappe.msgprint("Update printer to product successfully")                            
                        }
                    });
                    d.freeze= false;
                    d.hide();
                },              
            })
            d.show();           
        });

    
        //remove printer from product
        me.page.add_action_item('Remove Printer', function() {
            let d = new frappe.ui.Dialog({
                title: 'Remove Printer',
                fields: [
                    {'fieldname': 'printer', 'fieldtype': 'Link', 'options': 'Printer'},
                ],
                primary_action_label: 'Save',
                primary_action(values) {
                    const selected =  me.get_checked_items() ;
                    const result = selected.map(item => item.name).join(',');
                    d.freeze= true;
                    frappe.call({
                        method: "epos_restaurant_2023.inventory.doctype.product.product.remove_printer",
                        args: {
                            "products": result,
                            "printer": values.printer
                        },
                        callback: function(r) {
                            frappe.msgprint("Remove printer successfully")                            
                        }
                    });
                    d.freeze= false;
                    d.hide();
                },            
            })
            d.show();
        });

        me.page.add_action_item('Remove All Printers', function() {
            frappe.confirm("Are you sure you want to remove all printers from the selected products?",
                function(){
                    const selected =  me.get_checked_items() ;
                    const result = selected.map(item => item.name).join(',');                
                    frappe.call({
                        method: "epos_restaurant_2023.inventory.doctype.product.product.clear_all_printer_from_product",
                        args: {
                            "products": result,
                        },
                        callback: function(r) {
                            frappe.msgprint("Remove all printers successfully")                    
                        }
                    });
                }
            );
        });
    }
}