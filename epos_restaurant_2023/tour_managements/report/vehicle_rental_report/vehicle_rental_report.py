# Copyright (c) 2024, Frappe Technologies and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):

	return get_columns(), get_data(filters)

def get_columns():
	columns = []
	columns.append({'fieldname':'customer','label':"Guest",'fieldtype':'Data','align':'left','width':120})
	columns.append({'fieldname':'NAME','label':"Doc #",'fieldtype':'Link','options':'Vehicle Rental','align':'center','width':140})
	columns.append({'fieldname':'booking_date','label':"Booking Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'start_date','label':"Start Date",'fieldtype':'Datetime','align':'center','width':180})
	columns.append({'fieldname':'end_date','label':"End Date",'fieldtype':'Datetime','align':'center','width':180})
	columns.append({'fieldname':'adult','label':"Adult",'fieldtype':'data','align':'center','width':80})
	columns.append({'fieldname':'child','label':"Child",'fieldtype':'data','align':'center','width':80})
	columns.append({'fieldname':'total_amount','label':"Total Amount",'fieldtype':'Currency','align':'right','width':120})
	columns.append({'fieldname':'discount_amount','label':"Total Discount",'fieldtype':'Currency','align':'right','width':120})
	columns.append({'fieldname':'grand_total','label':"Grand Total",'fieldtype':'Currency','align':'right','width':100})
	columns.append({'fieldname':'total_payment','label':"Total Paid",'fieldtype':'Currency','align':'right','width':100})
	columns.append({'fieldname':'balance','label':"Balance",'fieldtype':'Currency','align':'right','width':100})
	return columns

def get_data(filters):
	sql = """
			SELECT 
				customer,
				NAME,
				booking_date,
				start_date,
				end_date,
				adult,
				child,
				total_amount,
				discount_amount,
				total_amount-discount_amount grand_total,
				total_payment,
				balance
			FROM `tabVehicle Rental`
			{0}""".format(get_conditions(filters))
	data = frappe.db.sql(sql,filters, as_dict=1)
	return data

def get_conditions(filters):
	conditions = " where booking_date between %(start_date)s AND %(end_date)s"
	if filters.get("guest"):
		conditions += " AND customer in %(guest)s"
		
	return conditions