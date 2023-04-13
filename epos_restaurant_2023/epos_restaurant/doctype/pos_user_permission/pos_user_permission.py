# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.model.document import Document

class POSUserPermission(Document):
	def validate(self):
		if self.discount_sale and len(self.discount_codes) == 0:
			frappe.throw(_("Please add discount code"))
