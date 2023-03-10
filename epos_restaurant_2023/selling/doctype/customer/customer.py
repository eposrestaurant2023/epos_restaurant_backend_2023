# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
from frappe import utils
from frappe import _
import datetime

class Customer(Document):
	def validate(self):
		if self.date_of_birth:
			if self.date_of_birth>utils.today():
				frappe.throw(_("Date of birth cannot be greater than the current time"))

		if not self.customer_name_kh:
			self.customer_name_kh = self.customer_name_en
   
		self.customer_code_name = "{} - {}".format(self.name,self.customer_name_en)
  
@frappe.whitelist()
def get_customer_order_summary(customer):
	total_visit = 0
	total_order = 0
	total_annual_order = 0
	sql = "select count(name) as total_visit, sum(grand_total) as total_amount from `tabSale` where customer='{}' and docstatus=1".format(customer)
	data = frappe.db.sql(sql, as_dict=1)
	today = datetime.date.today()
	year = today.year
	
	if data:
		total_visit = data[0]["total_visit"]
		total_order = data[0]["total_amount"]
		
	sql = "select  sum(grand_total) as total_amount from `tabSale` where posting_date<='{}-01-01' and customer='{}' and docstatus=1".format(year,customer)
	data = frappe.db.sql(sql, as_dict=1)
	if data:
		total_annual_order = data[0].total_amount
  
	return {
		"total_visit":total_visit,
		"total_order":total_order,
		"total_annual_order":total_annual_order
	}
