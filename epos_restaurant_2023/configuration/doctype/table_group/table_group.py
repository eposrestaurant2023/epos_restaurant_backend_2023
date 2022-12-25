# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document

class TableGroup(Document):
	def validate(self):
		if not self.table_group_name_kh:
			self.table_group_name_kh = self.table_group_name_en
