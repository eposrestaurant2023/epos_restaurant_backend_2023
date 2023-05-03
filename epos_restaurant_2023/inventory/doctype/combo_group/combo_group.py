# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import json
import frappe
from frappe.model.document import Document
from py_linq import Enumerable
class ComboGroup(Document):
	def validate(self):

		#generate combo menu to json and update to combo menu data 
		description = []
		if  self.products:
			if len(self.products) < self.item_selection:
				frappe.throw("Item selection cannot greater than total menu count")
			
			combo_menus = []
			for m in self.products:
				description.append("{} x {}".format(m.product_name, m.quantity))
				combo_menus.append({
					"menu_name":m.name,
					"product_code":m.product,
					"product_name":m.product_name,
					"unit":m.unit,
					"quantity":m.quantity,
					"price":m.price,
					"photo":m.photo
				})
			self.combo_menu_data = json.dumps(combo_menus)
		self.total_quantity = Enumerable(self.products).sum(lambda x: (x.quantity or 1))
		self.total_amount = Enumerable(self.products).sum(lambda x: (x.quantity or 1) * (x.price or 0))
		if description:
			self.description = ','.join(description)
		

	def on_update(self):
		#frappe.throw("you save me")
		frappe.enqueue("epos_restaurant_2023.inventory.doctype.combo_group.combo_group.update_combo_to_product", queue='short', self=self)
		#update_combo_to_product(self)

def update_combo_to_product(self):
	products =  frappe.db.sql("select distinct parent  as product from `tabProduct Combo Group` where combo_group='{}'".format(self.name),as_dict=1)
	for p in products:
		doc = frappe.get_doc("Product",p["product"])
		doc.save()