# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt
import json
from py_linq import Enumerable
import frappe
from frappe.model.document import Document
from frappe.model.naming import NamingSeries
class CashierShift(Document):
	def validate(self):
		# #if close shift check current bill open 
		# if self.is_closed==1:
		# 	pending_orders = frappe.db.sql("select name from `tabSale` where docstatus = 0 and cashier_shift = '{}'".format(self.name), as_dict=1)
		# 	if pending_orders:
		# 		frappe.throw("Please close all pending order before close cashier shift.")
		
		if self.is_new():
			data = frappe.get_list("Cashier Shift",filters={"pos_profile":self.pos_profile,"business_branch":self.business_branch, "is_closed":0})
			
			if data:
				frappe.throw("Cashier shift is already opened")
				
		
		for c in self.cash_float:
			exchange_rate = frappe.get_value("Payment Type", c.payment_method,"exchange_rate")
			exchange_rate = exchange_rate or 1
			c.exchange_rate = exchange_rate
			
			c.opening_amount = float(c.input_amount) / exchange_rate
			c.close_amount = float(c.input_close_amount) / exchange_rate
			c.system_close_amount = float(c.input_system_close_amount) / exchange_rate

   
		self.total_opening_amount = Enumerable(self.cash_float).sum(lambda x: x.opening_amount)
		self.total_system_close_amount = Enumerable(self.cash_float).sum(lambda x: x.system_close_amount)
		self.total_close_amount = Enumerable(self.cash_float).sum(lambda x: x.close_amount)
		self.total_different_amount = self.total_close_amount -  self.total_system_close_amount

		# check if close shift then check 
		if self.is_closed==1:
			pos_profile = frappe.get_doc("POS Profile", self.pos_profile)
			if pos_profile.reset_waiting_number_after=="Close Cashier Shift":
				prefix = pos_profile.waiting_number_prefix.replace('.','').replace("#",'')
				naming_series = NamingSeries(prefix)
				naming_series.update_counter(0)




	def after_insert(self):
		
		frappe.db.sql("update `tabSale` set cashier_shift='{}' where docstatus = 0 and pos_profile = '{}'".format(self.name, self.pos_profile))


		