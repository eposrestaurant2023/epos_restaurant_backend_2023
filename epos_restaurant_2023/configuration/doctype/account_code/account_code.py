# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt


import frappe
from frappe.model.document import Document

class AccountCode(Document):
	def validate(self):
		self.account_code_name = self.code + ' - ' + self.account_name
	def on_update(self):
		apps = frappe.get_installed_apps()
		if "edoor" in apps:
			update_tax_to_related_transaction({"account_code":self.name})

@frappe.whitelist()
def update_tax_to_related_transaction(data):
	working_day_data = frappe.db.sql("select max(posting_date) as posting_date from `tabWorking Day`",as_dict=1)
	working_day = working_day_data[0]["posting_date"]
 

	 
	#update to room_rate
	sql="select name from `tabRate Type` where account_code='{}'".format(data["account_code"])
	account_doc = frappe.get_doc("Account Code", data["account_code"])
	rate_types = frappe.db.sql(sql, as_dict=1)
	for r in rate_types:
		if account_doc.allow_tax==1 and account_doc.tax_rule:
			tax_rule = frappe.get_doc("Tax Rule", account_doc.tax_rule)
			frappe.db.sql("""
				update `tabReservation Room Rate` 
		 		set
		 			tax_rule='{}',
		 			tax_rule_data = '{}',
		 			rate_include_tax = '{}'
		 		where
		 			rate_type = '{}' and 
		 			date >= '{}' and
		 			tax_1_rate = 0 and 
		 			tax_2_rate = 0 and 
		 			tax_3_rate = 0

			""".format(
				account_doc.tax_rule,
				tax_rule.tax_rule_data,
				"Yes" if tax_rule.is_rate_include_tax == 1 else "No",
				r["name"],
				working_day
			))
		else:
			
			frappe.db.sql("""
				update `tabReservation Room Rate` 
		 		set
		 			tax_rule='',
		 			tax_rule_data = NULL,
		 			rate_include_tax = 'No'
		 		where
		 			rate_type = '{}' and 
		 			date >= '{}' and
		 			tax_1_rate = 0 and 
		 			tax_2_rate = 0 and 
		 			tax_3_rate = 0

			""".format(
				r["name"],
				working_day
			))


	frappe.db.commit()



			
