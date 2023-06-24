# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
import json # from python std library
from py_linq import Enumerable
from frappe.model.document import Document


class ModifierGroup(Document):
	def validate(self):	
		old_product_categories = []
		new_product_categories = []
		if not self.is_new():
			old_doc = frappe.get_doc('Modifier Group', self.name)
			for c in old_doc.product_category:
				old_product_categories.append(c) 
		
		for n in self.product_category:
			new_product_categories.append(n)	
				

		for x in new_product_categories: 
			value = Enumerable(old_product_categories).where(lambda z:z.product_category == x.product_category)
			if value:
				old_product_categories.remove(value[0])	
		
	 	#update modifier that removed product categories	
		if old_product_categories:	
			on_update_modifier_to_temm_product_menu(self, old_product_categories,True)
		
		#update update modifier all product categories
		on_update_modifier_to_temm_product_menu(self, self.product_category)
 


## custom method
def on_update_modifier_to_temm_product_menu(self, product_categories, remove = False):

	for pc in product_categories:		
		products =	frappe.db.get_list('Product',
						filters=[['product_category','=',pc.product_category]],
						fields=['name'],
						limit=200
					)		
		
		for p in products:
			
			modifiers = []
			product = frappe.get_doc('Product',p.name)
			if product.pos_menus and not product.disabled and product.allow_sale:
				#get product modifier category from product
				mc0 = []
				mc1 = Enumerable(product.product_modifiers).select(lambda x: x.modifier_category).distinct()			
				
				for mc in mc1:
					mc0.append(mc)

				if not remove:
					mc2 = Enumerable(self.modifiers).select(lambda x: x.modifier_category).distinct() 
					for mc in mc2:
						mc0.append(mc)
				
				modifier_categories = Enumerable(mc0).select(lambda x: x).distinct() 
				for mc in modifier_categories:
					doc_category = frappe.get_doc("Modifier Category",mc)
					modifier_items = []		
					items = []			
							
					#global modifier group
					if not remove:
						for m in self.modifiers:
							if m.modifier_category == mc:
								items.append({
									"name":m.name,
									"branch":m.business_branch or "" , 
									"prefix":m.prefix, 
									"modifier":m.modifier_code, 
									"value": str(m.business_branch or "") + str(m.prefix) + ''+str( m.modifier_code), 
									"price":m.price 
									})	
							
					#get modifier from product
					for m in product.product_modifiers:
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
						data ={
							"name":i['name'],
							"branch":i['branch'], 
							"prefix":i['prefix'], 
							"modifier":i['modifier'], 
							"price": i['price'] 
						}
						modifier_items.append(data)

					modifiers.append({
						"category":mc,
						"is_required":doc_category.is_required,
						"is_multiple":doc_category.is_multiple,
						"items":modifier_items
					})
				
				# frappe.msgprint( p.name +" =====> "+json.dumps(modifiers))

				for m in product.pos_menus:
					doc = frappe.get_doc('Temp Product Menu',m.name) 
					doc.modifiers = json.dumps(modifiers)						 
					doc.save()


		

			
				 


