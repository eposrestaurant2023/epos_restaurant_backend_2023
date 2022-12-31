# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class StockLocationProduct(Document):
	pass

@frappe.whitelist()
def get_stock_location_product(stock_location=None, product_code = None):
    blank_data = {"cost":0,'quantity':0}
    if stock_location and product_code:
        data = frappe.get_value('Stock Location Product', {"stock_location": stock_location, "product_code": product_code}, ['quantity', 'cost'], as_dict=1)
        return data if data else blank_data
    return blank_data