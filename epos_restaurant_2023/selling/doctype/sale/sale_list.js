frappe.listview_settings['Sale'] = {
    add_fields: ['balance', 'total_amount','total_paid','is_foc'],
    hide_name_column: true, // hide the last column which shows the `name`
    // set this to true to apply indicator function on draft documents too
    has_indicator_for_draft: false,

    get_indicator(doc) {
        if(doc.is_foc==1){
            return [__("FOC"), "blue"];
        }else {
            if(doc.balance==0){ 
                return [__("Paid"), "green"];
            }else if(doc.total_paid>0 && doc.balance>0){
                return [__("Partially Paid"), "orange"];
            }else if(doc.total_paid==0){
                return [__("Unpaid"), "red"];
            }
        }
      
        
    },
}
