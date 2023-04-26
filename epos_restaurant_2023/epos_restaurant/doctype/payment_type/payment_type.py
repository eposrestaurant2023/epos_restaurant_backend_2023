# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class PaymentType(Document):
	def validate(self):
		 
		data = frappe.db.sql("select exchange_rate from `tabCurrency Exchange` where to_currency='{}' order by posting_date desc limit 1".format(self.currency))
		if data:
			 
			self.exchange_rate = data[0][0] or 1
		else:
			self.exchange_rate  =1 
	
	def on_update(self):
		if self.has_value_changed("payment_type_group"):
			frappe.db.sql("update `tabSale Payment` set payment_type_group='{}' where payment_type='{}'".format(self.payment_type_group,self.name))
   

	