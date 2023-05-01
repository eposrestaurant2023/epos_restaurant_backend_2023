# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt
import frappe
from py_linq import Enumerable
from frappe.model.document import Document

class TicketBooking(Document):
	def validate(self):
		for d in self.ticket_booking_item:
			d.total_amount = (d.price or 0 ) * (d.quantity or 1)

		self.total_quantity =   Enumerable(self.ticket_booking_item).sum(lambda x: (x.quantity or 0))
		self.total_amount =   Enumerable(self.ticket_booking_item).sum(lambda x: (x.total_amount or 0))
		self.total_pax = (self.adult or 0) + (self.child or 0)

		self.total_payment =   Enumerable(self.payments).sum(lambda x: (x.payment_amount or 0)) or 0
		self.balance = (self.total_amount or 0) - (self.total_payment or 0)-(self.total_discount or 0)
		
		if(self.discount_type == "Percent"):
			self.total_discount = (self.total_amount or 0) * (self.discount or 0) / 100
		else:
			self.total_discount = (self.discount or 0)

		

