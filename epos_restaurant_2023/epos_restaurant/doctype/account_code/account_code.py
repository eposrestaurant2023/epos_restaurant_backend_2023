# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
 
class AccountCode(Document):
	def validate(self):
		self.account_code_name = self.code + ' - ' + self.account_name
 
	
