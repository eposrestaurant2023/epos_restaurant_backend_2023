# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt


from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, get_uom_conversion, update_product_quantity,get_stock_location_product
from epos_restaurant_2023.inventory.inventory import check_uom_conversion
import frappe
from frappe import _
from py_linq import Enumerable
from frappe.model.document import Document
from decimal import Decimal


class PurchaseOrder(Document):
	def validate(self):
		validate_po_discount(self)
		#validate sale summary
		total_quantity = Enumerable(self.purchase_order_products).sum(lambda x: x.quantity or 0)
		sub_total = Enumerable(self.purchase_order_products).sum(lambda x: (x.quantity or 0)* (x.cost or  0))
		po_discountable_amount =Enumerable(self.purchase_order_products).where(lambda x:(x.discount_amount or 0)==0).sum(lambda x: (x.quantity or 0)* (x.cost or  0))

		self.total_quantity = total_quantity
		self.po_discountable_amount = po_discountable_amount
		self.sub_total = sub_total
		# calculate sale discount
		if self.discount:
			if self.discount_type =="Percent":
				self.po_discount = self.po_discountable_amount * self.discount / 100
			else:
				self.po_discount = self.discount or 0

		self.product_discount = Enumerable(self.purchase_order_products).sum(lambda x: x.discount_amount)
		self.total_discount = (self.product_discount or 0) + (self.po_discount or 0)
		self.grand_total =( sub_total - (self.total_discount or 0))

		currency_precision = frappe.db.get_single_value('System Settings', 'currency_precision')
		if currency_precision=='':
			currency_precision = "2"

		self.balance = round(self.grand_total  , int(currency_precision))-  round((self.total_paid or 0)  , int(currency_precision))

		if self.balance<0:
			self.balance = 0

	def on_submit(self):
		#update_inventory_on_submit(self)
		frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_submit", queue='short', self=self)
	
	def on_cancel(self):
		#update_inventory_on_cancel(self)
		frappe.enqueue("epos_restaurant_2023.purchasing.doctype.purchase_order.purchase_order.update_inventory_on_cancel", queue='short', self=self)


	def before_submit(self):
		for d in self.purchase_order_products:
			if d.is_inventory_product:
				if d.base_unit != d.unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion from {} to {}".format(d.base_unit, d.unit)))


def update_inventory_on_submit(self):
	
	for p in self.purchase_order_products:
		if p.is_inventory_product:
			
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Purchase Order",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'in_quantity':p.quantity / uom_conversion,
				"uom_conversion":uom_conversion,
				"price":p.cost,
				'note': 'New purchase order submitted.',
    			'action': 'Submit'
			})

 
			
def update_inventory_on_cancel(self):
	for p in self.purchase_order_products:
		if p.is_inventory_product:
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Purchase Order",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'out_quantity':p.quantity / uom_conversion,
				"price":p.cost,
				'note': 'Purchase order cancelled.',
				'action': 'Cancel'
			})

def validate_po_discount(self):
	po_discount = self.discount  
	if po_discount>0:
		if self.discount_type=="Amount":
			discountable_amount = Enumerable(self.purchase_order_products).where(lambda x: x.discount==0).sum(lambda x: (x.quantity or 0)* (x.cost or  0))
			po_discount=(po_discount / discountable_amount ) * 100
 
	for d in self.purchase_order_products:
		d.sub_total = (d.quantity or 0) * (d.cost or 0)
		if (d.discount_type or "Percent")=="Percent":
			d.discount_amount = d.sub_total * (d.discount or 0) / 100
		else:
			d.discount_amount = d.discount or 0
		# check if sale has discount
		if po_discount>0 and d.discount==0:
			
			d.po_discount_percent = po_discount  
			d.po_discount_amount = (po_discount/100) * d.sub_total
		else:
			d.po_discount_percent = 0  
			d.po_discount_amount = 0

		d.total_discount = (d.po_discount_amount or 0) + (d.discount_amount or 0)
		d.amount = (d.sub_total - d.discount_amount)