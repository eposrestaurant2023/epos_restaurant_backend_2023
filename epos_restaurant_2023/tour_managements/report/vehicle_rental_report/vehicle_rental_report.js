// Copyright (c) 2024, Tes Pheakdey and contributors
// For license information, please see license.txt
/* eslint-disable */

frappe.query_reports["Vehicle Rental Report"] = {
	onload: function(report) {
		report.page.add_inner_button ("Preview Report", function () {
			frappe.query_report.refresh();
		});
		
	},
	"filters": [
		{
			fieldname: "start_date",
			label: "Start Date",
			fieldtype: "Date",
			on_change: function (query_report) {},
			default:frappe.datetime.get_today()
		},
		{
			fieldname: "end_date",
			label: "End Date",
			fieldtype: "Date",
			on_change: function (query_report) {},
			default:frappe.datetime.get_today()
		},
		{
			"fieldname": "guest",
			"label": __("Guest Profile"),
			"fieldtype": "MultiSelectList",
			on_change: function (query_report) {},
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Customer', txt);
			}
		},
	]
};
