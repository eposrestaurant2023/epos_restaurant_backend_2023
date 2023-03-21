# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class SaleStatus(Document):
	def on_update(self):
		frappe.db.sql("update `tabSale` set sale_status_color='{0}', sale_status_priority={1} where sale_status='{2}' ".format( self.background_color,self.priority, self.name))

