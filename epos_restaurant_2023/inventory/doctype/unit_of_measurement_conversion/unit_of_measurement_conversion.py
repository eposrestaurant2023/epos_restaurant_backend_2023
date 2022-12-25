# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.model.document import Document

class UnitofMeasurementConversion(Document):
	def validate(self):
			exist = frappe.db.exists("Unit of Measurement Conversion",{"from_uom":self.from_uom,"to_uom":self.to_uom})
			if exist:
				if self.is_new():
					frappe.throw(_("Conversion of {} to {} already exist".format(self.from_uom, self.to_uom)))
				else:
					if self.name != exist:
						frappe.throw(_("Conversion of {} to {} already exist".format(self.from_uom, self.to_uom)))
