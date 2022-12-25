# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class CurrencyExchange(Document):
	
	def on_submit(self):
		data = frappe.db.get_list("Payment Type",{"currency":self.to_currency} )
		if data:
			for d in data:
				doc = frappe.get_doc("Payment Type", d.name)
				doc.exchange_rate = self.exchange_rate
				doc.save()
