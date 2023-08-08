# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.model.document import Document

class SalePayment(Document):
	def validate(self):
		if (self.exchange_rate or 0) ==0 or self.currency == frappe.db.get_default("currency"):
			self.exchange_rate = 1
   
		self.payment_amount = self.input_amount /self.exchange_rate 
   		
		if (self.payment_amount or 0) ==0:
			frappe.throw(_("Please enter payment amount"))
   
		#validate expense if is a submitted expense
		sale_status = frappe.db.get_value("Sale",self.sale,"docstatus")
		
		if not sale_status==1:
			frappe.throw(_("This sale is not submitted yet"))

		#check paid amount cannot over balance
		if self.check_valid_payment_amount:
			if self.payment_amount > self.balance:
				frappe.throw("Payment amount cannot greater than sale balance")

	def on_submit(self):
		update_sale(self)
	
	def on_cancel(self):
		update_sale(self)


def update_sale(self):
	data = frappe.db.sql("select  ifnull(sum(payment_amount),0)  as total_paid from `tabSale Payment` where docstatus=1 and sale='{}' and payment_amount>0".format(self.sale))
	sale_amount = frappe.db.get_value('Sale', self.sale, 'grand_total')
	currency_precision = frappe.db.get_single_value('System Settings', 'currency_precision')
	if currency_precision=='':
		currency_precision = "2"

	
	if data and sale_amount:
		balance =round(sale_amount  , int(currency_precision))-  round(data[0][0]    , int(currency_precision))  

		if balance<0:
			balance = 0
		frappe.db.set_value('Sale', self.sale,  {
			'total_paid': data[0][0] ,
			'balance': balance  
		})

