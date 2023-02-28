# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CashTransaction(Document):
	def validate(self):

	 

		self.amount = self.input_amount / self.exchange_currency
		if not self.created_by:
			self.created_by = frappe.get_user().doc.full_name
