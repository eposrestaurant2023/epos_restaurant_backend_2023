# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):
	return ["name", "product_name_en","product_name_kh","photo"], frappe.db.sql("select * from `tabProduct`", as_dict=1)

