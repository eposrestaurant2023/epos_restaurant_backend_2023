# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class ReservationStatus(Document):
	def  validate(self):
		if  frappe.session.user !="Administrator":
			frappe.throw("Please contact your system administrator to update this record")
