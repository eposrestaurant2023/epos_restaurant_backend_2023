# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from frappe import utils
from frappe import _
class Customer(Document):
	def validate(self):
		if self.date_of_birth>utils.today():
			frappe.throw(_("Date of birth cannot be greater than the current time"))

		if not self.customer_name_kh:
			self.customer_name_kh = self.customer_name_en
   
	def on_update(self):
		self.customer_code_name = "{} - {}".format(self.name,self.customer_name_en)