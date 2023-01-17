# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CashierShift(Document):
	def validate(self):
		
		for c in self.cash_float:
			exchange_rate = frappe.get_value("Payment Type", c.payment_method,"exchange_rate")
			exchange_rate = exchange_rate or 1

			c.amount = float(c.input_amount) / exchange_rate
		
