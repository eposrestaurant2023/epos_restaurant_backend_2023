# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import json
import frappe
from frappe.model.document import Document

class TaxRule(Document):
	def validate(self):
		self.tax_rule_data = json.dumps(
			{
				"name":self.name,
				"tax_1_rate":self.tax_1_rate,
				"tax_1_name":self.tax_1_name,
				"percentage_of_price_to_calculate_tax_1":self.percentage_of_price_to_calculate_tax_1,
				"calculate_tax_1_after_discount":self.calculate_tax_1_after_discount,
				"tax_1_account":self.tax_1_account,
				"tax_2_name":self.tax_2_name,
				"tax_2_rate":self.tax_2_rate,
				"percentage_of_price_to_calculate_tax_2":self.percentage_of_price_to_calculate_tax_2,
				"calculate_tax_2_after_discount":self.calculate_tax_2_after_discount,
				"calculate_tax_2_after_adding_tax_1":self.calculate_tax_2_after_adding_tax_1,
				"tax_2_account":self.tax_2_account,
				"tax_3_name":self.tax_3_name,
				"tax_3_rate":self.tax_3_rate,
				"percentage_of_price_to_calculate_tax_3":self.percentage_of_price_to_calculate_tax_3,
				"calculate_tax_3_after_discount":self.calculate_tax_3_after_discount,
				"calculate_tax_3_after_adding_tax_1":self.calculate_tax_3_after_adding_tax_1,
				"calculate_tax_3_after_adding_tax_2":self.calculate_tax_3_after_adding_tax_2,
				"tax_3_account":self.tax_3_account
				}
		) 
	
	def after_rename(self, old_name, new_name, merge=False):
		doc = frappe.get_doc("Tax Rule",new_name)
		doc.save()

	def on_update(self): 
		frappe.db.sql("update `tabTemp Product Menu` set tax_rule_data='{}' where tax_rule='{}'".format(self.tax_rule_data, self.name))
		frappe.db.sql("update `tabPOS Profile Tax Rule` set tax_rule_data='{}' where tax_rule='{}'".format(self.tax_rule_data, self.name))
	
