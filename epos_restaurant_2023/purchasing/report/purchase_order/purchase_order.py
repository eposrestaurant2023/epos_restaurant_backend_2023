# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from py_linq import Enumerable


def execute(filters=None):
	if not filters.vendor_name:
		filters.vendor_name ="All"
	
 

	if not filters.stock_location:
		filters.stock_location= frappe.db.get_list("Stock Location",pluck='name')
	if not filters.business_branch:
		filters.business_branch= frappe.db.get_list("Business Branch",pluck='name')

	data =  get_report_data(filters)
	return get_report_columns(),data,None,None, get_report_summary(data)

def get_report_data(filters):
	sql="""
		select 
			name,
			posting_date,
			vendor_name,
			business_branch,
			stock_location,
			grand_total,
			total_quantity,
			sub_total,
			total_discount,
			total_paid,
			balance
		from `tabPurchase Order` po
		where
			stock_location in %(stock_location)s and 
			business_branch in %(business_branch)s and 
			posting_date between %(start_date)s and %(end_date)s and 
			po.vendor = if(%(vendor_name)s='All',po.vendor,%(vendor_name)s) and
			docstatus = 1 
	"""
 
	data = frappe.db.sql(sql,filters , as_dict=1)
	return data

def get_report_columns():
    return [
		{"label":"Name", "fieldname":"name", "fieldtype":"Link","options":"Purchase Order","width":150},
  		{"label":"Date", "fieldname":"posting_date", "fieldtype":"Date","align":"center","width":120},
		{"label":"Branch", "fieldname":"business_branch","fieldtype":"Data","align":"left","width":120},
  		{"label":"Stock Location","fieldname":"stock_location","fieldtype":"Data","align":"left","width":150},
    	{"label":"Vendor", "fieldname":"vendor_name", "fieldtype":"Data","align":"left","width":120},
    	{"label":"QTY", "fieldname":"total_quantity", "fieldtype":"Data","align":"center","width":100},
    	{"label":"Sub Total", "fieldname":"sub_total", "fieldtype":"Currency","align":"center","width":120},
     	{"label":"Total Discount", "fieldname":"total_discount", "fieldtype":"Currency","align":"center","width":120},
		{"label":"Grand Total", "fieldname":"grand_total", "fieldtype":"Currency","align":"center","width":120},
		{"label":"Total Paid", "fieldname":"total_paid", "fieldtype":"Currency","align":"center","width":120},
  		{"label":"Balance", "fieldname":"balance", "fieldtype":"Currency","align":"right","width":120},

		
	]
def get_report_summary(data,):
    report_summary = []
    report_summary.append({"label":_("QTY"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_quantity or 0)),"indicator":"green"})
    report_summary.append({"label":_("Sub Total"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.sub_total or 0)),"indicator":"green"})
    report_summary.append({"label":_("Total Discount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_discount or 0)),"indicator":"green"})
    report_summary.append({"label":_("Grand Total"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.grand_total or 0)),"indicator":"blue"})
    report_summary.append({"label":_("Total Paid"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_paid or 0)),"indicator":"blue"})
    report_summary.append({"label":_("Balance"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.balance or 0)),"indicator":"red"})
    
    
    return report_summary