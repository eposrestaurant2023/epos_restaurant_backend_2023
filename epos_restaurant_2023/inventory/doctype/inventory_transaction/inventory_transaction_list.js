frappe.listview_settings['Inventory Transaction'] = {
    hide_name_column: true,
    get_indicator(doc) {
        if(doc.in_quantity>0){
            return [__("IN"), "green"];
        }else {
            return [__("OUT"), "red"];
        }
      
    },
}