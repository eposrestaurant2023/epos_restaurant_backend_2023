# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from py_linq import Enumerable
class StockAdjustment(Document):
	def validate(self):
		if self.products:
			for d in self.products:
				d.total_cost = d.quantity * d.cost

			total_current_quantity = Enumerable(self.products).sum(lambda x: x.quantity or 0)
			total_current_cost = Enumerable(self.products).sum(lambda x: x.cost or 0)
			total_quantity = Enumerable(self.products).sum(lambda x: x.old_quantity or 0)
			total_cost = Enumerable(self.products).sum(lambda x: x.old_cost or 0)
   

			self.total_current_quantity = total_current_quantity
			self.total_current_cost = total_current_cost
   
			self.total_quantity = total_quantity
			self.total_cost = total_cost
   
