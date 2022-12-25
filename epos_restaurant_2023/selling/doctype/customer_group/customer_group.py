# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CustomerGroup(Document):
	def validate(self):
		if not self.customer_group_kh:
			self.customer_group_kh = self.name

