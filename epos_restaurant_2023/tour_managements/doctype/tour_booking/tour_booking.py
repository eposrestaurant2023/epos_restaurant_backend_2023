# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
from datetime import datetime
from epos_restaurant_2023.utils import date_diff, get_room_rate, get_tour_package_price
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
			if d.document_type == 'Tour Guides' and  not d.phone_number: 
				d.phone_number = frappe.db.get_value('Tour Guides', d.name1, 'phone_number')
				
				
				
			if d.document_type == 'Tour Guides' and  not d.spoken_language: 
				d.spoken_language = frappe.db.get_value('Tour Guides', d.name1, 'speaking_language') 

		for d in self.guides_and_drivers:
			d.total_days = date_diff(d.end_date, d.start_date) + 1
			d.total_amount = d.total_days * d.rate

		self.total_day = Enumerable(self.guides_and_drivers).sum(lambda x: (x.total_days or 0))
		self.total_amount_2 = Enumerable(self.guides_and_drivers).sum(lambda x: (x.total_amount or 0))

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


		for d in self.transportation:
			d.total_day = date_diff(d.to_date, d.from_date) + 1
			d.total_amount = d.rate * d.total_day

		self.total_transportation_amount = Enumerable(self.transportation).sum(lambda x: (x.total_amount or 0))
				
