# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document

class CategoryNote(Document):
	def validate(self):
		if not self.category_note_name_kh:
			self.category_note_name_kh = self.category_note_name_en
