# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class Drivers(Document):
	def after_insert(self):
		if hasattr(self,"file_name") and self.file_name:
			# update file image upload
			file = frappe.get_doc('File', self.file_name)
			file.attached_to_name = self.name
			file.attached_to_field = 'photo'
			file.attached_to_doctype = 'Drivers'
			file.save()
			frappe.db.commit()
