# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.utils.nestedset import NestedSet

class KitchenGroup(NestedSet):
	def validate(self):
		if not self.kitchen_name_kh:
			self.kitchen_name_kh = self.kitchen_group_en
