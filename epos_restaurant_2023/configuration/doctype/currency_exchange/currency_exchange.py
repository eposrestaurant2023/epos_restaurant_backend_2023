# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CurrencyExchange(Document):
	
	def on_submit(self):   
		frappe.db.sql("update `tabPayment Type` set exchange_rate = {} where currency='{}'".format(self.exchange_rate,self.to_currency))
		#update to pos profile payment type
		sql = "update `tabPOS Config Payment Type` set exchange_rate = {} where currency='{}'".format(self.exchange_rate,self.to_currency)
		frappe.db.sql(sql)

	
