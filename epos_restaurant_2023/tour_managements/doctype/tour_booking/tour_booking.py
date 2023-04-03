# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
class TourBooking(Document):
	def validate(self):
		# calculate additional charge
		for d in self.additional_charges:
			d.amount = d.price * d.quantity

		self.total_additional_charge =   Enumerable(self.additional_charges).sum(lambda x: (x.amount or 0))

		
		self.total_paid =   Enumerable(self.tour_booking_payment).sum(lambda x: (x.payment_amount or 0))
		self.total_expense =   Enumerable(self.expenses).sum(lambda x: (x.expense_amount or 0))

		self.balance =(self.price + self.total_additional_charge)  - self.total_paid
		


		