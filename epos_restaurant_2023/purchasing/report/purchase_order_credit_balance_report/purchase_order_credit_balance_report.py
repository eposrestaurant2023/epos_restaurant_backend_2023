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
	if filters.show_po_transaction:
		skip_total_row=True
		
	report_data = get_report_data(filters) 
	report_summary = None
	if filters.show_summary==1:
		report_summary =get_report_summary(report_data,filters)
  
	return get_columns(filters), report_data, None, get_report_chart(report_data), report_summary,skip_total_row

def validate(filters):
	if not filters.end_date:
		filters.end_date = today()
		 
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')

 
def get_columns(filters):
	if filters.show_po_transaction:
		return [
			{"label":"Vendor", "fieldname":"row_group","fieldtype":"Data","align":"left","width":200},
			{"label":"Date", "fieldname":"posting_date","fieldtype":"Date","align":"center","width":100},
			{"label":"Total Amount", "fieldname":"total_amount","fieldtype":"Currency","align":"right","width":120},
			{"label":"Total Paid", "fieldname":"total_paid","fieldtype":"Currency","align":"right","width":120},
			{"label":"Balance", "fieldname":"balance","fieldtype":"Currency","align":"right","width":120},
			{"label":"Current", "fieldname":"current","fieldtype":"Currency","align":"right","width":120},
			{"label":"1-7 Days", "fieldname":"balance_7_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"8-14 Days", "fieldname":"balance_14_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"15-30 Days", "fieldname":"balance_30_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"31-60 Days", "fieldname":"balance_60_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"61-90 Days", "fieldname":"balance_90_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"Over 90 Day", "fieldname":"balance_over_90","fieldtype":"Currency","align":"right","width":120},
			
		]
	else:
		return [
			{"label":"Vendor", "fieldname":"row_group","fieldtype":"Data","align":"left","width":200},
			{"label":"Total Amount", "fieldname":"total_amount","fieldtype":"Currency","align":"right","width":120},
			{"label":"Total Paid", "fieldname":"total_paid","fieldtype":"Currency","align":"right","width":120},
			{"label":"Balance", "fieldname":"balance","fieldtype":"Currency","align":"right","width":120},
			{"label":"Current", "fieldname":"current","fieldtype":"Currency","align":"right","width":120},
			{"label":"1-7 Days", "fieldname":"balance_7_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"8-14 Days", "fieldname":"balance_14_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"15-30 Days", "fieldname":"balance_30_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"31-60 Days", "fieldname":"balance_60_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"61-90 Days", "fieldname":"balance_90_day","fieldtype":"Currency","align":"right","width":120},
			{"label":"Over 90 Day", "fieldname":"balance_over_90","fieldtype":"Currency","align":"right","width":120},
			
		]

 


 
def get_conditions(filters,group_filter=None):
	conditions = " a.docstatus = 1 "
	end_date = filters.end_date

	conditions += " AND a.posting_date<= '{}'".format(end_date)


	if filters.get("vendor_group"):
		conditions += " AND a.vendor_group in %(vendor_group)s"
  
	if filters.get("vendor"):
		conditions += " AND a.vendor = %(vendor)s"
 
	conditions += " AND a.business_branch in %(business_branch)s"


	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	
	sql = """select
		0 as indent,
		a.vendor,
		if(ifnull(a.vendor,'')='','Not Set',concat(a.vendor ,'-',a.vendor_name)) as row_group,
		sum(a.grand_total) as total_amount,
		sum(a.total_paid) as total_paid,
		sum(a.balance) as balance ,
		sum(if(a.posting_date = '{0}',a.balance, 0)) as current,
		sum(if(DATEDIFF(NOW(),a.posting_date) BETWEEN 1 and  7,a.balance, 0)) as balance_7_day,
		sum(if(DATEDIFF(NOW(),a.posting_date) BETWEEN 8 and  14,a.balance, 0)) as balance_14_day,
		sum(if(DATEDIFF(NOW(),a.posting_date) BETWEEN 15 and  30,a.balance, 0)) as balance_30_day,
		sum(if(DATEDIFF(NOW(),a.posting_date) BETWEEN 31 and  60,a.balance, 0)) as balance_60_day,
		sum(if(DATEDIFF(NOW(),a.posting_date) BETWEEN 61 and  90,a.balance, 0)) as balance_90_day,
		sum(if(DATEDIFF(NOW(),a.posting_date) > 90,a.balance, 0)) as balance_over_90
		
	FROM `tabPurchase Order` AS a
	where
		a.balance > 0 and 
	{1}
	group by 
		if(ifnull(a.vendor,'')='', 'Not Set',concat(a.vendor ,'-',a.vendor_name))
		
		
	""".format(filters.end_date, get_conditions(filters,group_filter))	
	
	data = frappe.db.sql(sql,filters, as_dict=1)
	
	if not filters.show_po_transaction:
		return data
	else:
		report_data = []
		for  d in data:
			report_data.append(d)
			report_data = report_data + get_po_transaction_data(filters, d.Vendor)
		return report_data

def get_po_transaction_data(filters,vendor):
	
	sql = """select
		1 as indent,
		a.name as row_group,
		a.posting_date,
		a.grand_total as total_amount,
		a.total_paid,
		a.balance as balance ,
		if(a.posting_date = '{0}',a.balance, 0) as current,
		if(DATEDIFF(NOW(),a.posting_date) BETWEEN 1 and  7,a.balance, 0) as balance_7_day,
		if(DATEDIFF(NOW(),a.posting_date) BETWEEN 8 and  14,a.balance, 0) as balance_14_day,
		if(DATEDIFF(NOW(),a.posting_date) BETWEEN 15 and  30,a.balance, 0) as balance_30_day,
		if(DATEDIFF(NOW(),a.posting_date) BETWEEN 31 and  60,a.balance, 0) as balance_60_day,
		if(DATEDIFF(NOW(),a.posting_date) BETWEEN 61 and  90,a.balance, 0) as balance_90_day,
		if(DATEDIFF(NOW(),a.posting_date) > 90,a.balance, 0) as balance_over_90
		
	FROM `tabPurchase Order` AS a
	where
		a.balance > 0 and 
		a.vendor = '{1}' and
	{2}
	""".format(filters.end_date,vendor, get_conditions(filters))	

	data = frappe.db.sql(sql,filters, as_dict=1)
	return data

def get_report_chart(data):
	chart_data = []
	chart_data.append(sum(d["current"] for d in data))
	chart_data.append(sum(d["balance_7_day"] for d in data if d["indent"] == 0))
	chart_data.append(sum(d["balance_14_day"] for d in data if d["indent"] == 0))
	chart_data.append(sum(d["balance_30_day"] for d in data if d["indent"] == 0))
	chart_data.append(sum(d["balance_60_day"] for d in data if d["indent"] == 0))
	chart_data.append(sum(d["balance_90_day"] for d in data if d["indent"] == 0))
	chart_data.append(sum(d["balance_over_90"] for d in data if d["indent"] == 0))
	chart =  {'data':
     			{
            		'labels':[_('Current'),_("1-7 Days"),_("8-14 Days"),_("15-30 Days"),_("31-60 Days"),_("61-90 Days"),_("Over 90 Days")],
              		'datasets':[
                    			{'values':chart_data}
                       		]
                },
        		'type':'percentage'
             }
	return chart

def get_report_summary(data,filters):
	report_summary = [] 
	
	report_summary.append({"label":_("Total Amount"),"value":frappe.utils.fmt_money(Enumerable(data).where(lambda x: x.indent ==0).sum(lambda x: x.total_amount or 0)),"indicator":"blue"})	
	report_summary.append({"label":_("Total Paid"),"value":frappe.utils.fmt_money(Enumerable(data).where(lambda x: x.indent ==0).sum(lambda x: x.total_paid or 0)),"indicator":"green"})	
	report_summary.append({"label":_("Balance"),"value":frappe.utils.fmt_money(Enumerable(data).where(lambda x: x.indent ==0).sum(lambda x: x.balance or 0)),"indicator":"orange"})	
	
	return report_summary

 