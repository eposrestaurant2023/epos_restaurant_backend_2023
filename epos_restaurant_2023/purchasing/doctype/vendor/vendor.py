# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class Vendor(Document):
    def validate(self):
        self.vendor_code_name = "{} - {}".format(self.name,self.vendor_name)
		