# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document
import frappe
from passlib.hash import pbkdf2_sha256

class ResetData(Document):
	def validate(self):
		if self.password is None or self.password == "":
			frappe.throw("Please Enter Password")
		else:
			user = frappe.db.sql("select password from __Auth where name ='Administrator' and doctype='User' and fieldname='password'",as_dict=True)
			if pbkdf2_sha256.verify(self.password, user[0].password):
				frappe.msgprint("")
			else:
				frappe.throw("Wrong Password")