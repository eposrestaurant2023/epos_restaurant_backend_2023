# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import utils
from frappe import _
from frappe.model.document import Document

class Expense(Document):
	def validate(self):
		
		#validate expense dsate
		if self.posting_date>utils.today():
			frappe.throw(_("Expense date cannot greater than current date"))

		#validate amount
		total_amount = 0
		total_quantity = 0
		for d in self.expense_items:
			d.amount = d.price * d.quantity
			total_quantity = total_quantity + d.quantity
			total_amount = total_amount + d.amount

		self.total_quantity = total_quantity
		self.total_amount = total_amount

		self.balance = self.total_amount
  


	


		
