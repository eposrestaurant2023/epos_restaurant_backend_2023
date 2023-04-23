import frappe
from py_linq import Enumerable
from datetime import datetime

def date_diff(end_date, start_date):
	date_format = "%Y-%m-%d"
	date1 = datetime.strptime(start_date, date_format)
	date2 = datetime.strptime(end_date, date_format)

	delta = date2 - date1
	return delta.days

def get_tour_package_price(self):
	data = frappe.db.sql("select coalesce(max(price),0) as price from `tabTour Package Prices` where parent='{}' and number_of_person = {}".format(self.tour_package,self.adult or 1), as_dict=1)
	if data[0]["price"]>0:
		return data[0]["price"]
	price = frappe.db.get_value("Tour Packages",self.tour_package, "price")
	return price  

def get_room_rate(hotel_name, room_type):
	data = frappe.db.sql("select coalesce(max(room_rate),0) as rate from `tabTour Hotel Room Type` where parent='{}' and room_type = '{}'".format(hotel_name, room_type), as_dict=1)
	if data:
		return data[0]["rate"]
	return 0
