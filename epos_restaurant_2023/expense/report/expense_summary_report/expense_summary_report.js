// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */

frappe.query_reports["Expense Summary Report"] = {
	onload: function() {
		frappe.query_report.toggle_filter_display('from_fiscal_year', true);
		frappe.query_report.toggle_filter_display('start_date', true  );
		frappe.query_report.toggle_filter_display('end_date', true );

		if(frappe.query_report.get_filter_value('filter_based_on')=="Fiscal Year"){
			frappe.query_report.toggle_filter_display('from_fiscal_year', false);
		}
		if(frappe.query_report.get_filter_value('filter_based_on')=="Date Range"){
			frappe.query_report.toggle_filter_display('start_date', false  );
			frappe.query_report.toggle_filter_display('end_date', false );
		}
	},
	"filters": [
		{
			fieldname: "business_branch",
			label: "Business Branch",
			fieldtype: "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Business Branch', txt);
			}
			 
		},
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
		{
			"fieldname": "expense_category",
			"label": __("Expense Category"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Expense Category', txt);
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
			"fieldname": "parent_row_group",
			"label": __("Parent Group By"),
			"fieldtype": "Select",
			"options": "\nExpense Category\nExpense Code\nBusiness Branch\nEmployee\nVendor\nVendor Group\nDate\n\Month\nYear\nExpense Transaction",
			
		},
		{
			"fieldname": "row_group",
			"label": __("Row Group By"),
			"fieldtype": "Select",
			"options": "Expense Category\nExpense Code\nBusiness Branch\nEmployee\nVendor\nVendor Group\nDate\n\Month\nYear\nExpense Transaction",
			"default":"Expense Category"
		},
		{
			"fieldname": "column_group",
			"label": __("Column Group By"),
			"fieldtype": "Select",
			"options": "None\nDaily\nWeekly\nMonthly\nQuarterly\nHalf Yearly\nYearly",
			"default":"None"
		},
		{
			"fieldname": "hide_columns",
			"label": __("Hide Columns"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				return [
					{"value":"Quantity","description":"Quantity"}
				]
			},
			"default":"All"
		},
		{
			"fieldname": "chart_type",
			"label": __("Chart Type"),
			"fieldtype": "Select",
			"options": "None\nbar\nline\npie",
			"default":"bar"
		},

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
