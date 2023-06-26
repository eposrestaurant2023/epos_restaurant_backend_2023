# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document
class InventoryTransaction(Document):
	def validate(self):
		data = frappe.db.sql("select name, quantity, cost from `tabStock Location Product` where product_code ='{}' and stock_location = '{}' limit 1".format(self.product_code, self.stock_location), as_dict=1)
		if data:
			current_qty = data[0]["quantity"]

			current_cost = data[0]["cost"]
			if (current_cost or 0) == 0:
				#get cost from product
				p = frappe.get_doc("Product",self.product_code, "cost")
				current_cost = p.cost or 0
			
			self.price = self.price or current_cost
			
			self.beginning_stock_value = current_qty * current_cost
			self.quantity_on_hand = current_qty
			self.balance = self.quantity_on_hand + self.in_quantity - self.out_quantity
			if self.transaction_type =='Stock Adjustment' and self.action =="Submit":
				self.ending_stock_value = self.balance * self.price
			else:
				self.ending_stock_value = self.beginning_stock_value + (self.in_quantity - self.out_quantity) * (self.price or current_cost)
			
			self.product_has_in_stock_location = 1
			self.stock_location_product_name = data[0]["name"]
		else:
			self.product_has_in_stock_location = 0
			self.balance = self.in_quantity - self.out_quantity
			if (self.price or 0)==0:
				p = frappe.get_doc("Product",self.product_code, "cost")
				self.price = p.cost or 0
    
			if self.transaction_type =='Stock Adjustment' and self.action =="Submit":
				self.ending_stock_value = self.balance * self.price
			else:
				self.ending_stock_value =  (self.in_quantity - self.out_quantity) * (self.price or 0 )
		
		

	def after_insert(self):
		
		if self.product_has_in_stock_location==0:
			add_stock_location_product(self)
		else:
			update_stock_location_product(self)
        

def add_stock_location_product(self):
	cost = 0
	if self.transaction_type=="Stock Adjustment" and self.action =="Submit":
		cost = self.price
	else:
		cost = self.ending_stock_value / self.balance
		
	 
	doc = frappe.get_doc({
			"doctype":"Stock Location Product",
			"product_code" : self.product_code,
			"stock_location" : self.stock_location, 
			"cost" : cost,
			"quantity" : self.balance,
			"total_cost" :  cost * (self.balance or 0)
		}
	)

	doc.insert()

def update_stock_location_product(self):
	doc = frappe.get_doc("Stock Location Product",self.stock_location_product_name )
	balance = 1 if self.balance == 0 else self.balance

	if self.transaction_type=="Stock Adjustment":
		if self.action =="Submit":
			doc.cost = self.price
		else:
			doc.cost = self.price
	else:
			doc.cost = self.ending_stock_value / balance

	doc.quantity = self.balance
	doc.total_cost =  self.ending_stock_value
	
	doc.save()
