# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.utils.nestedset import NestedSet

class AccountCode(NestedSet):
	def validate(self):
		self.account_code_name = "{} - {}".format(self.name, self.account_name)
	