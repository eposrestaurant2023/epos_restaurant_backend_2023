# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

from datetime import datetime
import json # from python std library
import frappe
from frappe import _
from frappe.model.document import Document
from frappe.utils.data import strip
from py_linq import Enumerable
from frappe.utils import add_years
from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, check_uom_conversion, get_uom_conversion, update_product_quantity,get_stock_location_product
import itertools
import os


class Product(Document):
	def validate(self):
		 
		if self.is_combo_menu==1:
			self.is_recipe=0
			if self.is_inventory_product:
				self.is_inventory_product = 0
			

		error_list=[]
		for v in self.product_variants:
			if v.variant_code is None or v.variant_code == "":
				error_list.append("""Row: {0} Product Code Can't Be Empty""".format(v.idx))
			item = frappe.db.sql("select name from `tabProduct` where name = '{0}'".format(v.variant_code),as_dict=1)
			if item : 
				error_list.append("""Row: {0} Product Code {1} Already Exist""".format(v.idx,frappe.bold(v.variant_code)))
			variant = frappe.db.sql("select name from `tabProduct Variants` where variant_code = '{0}' and name != '{1}'".format(v.variant_code,v.name),as_dict=1)
			if variant : 
				error_list.append("""Row: {0} Product Code {1} Already Exist""".format(v.idx,frappe.bold(v.variant_code)))
		if len(error_list) > 0:
				for msg in error_list:
					frappe.msgprint(msg)
				raise frappe.ValidationError(error_list)

		# lock uncheck inventory product
		if not self.is_new():
			old_product = frappe.get_doc('Product', self.product_code)
			has_inventory_transaction = frappe.db.exists("Inventory Transaction", {"product_code": self.name})
			if old_product.is_inventory_product and not self.is_inventory_product and has_inventory_transaction:
				frappe.throw(_("Cannot uncheck inventory product"))

			if old_product.unit != self.unit and has_inventory_transaction:
				frappe.throw(_("Cannot change unit"))
    
		if strip(self.naming_series) =="" and strip(self.product_code) =="":
			frappe.throw(_("Please enter product code"))
   
		if self.is_inventory_product == False and self.is_new():
			self.opening_quantity = 0
   
		elif self.is_inventory_product and self.opening_quantity > 0 and not self.stock_location and self.is_new():
			frappe.throw(_("Please select stock location"))
			

		if strip(self.product_name_kh)=="":
			self.product_name_kh = strip(self.product_name_en)

		#validate uom conversion product price
		if self.is_inventory_product:
			for d in self.product_price:
				if d.unit != self.unit:
					if not check_uom_conversion(d.unit, self.unit):
							frappe.throw(_("There is no UoM conversion  from {} to {}".format( d.unit, self.unit)))


		#validate uom product recipe
		for d in self.product_recipe:
			if d.unit != d.base_unit:
				if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(d.product, d.product_name, d.base_unit, d.unit)))


		self.total_recipe_quantity = Enumerable(self.product_recipe).sum(lambda x: x.quantity)

		#generate combo menu to json and update to combo menu data 
		if self.is_combo_menu and self.product_combo_menus and self.use_combo_group==0:
			combo_menus = []
			for m in self.product_combo_menus:
				if m.unit != m.base_unit:
					if not check_uom_conversion(m.base_unit, m.unit):
							frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(m.product, m.product_name, m.base_unit, m.unit)))

				combo_menus.append({
					"menu_name":m.name,
					"product_code":m.product,
					"product_name":m.product_name,
					"unit":m.unit,
					"quantity":m.quantity,
					"price":m.price,
					"photo":m.photo
				})
				
			self.combo_menu_data = json.dumps(combo_menus)

		if self.is_combo_menu and self.combo_groups and self.use_combo_group==1:
			combo_groups = []
			for m in self.combo_groups:
				combo_groups.append({
					"combo_group":m.combo_group,
					"pos_title":m.pos_title,
					"item_selection":m.item_selection,
					"menus":json.loads(m.combo_menu_data),
				})
			
			self.combo_group_data = json.dumps(combo_groups)

		
		# price = get_product_price(product=self, business_branch="SR Branch",portion="Normal", price_rule="Normal Rate", unit="Box" )
		# if price:
		# 	frappe.msgprint(str(price["cost"]))

		#check if portion price exists 
		if	len(self.product_price) > 0:
			self.price = Enumerable(self.product_price).min(lambda x: x.price)

	def autoname(self):
		from frappe.model.naming import set_name_by_naming_series, get_default_naming_series,make_autoname

		if strip(self.naming_series) !="" and strip(self.product_code) =="":
			set_name_by_naming_series(self)
			self.product_code = self.name		

		self.product_code = strip(self.product_code)
		self.name = self.product_code
		 
	
	def after_insert(self):
		if self.is_inventory_product:
			if self.opening_quantity and self.opening_quantity>0:
				add_to_inventory_transaction(
					{
						"doctype":"Inventory Transaction",
						"transaction_date":datetime.now(),
						"product_code":self.name,

						"stock_location":self.stock_location,
						"in_quantity":self.opening_quantity,
						"price":self.cost,
						"note":"Opening Quantity"
					}
				)


	def before_save(self):
		prices = []
		if self.product_price:
			
			for p in self.product_price:
				prices.append({
					"price":p.price,
					'branch':p.business_branch or "",
					'price_rule':p.price_rule, 
					'portion':p.portion,
					'unit':p.unit, 
					'price_rule' : p.price_rule
				})
		self.prices = json.dumps(prices)	
	
	def on_update(self):
		#add_product_to_temp_menu(self)
		frappe.enqueue("epos_restaurant_2023.inventory.doctype.product.product.add_product_to_temp_menu", queue='short', self=self)

	def on_trash(self):
 
		frappe.db.sql("delete from `tabTemp Product Menu` where product_code='{}'".format(self.name))
	 

	@frappe.whitelist()
	def get_product_summary_information(self):
		stock_information = []

		stock_data =frappe.db.get_list('Stock Location Product',
					filters={
						'product_code': self.name
					},
					fields=['stock_location', 'quantity','unit'],
				)

		if stock_data:
			for d in stock_data:
				stock_information.append({"stock_location": d.stock_location, "quantity":d.quantity,"unit":d.unit})

		return {
			"total_annual_sale":get_product_annual_sale(self),
			"stock_information":stock_information,
			"precision": frappe.db.get_default("float_precision"),
			
		}

	@frappe.whitelist()
	def generate_variant(self):
	
		variant_1 =  [d.variant_value for d in self.variant_1_value]
		variant_2 =  [d.variant_value for d in self.variant_2_value]
		variant_3 =  [d.variant_value for d in self.variant_3_value]

		product_variants = []
		if variant_1 and not variant_2 and not variant_3:
			for v1 in variant_1:
				product_variants.append({
					"variant_code":"",
					"variant_name": v1,
					"variant_1":v1,
					"opening_qty":0,
					"cost": 0,
					"price":0
				})
		elif variant_1 and   variant_2 and not variant_3:
			for v1, v2 in itertools.product(variant_1, variant_2):
				product_variants.append({
					"variant_code":"",
					"variant_name": "{}-{}".format(v1,v2 ),
					"variant_1":v1,
					"variant_2":v2,
					"opening_qty":0,
					"cost": 0,
					"price":0
				})
		else:
			for v1, v2,v3   in itertools.product(variant_1, variant_2, variant_3):
				product_variants.append({
					"variant_code":"{}-{}-{}-{}".format(self.name,frappe.db.get_value("Variant Code", v1,"code_prefix"),frappe.db.get_value("Variant Code", v2,"code_prefix"),frappe.db.get_value("Variant Code", v3,"code_prefix")),
					"variant_name": "{}-{}-{}".format(v1,v2,v3 ),
					"variant_1":v1,
					"variant_2":v2,
					"variant_3": v3,
					"opening_qty":0,
					"cost": 0,
					"price":0
				})
		return  product_variants


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

def add_product_to_temp_menu(self):
	frappe.db.sql("delete from `tabTemp Product Menu` where product_code='{}'".format(self.name))

	if self.pos_menus and not self.disabled and self.allow_sale:
		printers = []
		for p in self.printers:
			printers.append({
				"printer":p.printer_name,
				"group_item_type":p.group_item_type,
				"is_label_printer":p.is_label_printer
				})
	
		prices = []
		for p in self.product_price:
			prices.append({

					"price":p.price,
					'branch':p.business_branch or "",
					'price_rule':p.price_rule, 
					'portion':p.portion,
					  'unit':p.unit, 
					  'price_rule' : p.price_rule
					})
			
		#get product modifier
		mc0 = []
		mc1 = Enumerable(self.product_modifiers).select(lambda x: x.modifier_category).distinct()
		mc2 = [] #global modifier category



		# #get global modifier category
		global_modifier_product_categorie = frappe.get_all('Modifier Group Product Category',
								filters=[['product_category','=',self.product_category]],
								fields=['parent','name'],
								limit=200
							 )
		global_modifiers = []
		for gmpc in global_modifier_product_categorie:
			gmodifiers = frappe.get_doc('Modifier Group',gmpc.parent)
			for g in gmodifiers.modifiers:
				global_modifiers.append(g)
		
		if global_modifiers != []:
			mc2 = Enumerable(global_modifiers).select(lambda x: x.modifier_category).distinct()
		
		for mc in mc1:
			mc0.append(mc)
		for mc in mc2:
			mc0.append(mc)

		
		modifier_categories = Enumerable(mc0).select(lambda x: x).distinct()	
		modifiers = []


		## get modifier data
		for mc in modifier_categories:
			doc_category = frappe.get_doc("Modifier Category",mc)
			modifier_items = []	
			items = []			
			#global modifier group
			for m in global_modifiers:
				if m.modifier_category == mc:
					items.append({
						"name":m.name,
						"branch":m.business_branch or "" , 
						"prefix":m.prefix, 
						"modifier":m.modifier_code, 
						"value": str(m.business_branch or "") + str(m.prefix) + ''+str( m.modifier_code), 
						"price":m.price 
						})	
			
			#product modifier
			for m in self.product_modifiers:
				if m.modifier_category == mc:							
					items.append({
						"name":m.name,
						"branch":m.business_branch or "" , 
						"prefix":m.prefix, 
						"modifier":m.modifier_code, 
						"value":  str(m.business_branch or "") + str(m.prefix) + ''+str( m.modifier_code), 
						"price":m.price 
					})
			
			for i in items:	
				modifier_items.append({
					"name":i['name'],
					"branch":i['branch'], 
					"prefix":i['prefix'], 
					"modifier":i['modifier'], 
					"price": i['price'] 
				})

			modifiers.append({
				"category":mc,
				"is_required":doc_category.is_required,
				"is_multiple":doc_category.is_multiple,
				"items":modifier_items
			})
			
		## end get modifier data


		for m in self.pos_menus:			
			doc = frappe.get_doc({
							"pos_menu_id":m.name,
							'doctype': 'Temp Product Menu',
							'product_code': self.name,
							'sort_order':self.sort_order,
							'pos_menu':m.pos_menu,
							'printers':json.dumps(printers),
							'prices':json.dumps(prices),
							'revenue_group':self.revenue_group,
							'modifiers':json.dumps(modifiers)
						})
			doc.insert() 
   
@frappe.whitelist()
def update_product_to_temp_product_menu():
	products = frappe.db.sql("select name from `tabProduct`", as_dict=1)
	for pro in products:
		doc = frappe.get_doc("Product", pro.name)
		
		add_product_to_temp_menu(doc)
	frappe.db.commit()
	return "Done"


@frappe.whitelist()
def assign_menu(products,menu):
	pos_menu_doc = frappe.get_doc("POS Menu",menu)
	for p in products.split(","):
		product = frappe.get_doc("Product",p)	 
		if len(product.pos_menus) ==0:
			# Create a new child document
			child_doc = frappe.new_doc("Product Menu")
			child_doc.pos_menu =menu 
			child_doc.pos_menu_name_kh= pos_menu_doc.pos_menu_name_kh
			# Add the child document to the parent document
			product.append("pos_menus", child_doc)
		else:
			result = [d for d in product.pos_menus if d.pos_menu== menu]
			
			if not result:				
				child_doc = frappe.new_doc("Product Menu")
				child_doc.pos_menu =menu 
				child_doc.pos_menu_name_kh= pos_menu_doc.pos_menu_name_kh
				# Add the child document to the parent document
				product.append("pos_menus", child_doc)	
		product.save()

	frappe.db.commit()

@frappe.whitelist()
def assign_printer(products,printer):
 
	printer_doc = frappe.get_doc("Printer",printer)
	for p in products.split(","):
		product = frappe.get_doc("Product",p)
	 
		if len(product.printers) ==0:
			# Create a new child document
			child_doc = frappe.new_doc("Product Printer")
			child_doc.printer =printer 
			child_doc.printer_name= printer_doc.printer_name
			# Add the child document to the parent document
			product.append("printers", child_doc)
		else:
			result = [d for d in product.printers if d.printer== printer]
			
			if not result:
				
				child_doc = frappe.new_doc("Product Printer")
				child_doc.printer =printer 
				child_doc.printer_name= printer_doc.printer_name
				# Add the child document to the parent document
				product.append("printers", child_doc)

			 
			 
		product.save()

	frappe.db.commit()

	#frappe.throw("u run assign pritner")

@frappe.whitelist()
def remove_printer(products,printer):
	for p in products.split(","):
		product = frappe.get_doc("Product",p)
		printers = product.get('printers')
		for row in printers:
			if row.printer == printer:
				printers.remove(row)
		product.save()

	frappe.db.commit()


@frappe.whitelist()
def clear_all_printer_from_product(products):
	for p in products.split(","):
		product = frappe.get_doc("Product",p)
		product.printers = []
		product.save()

	frappe.db.commit()
