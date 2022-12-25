# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):
	if not filters.stock_location:
		filters.stock_location= frappe.db.get_list("Stock Location",pluck='name')

 

	columns =[
		{"label":"Stock Location", "fieldname":"stock_location", "fieldtype":"Data", "width":200, "align":"left"},
		{"label":"Stock Value", "fieldname":"total_cost", "fieldtype":"Currency","align":"right", "width":200},
	]
	report_data = get_data(filters)
 
	return columns, report_data, None, get_report_chart(report_data)

def get_data(filters):
    sql = """
	select 
		stock_location,
		sum(total_cost) as total_cost
	from `tabStock Location Product` 
	where 
		stock_location in %(stock_location)s
	group by
	stock_location
    """
    data = frappe.db.sql(sql,filters, as_dict=1)
    return data



def get_report_chart(data):
	columns=[]
	for d in data:
		columns.append(d["stock_location"])

	dataset = []
	colors = []

	
	#loop sum dynamic column data data set value
	dataset_values = []
	for d in data:
		dataset_values.append(d.total_cost)

	dataset.append({'name':"Grand Total",'values':dataset_values})
	colors.append("red")


	chart = {
		'data':{
			'labels':columns,
			'datasets':dataset
		},
		"type": "bar",
		"lineOptions": {
			"regionFill": 1,
		},
		"axisOptions": {"xIsSeries": 1}
	}
	return chart