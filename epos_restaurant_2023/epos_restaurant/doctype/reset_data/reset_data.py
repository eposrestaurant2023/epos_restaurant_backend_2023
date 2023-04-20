# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

# import frappe
from frappe.model.document import Document
import frappe
from passlib.hash import pbkdf2_sha256

class ResetData(Document):
	def validate(self):
			if self.stored_password is None or self.stored_password == "" : 
				self.stored_password = self.password
			if self.stored_password is None or self.stored_password == "":
				frappe.throw("Please Enter Password")
			else:
				user = frappe.db.sql("select password from __Auth where name ='Administrator' and doctype='User' and fieldname='password'",as_dict=True)
				if not pbkdf2_sha256.verify(self.stored_password, user[0].password):
					frappe.throw("Wrong Password")

	def on_submit(self):
		if self.transaction_type == "Reset Database":
			frappe.call('epos_restaurant_2023.install.reset_database')
		elif self.transaction_type == "Reset Sale Transaction":
			frappe.call('epos_restaurant_2023.install.reset_sale_transaction')
		else:
			frappe.msgprint("?")
		pass

	def on_update(self):
		frappe.db.sql("""update `tabReset Data` set stored_password = '' where docstatus = 1 and name = '{0}'""".format(self.name))
		self.reload()
		

	