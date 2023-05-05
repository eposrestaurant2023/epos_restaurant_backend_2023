# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CurrencyExchange(Document):
	def validate(self):
		
		if  frappe.db.get_default("currency") == self.from_currency:
			self.exchange_rate = self.exchange_rate_input
		else:
			if frappe.db.get_default("exchange_rate_main_currency") == self.from_currency and    frappe.db.get_default("currency")  != frappe.db.get_default("exchange_rate_main_currency"):
				self.exchange_rate = 1/ self.exchange_rate_input
		
		if self.from_currency == self.to_currency:
			self.exchange_rate_input = 1
			self.exchange_rate = 1

	def on_submit(self):
		currency = self.to_currency
		if frappe.db.get_default("exchange_rate_main_currency") == self.from_currency and    frappe.db.get_default("currency")  != frappe.db.get_default("exchange_rate_main_currency"):
			currency = self.from_currency

		frappe.db.sql("update `tabPayment Type` set exchange_rate = {} where currency='{}'".format(self.exchange_rate,currency))
		#update to pos profile payment type
		sql = "update `tabPOS Config Payment Type` set exchange_rate = {} where currency='{}'".format(self.exchange_rate,currency)
		frappe.db.sql(sql)


