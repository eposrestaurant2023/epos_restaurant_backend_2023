// Copyright (c) 2022, Frappe Technologies and contributors
// For license information, please see license.txt
/* eslint-disable */
frappe.query_reports["Stock Value Report"] = {
	"filters": [
		{
			"fieldname": "stock_location",
			"label": __("Stock Location"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Stock Location', txt);
			}
		}

	],
	 
	
};

 