# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from py_linq import Enumerable


def execute(filters=None):
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
			business_branch,
			stock_location,
			total_quantity,
			total_amount
     	from `tabStock Take`
		where
			stock_location in %(stock_location)s and 
			business_branch in %(business_branch)s and 
			posting_date between %(start_date)s and %(end_date)s and 
			docstatus = 1

    """
    	
    data = frappe.db.sql(sql,filters , as_dict=1)
    return data

def get_report_columns():
    return [
		{"label":"Name", "fieldname":"name", "fieldtype":"Link","options":"Stock Take","width":200},
  		{"label":"Date", "fieldname":"posting_date", "fieldtype":"Date","align":"center","width":120},
		{"label":"Branch", "fieldname":"business_branch","fieldtype":"Data","align":"left","width":120},
  		{"label":"Stock Location","fieldname":"stock_location","field":"Stock Location","align":"left","width":150},	
		{"label":"QTY", "fieldname":"total_quantity", "fieldtype":"data","align":"center","width":90},
  		{"label":"Total Amount", "fieldname":"total_amount", "fieldtype":"Currency","align":"right","width":150},

		
	]
def get_report_summary(data,):
    report_summary = []
    report_summary.append({"label":_("Total Quantity"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_quantity or 0)),"indicator":"green"})
    report_summary.append({"label":_("Total Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_amount or 0)),"indicator":"red"})
   
    
    return report_summary