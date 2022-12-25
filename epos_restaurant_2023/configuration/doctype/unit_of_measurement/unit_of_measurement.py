# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
from frappe.model.document import Document

class UnitOfMeasurement(Document):
	def after_insert(self):
		if not check_uom_conversion(self.unit_name,self.unit_name ):
			doc = frappe.get_doc({
				'doctype': 'Unit of Measurement Conversion',
				'from_uom':self.unit_name,
				'to_uom':self.unit_name,
				'conversion':1
			})
			doc.insert()
