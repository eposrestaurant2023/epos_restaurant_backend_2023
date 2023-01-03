# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, get_product_cost, get_uom_conversion,get_stock_location_product,update_product_quantity
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
import frappe
from frappe import _
from py_linq import Enumerable
from frappe.model.document import Document

class StockTake(Document):
	def validate(self):
		for d in self.stock_take_products:
			d.base_cost  = get_product_cost(self.stock_location, d.product_code)
			d.price = d.base_cost
			d.amount = d.quantity * d.price
			if d.base_unit != d.unit:
				uom_conversion = get_uom_conversion(d.base_unit, d.unit)
				
				d.price = d.price / uom_conversion
				d.amunt = d.price * d.quantity

		total_quantity = Enumerable(self.stock_take_products).sum(lambda x: x.quantity or 0)
		total_amount = Enumerable(self.stock_take_products).sum(lambda x: (x.quantity or 0)* (x.price or  0))

		self.total_quantity = total_quantity
		self.total_amount = total_amount
  
	def before_submit(self):
		for d in self.stock_take_products:
			if d.unit !=d.base_unit:
				if not check_uom_conversion(d.base_unit, d.unit):
					frappe.throw(_("There is no UoM conversion from {} to {}".format(d.base_unit, d.unit)))

				current_stock = get_stock_location_product(self.stock_location, d.product_code)
				
				if current_stock is None or current_stock.quantity <= 0:
					frappe.throw(_("{} is not available in stock".format(d.product_code)))
				else:
					uom_conversion = get_uom_conversion(current_stock.unit, d.unit)
					if current_stock.quantity * uom_conversion < d.quantity:
						frappe.throw(_("{} is available only {} {} in stock".format(d.product_code, current_stock.quantity,current_stock.unit)))
      
	def on_submit(self):
		update_inventory_on_submit(self)
		#frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_submit", queue='short', self=self)
	
	def on_cancel(self):
		update_inventory_on_cancel(self)
		#frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_cancel", queue='short', self=self)
  
def update_inventory_on_submit(self):
	for p in self.stock_take_products:
		if p.is_inventory_product:
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)			
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Stock Take",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'out_quantity':p.quantity / uom_conversion,
				"price":p.base_cost,
				'note': 'New stock take submitted.',
				"action": "Submit"
			})

def update_inventory_on_cancel(self):
	for p in self.stock_take_products:
		if p.is_inventory_product:
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Stock Take",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'in_quantity':p.quantity / uom_conversion,
				"price":p.base_cost,
				'note': 'Stock take cancelled.',
    			"action": "Cancel"
			})