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
	return get_columns(filters), report_data, message, report_chart, [],skip_total_row


## on validate filter
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')  

## on get columns report
def get_columns(filters):
	columns = []

	# ## append columns
	# columns.append({'fieldname':'row_group','label':filters.row_group,'fieldtype':'Data','align':'left','width':250})

	## generate dynamic columns
	for c in get_dynamic_columns(filters):
		columns.append(c) 

	return columns

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
			'align':rf["align"],
			'width':rf['width']}
		)

	return columns


## on get report field
def get_report_field(filters):
	fields = []
	fields.append({"label":"Employee","short_label":"Employee", "fieldname":"employee_name","fieldtype":"Data","indicator":"Grey","precision":2, "align":"left","chart_color":"#FF8A65",'width':250})
	fields.append({"label":"Phone","short_label":"Phone", "fieldname":"phone_number","fieldtype":"Data","indicator":"Grey","precision":2, "align":"right","chart_color":"#FF8A65",'width':150})
	fields.append({"label":"Salary","short_label":"Salary", "fieldname":"basic_salary","fieldtype":"Currency","indicator":"Grey","precision":2, "align":"right","chart_color":"#FF8A65",'width':100})
	fields.append({"label":"Duration","short_label":"Duration", "fieldname":"duration_title","fieldtype":"Data","indicator":"Red","precision":2, "align":"right","chart_color":"#2E7D32",'width':100})
	fields.append({"label":"Commission Amount","short_label":"Com. Amt", "fieldname":"commission_amount","fieldtype":"Currency","indicator":"Grey","precision":2, "align":"right","chart_color":"#FF8A65",'width':130})
	fields.append({"label":"Total Amount","short_label":"Total Amt", "fieldname":"total_amount","fieldtype":"Currency","indicator":"Grey","precision":2, "align":"right","chart_color":"#FF8A65",'width':130})
	
	return fields

def get_report_data(filters):

	emp_query = """	SELECT
						e.employee_code,
						concat(e.employee_code,'-',e.employee_name) as employee_name,
						coalesce(e.phone_number_1,'') as phone_number,
						e.basic_salary,
						0 as duration,
						'-' as duration_title,
						0 as commission_amount,		
						e.basic_salary as total_amount	
					FROM tabEmployee e"""
	
	data_query = """select 
						e.employee_code,
						sum(coalesce(s.duration,0)) as duration,
						sum(coalesce(s.commission_amount,0)) as commission_amount
					from tabEmployee e
					inner join `tabSale Product SPA Commission` s on e.`name` = s.employee
					WHERE {}
					group by
						e.employee_code""".format(get_filter_condition(filters)) 

	
	
	employees = frappe.db.sql(emp_query, as_dict=1)	
	data = frappe.db.sql(data_query,filters, as_dict=1)

	for e in employees:
		d = Enumerable(data).where(lambda x:x.employee_code == e.employee_code)	
 
		if d.count() > 0:
			e.duration = d[0].duration
			if e.duration < 60:
				e.duration_title = '{}MIN'.format(int(e.duration))
			elif e.duration == 60:
				e.duration_title = '{}H'.format(1)
			else:
				hour = int(e.duration / 60)
				minute = int(e.duration % 60)
				e.duration_title = '{}H: {}MIN'.format(hour,minute)

			e.commission_amount = d[0].commission_amount
			e.total_amount += d[0].commission_amount

	return employees


# get sql filter condition
def get_filter_condition(filters):
	conditions = " 1 = 1 "
	start_date = filters.start_date
	end_date = filters.end_date
	conditions += " AND s.is_deleted = 0 "
	conditions += " AND (s.posting_date between '{}' AND '{}')".format(start_date,end_date)
	conditions += " AND s.business_branch in %(business_branch)s"
	
	return conditions