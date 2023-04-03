# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt
from frappe.model import no_value_fields
import frappe
from frappe.model.document import Document

class ePOSSettings(Document):

	def on_update(self):
		
		for df in self.meta.get("fields"):
			if df.fieldtype not in no_value_fields and self.has_value_changed(df.fieldname):
				frappe.db.set_default(df.fieldname, self.get(df.fieldname))
