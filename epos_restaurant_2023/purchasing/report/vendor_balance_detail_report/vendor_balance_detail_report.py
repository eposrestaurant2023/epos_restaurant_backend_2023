# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe


def execute(filters=None):
	 
	return columns(), get_report_data(filters)


def get_vendor(opening_data, current_transaction_data):
	 
	vendor_data = []
	if opening_data:
		
		vendor_data = [{"vendor":d["vendor"], "vendor_name":d["vendor_name"],"indent":0} for d in opening_data]
		
		
	if current_transaction_data:
	 
		vendor_data = vendor_data +  [{"vendor":d["vendor"], "vendor_name":d["vendor_name"],"indent":0} for d in current_transaction_data]

	unique_vendor = list(set(str(d) for d in vendor_data))

	 
	return [eval(d) for d in unique_vendor]


def get_report_data(filters):
	current_transaction_data= get_current_transaction(filters)
	opening_data = get_opening_balance(filters)
	 
	vendor_data = get_vendor(opening_data,current_transaction_data)
	report_data = []
	ending_balance = 0
	for v in sorted(vendor_data, key=lambda x: x['vendor']):
		opening_balance = [d for d in opening_data or [] if d["vendor"]==v["vendor"] ]
		ending_balance = ending_balance + (0 if not opening_balance else opening_balance[0]["amount"])

		report_data.append({
			"indent":0,
			"transaction_number": v["vendor_name"],
			"begining_balance":0 if not opening_balance else opening_balance[0]["amount"],
			"operation_balance":0,
			"last_balance":ending_balance
		})
		#render transaction
		for t in [d for d in current_transaction_data if d["vendor"] == v["vendor"]]:
				 
				ending_balance = ending_balance +  t["operation_balance"]
				report_data.append({
							"indent":1,
							"transaction_number": t["transaction_number"],
							"transaction_date": t["transaction_date"],
							"reference_number": t["reference_number"],
							"transaction_type": t["transaction_type"],
							"begining_balance":0,
							"operation_balance": t["operation_balance"],
							"last_balance":ending_balance
						})

		ending_balance = 0

	report_data.append({"indent":0,"begining_balance": 0,"operation_balance":0,"is_separate":1})
	report_data.append({
			"indent":0,
			"transaction_number": "Ending Balance",
			"begining_balance":sum([(d["begining_balance"] or 0)   for d in report_data]),
			"operation_balance":sum([  (d["operation_balance"] or 0)  for d in report_data]),
			"last_balance":sum([(d["begining_balance"] or 0)  +  (d["operation_balance"] or 0)  for d in report_data])
	})

	return report_data 


def get_opening_balance(filters):
	 
	sql = """
			with a as (
				select vendor, concat(vendor, '-',vendor_name) as vendor_name ,sum(grand_total) as amount   from `tabPurchase Order` where docstatus=1 and  posting_date < '{0}'    GROUP by vendor,vendor_name
				union all 
				select vendor,concat(vendor, '-',vendor_name) as vendor_name,sum(payment_amount*-1) as amount   from `tabPurchase Order Payment` where docstatus=1 and posting_date < '{0}'    group by vendor,vendor_name
			)
			select vendor,vendor_name, sum(amount) as amount from a group by vendor,vendor_name 
			
		""".format(filters.start_date)
	return frappe.db.sql(sql,as_dict=1)

def get_current_transaction(filters):
	condition = "where docstatus=1  and posting_date between %(start_date)s and %(end_date)s"
	if filters.get("vendor"):
		condition += "and vendor in %(vendor)s"
	sql = """
				select 
					vendor, 
					concat(vendor, '-',vendor_name) as vendor_name,
					name as transaction_number,
					referance as reference_number,
					posting_date as transaction_date,
					'Purchase' as transaction_type, 
					0 as begining_balance,
					grand_total as operation_balance,
					0 as last_balance 
					from `tabPurchase Order`
						{0}
				union
				select 
					vendor,
					concat(vendor, '-',vendor_name) as vendor_name,
					name as transaction_number,
					referance as reference_number,
					posting_date as transaction_date,
					'Payment' as transaction_type,
					0 as begining_balance,
					payment_amount * -1 as operation_balance,
					0 as last_balance 
					from `tabPurchase Order Payment`
						{0}
		""".format(condition)
	
	data = frappe.db.sql(sql,filters, as_dict=1)

	return data

def columns():
	return  [
		 
		{"fieldname":"transaction_number","label":"Transaction #","width":250 },
		{"fieldname":"reference_number", "label":"Ref. #" },
		{"fieldname":"transaction_date", "label":"Date","fieldtype":"Date","width": 120},
		{"fieldname":"transaction_type", "label":"Transaction Type" },
		{"fieldname":"begining_balance", "label":"Begining Balance","fieldtype":"Currency"},
		{"fieldname":"operation_balance", "label":"Operation Balance","fieldtype":"Currency"},
		{"fieldname":"last_balance", "label":"Last Balance","fieldtype":"Currency"},

	]
