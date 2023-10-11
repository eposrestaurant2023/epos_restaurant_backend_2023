# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.model.document import Document
from decimal import Decimal

class PurchaseOrderPayment(Document):
	def validate(self):
		if (self.exchange_rate or 0) ==0 or self.currency == frappe.db.get_default("currency"):
			self.exchange_rate = 1
   
		self.payment_amount = self.input_amount /self.exchange_rate 
   		
		if (self.payment_amount or 0) ==0:
			frappe.throw(_("Please enter payment amount"))

		#validate expense if is a submitted expense
		purchase_order_status = frappe.db.get_value("Purchase Order",self.purchase_order,"docstatus")
		
		if not purchase_order_status==1:
			frappe.throw("This purchase order is not submitted yet")

		#check paid amount cannot over balance
		currency_precision = frappe.db.get_single_value('System Settings', 'currency_precision')
		if currency_precision=='':
			currency_precision = "2"

		balance =  round(self.payment_amount  , int(currency_precision))-  round((self.balance or 0)  , int(currency_precision))
		if balance<0:
			balance = 0
		if balance  > 0:
			frappe.throw("Payment amount cannot greater than purchase balance")

	def on_submit(self):
		update_purchase_order(self)

	
	def on_cancel(self):
		update_purchase_order(self)


def update_purchase_order(self):
	data = frappe.db.sql("select  ifnull(sum(payment_amount),0)  as total_paid from `tabPurchase Order Payment` where docstatus=1 and purchase_order='{}'".format(self.purchase_order))
	purchase_order_amount = frappe.db.get_value('Purchase Order', self.purchase_order, 'grand_total')
	
	if data and purchase_order_amount:
		frappe.db.set_value('Purchase Order', self.purchase_order,  {
			'total_paid': data[0][0] ,
			'balance': purchase_order_amount - data[0][0]  
		})
