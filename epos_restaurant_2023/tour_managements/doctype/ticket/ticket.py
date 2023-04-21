# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.utils.data import strip
from frappe.model.document import Document

class Ticket(Document):
	def autoname(self):
		from frappe.model.naming import set_name_by_naming_series, get_default_naming_series,make_autoname

		if strip(self.naming_series) !="" and strip(self.ticket_code) =="":
			set_name_by_naming_series(self)
			self.ticket_code = self.name		

		self.ticket_code = strip(self.ticket_code)
		self.name = self.ticket_code
