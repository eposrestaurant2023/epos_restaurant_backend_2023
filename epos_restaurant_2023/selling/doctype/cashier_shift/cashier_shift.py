# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt
import json
from py_linq import Enumerable
import frappe
from frappe.model.document import Document
from frappe.model.naming import NamingSeries
class CashierShift(Document):
	def validate(self):
		# #if close shift check current bill open 
		# if self.is_closed==1:
		# 	pending_orders = frappe.db.sql("select name from `tabSale` where docstatus = 0 and cashier_shift = '{}'".format(self.name), as_dict=1)
		# 	if pending_orders:
		# 		frappe.throw("Please close all pending order before close cashier shift.")
		
		if self.is_new():
			data = frappe.get_list("Cashier Shift",filters={"pos_profile":self.pos_profile,"business_branch":self.business_branch, "is_closed":0})
			
			if data:
				frappe.throw("Cashier shift is already opened")
				
		
		for c in self.cash_float:
			exchange_rate = frappe.get_value("Payment Type", c.payment_method,"exchange_rate")
			exchange_rate = exchange_rate or 1
			c.exchange_rate = exchange_rate
			
			c.opening_amount = float(c.input_amount) / exchange_rate
			c.close_amount = float(c.input_close_amount) / exchange_rate
			c.system_close_amount = float(c.input_system_close_amount) / exchange_rate

   
		self.total_opening_amount = Enumerable(self.cash_float).sum(lambda x: x.opening_amount)
		self.total_system_close_amount = Enumerable(self.cash_float).sum(lambda x: x.system_close_amount)
		self.total_close_amount = Enumerable(self.cash_float).sum(lambda x: x.close_amount)
		self.total_different_amount = self.total_close_amount -  self.total_system_close_amount

		# check if close shift then check 
		if self.is_closed==1:
			pos_profile = frappe.get_doc("POS Profile", self.pos_profile)
			if pos_profile.reset_waiting_number_after=="Close Cashier Shift":
				prefix = pos_profile.waiting_number_prefix.replace('.','').replace("#",'')
				naming_series = NamingSeries(prefix)
				naming_series.update_counter(0)

			# check if have edoor app for generate FB revenue to Folio Transaction group by acc.code
			if 'edoor' in frappe.get_installed_apps():
				sales = frappe.db.get_list("Sale",filters=[
												{"cashier_shift":self.name},
												{"outlet":self.outlet},
												{"shift_name":self.shift_name},
												{"docstatus":1}
											],
											limit=500,
											fields=["name"])
				sale_products = []
				sale_payments =[]
				for s in sales:
					# get sale product by sale
					sale  = frappe.get_doc('Sale',s['name'])
					for sp in sale.sale_products:
						sale_products.append({
							"sale_name":sale.name,
							"cashier_shift":sale.cashier_shift,
							"outlet":sale.outlet,
							"shift_name":sale.shift_name,
							"revenue_group":sp.revenue_group,
							"revenue_code":sp.revenue_code,
							"account_code":sp.account_code,
							"revenue_amount":sp.total_revenue,
							"discount_account":sp.discount_account,
							"discount_amount":(sp.discount_amount or 0) + (sp.sale_discount_amount or 0),
							"tax_1_account":sp.tax_1_account,
							"tax_1_amount":sp.tax_1_amount,
							"tax_2_account":sp.tax_2_account,
							"tax_2_amount":sp.tax_2_amount,
							"tax_3_account":sp.tax_3_account,
							"tax_3_amount":sp.tax_3_amount,
						})
					
					#get sale payment data
					payments = frappe.db.get_list("Sale Payment",
								filters={"sale":s['name']},
								fields=["account_code","payment_type_group","payment_type","payment_amount","fee_amount"],
								as_list=False)
					
					#get pos sale payment data
					pos_payments = frappe.db.get_all("POS Sale Payment",
								filters={
									'parent':s['name'],
									'payment_type_group':'On Account'
									},
								fields=["account_code","payment_type_group","payment_type","amount","fee_amount"],
								as_list=False)
					
					#assign data to temp
					for p in payments:
						sale_payments.append({
							"sale_name":sale.name,
							"cashier_shift":sale.cashier_shift,
							"outlet":sale.outlet,
							"shift_name":sale.shift_name,
							"account_code":p.account_code,
							"payment_type_group":p.payment_type_group,
							"payment_type":p.payment_type,
							"payment_amount":p.payment_amount + (p.fee_amount or 0),
							"fee_amount":p.fee_amount or 0
						})
					
					#assign data to temp get from pos sale payment  (on account pay transaction)
					for p in pos_payments:
						sale_payments.append({
							"sale_name":sale.name,
							"cashier_shift":sale.cashier_shift,
							"outlet":sale.outlet,
							"shift_name":sale.shift_name,
							"account_code":p.account_code,
							"payment_type_group":p.payment_type_group,
							"payment_type":p.payment_type,
							"payment_amount":p.amount + (p.fee_amount or 0),
							"fee_amount":p.fee_amount or 0
						})
					
				
				#create folio transaction buy account code transaction
				## revenue sale revenue
				revenue = get_sale_product_revenue(sale_products)				 
				for g in revenue['revenue_group']:
					if g['account'] !="" and g['amount'] > 0:	
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":"Debit"
							} 
						doc = frappe.get_doc(data)
						doc.insert()
		
							 
				## get discount
				for g in revenue['discount']:
					if g['account'] !="" and g['amount'] > 0:						
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":"Debit"
							} 
						doc = frappe.get_doc(data)
						doc.insert()
				
				## get tax 
				for g in revenue['tax_1']:
					if g['account'] !="" and g['amount'] > 0:	
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":"Debit"
							} 
						doc = frappe.get_doc(data)
						doc.insert()

				## get tax 2
				for g in revenue['tax_2']:
					if g['account'] !="" and g['amount'] > 0:
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":"Debit"
							} 
						doc = frappe.get_doc(data)
						doc.insert()
				
				## get tax 3
				for g in revenue['tax_3']:
					if g['account'] !="" and g['amount'] > 0:	
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":"Debit"
							} 
						doc = frappe.get_doc(data)
						doc.insert()
				
				
				## revenue payment
				payment_revenue = get_sale_payment(sale_payments)
				for g in payment_revenue:
					if g['account'] !="" and g['amount'] > 0:
						## create folio transaction bank fee 
						if g['fee_amount'] > 0:
							account_code = frappe.get_doc("Account Code",str(g['account']))					

							if account_code.allow_bank_fee==1:
								if  account_code.bank_fee_account != "":
									doc_fee = frappe.get_doc({
											'doctype': 'Folio Transaction',
											'property':self.business_branch,
											'working_day':self.working_day,
											'posting_date':self.posting_date,
											'transaction_type': "Cashier Shift",
											'transaction_number': self.name,
											'reference_number':self.name,
											"input_amount":g['fee_amount'],
											"account_code":account_code.bank_fee_account
										})
									doc_fee.insert()

						##end create folio transaction bank fee

						tran_type = "Debit" 
						if g['payment_type_group'] =="Pay to Room":
							tran_type = "Credit"						
						data = {
								'doctype': 'Folio Transaction',
								'property':self.business_branch,
								'working_day':self.working_day,
								'posting_date':self.posting_date,
								'transaction_type': "Cashier Shift",
								'transaction_number': self.name,
								'reference_number':self.name,
								"input_amount":g['amount'],
								"account_code":g['account'],
								"type":tran_type
							} 
						
						doc = frappe.get_doc(data)
						doc.insert()
						

				#commit data
				frappe.db.commit()		

	def after_insert(self):	
		query ="update `tabSale` set working_day='{}', cashier_shift='{}', shift_name='{}' "
		query = query + " where docstatus = 0 and pos_profile = '{}'"

		query = query.format(self.working_day, self.name,self.shift_name, self.pos_profile)

		frappe.db.sql(query)

# get sale product revenue sum group by
def get_sale_product_revenue(sale_products):
	sale_product_revenue ={
		"revenue_group":sum_revenue_by_field(sale_products,field_account="account_code",field_amount="revenue_amount"),
		"discount":sum_revenue_by_field(sale_products,field_account="discount_account",field_amount="discount_amount"),
		"tax_1":sum_revenue_by_field(sale_products,field_account="tax_1_account",field_amount="tax_1_amount"),
		"tax_2":sum_revenue_by_field(sale_products,field_account="tax_2_account",field_amount="tax_2_amount"),
		"tax_3":sum_revenue_by_field(sale_products,field_account="tax_3_account",field_amount="tax_3_amount")
	}

	return sale_product_revenue
 
# sale prduct sum group by with field
def sum_revenue_by_field(sale_products,field_amount,field_account):
	result = []
	groups = {}
	for row in sale_products:
		group = {
				"cashier_shift":row["cashier_shift"],
				"outlet": row["outlet"],
				"shift_name":row["shift_name"],
				"revenue_group":row["revenue_group"],
				"revenue_code":row["revenue_code"],
				field_account:row[field_account]
			}
		
		_field_amount = row[field_amount]
		g = json.dumps(group)	  
		if g not in groups:
			groups[g] = {field_amount: []} 

		groups[g][field_amount].append(_field_amount)


	for group, total in groups.items():	 
		total_amount = sum(total[field_amount])
		g = json.loads(group)	
		
		_result = {}
		_result.update({
			"revenue_code":g["revenue_code"],
			"account":(g[field_account] or ""),
			"amount":(total_amount or 0)
			})	

		result.append(_result)	

	return result


def get_sale_payment(sale_payments):
	result = []
	groups = {}
	for row in sale_payments:
		group = {				
				"payment_type_group": row["payment_type_group"],
				"account_code":row["account_code"]
			}
		
		payment_amount = row['payment_amount']
		fee_amount = row['fee_amount']
		g = json.dumps(group)	  
		if g not in groups:
			groups[g] = {'payment_amount': [],'fee_amount':[]} 

		groups[g]['payment_amount'].append(payment_amount)
		groups[g]['fee_amount'].append(fee_amount)


	for group, total in groups.items():	 
		total_amount = sum(total['payment_amount'])
		total_fee = sum(total['fee_amount'])
		g = json.loads(group)	
		
		_result = {}
		_result.update({
			"payment_type_group":(g['payment_type_group'] or ""),
			"account":(g['account_code'] or ""),
			"amount":(total_amount or 0),
			"fee_amount":(total_fee or 0),
			})	
		result.append(_result)	
	
	return result