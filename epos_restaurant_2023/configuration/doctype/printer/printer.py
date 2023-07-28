# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class Printer(Document):
	def validate(self):
		self.business_branch_printer = "{} > {}".format(self.business_branch, self.printer_name)