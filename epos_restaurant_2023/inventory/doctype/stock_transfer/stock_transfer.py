# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from py_linq import Enumerable
from epos_restaurant_2023.inventory.inventory import get_stock_location_product, get_uom_conversion, update_product_quantity
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
from frappe.model.document import Document

class StockTransfer(Document):
	def validate(self):
		if self.from_stock_location == self.to_stock_location:
			frappe.throw("Cannot transfer to the same stock location.")
		for d in self.stock_transfer_products:
			d.amount = d.quantity * d.cost

		total_quantity = Enumerable(self.stock_transfer_products).sum(lambda x: x.quantity or 0)
		total_amount = Enumerable(self.stock_transfer_products).sum(lambda x: (x.quantity or 0)* (x.cost or  0))

		self.total_quantity = total_quantity
		self.total_amount = total_amount
	def on_submit(self):
		for p in self.stock_transfer_products:
			if(p.is_inventory_product):
				current_stock = get_stock_location_product(self.from_stock_location, p.product_code)
				if current_stock is None or current_stock.quantity < 1:
					frappe.throw("{} is not available in stock".format(p.product_code))
				elif current_stock.quantity < p.quantity:
					frappe.throw("{} is available only {} in stock".format(p.product_code, current_stock.quantity))
				else:
					update_inventory_on_submit(self)
					#frappe.enqueue("epos_restaurant_2023.inventory.doctype.stock_transfer.stock_transfer.update_inventory_on_submit", queue='short', self=self)
			
	def on_cancel(self):
		update_inventory_on_cancel(self)
	# 	frappe.enqueue("epos_restaurant_2023.inventory.doctype.stock_transfer.stock_transfer.update_inventory_on_cancel", queue='short', self=self)


	def before_submit(self):
		for d in self.stock_transfer_products:
			if d.is_inventory_product:
				if d.unit !=d.base_unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw("There is no UoM conversion from {} to {}".format(d.base_unit, d.unit))

def update_inventory_on_submit(self):
	for p in self.stock_transfer_products:
		if p.is_inventory_product:
			update_to_stock(cancel=False, self=self,p=p)
			update_from_stock(cancel=False, self=self,p=p)

def update_inventory_on_cancel(self):
	for p in self.stock_transfer_products:
		if p.is_inventory_product:
			update_to_stock(cancel=True, self=self, p=p)
			update_from_stock(cancel=True, self=self,p=p)

def update_to_stock(cancel = False, self=None, p=None):
	inv=frappe.db.sql("select balance from `tabInventory Transaction` where stock_location = '{}' and product_code='{}' order by creation desc limit 1".format(self.to_stock_location, p.product_code), as_dict=1)
	
	quantity_on_hand = 0
	if inv:
		quantity_on_hand = inv[0].balance or 0
	else:
		# insert product to stock location product
		update_product_quantity(product_code=p.product_code, stock_location=self.to_stock_location,quantity=0,cost=0, doc=None)
	
	
	current_stock = get_stock_location_product(self.to_stock_location, p.product_code)
	uom_conversion = get_uom_conversion(p.base_unit, p.unit)
	ending_stock_value = 0

	if cancel:
		ending_stock_value =  current_stock.total_cost  - (( p.quantity / uom_conversion) * p.cost)
	else:
		ending_stock_value =  current_stock.total_cost  + (( p.quantity / uom_conversion) * p.cost)

	doc = frappe.get_doc({
		'doctype': 'Inventory Transaction',
		'transaction_type':"Stock Transfer",
		'transaction_date':self.posting_date,
		'transaction_number':self.name,
		'product_code': p.product_code,
		'unit':p.unit,
		'stock_location':self.to_stock_location,
		'quantity_on_hand': quantity_on_hand,
		'in_quantity': 0 if cancel else p.quantity / uom_conversion,
		'out_quantity': p.quantity / uom_conversion if cancel else 0,
		'balance': quantity_on_hand-(p.quantity / uom_conversion) if cancel else (p.quantity / uom_conversion) + quantity_on_hand,
		"uom_conversion":uom_conversion,
		"price":p.cost,
		"beginning_stock_value":current_stock.total_cost,
		"ending_stock_value":ending_stock_value,
		'note': "New stock transfer from {} to {} submitted.".format(self.from_stock_location,self.to_stock_location)
	})
	doc.insert()
	update_product_quantity(doc=current_stock, stock_location=self.to_stock_location, product_code=p.product_code, quantity=p.quantity / uom_conversion, cost=(p.amount/p.quantity)*uom_conversion)

def update_from_stock(cancel = False, self=None, p=None):
	inv=frappe.db.sql("select balance from `tabInventory Transaction` where stock_location = '{}' and product_code='{}' order by creation desc limit 1".format(self.from_stock_location, p.product_code), as_dict=1)
	quantity_on_hand = 0
	current_stock = get_stock_location_product(self.from_stock_location, p.product_code)
	if inv:
		quantity_on_hand = inv[0].balance or 0

	uom_conversion = get_uom_conversion(p.base_unit, p.unit)
	if cancel:
		ending_stock_value =  current_stock.total_cost  + (( p.quantity / uom_conversion) * p.cost)
	else:
		ending_stock_value =  current_stock.total_cost  - (( p.quantity / uom_conversion) * p.cost)

	doc = frappe.get_doc({
		'doctype': 'Inventory Transaction',
		'transaction_type':"Stock Transfer",
		'transaction_date':self.posting_date,
		'transaction_number':self.name,
		'product_code': p.product_code,
		'unit':p.unit,
		'stock_location':self.from_stock_location,
		'quantity_on_hand': quantity_on_hand,
		'out_quantity':0 if cancel else p.quantity / uom_conversion,
		'in_quantity':p.quantity / uom_conversion if cancel else 0,
		'balance':(p.quantity / uom_conversion) + quantity_on_hand if cancel else quantity_on_hand - (p.quantity / uom_conversion),
		"uom_conversion":uom_conversion,
		"beginning_stock_value":current_stock.total_cost,
		"ending_stock_value":ending_stock_value,
		"price":p.cost,
		'note': "New stock transfer from {} to {} submitted.".format(self.from_stock_location,self.to_stock_location)
	})
	doc.insert()
	update_product_quantity(doc=current_stock, stock_location=self.from_stock_location, product_code=p.product_code, quantity=(p.quantity / uom_conversion) * (1 if cancel else -1), cost=(p.amount/p.quantity)*uom_conversion)