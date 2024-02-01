# Copyright (c) 2024, Frappe Technologies and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):

	return get_columns(), get_data(filters)

def get_columns():
	columns = []
	columns.append({'fieldname':'guest_profile','label':"Guest",'fieldtype':'Data','align':'left','width':120})
	columns.append({'fieldname':'NAME','label':"Doc #",'fieldtype':'Link','options':'Hotel Booking','align':'center','width':140})
	columns.append({'fieldname':'booking_date','label':"Booking Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'arrival_date','label':"Arrival Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'departure_date','label':"Departure Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'total_nights','label':"Total Nights",'fieldtype':'Data','align':'center','width':110})
	columns.append({'fieldname':'total_rooms','label':"Total Rooms",'fieldtype':'Data','align':'center','width':110})
	columns.append({'fieldname':'total_room_night','label':"Total Room Night",'fieldtype':'Data','align':'center','width':150})
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
				guest_profile,
				NAME,
				booking_date,
				arrival_date,
				departure_date,
				total_nights,
				total_rooms,
				total_room_night,
				adult,
				child,
				total_amount,
				discount_amount,
				total_amount-discount_amount grand_total,
				total_payment,
				balance
			FROM `tabHotel Booking`
			{0}""".format(get_conditions(filters))
	data = frappe.db.sql(sql,filters, as_dict=1)
	return data

def get_conditions(filters):
	conditions = " where booking_date between %(start_date)s AND %(end_date)s"
	if filters.get("guest"):
		conditions += " AND guest_profile in %(guest)s"
		
	return conditions