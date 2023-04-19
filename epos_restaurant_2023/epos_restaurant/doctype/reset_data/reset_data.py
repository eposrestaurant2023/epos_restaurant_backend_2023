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
			users = frappe.db.sql("""select name,password from __Auth where doctype='User' and fieldname='password' and encrypted = 0""",as_dict=True)
			for user in users:
				
				if pbkdf2_sha256.verify(self.password, user.password):
					frappe.msgprint(user.name)
					user_role_profile = frappe.db.get_list('User',filters={'name': user.name},fields=['role_profile_name'],as_list=True)
					if len(user_role_profile) > 0:
						role_profile = frappe.db.get_list('Role Profile',filters={'name': user_role_profile[0].role_profile_name},fields=['role_profile'],as_list=True)
						if len(role_profile) > 0:
							roles = frappe.db.get_list('Has Role',filters={'parent': role_profile[0].role_profile},fields=['role'],as_list=True)
							if len(roles) > 0:
								if "System Manager" in roles:
									frappe.msgprint("yes")
								else:
									frappe.msgprint("no")
				
			