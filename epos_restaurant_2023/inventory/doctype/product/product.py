# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

from datetime import datetime # from python std library
import frappe
from frappe import _
from frappe.model.document import Document
from frappe.utils.data import strip
from py_linq import Enumerable
from epos_restaurant_2023.inventory.inventory import get_uom_conversion, update_product_quantity,get_stock_location_product

class Product(Document):
	def validate(self):
		if strip(self.naming_series) =="" and strip(self.product_code) =="":
			frappe.throw(_("Please enter product code"))
   
		if strip(self.product_name_kh)=="":
			self.product_name_kh = strip(self.product_name_en)

		# price = get_product_price(product=self, business_branch="SR Branch",portion="Normal", price_rule="Normal Rate", unit="Box" )
		# if price:
		# 	frappe.msgprint(str(price["cost"]))
	
	def autoname(self):

		if strip(self.naming_series) !="" and strip(self.product_code) =="":
		 
			from frappe.model.naming import set_name_by_naming_series
			set_name_by_naming_series(self)
			self.product_code = self.name		

		self.product_code = strip(self.product_code)
		self.name = self.product_code
		 
  
	def after_insert(self):
		if self.is_inventory_product:
			if self.opening_quantity and self.opening_quantity>0:
				current_stock = get_stock_location_product(self.stock_location, self.product_code)
				update_product_quantity(doc=current_stock, stock_location=self.stock_location, product_code=self.name, quantity=self.opening_quantity, cost=self.cost)
	
	@frappe.whitelist()
	def get_product_summary_information(self):
		stock_information = []
  
		stock_data =frappe.db.get_list('Stock Location Product',
					filters={
						'product_code': self.name
					},
					fields=['stock_location', 'quantity'],
				)

		if stock_data:
			for d in stock_data:
				stock_information.append({"stock_location": d.stock_location, "quantity":d.quantity})

		return {
			"total_annual_sale":get_product_annual_sale(self),
			"stock_information":stock_information,
		}



@frappe.whitelist()
def get_product(barcode,business_branch=None,price_rule=None, unit=None,portion = None,allow_sale=None,allow_purchase=None):
	try:
		frappe.flags.mute_messages = True
		p = frappe.get_doc("Product",{"product_code":barcode,"disabled":0},["*"])
		if allow_sale and not p.allow_sale:
			return {
				"status":404,
				"message":_("This product is not allow to sale")
			}
		
		if allow_purchase and not p.allow_purchase:
			return {
				"status":404,
				"message":_("This product is not allow to purchase")
			}
  
		if p :
			price = get_product_price(product=p,business_branch=business_branch, price_rule=price_rule,unit=unit or p.unit ,portion=portion  )
			tax_rule = None
			if p.tax_rule:
				tax_rule = frappe.get_doc("Tax Rule", p.tax_rule, cache = True)
			
			return {
				"status":0,#success
				"product_code": p.product_code,
				"product_name_en":p.product_name_en,
				"unit":p.unit,
				"cost":price["cost"],
				"price":price["price"],
				"allow_discount":p.allow_discount,
				"allow_free":p.allow_free,
				"allow_change_price":p.allow_change_price,
				"is_inventory_product":p.is_inventory_product,
				"tax_rule":p.tax_rule,
				"tax_rule_doc":tax_rule
			}
		else:
			return {
				"status":404,
				"message":_("Product code {} is not exist".format(barcode))
			}
	except frappe.DoesNotExistError:
		return {
				"status":404,
				"message":_("Product code {} is not exist".format(barcode))
			}
		
	finally:
		frappe.flags.mute_messages = False
  
@frappe.whitelist()
def get_product_price(product=None,barcode=None,unit=None, business_branch=None,price_rule=None,portion=None):
	uom_conversion = 1
	if unit and product:
		if unit != product.unit:
			uom_conversion = get_uom_conversion(product.unit, unit)
	price = 0
	cost = 0
	if product:
		price = (product.price or 0) / uom_conversion
		cost = (product.cost or 0) / uom_conversion
		if product.product_price:
			data = Enumerable(product.product_price).where(lambda x: x.business_branch==(business_branch or x.business_branch) and x.price_rule==(price_rule or x.price_rule) and x.portion == (portion or x.portion) and x.unit == (unit or x.unit) )
			if data:
				if data[0]:
					price= data[0].price or 0
					cost= data[0].cost or 0
	else:
		p = frappe.get_doc("Product",{"product_code":barcode,"disabled":0},["*"])
		return get_product_price (product=p, unit=unit, business_branch=business_branch,portion=portion,price_rule=price_rule)
  
	return {"price":price,"cost":cost}


def get_product_annual_sale(self):
	year = datetime.now().strftime('%Y')

	#get branch for permission amount
	sql = "select sum(a.amount) from `tabSale Product` a inner join `tabSale` b on a.parent = b.name where year(b.posting_date) = {} and a.product_code = '{}'".format(year, self.name)
	data = frappe.db.sql(sql)
	if data:
		return data[0][0]
	return 0
@frappe.whitelist()
def get_product_cost_by_stock(product_code=None, stock_location=None):
	result = frappe.db.sql("select cost from `tabStock Location Product` where stock_location='{}' and product_code='{}'".format(stock_location, product_code), as_dict=1)
	if result:
		return result[0]
	return {'cost':0}