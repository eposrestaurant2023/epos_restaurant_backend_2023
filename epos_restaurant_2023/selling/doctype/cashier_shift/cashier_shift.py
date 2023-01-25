# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt
from py_linq import Enumerable
import frappe
from frappe.model.document import Document

class CashierShift(Document):
	def validate(self):
		#if close shift check current bill open 
		if self.is_closed==1:
			pending_orders = frappe.db.sql("select name from `tabSale` where docstatus = 0 and cashier_shift = '{}'".format(self.name), as_dict=1)
			if pending_orders:
				frappe.throw("Please close all pending order before close cashier shift.")

		
		for c in self.cash_float:
			exchange_rate = frappe.get_value("Payment Type", c.payment_method,"exchange_rate")
			exchange_rate = exchange_rate or 1
			c.exchange_rate = exchange_rate
			
			c.opening_amount = float(c.input_amount) / exchange_rate
			c.close_amount = float(c.input_close_amount) / exchange_rate

   
		self.total_opening_amount = Enumerable(self.cash_float).sum(lambda x: x.opening_amount)
		self.total_system_close_amount = Enumerable(self.cash_float).sum(lambda x: x.system_close_amount)
		self.total_close_amount = Enumerable(self.cash_float).sum(lambda x: x.close_amount)
		self.total_different_amount = self.total_close_amount -  self.total_system_close_amount
		