from epos_restaurant_2023.inventory.inventory import add_to_inventory_transaction, check_uom_conversion, get_product_cost, get_stock_location_product, get_uom_conversion, update_product_quantity
import frappe
from frappe import utils
from frappe import _
from frappe.utils.data import fmt_money
from py_linq import Enumerable


from frappe.model.document import Document

class SaleQuotation(Document):
	def validate(self):
		if self.discount_type =="Percent" and self.discount> 100:
			frappe.throw(_("Discount percent cannot greater than 100%"))
		#validate outlet
		if self.outlet and self.business_branch:
			if frappe.get_value("Outlet",self.outlet,"business_branch") != self.business_branch:
				frappe.throw(_("The outlet {} is not belong to business branch {}".format(self.outlet, self.business_branch)))
		
		#validate stock location
		if self.stock_location and self.business_branch:
			if frappe.get_value("Stock Location",self.stock_location,"business_branch") != self.business_branch:
				frappe.throw(_("The stock location {} is not belong to business branch {}".format(self.stock_location, self.business_branch)))
			
		#validate sale product 
		validate_sale_product(self)		

		total_quantity = Enumerable(self.products).sum(lambda x: x.quantity or 0)
		sub_total = Enumerable(self.products).sum(lambda x: (x.quantity or 0)* (x.price or  0))
		sale_discountable_amount =Enumerable(self.products).where(lambda x:x.allow_discount ==1 and (x.discount_amount or 0)==0).sum(lambda x: (x.quantity or 0)* (x.price or  0))

		self.total_quantity = total_quantity
		self.sale_discountable_amount = sale_discountable_amount
		self.sub_total = sub_total
		# calculate sale discount
		if self.discount:
			if self.discount_type =="Percent":
				self.sale_discount = self.sale_discountable_amount * self.discount / 100
			else:
				self.sale_discount = self.discount or 0

		self.product_discount = Enumerable(self.products).where(lambda x:x.allow_discount ==1).sum(lambda x: x.discount_amount)
		
		self.total_discount = (self.product_discount or 0) + (self.sale_discount or 0)
  
		#tax 
		self.taxable_amount_1  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_1)
		self.taxable_amount_2  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_2)
		self.taxable_amount_3  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.taxable_amount_3)
		self.tax_1_amount  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_1_amount)
		self.tax_2_amount  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_2_amount)
		self.tax_3_amount  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.tax_3_amount)
		self.total_tax  = Enumerable(self.products).where(lambda x:x.tax_rule).sum(lambda x: x.total_tax)

		currency_precision = frappe.db.get_single_value('System Settings', 'currency_precision')
		if currency_precision=='':
			currency_precision = "2"

		self.grand_total =( sub_total - (self.total_discount or 0))  + self.total_tax

		if not self.created_by:
			self.created_by = frappe.get_user().doc.full_name


	def before_submit(self):
		self.append_quantity = None
		self.scan_barcode = None

	def on_update(self):
		pass

	def before_submit(self):
		for d in self.products:
			if d.is_inventory_product:
				if d.unit !=d.base_unit:
					if not check_uom_conversion(d.base_unit, d.unit):
						frappe.throw(_("There is no UoM conversion for product {}-{} from {} to {}".format(d.product_code, d.product_name, d.base_unit, d.unit)))


def validate_sale_product(self):
	sale_discount = self.discount  
	if sale_discount>0:
		if self.discount_type=="Amount":
			discountable_amount = Enumerable(self.products).where(lambda x: x.allow_discount==1 and x.discount==0).sum(lambda x: (x.quantity or 0)* (x.price or  0))
			sale_discount=(sale_discount / discountable_amount ) * 100
 
	for d in self.products:
		d.sub_total = (d.quantity or 0) * (d.price or 0)
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
		