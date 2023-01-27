# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class TablesNumber(Document):
	def validate(self):
		pass
		# if not frappe.db.exists('ePOS Table Position', {'tbl_number': self.tbl_number, 'table_group': self.tbl_group}):
   		# 	frappe.throw("Table number {} is already exist in group {}".format(self.tbl_number, self.tbl_group))
