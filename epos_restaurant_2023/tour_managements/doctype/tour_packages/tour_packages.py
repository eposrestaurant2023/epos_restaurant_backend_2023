# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from py_linq import Enumerable
from frappe.model.document import Document

class TourPackages(Document):
	def validate(self):
		self.total_cost =   Enumerable(self.cost_breakdowns).sum(lambda x: (x.rate or 0))
