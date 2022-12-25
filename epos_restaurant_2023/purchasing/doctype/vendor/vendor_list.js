frappe.listview_settings['Vendor'] = {
    add_fields: ["disabled"],
	filters: [["disabled", "=", "0"]],
	get_indicator: function(doc) {
		if (doc.disabled) {
			return [__("Disabled"), "grey", "disabled,=,Yes"];
		} else{
            return [__("Enabled"), "green", "disabled,=,No"];
        }
	},
};
