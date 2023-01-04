// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */

frappe.query_reports["Daily Expense Payment Transaction"] = {
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
			"fieldname":"start_date",
			"label": __("Start Date"),
			"fieldtype": "Date",
			default:frappe.datetime.get_today(),
			 
			"reqd": 1
		},
		{
			"fieldname":"end_date",
			"label": __("End Date"),
			"fieldtype": "Date",
			default:frappe.datetime.get_today(),
			 
			"reqd": 1
		},
		{
			"fieldname": "payment_type_group",
			"label": __("Payment Type Group"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Payment Type Group', txt);
			}
		},
		{
			"fieldname": "payment_type",
			"label": __("Payment Type"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Payment Type', txt);
			}
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
			"fieldname": "expense",
			"label": __("Expense"),
			"fieldtype": "Link",
			"options":"Expense",
			
		}

	],
	"formatter": function(value, row, column, data, default_formatter) {
	
		value = default_formatter(value, row, column, data);

		if (data && data.is_group==1) {
			value = $(`<span>${value}</span>`);

			var $value = $(value).css("font-weight", "bold");
			

			value = $value.wrap("<p></p>").parent().html();
		}
		
		return value;
	},
	
};

 