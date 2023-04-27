# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt
import frappe
from frappe import _
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
from frappe.model.document import Document

class ModifierCode(Document):
	def validate(self):
		#validate product recipe
		for d in self.product_recipe:
			if d.unit != d.base_unit:
				if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(d.product, d.product_name, d.base_unit, d.unit)))

