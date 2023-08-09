// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */
frappe.provide("epos_restaurant_2023.employee_time_sheet_summary");

epos_restaurant_2023.employee_time_sheet_summary = {
  "open_report": function(data) {
    // frappe.msgprint((data))
  }
}


frappe.query_reports["Employee Time Sheet Summary"] = {
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
	],
	"formatter": function(value, row, column, data, default_formatter) {	
		value = default_formatter(value, row, column, data);
		if (column.fieldname=="employee_name") {
			value = $(`<span><a onclick="epos_restaurant_2023.employee_time_sheet_summary.open_report('${value}')">${value}</a></span>`);

			var $value = $(value).css("font-weight", "bold");
			

			value = $value.wrap("<p></p>").parent().html();
		}
		return value;
	},
};
