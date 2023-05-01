# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import json
import frappe
from frappe.model.document import Document

class ComboGroup(Document):
	def validate(self):
		#generate combo menu to json and update to combo menu data 
		if  self.products:
			combo_menus = []
			for m in self.products:
				combo_menus.append({
					"product_code":m.product,
					"product_name":m.product_name,
					"unit":m.unit,
					"quantity":m.quantity,
					"price":m.price
				})
			self.combo_menu_data = json.dumps(combo_menus)

