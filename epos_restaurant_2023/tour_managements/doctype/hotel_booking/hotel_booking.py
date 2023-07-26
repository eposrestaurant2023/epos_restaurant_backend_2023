# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
from epos_restaurant_2023.utils import date_diff, get_room_rate
class HotelBooking(Document):
	def validate(self):
		self.total_nights = date_diff(self.departure_date, self.arrival_date)
		#validate discount
		if(self.discount_type == "Percent"):
			self.discount_amount = (self.total_amount or 0) * (self.discount or 0) / 100
		else:
			self.discount_amount = (self.discount or 0)
		for d in self.room_types:
			d.number_of_room = (d.single_room or 0 ) + (d.double_room or 0) + (d.twin_room or 0)
			d.number_of_room = d.number_of_room or 1
			
			if d.rate == 0:
				d.rate = get_room_rate(self.hotel_name,d.room_type)

			d.total_amount = d.rate * d.number_of_room * self.total_nights

		self.total_rooms = Enumerable(self.room_types).sum(lambda x: (x.number_of_room or 0))
		self.total_room_night = self.total_rooms * self.total_nights
		self.total_amount = Enumerable(self.room_types).sum(lambda x:(x.total_amount or 0))
		self.total_payment = Enumerable(self.payments).sum(lambda x:(x.payment_amount or 0))
		self.balance = (self.total_amount or 0) - (self.discount_amount or 0) - (self.total_payment or 0)

@frappe.whitelist()
def get_rate(hotel_name, room_type):
	return get_room_rate(hotel_name,room_type)