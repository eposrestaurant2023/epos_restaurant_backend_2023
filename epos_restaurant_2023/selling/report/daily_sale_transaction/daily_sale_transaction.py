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
		{"label":"Doc. #", "fieldname":"name","fieldtype":"Link","options":"Sale", "align":"center"},
		{"label":"Date",  "fieldname":"posting_date","fieldtype":"Date", "align":"center",},
		{"label":"Branch", "fieldname":"business_branch","fieldtype":"Data","align":"left","width":120},
		{"label":"Outlet", "fieldname":"outlet","fieldtype":"Data","align":"left"},
		{"label":"Customer", "fieldname":"customer_name","fieldtype":"Data","align":"left","width":150},
		{"label":"Tbl #", "fieldname":"tbl_number","fieldtype":"Data","align":"left"},
		{"label":"QTY", "fieldname":"total_quantity","fieldtype":"Float","precision":2, "align":"center","width":75},
  		{"label":"Sub Total", "fieldname":"sub_total","fieldtype":"Currency","align":"right"},
  		
		{"label":"Discount", "fieldname":"total_discount","fieldtype":"Currency","align":"right","width":100},
		{"label":"Tax", "fieldname":"total_tax","fieldtype":"Currency","align":"right","width":100},
		{"label":"Cost", "fieldname":"total_cost","fieldtype":"Currency","align":"right","width":100},
		{"label":"Total Amt", "fieldname":"grand_total","fieldtype":"Currency","align":"right","width":100},
		{"label":"Profit", "fieldname":"profit","fieldtype":"Currency","align":"right","width":100},
		{"label":"User", "fieldname":"created_by","fieldtype":"Data"},
		
	]
 
 


 
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
  
	if filters.get("customer"):
		conditions += " AND a.customer = %(customer)s"
 
	conditions += " AND a.business_branch in %(business_branch)s"
	conditions += " AND a.outlet in %(outlet)s"

	if filters.get("pos_profile"):
		conditions += " AND a.pos_profile in %(pos_profile)s"
	
	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	
	sql = """select  
			name,
	
			a.tbl_number,
			a.posting_date,
			a.business_branch,
			a.outlet,
			a.stock_location,
			concat(a.customer ,'-',a.customer_name) as customer_name,
			total_quantity,
			a.sub_total,
			a.total_discount,
			a.grand_total,
			a.total_cost,
			a.profit,
			a.total_tax,
			a.created_by
		FROM `tabSale` AS a
		WHERE
			{}
		
	""".format(get_conditions(filters,group_filter))	
	 
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

 
	return [
		{
			"fieldname":"a.parent",
			"label":"Sale Invoice",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"a.product_category",
			"label":"Category",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"a.product_group",
			"label":"Product Group",
			"parent_row_group_filter_field":"row_group"
		},
 		 {
			"fieldname":"a.revenue_group",
			"label":"Revenue Group",
			"parent_row_group_filter_field":"row_group"
		},
   		 {
			"fieldname":"b.outlet",
			"label":"Outlet",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"b.business_branch",
			"label":"Business Branch",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"if(ifnull(b.pos_profile,'')='','Not Set',b.pos_profile)",
			"label":"POS Profile",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"if(ifnull(b.customer,'')='','Not Set',concat(b.customer,'-',b.customer_name))",
			"label":"Customer",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"b.customer_group",
			"label":"Customer Group",
			"parent_row_group_filter_field":"row_group"
		},		
		{
			"fieldname":"ifnull(b.stock_location,'Not Set')",
			"label":"Stock Location",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%d/%%m/%%Y')",
			"label":"Date",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%m/%%Y')",
			"label":"Month",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%Y')",
			"label":"Year",
			"parent_row_group_filter_field":"row_group"
		},
  
		{
			"fieldname":"concat(a.product_code,'-',a.product_name)",
			"label":"Product"
		}
	]