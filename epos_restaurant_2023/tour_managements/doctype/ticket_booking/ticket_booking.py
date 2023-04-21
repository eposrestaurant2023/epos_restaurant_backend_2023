# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt
import frappe
from py_linq import Enumerable
from frappe.model.document import Document

class TicketBooking(Document):
	def validate(self):
		for d in self.ticket_booking_item:
			d.total_amount = d.price * d.quantity

		self.total_quantity =   Enumerable(self.ticket_booking_item).sum(lambda x: (x.quantity or 0))
		self.total_amount =   Enumerable(self.ticket_booking_item).sum(lambda x: (x.total_amount or 0))