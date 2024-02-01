# Copyright (c) 2024, Frappe Technologies and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):

	return get_columns(), get_data(filters)

def get_columns():
	columns = []
	columns.append({'fieldname':'guest_profile','label':"Guest",'fieldtype':'Data','align':'left','width':120})
	columns.append({'fieldname':'NAME','label':"Doc #",'fieldtype':'Link','options':'Tour Booking','align':'center','width':140})
	columns.append({'fieldname':'booking_date','label':"Booking Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'start_date','label':"Start Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'end_date','label':"End Date",'fieldtype':'Date','align':'center','width':120})
	columns.append({'fieldname':'tour_package','label':"Tour Package",'fieldtype':'Data','align':'center','width':110})
	columns.append({'fieldname':'duration','label':"Tour Duration",'fieldtype':'Data','align':'center','width':110})
	columns.append({'fieldname':'adult','label':"Adult",'fieldtype':'data','align':'center','width':80})
	columns.append({'fieldname':'child','label':"Child",'fieldtype':'data','align':'center','width':80})
	columns.append({'fieldname':'total_pax','label':"Total Pax",'fieldtype':'data','align':'right','width':100})
	columns.append({'fieldname':'total_tour_package_price','label':"Tour Package Amount",'fieldtype':'Currency','align':'right','width':170})
	columns.append({'fieldname':'total_hotel_amount','label':"Hotel Amount",'fieldtype':'Currency','align':'right','width':120})
	columns.append({'fieldname':'total_restaurant_amount','label':"Restaurant Amount",'fieldtype':'Currency','align':'right','width':150})
	columns.append({'fieldname':'total_tour_guide_amount','label':"Tour Guide Amount",'fieldtype':'Currency','align':'right','width':150})
	columns.append({'fieldname':'total_transportation_amount','label':"Transportation Amount",'fieldtype':'Currency','align':'right','width':180})
	columns.append({'fieldname':'total_additional_charge','label':"Addtional Charge Amount",'fieldtype':'Currency','align':'right','width':190})
	columns.append({'fieldname':'total_expense','label':"Expense Amount",'fieldtype':'Currency','align':'right','width':140})
	columns.append({'fieldname':'total_amount','label':"Total Amount",'fieldtype':'Currency','align':'right','width':120})
	columns.append({'fieldname':'total_discount','label':"Total Discount",'fieldtype':'Currency','align':'right','width':120})
	columns.append({'fieldname':'grand_total','label':"Grand Total",'fieldtype':'Currency','align':'right','width':100})
	columns.append({'fieldname':'total_paid','label':"Total Paid",'fieldtype':'Currency','align':'right','width':100})
	columns.append({'fieldname':'balance','label':"Balance",'fieldtype':'Currency','align':'right','width':100})
	return columns

def get_data(filters):
	sql = """
			SELECT 
				guest_profile,
				NAME,
				booking_date,
				start_date,
				end_date,
				tour_package,
				duration,
				adult,
				child,
				total_pax,
				total_tour_package_price,
				total_hotel_amount,
				total_restaurant_amount,
				total_tour_guide_amount,
				total_transportation_amount,
				total_additional_charge,
				total_expense,
				(total_tour_package_price+total_hotel_amount+total_restaurant_amount+total_tour_guide_amount+total_transportation_amount+total_additional_charge+total_expense) total_amount,
				total_discount,
				(total_tour_package_price+total_hotel_amount+total_restaurant_amount+total_tour_guide_amount+total_transportation_amount+total_additional_charge+total_expense-total_discount) grand_total,
				total_paid,
				abs(total_paid - (total_tour_package_price+total_hotel_amount+total_restaurant_amount+total_tour_guide_amount+total_transportation_amount+total_additional_charge+total_expense-total_discount)) balance
			FROM `tabTour Booking`
			{0}""".format(get_conditions(filters))
	data = frappe.db.sql(sql,filters, as_dict=1)
	return data

def get_conditions(filters):
	conditions = " where booking_date between %(start_date)s AND %(end_date)s"
	if filters.get("guest"):
		conditions += " AND guest_profile in %(guest)s"
	if filters.get("tour_package"):
		conditions += " AND tour_package in %(tour_package)s"
		
	return conditions