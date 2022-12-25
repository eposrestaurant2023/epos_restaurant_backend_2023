# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.model.document import Document

class ExpensePayment(Document):
	def validate(self):
		#validate expense if is a submitted expense
		expense_status = frappe.db.get_value("Expense",self.expense,"docstatus")
		
		if not expense_status==1:
			frappe.throw(_("This expense is not submitted yet"))
   
		#check paid amount cannot over balance
		if self.payment_amount > self.balance:
			frappe.throw(_("Payment amount cannot greater than expense balance"))
   
	def on_submit(self):
		update_expense(self)
	
	def on_cancel(self):
		update_expense(self)

def update_expense(self):
	data = frappe.db.sql("select  ifnull(sum(payment_amount),0)  as total_paid from `tabExpense Payment` where docstatus=1 and expense='{}'".format(self.expense))
	expense_amount = frappe.db.get_value('Expense', self.expense, 'total_amount')
	
	if data and expense_amount:
		
		frappe.db.set_value('Expense', self.expense,  {
			'total_paid': data[0][0] ,
			'balance': expense_amount -data[0][0]  
		})

		