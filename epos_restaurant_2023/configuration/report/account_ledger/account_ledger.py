# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import json
import frappe
from frappe import _
from frappe.utils import date_diff,today ,add_months, add_days
from frappe.utils.data import strip
import datetime
from py_linq import Enumerable

def execute(filters=None):
	if filters.filter_based_on =="Fiscal Year":
	# if not filters.from_fiscal_year:
		filters.from_fiscal_year = datetime.date.today().year		
		filters.start_date = '{}-01-01'.format(filters.from_fiscal_year)
		filters.end_date = '{}-12-31'.format(filters.from_fiscal_year) 


	elif filters.filter_based_on =="This Month":
		filters.start_date = datetime.date.today().replace(day=1)
		filters.end_date =add_days(add_months(filters.start_date ,1),-1)

	validate(filters)

	report_data = []
	skip_total_row=False
	message=None


	report_data = get_report_data(filters) 
	report_chart = None 

	# columns, report data , message, report chart, report summary, skip total row
	return get_columns(), report_data, message, report_chart, [],skip_total_row


## on validate filter
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')  


## on get columns report
def get_columns():
	columns = []
	columns.append({
		"label":"Transaction",
		"short_label":"Transaction",
		"fieldname":"account_code_name",
		"fieldtype":"Data",
		"indicator":"Grey",
		"precision":2,
		"align":"left",
		"chart_color":"#FF8A65",
		'width':350
	})
	columns.append({
		"label":"Debit",
		"short_label":"Debit",
		"fieldname":"debit",
		"fieldtype":"Currency",
		"indicator":"Grey",
		"precision":2,
		"align":"left",
		"chart_color":"#FF8A65",
		'width':100
	})
	columns.append({
		"label":"Credit",
		"short_label":"Credit",
		"fieldname":"credit",
		"fieldtype":"Currency",
		"indicator":"Grey",
		"precision":2,
		"align":"left",
		"chart_color":"#FF8A65",
		'width':100
	})
	columns.append({
		"label":"Balance",
		"short_label":"Balance",
		"fieldname":"balance",
		"fieldtype":"Currency",
		"indicator":"Grey",
		"precision":2,
		"align":"left",
		"chart_color":"#FF8A65",
		'width':100
	})
	

	return columns

#get data
def get_report_data(filters):
	query = """select 
					`code`,
					parent_account_code,
					account_name					 
				from `tabAccount Code` 
				order by 
				parent_account_code,
				`code`"""
	
	opening_query ="""select
		sum(if(type='Credit',0,1) * amount) as debit,
		sum(if(type='Credit',-1,0) * amount) as credit
	from `tabFolio Transaction`
	where {}""".format(get_opening_filter_condition(filters)) 

	
	sql ="""select
				account_code,
				if(type='Credit',0,1) * amount as debit,
				if(type='Credit',-1,0) * amount as credit
			from `tabFolio Transaction`
			where {}
		""".format(get_filter_condition(filters)) 


	## get all account code data
	data_acc = frappe.db.sql(query,as_dict=1)
	
	#get opening data of folio transaction
	data_opening = frappe.db.sql(opening_query,as_dict=1)
	
	#get data folio transation
	data_sql = frappe.db.sql(sql,as_dict=1) 
		
	
	if Enumerable(data_opening).count()>0:	
		for o in data_opening:
			o.balance = (o.debit or 0) + (o.credit or 0)
	else:
		data_opening.append({"debit":0,"credit":0,"balance":0})
			
	
	result = []
	#opening balance
	result.append({
			"code":"",
			"parent_account_code":"",
			"account_name":"",
			"account_code_name":"Opening Balance",
			"sort": -99999,
			"debit":data_opening[0].debit or 0,
			"credit":data_opening[0].credit or 0,
			"balance":data_opening[0].balance or 0
		})
	
	#data transaction
	for r in data_acc:
		value = Enumerable(data_sql).where(lambda x:x.account_code == r.code)
		debit,credit,balance = 0,0,0
		 
		if value.count()>0:
			debit = value[0].debit
			credit = value[0].credit
			balance = value[0].balance

		if r.parent_account_code != None:
			result.append({
				"code":r.code,
				"parent_account_code":r.parent_account_code,
				"account_name":r.account_name,
				"account_code_name":"{}-{}".format(r.code,r.account_name),
				"sort": 0,
				"debit":debit or 0,
				"credit":credit or 0,
				"balance":balance or 0
			})

	
 
	data = sorted(list(Enumerable(result)), key=lambda x: x['sort'])
	return data


# get sql filter condition
def get_filter_condition(filters):
	business_branchs = str(filters.business_branch).replace('[','').replace(']','') or ''
	conditions = " 1 = 1 "
	start_date = filters.start_date
	end_date = filters.end_date
	conditions += " AND (posting_date between '{}' AND '{}')".format(start_date,end_date)
	conditions += " AND property in ({})".format(business_branchs)

	return conditions

# # get sql filter condition
def get_opening_filter_condition(filters):
	business_branchs = str(filters.business_branch).replace('[','').replace(']','') or ''
	conditions = " 1 = 1 "
	start_date = filters.start_date
	conditions += " AND (posting_date < '{}')".format(start_date)
	conditions += " AND property in ({})".format(business_branchs)

	return conditions