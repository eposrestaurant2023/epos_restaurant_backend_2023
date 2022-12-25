# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class PrintBarcode(Document):
   pass

@frappe.whitelist()
def get_print_barcode():
    return frappe.db.get_list("Print Barcode",filters={"disabled": 0}, fields=['name','title','barcode_url'])