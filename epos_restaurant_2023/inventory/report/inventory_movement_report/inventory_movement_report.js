// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */

frappe.query_reports["Inventory Movement Report"] = {
	onload: function() {
		switch(frappe.query_report.get_filter_value('filter_based_on')){
			case 'This Month':
				frappe.query_report.toggle_filter_display('from_fiscal_year', true);
				frappe.query_report.toggle_filter_display('start_date', true  );
				frappe.query_report.toggle_filter_display('end_date', true );
				break;
			case 'Fiscal Year':
				frappe.query_report.toggle_filter_display('start_date', true  );
				frappe.query_report.toggle_filter_display('end_date', true );
				break
			default:
				frappe.query_report.toggle_filter_display('from_fiscal_year', true);
				break;
		} 
	},
	"filters": [
		// Business Branch
		{
			fieldname: "business_branch",
			label: "Business Branch",
			fieldtype: "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Business Branch', txt);
			}
			 
		},
		// Date and Date Rang
		{
			"fieldname":"filter_based_on",
			"label": __("Filter Based On"),
			"fieldtype": "Select",
			"options": ["Fiscal Year","This Month", "Date Range"],
			"default": ["This Month"],
			"reqd": 1,
			on_change: function() {
				let filter_based_on = frappe.query_report.get_filter_value('filter_based_on');
				if(filter_based_on!="This Month"){ 
					frappe.query_report.toggle_filter_display('from_fiscal_year', filter_based_on === 'Date Range');
					frappe.query_report.toggle_filter_display('start_date', filter_based_on === 'Fiscal Year'  );
					frappe.query_report.toggle_filter_display('end_date', filter_based_on === 'Fiscal Year' );

				
				}else{
					frappe.query_report.toggle_filter_display('from_fiscal_year', true);
					frappe.query_report.toggle_filter_display('start_date', true  );
					frappe.query_report.toggle_filter_display('end_date', true );

				}

				frappe.query_report.refresh();
	 
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
			"fieldname":"from_fiscal_year",
			"label": __("Start Year"),
			"fieldtype": "Int",
			
			"default": (new Date()).getFullYear()
		},
		// stock location
		{
			"fieldname": "stock_location",
			"label": __("Stock Location"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				group = frappe.query_report.get_filter_value("business_branch");
				if(group==""){
					return frappe.db.get_link_options('Stock Location', txt);
				}
				else {
					return frappe.db.get_link_options('Stock Location', txt,filters={
						"business_branch":["in",group]
					});
				}
			}
		},


		// Product Group
		{
			"fieldname": "product_group",
			"label": __("Product Group"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Product Category', txt,{"is_group":1});
			}
		},

		// Product Category
		{
			"fieldname": "product_category",
			"label": __("Product Category"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				group = frappe.query_report.get_filter_value("product_group");
				if(group==""){
					return frappe.db.get_link_options('Product Category', txt,filters={
						is_group:0
					});
				}
				else {
					return frappe.db.get_link_options('Product Category', txt,filters={
						is_group:0,
						"parent_product_category":["in",group]
					});
				}
			}
		},
		// {
		// 	"fieldname": "parent_row_group",
		// 	"label": __("Parent Group By"),
		// 	"fieldtype": "Select",
		// 	"options": "\nBusiness Branch\nStock Location\nProduct Group\nProduct Category",
		// },
		{
			"fieldname": "row_group",
			"label": __("Row Group By"),
			"fieldtype": "Select",
			"options": "Product\nProduct Category\nProduct Group\nBusiness Branch\nStock Location",
			"default":"Product"
		},
		// {
		// 	"fieldname": "chart_type",
		// 	"label": __("Chart Type"),
		// 	"fieldtype": "Select",
		// 	"options": "None\nbar\nline\npie",
		// 	"default":"bar"
		// }

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

 