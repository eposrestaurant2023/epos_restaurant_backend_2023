# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt


from epos_restaurant_2023.inventory.inventory import get_uom_conversion, update_product_quantity,get_stock_location_product
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
import frappe
from frappe import _
from py_linq import Enumerable
from frappe.model.document import Document

class PurchaseOrder(Document):
	def validate(self):
		#validate sale summary
		for d in self.purchase_order_products:
			d.amount = d.quantity * d.cost

		total_quantity = Enumerable(self.purchase_order_products).sum(lambda x: x.quantity or 0)
		sub_total = Enumerable(self.purchase_order_products).sum(lambda x: (x.quantity or 0)* (x.cost or  0))

		self.total_quantity = total_quantity
		self.sub_total = sub_total
		self.grand_total = sub_total - (self.total_discount or 0)
		self.balance = self.grand_total  - (self.total_paid or 0)
		if self.balance == 0:
			frappe.throw("Total amount is 0")

	def on_submit(self):
		update_inventory_on_submit(self)
		#frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_submit", queue='short', self=self)
	
	def on_cancel(self):
		update_inventory_on_cancel(self)
		#frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_cancel", queue='short', self=self)


	def before_submit(self):
		for d in self.purchase_order_products:
			if d.is_inventory_product:
				if d.base_unit != d.unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion from {} to {}".format(d.base_unit, d.unit)))


def update_inventory_on_submit(self):
	for p in self.purchase_order_products:
		if p.is_inventory_product:
			current_stock = get_stock_location_product(self.stock_location, p.product_code)
			inv=frappe.db.sql("select balance from `tabInventory Transaction` where stock_location = '{}' and product_code='{}' order by creation desc limit 1".format(self.stock_location, p.product_code), as_dict=1)
			quantity_on_hand = 0
			if inv:
				quantity_on_hand = inv[0].balance or 0
				
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
				
			doc = frappe.get_doc({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Purchase Order",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'quantity_on_hand': quantity_on_hand,
				'in_quantity':p.quantity / uom_conversion,
				'balance':(p.quantity / uom_conversion) + quantity_on_hand,
				"uom_conversion":uom_conversion,
				"price":p.cost,
				'note': 'New purchase order submitted.'
			})
			doc.insert()
			update_product_quantity(stock_location=self.stock_location, product_code=p.product_code, quantity=p.quantity / uom_conversion, cost=(p.amount/p.quantity)*uom_conversion, doc=current_stock)

def update_inventory_on_cancel(self):
	for p in self.purchase_order_products:
		if p.is_inventory_product:
			current_stock = get_stock_location_product(self.stock_location, p.product_code)
			inv=frappe.db.sql("select balance from `tabInventory Transaction` where stock_location = '{}' and product_code='{}' order by creation desc limit 1".format(self.stock_location, p.product_code), as_dict=1)
			quantity_on_hand = 0
			if inv:
				quantity_on_hand = inv[0].balance or 0
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			doc = frappe.get_doc({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Purchase Order",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'quantity_on_hand': quantity_on_hand,
				'out_quantity':p.quantity / uom_conversion,
				'balance': quantity_on_hand-(p.quantity / uom_conversion) ,
				"uom_conversion":uom_conversion,
				"price":p.cost,
				'note': 'Purchase order cancelled.'
			})
			doc.insert()
			update_product_quantity(stock_location=self.stock_location, product_code=p.product_code, quantity=p.quantity / uom_conversion * -1, cost=((p.amount/p.quantity)*uom_conversion), doc=current_stock)