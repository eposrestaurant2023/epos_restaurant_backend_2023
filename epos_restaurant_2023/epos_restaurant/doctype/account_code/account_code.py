# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from frappe.installer import list_apps
class AccountCode(Document):
	def validate(self):
		self.account_code_name = self.code + ' - ' + self.account_name
		frappe.throw("x")
		apps = list_apps()
		if "ABC" in apps:
			frappe.throw("ABC is installed")
		else:
			frappe.throw("ABC is not installed")
	
