# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class ModifierCategory(Document):
	def on_update(self):
		frappe.db.sql("update `tabModifiers` set prefix = '{}' where modifier_category='{}'".format(self.prefix,self.name))
  
		frappe.enqueue("epos_restaurant_2023.inventory.doctype.product.product.update_product_to_temp_product_menu", queue='short')
     
