# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from py_linq import Enumerable
from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, get_product_cost, get_stock_location_product, get_uom_conversion, update_product_quantity
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
from frappe.model.document import Document

class StockTransfer(Document):
	def validate(self):
		if self.from_stock_location == self.to_stock_location:
			frappe.throw("Cannot transfer to the same stock location.")
		for d in self.stock_transfer_products:
			d.base_cost  = get_product_cost(self.from_stock_location, d.product_code)
			d.cost = d.base_cost
			d.amount = d.quantity * d.cost
			if d.base_unit != d.unit:
				uom_conversion = get_uom_conversion(d.base_unit, d.unit)
				
				d.cost = d.cost / uom_conversion
				d.amunt = d.cost * d.quantity

		total_quantity = Enumerable(self.stock_transfer_products).sum(lambda x: x.quantity or 0)
		total_amount = Enumerable(self.stock_transfer_products).sum(lambda x: (x.quantity or 0)* (x.cost or  0))

		self.total_quantity = total_quantity
		self.total_amount = total_amount
  
	def before_submit(self):
		for p in self.stock_transfer_products:
			if(p.is_inventory_product):
				if p.unit !=p.base_unit:
					if not check_uom_conversion(p.base_unit, p.unit):
						frappe.throw("There is no UoM conversion from {} to {}".format(p.base_unit, p.unit))
      
				current_stock = get_stock_location_product(self.from_stock_location, p.product_code)
				if current_stock is None or current_stock.quantity <= 0:
					frappe.throw("{} is not available in {}".format(p.product_code, self.from_stock_location))
				else:
					uom_conversion = get_uom_conversion(current_stock.unit, p.unit)
					if current_stock.quantity * uom_conversion < p.quantity:
						frappe.throw(_("{} is available only {} in stock".format(p.product_code, current_stock.quantity)))

	
	def on_submit(self):
		update_inventory_on_submit(self)
		#frappe.enqueue("epos_restaurant_2023.inventory.doctype.stock_transfer.stock_transfer.update_inventory_on_submit", queue='short', self=self)
					
			
	def on_cancel(self):
		update_inventory_on_cancel(self)
	# 	frappe.enqueue("epos_restaurant_2023.inventory.doctype.stock_transfer.stock_transfer.update_inventory_on_cancel", queue='short', self=self)


 
def update_inventory_on_submit(self):
	for p in self.stock_transfer_products:
		if p.is_inventory_product:
      		#frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_submit", queue='short', self=self)
			update_to_stock(cancel=False, self=self,p=p)
			update_from_stock(cancel=False, self=self,p=p)

def update_inventory_on_cancel(self):
	for p in self.stock_transfer_products:
		if p.is_inventory_product:
			update_to_stock(cancel=True, self=self, p=p)
			update_from_stock(cancel=True, self=self,p=p)

def update_to_stock(cancel = False, self=None, p=None):
	uom_conversion = get_uom_conversion(p.base_unit, p.unit)
	
	add_to_inventory_transaction({
		'doctype': 'Inventory Transaction',
		'transaction_type':"Stock Transfer",
		'transaction_date':self.posting_date,
		'transaction_number':self.name,
		'product_code': p.product_code,
		'unit':p.unit,
		'stock_location':self.to_stock_location,
		'in_quantity': 0 if cancel else p.quantity / uom_conversion,
		'out_quantity': p.quantity / uom_conversion if cancel else 0,
		"uom_conversion":uom_conversion,
		"price":p.base_cost,
		'note': "New stock transfer from {} to {} submitted.".format(self.from_stock_location,self.to_stock_location),
		"action": "Cancel" if cancel else "Submit"
	})
def update_from_stock(cancel = False, self=None, p=None):
	uom_conversion = get_uom_conversion(p.base_unit, p.unit)
	add_to_inventory_transaction({
		'doctype': 'Inventory Transaction',
		'transaction_type':"Stock Transfer",
		'transaction_date':self.posting_date,
		'transaction_number':self.name,
		'product_code': p.product_code,
		'unit':p.unit,
		'stock_location':self.from_stock_location,
		'out_quantity':0 if cancel else p.quantity / uom_conversion,
		'in_quantity':p.quantity / uom_conversion if cancel else 0,
		'note': "New stock transfer from {} to {} submitted.".format(self.from_stock_location,self.to_stock_location),
  		"action": "Cancel" if cancel else "Submit"
	})