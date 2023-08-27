# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class Employee(Document):
    pass
	# def on_update(self):
	# 	user_doc = frappe.get_doc("User", "admin@mail.com")
	# 	user_doc.new_password = "HelloWorld"
	# 	user_doc.save()
