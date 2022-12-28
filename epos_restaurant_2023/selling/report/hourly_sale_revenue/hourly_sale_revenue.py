import frappe
from frappe.utils import date_diff,today 
from frappe.utils.data import strip
from frappe import _
from py_linq import Enumerable
import datetime

def execute(filters=None): 
 
	validate(filters)
	#run this to update parent_product_group in table sales invoice item

	report_data = []
	skip_total_row=False
 
	
	report_data = get_report_data(filters) 

	return get_columns(filters), report_data, None, get_report_chart(report_data), get_report_summary(report_data,filters),skip_total_row
 
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')
  
	if not filters.outlet:
		filters.outlet = frappe.db.get_list("Outlet",pluck='name')
  
	if not filters.start_date:
		filters.start_date = datetime.date.today()
  
	if not filters.end_date:
		filters.end_date =  datetime.date.today()

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:

			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))



	if filters.row_group and filters.parent_row_group:
		if(filters.row_group == filters.parent_row_group):
			frappe.throw("Parent row group and row group can not be the same")
 
def get_columns(filters):
	return [
		{"label":"Hour", "fieldname":"hour","fieldtype":"Data", "align":"center", "width":100},
		{"label":"Total Orders", "fieldname":"total_transaction","fieldtype":"Int", "align":"center","width":120},
		{"label":"QTY", "fieldname":"total_quantity","fieldtype":"Float","precision":2, "align":"center","width":75},
  		{"label":"Sub Total", "fieldname":"sub_total","fieldtype":"Currency","align":"right"},
  		
		{"label":"Discount", "fieldname":"total_discount","fieldtype":"Currency","align":"right","width":100},
		{"label":"Tax", "fieldname":"total_tax","fieldtype":"Currency","align":"right","width":100},
		{"label":"Cost", "fieldname":"total_cost","fieldtype":"Currency","align":"right","width":100},
		{"label":"Total Amt", "fieldname":"grand_total","fieldtype":"Currency","align":"right","width":100},
		{"label":"Profit", "fieldname":"profit","fieldtype":"Currency","align":"right","width":100},
		
		
	]
 
 


 
def get_report_chart(data):
	columns=[]
	for d in data:
		columns.append(d["hour"])

	dataset = []
	colors = []

	
	#loop sum dynamic column data data set value
	dataset_values = []
	for d in data:
		dataset_values.append(d.grand_total)

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

def get_conditions(filters,group_filter=None):
	conditions = " a.docstatus = 1 "

	start_date = filters.start_date
	end_date = filters.end_date



	conditions += " AND a.posting_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("product_group"):
		conditions += " AND a.product_group in %(product_group)s"

	if filters.get("product_category"):
		conditions += " AND a.product_category in %(product_category)s"

	if filters.get("customer_group"):
		conditions += " AND a.customer_group in %(customer_group)s"
 
	conditions += " AND a.business_branch in %(business_branch)s"
	conditions += " AND a.outlet in %(outlet)s"

	if filters.get("pos_profile"):
		conditions += " AND a.pos_profile in %(pos_profile)s"
	
	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	
	sql = """select  
			 LPAD(n.number, 2, 0) as `hour`,
			count(a.name) as total_transaction,
			sum(total_quantity) as total_quantity,
			sum(a.sub_total) as sub_total,
			sum(a.total_discount) as total_discount,
			sum(a.grand_total) as grand_total,
			sum(a.total_cost) as total_cost,
			sum(a.profit) as profit,
			sum(a.total_tax) as total_tax
   		FROM `tabSale` AS a
			right join `tabNumbers` n on n.number = hour(a.creation) and 
			{}
		group by
			n.number
		order by
			n.number
		
	""".format(get_conditions(filters,group_filter))	
	#frappe.msgprint(sql)
	data = frappe.db.sql(sql,filters, as_dict=1)

	return data
 

def get_report_summary(data,filters):
	report_summary = [] 
	report_summary.append({"label":_("Quantity"),"value":Enumerable(data).sum(lambda x: x.total_quantity or 0),"indicator":"blue"})	
	report_summary.append({"label":_("Sub Total"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.sub_total or 0)),"indicator":"blue"})	
	report_summary.append({"label":_("Discount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_discount or 0)),"indicator":"red"})	
	report_summary.append({"label":_("Tax"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_tax or 0)),"indicator":"red"})	
	report_summary.append({"label":_("Cost"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_cost or 0)),"indicator":"orange"})	
	report_summary.append({"label":_("Total Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.grand_total or 0)),"indicator":"green"})	
	report_summary.append({"label":_("Profit"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.profit or 0)),"indicator":"green"})	

	return report_summary
