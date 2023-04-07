# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
from datetime import datetime
class TourBooking(Document):
	def validate(self):
		self.total_pax = self.total_pax or 1
		# calculate date total day
		
		self.duration = date_diff(self.end_date, self.start_date) + 1
		self.price = get_tour_package_price(self)
		
		# calculate additional charge
		for d in self.additional_charges:
			d.amount = d.price * d.quantity

		self.total_additional_charge =   Enumerable(self.additional_charges).sum(lambda x: (x.amount or 0))

		
		self.total_paid =   Enumerable(self.tour_booking_payment).sum(lambda x: (x.payment_amount or 0))
		self.total_expense =   Enumerable(self.expenses).sum(lambda x: (x.expense_amount or 0))
	
		self.price = self.price or 0
		self.total_additional_charge = self.total_additional_charge  or 0
		self.total_restaurant_amount = self.total_restaurant_amount or 0
		self.total_hotel_amount = self.total_hotel_amount or 0 
		self.total_paid = self.total_paid or 0

		
		self.balance =(self.price + self.total_additional_charge + self.total_restaurant_amount + self.total_hotel_amount)  - self.total_paid

		for d in self.guides_and_drivers:
			
			self.total_day = Enumerable(self.guides_and_drivers).sum(lambda x: (x.total_days or 0))
			self.total_amount_2 = self.total_day * d.rate

			if d.document_type == 'Tour Guides' and  not d.phone_number: 
				d.phone_number = frappe.db.get_value('Tour Guides', d.name1, 'phone_number')
				d.total_days = date_diff(d.end_date, d.start_date)
				
				
			if d.document_type == 'Tour Guides' and  not d.spoken_language: 
				d.spoken_language = frappe.db.get_value('Tour Guides', d.name1, 'speaking_language') 
				d.total_days = date_diff(d.end_date, d.start_date)
			
		#update total amount in hotel room
		for d in self.hotels:
			d.total_night = date_diff(d.departure, d.arrival) 
			d.rate = get_room_rate(d.hotel_name, d.room_type)
			d.total_rate = d.number_of_room * d.rate * d.total_night


		self.total_hotel_amount =   Enumerable(self.hotels).sum(lambda x: (x.total_rate or 0)) or 0
		self.total_room_nights =   Enumerable(self.hotels).sum(lambda x: (x.total_night or 0)) or 0
		self.total_pax1 = Enumerable(self.hotels).sum(lambda x: (x.pax or 0)) or 1
		
		for d in self.restaurants:
			d.total_amount = (d.pax or 1) * (d.price or 0)

		self.total_restaurant_amount =   Enumerable(self.restaurants).sum(lambda x: (x.total_amount or 0))

		self.total_transportation_amount = Enumerable(self.transportation).sum(lambda x: (x.rate or 0))
				
				

def date_diff(end_date, start_date):
	date_format = "%Y-%m-%d"
	date1 = datetime.strptime(start_date, date_format)
	date2 = datetime.strptime(end_date, date_format)

	delta = date2 - date1
	return delta.days

def get_tour_package_price(self):
	data = frappe.db.sql("select coalesce(max(price),0) as price from `tabTour Package Prices` where parent='{}' and number_of_person = {}".format(self.tour_package,self.total_pax or 1), as_dict=1)
	if data:
		return data[0]["price"]
	return self.price 

def get_room_rate(hotel_name, room_type):
	data = frappe.db.sql("select coalesce(max(room_rate),0) as rate from `tabTour Hotel Room Type` where parent='{}' and room_type = '{}'".format(hotel_name, room_type), as_dict=1)
	if data:
		return data[0]["rate"]
	return 0