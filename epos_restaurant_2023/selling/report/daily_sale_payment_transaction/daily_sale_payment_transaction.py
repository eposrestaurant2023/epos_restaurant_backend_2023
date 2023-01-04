import frappe
from frappe.utils import date_diff,today 
from frappe.utils.data import strip
from frappe import _
from py_linq import Enumerable

def execute(filters=None): 
 
	validate(filters)
	#run this to update parent_product_group in table sales invoice item

	report_data = []
	skip_total_row=False
 
	
	report_data = get_report_data(filters) 

	return get_columns(filters), report_data, None, None, get_report_summary(report_data,filters),skip_total_row
 
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')
  
	if not filters.outlet:
		filters.outlet = frappe.db.get_list("Outlet",pluck='name')
  

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:

			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))



	if filters.row_group and filters.parent_row_group:
		if(filters.row_group == filters.parent_row_group):
			frappe.throw("Parent row group and row group can not be the same")
 
def get_columns(filters):
	return [
		{"label":"Doc. #", "fieldname":"name","fieldtype":"Link","options":"Sale Payment", "align":"center"},
		{"label":"Date",  "fieldname":"posting_date","fieldtype":"Date", "align":"center",},
		{"label":"Reference", "fieldname":"referance","fieldtype":"Data","align":"left"},
  		{"label":"Sale", "fieldname":"sale","fieldtype":"Link","options":"Sale","align":"left"},
		{"label":"Branch", "fieldname":"business_branch","fieldtype":"Data","align":"left","width":120},
		{"label":"Outlet", "fieldname":"outlet","fieldtype":"Data","align":"left"},
		{"label":"Customer", "fieldname":"customer_name","fieldtype":"Data","align":"left","width":100},
		{"label":"Payment Type", "fieldname":"payment_type","fieldtype":"Data","align":"left","width":100},
		{"label":"Sale Amt", "fieldname":"sale_amount","fieldtype":"Currency","align":"right","width":100},
		{"label":"Balance", "fieldname":"balance","fieldtype":"Currency","align":"right","width":100},
  		{"label":"Payment Amount", "fieldname":"payment_amount","fieldtype":"Currency","align":"right","width":100}
	]
 
 


 
def get_conditions(filters,group_filter=None):
	conditions = " a.docstatus = 1 "

	start_date = filters.start_date
	end_date = filters.end_date



	conditions += " AND a.posting_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("payment_type_group"):
		conditions += " AND a.payment_type_group in %(payment_type_group)s"

	if filters.get("payment_type"):
		conditions += " AND a.payment_type in %(payment_type)s"

	if filters.get("customer_group"):
		conditions += " AND a.customer_group in %(customer_group)s"
	
	if filters.get("customer"):
		conditions += " AND a.customer = %(customer)s"
	if filters.get("sale"):
		conditions += " AND a.sale = %(sale)s"
 
	conditions += " AND a.business_branch in %(business_branch)s"
	conditions += " AND a.outlet in %(outlet)s"

	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	
	sql = """select  
			name,
			a.posting_date,
			a.business_branch,
			a.outlet,
			a.referance,
			a.sale,
			concat(a.customer ,'-',a.customer_name) as customer_name,
			a.sale_amount,
			a.payment_type,
			a.total_paid,
			a.balance,
   			a.payment_amount
		FROM `tabSale Payment` AS a
		WHERE
			{}
		
	""".format(get_conditions(filters,group_filter))	
	 
	data = frappe.db.sql(sql,filters, as_dict=1)

	return data
 

def get_report_summary(data,filters):
	report_summary = []
	report_summary.append({"label":_("Sale Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.sale_amount or 0)),"indicator":"blue"})	
	report_summary.append({"label":_("Total Balance"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.balance or 0)),"indicator":"red"})
	report_summary.append({"label":_("Total Payment Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.payment_amount or 0)),"indicator":"green"})		

	return report_summary