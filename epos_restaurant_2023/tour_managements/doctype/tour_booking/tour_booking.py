# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
from datetime import datetime
class TourBooking(Document):
	def validate(self):
		# calculate date total day
		
		self.duration = date_diff(self.end_date, self.start_date) + 1
		self.price = get_tour_package_price(self)

		  

		# calculate additional charge
		for d in self.additional_charges:
			d.amount = d.price * d.quantity

		self.total_additional_charge =   Enumerable(self.additional_charges).sum(lambda x: (x.amount or 0))

		
		self.total_paid =   Enumerable(self.tour_booking_payment).sum(lambda x: (x.payment_amount or 0))
		self.total_expense =   Enumerable(self.expenses).sum(lambda x: (x.expense_amount or 0))
	

		self.balance =(self.price + self.total_additional_charge + self.total_restaurant_amount + self.total_hotel_amount)  - self.total_paid

		for d in self.guides_and_drivers:
			if d.document_type == 'Tour Guides' and  not d.phone_number: 
				d.phone_number = frappe.db.get_value('Tour Guides', d.name1, 'phone_number') 
				
			if d.document_type == 'Tour Guides' and  not d.spoken_language: 
				d.spoken_language = frappe.db.get_value('Tour Guides', d.name1, 'speaking_language') 
		#update total amount in hotel room
		for d in self.hotels:
			d.rate = get_room_rate(d.hotel_name, d.room_type)
			d.total_rate = d.number_of_room * d.rate


		self.total_hotel_amount =   Enumerable(self.hotels).sum(lambda x: (x.total_rate or 0))
		
		for d in self.restaurants:
			d.total_amount = d.pax * d.price

		self.total_restaurant_amount =   Enumerable(self.restaurants).sum(lambda x: (x.total_amount or 0))

		self.total_transportation_amount = Enumerable(self.transportation).sum(lambda x: (x.rate or 0))
				
				

def date_diff(end_date, start_date):
	date_format = "%Y-%m-%d"
	date1 = datetime.strptime(start_date, date_format)
	date2 = datetime.strptime(end_date, date_format)

	delta = date2 - date1
	return delta.days

def get_tour_package_price(self):
	data = frappe.db.sql("select coalesce(max(price),0) as price from `tabTour Package Prices` where parent='{}' and number_of_person = {}".format(self.tour_package,self.total_pax), as_dict=1)
	if data:
		return data[0]["price"]
	return self.price 

def get_room_rate(hotel_name, room_type):
	data = frappe.db.sql("select coalesce(max(room_rate),0) as rate from `tabTour Hotel Room Type` where parent='{}' and room_type = '{}'".format(hotel_name, room_type), as_dict=1)
	if data:
		return data[0]["rate"]
	return 0