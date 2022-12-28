
frappe.query_reports["Stock Transfer Report"] = {
	"filters": [
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
			"fieldname":"from_business_branch",
			"label": __("From Business Branch"),
			"fieldtype":"MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Business Branch', txt);
			}
		},
		{
			fieldname: "from_stock_location",
			label: __("From Stock Location"),
			fieldtype: "MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Stock Location', txt);
			}
			 
		},
		{
			"fieldname":"to_business_branch",
			"label": __("To Business Branch"),
			"fieldtype":"MultiSelectList",
			get_data: function(txt) {
				return frappe.db.get_link_options('Business Branch', txt);
			}
		},
		{
			"fieldname": "to_stock_location",
			"label": __("To Stock Location"),
			"fieldtype": "MultiSelectList",
			get_data: function(txt) {
				
				return frappe.db.get_link_options('Stock Location', txt);
			}
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
 
