# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document
import frappe

class ResetData(Document):
	def validate(self):
		if frappe.get_roles(frappe.session.user)[0] == "System Manager":
			frappe.throw("a")
		else:
			frappe.throw("b")