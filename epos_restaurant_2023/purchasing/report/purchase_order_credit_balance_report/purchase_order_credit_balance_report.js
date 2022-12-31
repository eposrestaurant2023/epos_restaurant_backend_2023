// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */

frappe.query_reports["Purchase Order Credit Balance Report"] = {
	"filters": [
		{
			fieldname: "business_branch",
			label: __("Business Branch"),
			fieldtype: "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Business Branch', txt);
			}
			 
		},
		{
			fieldname: "stock_location",
			label: __("Stock Location"),
			fieldtype: "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Stock Location', txt);
			}
			 
		},
		{
			"fieldname":"end_date",
			"label": __("End Date"),
			"fieldtype": "Date",
		 
			
		},
	 
		
		{
			"fieldname": "vendor_group",
			"label": __("Vendor Group"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Vendor Group', txt);
			}
		},
		{
			"fieldname": "vendor",
			"label": __("Vendor"),
			"fieldtype": "Link",
			"options":"Vendor",
			
		},
		{
			"fieldname": "show_po_transaction",
			"label": __("Show PO Transaction"),
			"fieldtype": "Check",
			"default":0,
			
		},
		{
			"fieldname": "show_summary",
			"label": __("Show Summary"),
			"fieldtype": "Check",
			"default":1,
			
		},
		 

	],
	"formatter": function(value, row, column, data, default_formatter) {
	
		value = default_formatter(value, row, column, data);
		 
		if (data && data.indent==0 && frappe.query_report.get_filter_value('show_po_transaction')==1) {
			value = $(`<span>${value}</span>`);

			var $value = $(value).css("font-weight", "bold");
			

			value = $value.wrap("<p></p>").parent().html();
		}
		
		return value;
	},
};
