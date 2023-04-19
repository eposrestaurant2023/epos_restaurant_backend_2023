# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document
import frappe
from passlib.hash import pbkdf2_sha256

class ResetData(Document):
	def validate(self):
		if self.password is None or self.password == "":
			frappe.throw("shit")
		else:
			users = frappe.db.sql("""select name, password from __Auth where doctype='User' and fieldname='password' and encrypted = 0""",as_dict=True)
			for user in users:
				if pbkdf2_sha256.verify(self.password, user.password):
					frappe.throw(user.name)
				
			