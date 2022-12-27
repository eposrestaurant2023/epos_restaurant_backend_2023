# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from py_linq import Enumerable

def execute(filters=None):
	skip_total_row=False
	data =  get_report_data(filters)
	return get_report_columns(),data,None,None, get_report_summary(data),skip_total_row

def get_report_data(filters):
    sql="""
    	select 
			name,
			posting_date,
			employee_name,
			total_amount,
			total_paid,
			balance
     	from `tabExpense`
		where
			posting_date between %(start_date)s and %(end_date)s and 
			docstatus = 1
    """
    	
    data = frappe.db.sql(sql,filters , as_dict=1)
    return data

def get_report_columns():
    return [
		{"label":"Expense Code", "fieldname":"name", "fieldtype":"Data","width":120},
  		{"label":"Date", "fieldname":"posting_date", "fieldtype":"Date","align":"center","width":120},
  		{"label":"Employee By", "fieldname":"employee_name","fieldtype":"Data","align":"left","width":150},
  		{"label":"Total Amount", "fieldname":"total_amount", "fieldtype":"Currency","align":"right","width":150},
		{"label":"Total Paid", "fieldname":"total_paid", "fieldtype":"Currency","align":"right","width":150},
		{"label":"Balance", "fieldname":"balance", "fieldtype":"Currency","align":"right","width":150},
	]
    
    
def get_report_summary(data,):
    report_summary = []
    report_summary.append({"label":_("Total Amount"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_amount or 0)),"indicator":"green"})
    report_summary.append({"label":_("Total Paid"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.total_paid or 0)),"indicator":"red"})
    report_summary.append({"label":_("Balance"),"value":frappe.utils.fmt_money(Enumerable(data).sum(lambda x: x.balance or 0)),"indicator":"blue"})
   
    return report_summary