# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class WorkingDay(Document):
    
	def validate(self):
		if self.is_new():
			if frappe.db.exists('Working Day', {'pos_profile': self.pos_profile, 'is_closed': 0}):
				frappe.throw("This pos profile {} is already opened".format(self.pos_profile))
				