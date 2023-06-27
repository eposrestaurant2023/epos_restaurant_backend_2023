# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt
import json
import frappe
from frappe import _
from frappe.utils import date_diff,today ,add_months, add_days
from frappe.utils.data import strip
import datetime


def execute(filters=None):

	if filters.filter_based_on =="Fiscal Year":
		if not filters.from_fiscal_year:
			filters.from_fiscal_year = datetime.date.today().year
			
			filters.start_date = '{}-01-01'.format(filters.from_fiscal_year)
			filters.end_date = '{}-12-31'.format(filters.from_fiscal_year) 

	elif filters.filter_based_on =="This Month":
		filters.start_date = datetime.date.today().replace(day=1)
		filters.end_date =add_days(  add_months(filters.start_date ,1),-1)

	validate(filters)


	report_data = []
	skip_total_row=False
	message=None

	if filters.get("parent_row_group"):
		report_data = get_report_group_data(filters)
		message="Enable <strong>Parent Row Group</strong> making report loading slower. Please try  to select some report filter to reduce record from database "
		skip_total_row = True
	else:
		report_data = get_report_data(filters) 
	report_chart = None

	#return report columns and data
	if filters.chart_type !="None" and len(report_data)<=100:
		report_chart = get_report_chart(filters,report_data) 


	# columns, report data , message, report chart, report summary, skip total row
	return get_columns(filters), report_data, message, report_chart, [],skip_total_row


	# columns, data = [], []
	# return columns, data




## on validate filter
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')  
	
	if not filters.stock_location:
		filters.stock_location = frappe.db.get_list("Stock Location",pluck='name')

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:

			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))

	
	if filters.column_group=="Daily":
		n = date_diff(filters.end_date, filters.start_date)
		if n>30:
			frappe.throw("Date range cannot greater than 30 days")

	if filters.row_group and filters.parent_row_group:
		if(filters.row_group == filters.parent_row_group):
			frappe.throw("Parent row group and row group can not be the same")


## on get columns report
def get_columns(filters):
	columns = []

	## append columns
	columns.append({'fieldname':'row_group','label':filters.row_group,'fieldtype':'Data','align':'left','width':250})

	## generate dynamic columns
	if filters.row_group not in ["Date","Month","Year"]:
		for c in get_dynamic_columns(filters):
			columns.append(c) 
	return columns



## on get row group
def get_row_groups():
	return [
		{
			"fieldname":"concat(a.product_code,'-',a.product_name)",
			"label":"Product"
		},
		{
			"fieldname":"a.product_category",
			"label":"Product Category",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"if(ifnull(a.product_group,'')='','Not Set',a.product_group)",
			"label":"Product Group",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"a.business_branch",
			"label":"Business Branch",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"ifnull(a.stock_location,'Not Set')",
			"label":"Stock Location",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(a.transaction_date,'%%d/%%m/%%Y')",
			"label":"Date",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(a.transaction_date,'%%m/%%Y')",
			"label":"Month",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(a.transaction_date,'%%Y')",
			"label":"Year",
			"parent_row_group_filter_field":"row_group"
		}
	]


## on get dynamic columns
def get_dynamic_columns(filters):
	#static report field
	report_fields = get_report_field(filters)
	columns=[]
	for rf in report_fields:
		columns.append({
			'fieldname': rf["fieldname"],
			'label': rf["short_label"],
			'fieldtype':rf["fieldtype"],
			'precision': rf["precision"],
			'align':rf["align"]}
		)

	return columns


## on get report field
def get_report_field(filters):
	fields = []
	fields.append({"label":"Unit","short_label":"Unit", "fieldname":"stock_unit","fieldtype":"Data","indicator":"Grey","precision":2, "align":"left","chart_color":"#FF8A65","sql_expression":"a.stock_unit"})
	# fields.append({"label":"Sub Total", "short_label":"Sub To.", "fieldname":"sub_total","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.sub_total)"})
	# fields.append({"label":"Discount", "short_label":"Disc.", "fieldname":"discount_amount","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_discount)"})
	# fields.append({"label":"Tax", "short_label":"Tax", "fieldname":"total_tax","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_tax)"})
	# fields.append({"label":"Amount", "short_label":"Amt", "fieldname":"amount","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"})
	# fields.append({"label":"Cost", "short_label":"Cost", "fieldname":"cost","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.cost)"})

	# fields.append({"label":"Net Sale", "short_label":"net_sale", "fieldname":"net_sale","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"})
	# fields.append({"label":"Profit", "short_label":"Profit", "fieldname":"profit","fieldtype":"Currency","indicator":"Green","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue - a.cost)"})
	
	return fields


## on get report group data
def get_report_group_data(filters):
	parent = get_report_data(filters, filters.parent_row_group, 0)
	data=[] 
	for p in parent:
		p["is_group"] = 1
		data.append(p)

		row_group = [d for d in get_row_groups() if d["label"]==filters.parent_row_group][0]
		children = get_report_data(filters, None, 1, group_filter={"field":row_group["fieldname"],"value":p[row_group["parent_row_group_filter_field"]]})
		for c in children:
			data.append(c)
	return data


## on get report data
def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==filters.row_group][0]

	if(parent_row_group!=None):
		row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==parent_row_group][0]

	
	report_fields = get_report_field(filters)
	sql = "select {} as row_group, {} as indent ".format(row_group, indent)	
	
	# total last column
	item_code = ""
	groupdocstatus = ""
	normal_filter = " " 
	
	for rf in report_fields:
		#check sql variable if last character is , then remove it
		sql = strip(sql)
		if sql[-1]==",":
			sql = sql[0:len(sql)-1]	
		sql = sql + " ,{} AS '{}' ".format(rf["sql_expression"],rf["fieldname"])


	sql = sql + """ {2}
		FROM `tabInventory Transaction` AS a
		WHERE
			{4}
			{0}
		GROUP BY 
		{1} {2} {3}
	""".format(get_conditions(filters,group_filter), row_group,item_code,groupdocstatus,normal_filter)

	
	data = frappe.db.sql(sql,filters, as_dict=1)

	return data


## on get condition
def get_conditions(filters,group_filter=None):
	conditions = " 1 = 1 "
	start_date = filters.start_date
	end_date = filters.end_date


	if(group_filter!=None):		 
		conditions += " and {} ='{}'".format(group_filter["field"],group_filter["value"].replace("'","''").replace("%","%%"))

	conditions += " AND a.transaction_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("product_group"):
		conditions += " AND a.product_group in %(product_group)s"

	if filters.get("product_category"):
		conditions += " AND a.product_category in %(product_category)s"
 
	
	conditions += " AND a.business_branch in %(business_branch)s"

	conditions += " AND a.stock_location in %(stock_location)s"
	
	return conditions

## on get report chart
def get_report_chart(filters,data):
	columns = []
	dataset = []
	colors = []
	report_fields = get_report_field(filters)

	for d in data:
			if d["indent"] ==0:
				columns.append(d["row_group"])

	
	for rf in report_fields:	 
		fieldname = 'total_'+rf["fieldname"]
		if(fieldname=="total_qty"):
			dataset.append({'name':rf["label"],'values':(d["total_qty"] for d in data if d["indent"]==0)})
		elif(fieldname=="total_sub_total"):
			dataset.append({'name':rf["label"],'values':(d["total_sub_total"] for d in data if d["indent"]==0)})
		elif(fieldname=="total_cost"):
			dataset.append({'name':rf["label"],'values':(d["total_cost"] for d in data if d["indent"]==0)})
		elif(fieldname=="total_amount"):
			dataset.append({'name':rf["label"],'values':(d["total_amount"] for d in data if d["indent"]==0)})
		elif(fieldname=="total_profit"):
			dataset.append({'name':rf["label"],'values':(d["total_profit"] for d in data if d["indent"]==0)})		 

	chart = {
		'data':{
			'labels':columns,
			'datasets':dataset
		},
		"type": filters.chart_type,
		"lineOptions": {
			"regionFill": 1,
		},
		"axisOptions": {"xIsSeries": 1}
	}

	return chart

	













