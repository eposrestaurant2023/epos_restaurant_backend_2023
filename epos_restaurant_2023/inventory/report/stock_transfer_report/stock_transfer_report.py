
import frappe
from frappe import _
from py_linq import Enumerable


def execute(filters=None):
	if not filters.from_stock_location:
		filters.from_stock_location= frappe.db.get_list("Stock Location",pluck='name')
	if not filters.to_stock_location:
		filters.to_stock_location= frappe.db.get_list("Stock Location",pluck='name')
	if not filters.from_business_branch:
		filters.from_business_branch= frappe.db.get_list("Business Branch",pluck='name')
	if not filters.to_business_branch:
		filters.to_business_branch= frappe.db.get_list("Business Branch",pluck='name')
	
	data =  get_report_data(filters)
	return get_report_columns(),data,None,None, get_report_summary(data)


def get_report_data(filters):
    sql="""
    	select 
			name,
			posting_date,
			from_stock_location,
   			to_stock_location,
			from_business_branch,
			to_business_branch,
			total_quantity,
			total_amount
     	from `tabStock Transfer`
		where
			from_stock_location in %(from_stock_location)s and 
			to_stock_location in %(to_stock_location)s and 
			from_business_branch in %(from_business_branch)s and 
			to_business_branch in %(to_business_branch)s and 
			posting_date between %(start_date)s and %(end_date)s and 
			docstatus = 1

    """
    	
    data = frappe.db.sql(sql,filters , as_dict=1)
    return data

def get_report_columns():
    return [
		{"label":"Name", "fieldname":"name", "fieldtype":"Link","options":"Stock Transfer","width":150},
  		{"label":"Date", "fieldname":"posting_date", "fieldtype":"Date","align":"center","width":120},
		{"label":"From Business Branch", "fieldname":"from_business_branch","fieldtype":"Data","align":"left","width":170},
		{"label":"From Stock Location", "fieldname":"from_stock_location","fieldtype":"Data","align":"left","width":170},
		{"label":"To Business Branch", "fieldname":"to_business_branch","fieldtype":"Data","align":"left","width":150},
  		{"label":"To Stock Location", "fieldname":"to_stock_location","fieldtype":"Data","align":"left","width":150},
		{"label":"QTY", "fieldname":"total_quantity","fieldtype":"Data","align":"center","width":70},
		{"label":"Total Amount", "fieldname":"total_amount","fieldtype":"Currency","align":"right","width":110},
  		
		
	]
def get_report_summary(data,):
    report_summary = []
    report_summary.append({"label":_("QTY"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_quantity or 0)),"indicator":"green"})
    report_summary.append({"label":_("Total Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_amount or 0)),"indicator":"red"})
    
    
    
    return report_summary