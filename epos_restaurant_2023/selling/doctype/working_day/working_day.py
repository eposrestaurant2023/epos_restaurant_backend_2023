# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class WorkingDay(Document):
    
	def validate(self):

	 

		if self.is_new():
			if frappe.db.exists('Working Day', {'pos_profile': self.pos_profile, 'is_closed': 0}):
				frappe.throw("This pos profile {} is already opened".format(self.pos_profile))
		
	


		#if close shift check current bill open 
		if self.is_closed==1:
			# validate cashier shift open 
			pending_cashier_shift = frappe.db.sql("select name from `tabCashier Shift` where is_closed  = 0 and working_day = '{}'".format(self.name), as_dict=1)
			if pending_cashier_shift:
				frappe.throw("Please close cashier shift first.")

			pending_orders = frappe.db.sql("select name from `tabSale` where docstatus = 0 and working_day = '{}'".format(self.name), as_dict=1)
			if pending_orders:
				frappe.throw("Please close all pending order before close close working day.")
