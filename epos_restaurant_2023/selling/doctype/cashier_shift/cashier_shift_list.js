frappe.listview_settings['Cashier Shift'] = {
    add_fields: ['is_closed'],
    hide_name_column: false, // hide the last column which shows the `name`
    // set this to true to apply indicator function on draft documents too
    has_indicator_for_draft: false,

    get_indicator(doc) {
        if(doc.is_closed==0){
            return [__("Opened"), "orange"];
        }else {
                return [__("Closed"), "green"];
            
        }
      
        
    },
}
