# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import json
from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, check_uom_conversion, get_product_cost, get_stock_location_product, get_uom_conversion, update_product_quantity
import frappe
from frappe import utils
from frappe import _
from frappe.utils.data import fmt_money
from py_linq import Enumerable
from frappe.model.document import Document

class Sale(Document):
	def validate(self):

		#frappe.throw(_("Please select your working day"))
		if self.pos_profile:
			if not self.working_day:
				frappe.throw(_("Please select your working day"))

			if not self.cashier_shift: 
				frappe.throw(_("Please select your cashier shift"))

		# if self.posting_date:
		# 	if self.posting_date>utils.today():
		# 		frappe.throw(_("Sale date cannot greater than current date"))
		

		# set waiting number
		if self.is_new():
			if self.waiting_number_prefix:
				from frappe.model.naming import make_autoname
				self.waiting_number = make_autoname(self.waiting_number_prefix)


		if self.discount_type =="Percent" and self.discount> 100:
			frappe.throw(_("Discount percent cannot greater than 100%"))
   
		if self.docstatus ==0:
			if self.working_day:
				is_closed = frappe.db.get_value('Working Day', self.working_day,"is_closed")
				if is_closed==1:
					frappe.throw(_("Working day {} is already closed".format(self.working_day)))

			if self.cashier_shift:
				is_closed = frappe.db.get_value('Cashier Shift', self.cashier_shift,"is_closed")
				if is_closed==1:
					frappe.throw(_("Cashier shift {} is already closed".format(self.cashier_shift)))
		#validate outlet
		if self.outlet and self.business_branch:
			if frappe.get_value("Outlet",self.outlet,"business_branch") != self.business_branch:
				frappe.throw(_("The outlet {} is not belong to business branch {}".format(self.outlet, self.business_branch)))
		
		#validate stock location
		if self.stock_location and self.business_branch:
			if frappe.get_value("Stock Location",self.stock_location,"business_branch") != self.business_branch:
				frappe.throw(_("The stock location {} is not belong to business branch {}".format(self.stock_location, self.business_branch)))
		

		#validate exhcange rate change

		# exchange_rate = frappe.get_last_doc('Currency Exchange', filters={"to_currency": frappe.db.get_default("second_currency")})# frappe.get_last_doc("Currency Exchange",{})
		# if exchange_rate:
		# 	self.exchange_rate = exchange_rate.exchange_rate
		# else:
		# 	self.exchange_rate =1
   
		#validate sale product 
		validate_sale_product(self)
  
		validate_pos_payment(self)
		#validate sale summary

		#set is foc by check payment pyment if have is_foc payment type
		self.is_foc = 0

		
		if Enumerable(self.payment).where(lambda x: (x.is_foc or 0) ==1).count()>=1:
			self.is_foc = 1
  
  
		

		total_quantity = Enumerable(self.sale_products).sum(lambda x: x.quantity or 0)
		sub_total = Enumerable(self.sale_products).sum(lambda x: (x.quantity or 0)* (x.price or  0) + ((x.quantity or 0)*(x.modifiers_price or 0)))
  
		sale_discountable_amount =Enumerable(self.sale_products).where(lambda x:x.allow_discount ==1 and (x.discount_amount or 0)==0).sum(lambda x: (x.quantity or 0)* (x.price or  0) + + ((x.quantity or 0)*(x.modifiers_price or 0)))

		self.total_quantity = total_quantity
		self.sale_discountable_amount = sale_discountable_amount
		self.sub_total = sub_total
		# calculate sale discount
		if self.discount:
			if self.discount_type =="Percent":
				self.sale_discount = self.sale_discountable_amount * self.discount / 100
			else:
				self.sale_discount = self.discount or 0
				if self.discount > self.sale_discountable_amount:
					frappe.throw("Discount amount cannot greater than discountable amount")


		
		self.product_discount = Enumerable(self.sale_products).where(lambda x:x.allow_discount ==1).sum(lambda x: x.discount_amount)
		
		self.total_discount = (self.product_discount or 0) + (self.sale_discount or 0)
  
		#tax 
		self.taxable_amount_1  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_1)
		self.taxable_amount_2  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_2)
		self.taxable_amount_3  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_3)
		self.tax_1_amount  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_1_amount)
		self.tax_2_amount  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_2_amount)
		self.tax_3_amount  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_3_amount)
		self.total_tax  = Enumerable(self.sale_products).where(lambda x:x.tax_rule).sum(lambda x: x.total_tax)

		currency_precision = frappe.db.get_single_value('System Settings', 'currency_precision')
		if currency_precision=='':
			currency_precision = "2"

		self.grand_total =( sub_total - (self.total_discount or 0))  + self.total_tax 
	  
		self.total_paid =  Enumerable(self.payment).where(lambda x: x.payment_type_group !='On Account').sum(lambda x: x.amount or 0)
	
		self.balance = self.grand_total  - (self.total_paid or 0)
		
		if self.pos_profile:
			self.changed_amount = self.total_paid - self.grand_total
			if self.changed_amount< 0:
				self.changed_amount = 0

		if self.balance<0:
			self.balance = 0
		
		
				
		# else:
		# 	self.changed_amount = 0
		# 	if self.total_paid > self.grand_total:
		# 		frappe.throw(_("Paid amount cannot greater than grand total amount"))
		 
		if not self.created_by:
			self.created_by = frappe.get_user().doc.full_name

		if not self.closed_by and self.docstatus==1:
	 
			self.closed_by = frappe.get_doc("User",self.modified_by).full_name

		if self.sale_status:
			sale_status_doc = frappe.get_doc("Sale Status", self.sale_status)
			self.sale_status_color = sale_status_doc.background_color
			self.sale_status_priority  = sale_status_doc.priority
		# commission
		if self.agent_name:
			if self.commission_type=="Percent":
				self.commission_amount = (self.grand_total * self.commission/100); 
			else:
				self.commission_amount = self.commission
		if self.docstatus ==1:
			self.sale_status = "Closed"
			self.sale_status_color = frappe.get_value("Sale Status","Closed","background_color")



	def on_update(self):
		pass

	def before_submit(self):
		self.append_quantity = None
		self.scan_barcode = None
		for d in self.sale_products:
			if d.is_inventory_product:
				if d.unit !=d.base_unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(d.product_code, d.product_name, d.base_unit, d.unit)))

	
	def on_submit(self):
		# update_inventory_on_submit(self)
		frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.update_inventory_on_submit", queue='short', self=self)
		add_payment_to_sale_payment(self)
	
	def on_cancel(self):
		frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.update_inventory_on_cancel", queue='short', self=self)



def update_inventory_on_submit(self):
	cost = 0 
	for p in self.sale_products:
		if p.is_inventory_product:
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			cost = get_product_cost(self.stock_location, p.product_code)
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Sale",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'out_quantity':p.quantity / uom_conversion,
				"uom_conversion":uom_conversion,
				'note': 'New sale submitted.',
    			'action': 'Submit'
			})
		else:
			doc = frappe.get_doc("Product",p.product_code)
			#check if product has receipt and loop update from product receip

			update_product_recipe_to_inventory(self,doc, p.quantity, "Submit")
			

			#udpate cost for none stock product
			
			cost = doc.cost or 0
			if doc.product_price:
				prices = Enumerable(doc.product_price).where(lambda x:x.business_branch == self.business_branch and x.price_rule == self.price_rule and x.unit == "Unit" and x.portion ==p.portion).first_or_default()
				if prices:
					cost = prices.cost
		#check if product have modifier then check receipt in modifer and update to inventory
		if p.modifiers_data:
			for m in json.loads(p.modifiers_data):

				modifier_doc = frappe.get_doc("Modifier Code",m['modifier'])
				for d in modifier_doc.product_recipe:
					if d.is_inventory_product:
						uom_conversion = get_uom_conversion(d.base_unit, d.unit)
						add_to_inventory_transaction({
							'doctype': 'Inventory Transaction',
							'transaction_type':"Sale",
							'transaction_date':self.posting_date,
							'transaction_number':self.name,
							'product_code': d.product,
							'unit':d.unit,
							'stock_location':self.stock_location,
							'out_quantity':(p.quantity* d.quantity) / uom_conversion,
							"uom_conversion":uom_conversion,
							'note': 'Update Recipe Quantity from modifer ({}) after New sale submitted.'.format(m["modifier"]),
							'action': 'Submit'
						})

		
		#check if product is combo menu then get item from the combo menu item and update to inventory
		if p.is_combo_menu:
			update_combo_menu_to_inventory(self,p,"Submit")
		
		frappe.db.sql("update `tabSale Product` set cost = {} where name='{}'".format(cost, p.name))
   
	#update total cost to sale and profit to sale
	total_cost = 0
	cost_datas = frappe.db.sql("select sum(cost * quantity) from `tabSale Product` where parent='{}'".format(self.name))
	if cost_datas:
		total_cost = cost_datas[0][0]
  
	frappe.db.sql("update `tabSale` set total_cost = {0} , profit=grand_total - {0} where name='{1}'".format(total_cost, self.name))

def update_product_recipe_to_inventory(self,product,base_quantity,action):

	for d in product.product_recipe:
		if d.is_inventory_product:
			uom_conversion = get_uom_conversion(d.base_unit, d.unit)
			note = ""
			if action =="Submit":
				note = 'Update Recipe Quantity after New sale submitted.'
			else:
				note =  'Update Recipe Quantity after cancel order.'

			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Sale",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': d.product,
				'unit':d.unit,
				'stock_location':self.stock_location,
				'in_quantity':(base_quantity* d.quantity) / uom_conversion if action=="Cancel" else 0,
				'out_quantity':(base_quantity* d.quantity) / uom_conversion if action=="Submit" else 0,
				"uom_conversion":uom_conversion,
				'note': note,
				'action': action
			})

def update_combo_menu_to_inventory(self, product,action):
	if product.is_combo_menu:
		if product.use_combo_group:
			combo_menu_data = json.loads(product.combo_group_data)
			update_combo_menu_to_inventor_transaction(self,product,action, combo_menu_data)
		else:
			#get combo menu item from combo_menu_data field 
			combo_menu_data = json.loads(product.combo_menu_data)
			update_combo_menu_to_inventor_transaction(self,product,action,combo_menu_data)
			
def update_combo_menu_to_inventor_transaction(self,product,action,combo_menu_data):
	for p in combo_menu_data:
		doc = frappe.get_doc("Product",p["product_code"])
		if doc.is_inventory_product:
			uom_conversion = get_uom_conversion( doc.unit,p["unit"])
			note =""
			if action =="Submit":
				note = 'New sale submitted. Inventory deduct from combo menu {}({})'.format(product.product_name, product.product_code)
			else:
				note = "Order cancelled. Inventory added from combo menu {}({})".format(product.product_name, product.product_code)
			
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Sale",
				'transaction_date':self.posting_date,
				'transaction_number':self.name,
				'product_code': doc.name,
				'unit':p["unit"],
				'stock_location':self.stock_location,
				"in_quantity": (p["quantity"] * product.quantity) / uom_conversion if action =="Cancel" else 0,
				'out_quantity': (p["quantity"] * product.quantity) / uom_conversion if action =="Submit" else 0,
				"uom_conversion":uom_conversion,
				'note': note,
				'action': action
			})
		else:
			#check if product have receipt then update to stock
			# base qty here is = sale product quantity * combo product quantity
			update_product_recipe_to_inventory(self,doc,product.quantity * p["quantity"], action)
					


def update_inventory_on_cancel(self):
	for p in self.sale_products:
		if p.is_inventory_product:
			uom_conversion = get_uom_conversion(p.base_unit, p.unit)
			add_to_inventory_transaction({
				'doctype': 'Inventory Transaction',
				'transaction_type':"Sale",
				'transaction_number':self.name,
				'transaction_date':self.posting_date,
				'product_code': p.product_code,
				'unit':p.unit,
				'stock_location':self.stock_location,
				'in_quantity':p.quantity / uom_conversion,
				"uom_conversion":uom_conversion,
				"price":p.cost,
				'note': 'Sale invoice cancelled.',
				'action': 'Cancel'
			})
		else:
			doc = frappe.get_doc("Product",p.product_code)
			for d in doc.product_recipe:
				if d.is_inventory_product:
					uom_conversion = get_uom_conversion(d.base_unit, d.unit)
					
					add_to_inventory_transaction({
						'doctype': 'Inventory Transaction',
						'transaction_type':"Sale",
						'transaction_date':self.posting_date,
						'transaction_number':self.name,
						'product_code': d.product,
						'unit':d.unit,
						'stock_location':self.stock_location,
						'in_quantity':(p.quantity* d.quantity) / uom_conversion,
						"uom_conversion":uom_conversion,
						'note': 'Update Recipe Quantity after Sale Invoice Cancelled.',
						'action': 'Cancel'
					})	
		#check if product has modifier and then chekc if modiifer have receipt then run script to update receipe

		if p.modifiers_data:
			for m in json.loads(p.modifiers_data):
				modifier_doc = frappe.get_doc("Modifier Code",m['modifier'])
				for d in modifier_doc.product_recipe:	
					uom_conversion = get_uom_conversion(d.base_unit, d.unit)
					add_to_inventory_transaction({
						'doctype': 'Inventory Transaction',
						'transaction_type':"Sale",
						'transaction_date':self.posting_date,
						'transaction_number':self.name,
						'product_code': d.product,
						'unit':d.unit,
						'stock_location':self.stock_location,
						'in_quantity':(p.quantity* d.quantity) / uom_conversion,
						"uom_conversion":uom_conversion,
						'note': 'Update Recipe Quantity from modifer ({}) after Sale Invoice Cancelled.'.format(m["modifier"]),
						'action': 'Cancel'
					})	

		#check if product is combo menu then get item from the combo menu item and update to inventory
		if p.is_combo_menu:
			update_combo_menu_to_inventory(self,p,"Cancel")
		

def add_payment_to_sale_payment(self):
	if self.payment:
		for p in self.payment:
			 
			if p.payment_type_group !='On Account':
				if p.input_amount>0:
					doc = frappe.get_doc({
							'doctype': 'Sale Payment',
							'posting_date':self.posting_date,
							'payment_type': p.payment_type,
							'currency':p.currency,
							'exchange_rate':p.exchange_rate,
							'sale':self.name,
							'input_amount':p.input_amount,
							"payment_amount":p.amount,
							"docstatus":1,
							"check_valid_payment_amount":0,
							"pos_profile":self.pos_profile,
							"working_day":self.working_day,
							"cashier_shift":self.cashier_shift
						})
					doc.insert()
   
		if (self.changed_amount or 0)>0:
			doc = frappe.get_doc({
					'doctype': 'Sale Payment',
					"transaction_type":"Changed",
					'posting_date':self.posting_date,
					'payment_type': frappe.db.get_default("changed_payment_type"),
					'sale':self.name,
					'input_amount':self.changed_amount * -1,
					"docstatus":1,
					"check_valid_payment_amount":0,
					"pos_profile":self.pos_profile,
					"working_day":self.working_day,
					"cashier_shift":self.cashier_shift,
					"note": "Changed amount in sale order {}".format(self.name)
				})
			doc.insert()
		

def validate_sale_product(self):
	sale_discount = self.discount  
	
	if sale_discount>0:
		if self.discount_type=="Amount":
			discountable_amount = Enumerable(self.sale_products).where(lambda x: x.allow_discount==1 and x.discount==0).sum(lambda x: (x.quantity or 0)* (x.price or  0))
			if discountable_amount>0:
				sale_discount=(sale_discount / discountable_amount ) * 100
			sale_discount = sale_discount or 0

			
 
	for d in self.sale_products:
		d.sub_total = (d.quantity or 0) * (d.price or 0) + (d.quantity or 0) * (d.modifiers_price or 0)
		if (d.discount_type or "Percent")=="Percent":
			d.discount_amount = d.sub_total * (d.discount or 0) / 100
		else:
			d.discount_amount = d.discount or 0

		# check if sale has discount
		if sale_discount>0 and d.allow_discount and d.discount==0:
			
			d.sale_discount_percent = sale_discount  
			d.sale_discount_amount = (sale_discount/100) * d.sub_total
		else:
			d.sale_discount_percent = 0  
			d.sale_discount_amount = 0

		d.total_discount = (d.sale_discount_amount or 0) + (d.discount_amount or 0)

			
   
		validate_tax(d)
		d.amount = (d.sub_total - d.discount_amount) + d.total_tax
		d.total_revenue = (d.sub_total - d.total_discount) + d.total_tax

def validate_pos_payment(self):
    for d in self.payment:
		
        d.amount = (d.input_amount or 0 ) / (d.exchange_rate or 1)

def validate_tax(doc):
		if doc.tax_rule:
			if doc.calculate_tax_1_after_discount==1:
				doc.taxable_amount_1 =   doc.sub_total - doc.total_discount
			else:
				doc.taxable_amount_1 = doc.sub_total 
    
			doc.tax_1_amount =  doc.taxable_amount_1 * doc.tax_1_rate/100

			#tax 2
			if   doc.calculate_tax_2_after_discount==1:
				doc.taxable_amount_2 = doc.sub_total  - doc.total_discount
			else:
				doc.taxable_amount_2 = doc.sub_total  
			
			if doc.calculate_tax_2_after_adding_tax_1==1:
					doc.taxable_amount_2 = doc.taxable_amount_2 +  doc.tax_1_amount

			doc.tax_2_amount =  (doc.taxable_amount_2 or 0) *  (doc.tax_2_rate or 0) /100

			#tax 3
			if doc.calculate_tax_3_after_discount==1:
				doc.taxable_amount_3 = doc.sub_total - doc.total_discount 
			else:
				doc.taxable_amount_3 =  doc.sub_total
			
			if doc.calculate_tax_3_after_adding_tax_1==1:
				doc.taxable_amount_3 =   doc.taxable_amount_3 +  doc.tax_1_amount 
			
			if doc.calculate_tax_3_after_adding_tax_2==1:
				doc.taxable_amount_3 = doc.taxable_amount_3 +  doc.tax_2_amount 
			
			doc.tax_3_amount =  doc.taxable_amount_3 * doc.tax_3_rate/100
			
			#total tax
			doc.total_tax = doc.tax_1_amount + doc.tax_2_amount + doc.tax_3_amount
		else:
			doc.taxable_amount_1 =0
			doc.tax_1_amount=0
			doc.taxable_amount_2 =0
			doc.tax_2_amount=0
			doc.taxable_amount_3 =0
			doc.tax_3_amount=0
			doc.total_tax =0
		