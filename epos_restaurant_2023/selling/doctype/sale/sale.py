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
import datetime
from decimal import Decimal
class Sale(Document):
	def validate(self):
		 
		if not frappe.db.get_default('exchange_rate_main_currency'):
			frappe.throw('Main Exchange Currency not yet config. Please contact to system administrator for solve')


			#frappe.throw(_("Please select your working day"))
		if self.pos_profile:
			if not self.working_day:
				frappe.throw(_("Please start working day first"))

			if not self.cashier_shift: 
				frappe.throw(_("Please start shift first"))

		# if self.posting_date:
		# 	if self.posting_date>utils.today():
		# 		frappe.throw(_("Sale date cannot greater than current date"))
		

		# set waiting number
		if self.is_new():
			if self.waiting_number_prefix:
				from frappe.model.naming import make_autoname
				self.waiting_number = make_autoname(self.waiting_number_prefix)
 
			if not self.custom_bill_number:
				if self.pos_profile:
					pos_config = frappe.db.get_value("POS Profile",self.pos_profile,"pos_config")
					bill_number_prefix = frappe.db.get_value("POS Config",pos_config,"pos_bill_number_prefix")
					if bill_number_prefix:
						from frappe.model.naming import make_autoname
						self.custom_bill_number = make_autoname(bill_number_prefix)


		if self.discount_type =="Percent" and self.discount> 100:
			frappe.throw(_("discount percent cannot greater than 100 percent"))

		is_allow_user_edit_sale_after_close_working_day = frappe.db.get_single_value('ePOS Settings','allow_user_edit_sale')
		if is_allow_user_edit_sale_after_close_working_day != 1:
			if self.docstatus ==0:
				if self.working_day:
					is_closed = frappe.db.get_value('Working Day', self.working_day,"is_closed")
					if is_closed==1:
						# frappe.throw(_("Working day {} is already closed".format(self.working_day)))
						frappe.throw(_("Working day was closed"))

				if self.cashier_shift:
					is_closed = frappe.db.get_value('Cashier Shift', self.cashier_shift,"is_closed")
					if is_closed==1:
						# frappe.throw(_("Cashier shift {} is already closed".format(self.cashier_shift)))
						frappe.throw(_("Cashier shift was closed"))

		#validate outlet
		if self.outlet and self.business_branch:
			if frappe.get_value("Outlet",self.outlet,"business_branch") != self.business_branch:
				frappe.throw(_("The outlet {} is not belong to business branch {}".format(self.outlet, self.business_branch)))
		
		#validate stock location
		if self.stock_location and self.business_branch:
			if frappe.get_value("Stock Location",self.stock_location,"business_branch") != self.business_branch:
				frappe.throw(_("The stock location {} is not belong to business branch {}".format(self.stock_location, self.business_branch)))
		

		#validate exhcange rate change
		to_currency = frappe.db.get_default("second_currency")
		if( frappe.db.get_default("exchange_rate_main_currency") !=frappe.db.get_default("currency") ):
			to_currency = frappe.db.get_default("currency") 
			
		exchange_rate = frappe.get_last_doc('Currency Exchange', filters={"to_currency": to_currency})# frappe.get_last_doc("Currency Exchange",{})
		
		if exchange_rate:
			self.exchange_rate = exchange_rate.exchange_rate
		else:
			self.exchange_rate =1
 
		#validate sale product 
		validate_sale_product(self)

		#add sale product spa commission
		add_sale_product_spa_commission(self)
  
		validate_pos_payment(self)
		#validate sale summary

		#set is foc by check payment pyment if have is_foc payment type
		self.is_foc = 0

		## check table if have make foc to sale when discount 100%
		if self.table_id:
			_table = frappe.get_doc("Tables Number",self.table_id)
			if _table.is_foc and self.discount==100 and self.discount_type =="Percent":
				self.is_foc = 1


		
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
		self.total_fee =  Enumerable(self.payment).sum(lambda x: x.fee_amount or 0)
		self.total_paid_with_fee = self.total_paid + (self.total_fee or 0)


	
		self.balance = round(self.grand_total  , int(currency_precision))-  round((self.total_paid or 0)  , int(currency_precision))
	

		if self.pos_profile:
			self.changed_amount = self.total_paid - self.grand_total
			if round(self.changed_amount,int(currency_precision)) <= generate_decimal(int(currency_precision)):
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
			self.closed_date = datetime.datetime.now()


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
		on_get_revenue_account_code(self)
		self.append_quantity = None
		self.scan_barcode = None
		for d in self.sale_products:
			if d.is_inventory_product:
				if d.unit !=d.base_unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(d.product_code, d.product_name, d.base_unit, d.unit)))

	
	def on_submit(self):
		create_folio_transaction_from_pos_trnasfer(self) 
		# update_inventory_on_submit(self)			
		# add_payment_to_sale_payment(self)
		
		# frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.create_folio_transaction_from_pos_trnasfer", queue='short', self=self)
		frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.update_inventory_on_submit", queue='short', self=self)
		frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.add_payment_to_sale_payment", queue='short', self=self)
 
		
	
	def on_cancel(self):
		query = "update `tabSale Product SPA Commission` set is_deleted = 1  where sale = '{}'".format(self.name)			
		frappe.db.sql(query)
		frappe.enqueue("epos_restaurant_2023.selling.doctype.sale.sale.update_inventory_on_cancel", queue='short', self=self)



def generate_decimal(precision: int) -> Decimal:
    return Decimal('0.1') ** precision

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
				'portion':p.portion,
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
			if not d.sale_type or d.sale_type == self.sale_type:
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
		combo_menu_data = json.loads(product.combo_menu_data)
		update_combo_menu_to_inventor_transaction(self,product,action, combo_menu_data)
			
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
							"cashier_shift":self.cashier_shift,
							"room_number":p.room_number,
							"folio_number":p.folio_number,
							"use_room_offline":p.use_room_offline,
							"account_code":p.account_code,
							"fee_amount":p.fee_amount,
							"fee_percentage":p.fee_percentage
						})
					doc.insert()
   
		if (self.changed_amount or 0)>0:
			pos_config = frappe.db.get_value('POS Profile', self.pos_profile, 'pos_config')			
			payment_type = frappe.db.get_default("changed_payment_type")			
			pos_config_data = frappe.get_doc('POS Config', pos_config)
			pos_config_payment_type = Enumerable(pos_config_data.payment_type).where(lambda x:x.payment_type==payment_type)
			account_code = ""
			if pos_config_payment_type:
				account_code = pos_config_payment_type[0].account_code
			doc = frappe.get_doc({
					'doctype': 'Sale Payment',
					"transaction_type":"Changed",
					'posting_date':self.posting_date,
					'payment_type': payment_type,
					'sale':self.name,
					'input_amount':self.changed_amount * -1,
					"docstatus":1,
					"check_valid_payment_amount":0,
					"pos_profile":self.pos_profile,
					"working_day":self.working_day,
					"cashier_shift":self.cashier_shift,
					"note": "Changed amount in sale order {}".format(self.name),
					"account_code":account_code
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
		# validate product free
		if(d.is_free and d.price > 0):
			frappe.throw(_("Cannot set price becouse this product is free"))
		
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

def add_sale_product_spa_commission(self):	
	query = "delete from `tabSale Product SPA Commission` where sale = '{}'".format(self.name)			
	frappe.db.sql(query)
	for sp in self.sale_products:		 
		if sp.is_require_employee:
			if sp.employees: 
				for em in json.loads(sp.employees): 
					data ={
						'doctype': 'Sale Product SPA Commission',
						'sale':self.name,
						'sale_product': sp.name,
						'product_name':sp.product_name,
						'product_name_kh':sp.product_name_kh,
						"employee":em['employee_id'],
						"employee_name":em['employee_name'],
						"duration_title":em['duration_title'],
						"duration":em['duration'],
						"commission_amount":em['commission_amount'],
						"is_overtime":em['is_overtime']
					} 
					doc = frappe.get_doc(data)
					doc.insert() 
			
				
def create_folio_transaction_from_pos_trnasfer(self):
	for p in self.payment:
		if p.folio_number and not p.use_room_offline:
			data = {
					'doctype': 'Folio Transaction',
					'posting_date':self.posting_date,
					'transaction_type': 'Reservation Folio',
					'transaction_number':p.folio_number,
					'reference_number':self.name,
					"input_amount":p.amount,
					"account_code":p.account_code,
					"property":self.business_branch,
					"type":"Debit"
				} 
			doc = frappe.get_doc(data)
			doc.insert()	

def on_get_revenue_account_code(self):
	for sp in self.sale_products:
		values = {
			'outlet': self.outlet,
			'shift': self.shift_name,
			'revenue_group': sp.revenue_group
			}
		data = frappe.db.sql("""
				select 
					name,
					code,
					account_code,
					discount_account,
					tax_1_account,
					tax_2_account,
					tax_3_account
				from `tabRevenue Code` 
				where outlet=%(outlet)s
				and shift = %(shift)s
				and revenue_group=%(revenue_group)s
			""",values=values, as_dict=1)
		if data:
			sp.revenue_code = data[0].code
			sp.account_code = data[0].account_code
			sp.discount_account = data[0].discount_account
			sp.tax_1_account = data[0].tax_1_account
			sp.tax_2_account = data[0].tax_2_account
			sp.tax_3_account = data[0].tax_3_account

def validate_pos_payment(self):
	currency = frappe.db.get_default("currency")
	for d in self.payment:
		d.exchange_rate = d.exchange_rate if d.currency != currency else 1
		d.amount = (d.input_amount or 0 ) / (d.exchange_rate or 1)

def validate_tax(doc):
		if doc.tax_rule:
			#Tax 1
			doc.taxable_amount_1 = doc.sub_total 
			#cal tax1 taxable after disc.
			if doc.calculate_tax_1_after_discount == 1:
				doc.taxable_amount_1 =   doc.sub_total - doc.total_discount			 
				
			doc.taxable_amount_1 *= (doc.percentage_of_price_to_calculate_tax_1/100)
			doc.tax_1_amount =  (doc.taxable_amount_1 or 0) * ((doc.tax_1_rate or 0)/100)

			#Tax 2
			doc.taxable_amount_2 = doc.sub_total  
			#cal tax2 taxable after disc.
			if doc.calculate_tax_2_after_discount==1:
				doc.taxable_amount_2 = doc.sub_total  - doc.total_discount

			#cal tax2 taxable after add tax1
			if doc.calculate_tax_2_after_adding_tax_1==1:
				doc.taxable_amount_2 +=  doc.tax_1_amount

			doc.taxable_amount_2 *= (doc.percentage_of_price_to_calculate_tax_2/100)
			doc.tax_2_amount =  (doc.taxable_amount_2 or 0) *  ((doc.tax_2_rate or 0) /100)

			#tax 3
			doc.taxable_amount_3 =  doc.sub_total
			#cal tax3 taxable after disc.
			if doc.calculate_tax_3_after_discount==1:
				doc.taxable_amount_3 = doc.sub_total - doc.total_discount 
			
			#cal tax3 taxable after add tax1
			if doc.calculate_tax_3_after_adding_tax_1==1:
				doc.taxable_amount_3 =   doc.taxable_amount_3 +  doc.tax_1_amount 
			
			#cal tax3 taxable after add tax2
			if doc.calculate_tax_3_after_adding_tax_2==1:
				doc.taxable_amount_3 = doc.taxable_amount_3 +  doc.tax_2_amount 
			
			doc.taxable_amount_3 *= (doc.percentage_of_price_to_calculate_tax_3/100)
			doc.tax_3_amount =  (doc.taxable_amount_3 or 0) *  ((doc.tax_3_rate or 0) /100)
			
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
		