# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class WorkingDay(Document):
	def validate(self):
		frappe.throw(frappe.session.user)
