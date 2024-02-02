import frappe
from frappe import _
import datetime

def execute(filters=None):
	validate(filters)
	report_data = []
	report_data = get_report_data(filters)

	return get_columns(), report_data
 
def validate(filters):
	if not filters.start_date:
		filters.start_date = datetime.date.today()
  
	if not filters.end_date:
		filters.end_date =  datetime.date.today()

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:
			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))
 
def get_columns():
	return [
		{"label":"Date", "fieldname":"creation","fieldtype":"Datetime", "align":"center", "width":100},
		{"label":"Closed Date", "fieldname":"closed_date","fieldtype":"Datetime", "align":"center","width":120},
		{"label":"INV #", "fieldname":"name","fieldtype":"Data", "align":"center","width":75},
  		{"label":"Customer Type", "fieldname":"customer_group","fieldtype":"Link","options":"Customer Group"},
		{"label":"Customer Name", "fieldname":"customer_name","fieldtype":"Data"},
		{"label":"Product Category", "fieldname":"product_category","fieldtype":"Data"},
		{"label":"Product Code", "fieldname":"product_code","fieldtype":"Data","fieldtype":"Link","options":"Product"},
		{"label":"Product Name", "fieldname":"product_name","fieldtype":"Data"},
		{"label":"UOM", "fieldname":"unit","fieldtype":"Data"},
		{"label":"Payment Method", "fieldname":"payment_type","fieldtype":"Data","align":"right"},
		{"label":"Quantity", "fieldname":"quantity","fieldtype":"Float","align":"right","width":100},
		{"label":"Price", "fieldname":"price","fieldtype":"Currency","align":"right"},
		{"label":"Sub Total", "fieldname":"sub_total","fieldtype":"Currency","align":"right"},
		{"label":"Total Discount", "fieldname":"total_discount","fieldtype":"Currency","align":"right"},
		{"label":"Tax Amt", "fieldname":"total_tax","fieldtype":"Currency","align":"right"},
		{"label":"Grand Total", "fieldname":"total_revenue","fieldtype":"Currency","align":"right"},
		{"label":"Cost Amt", "fieldname":"cost","fieldtype":"Currency","align":"right"},
		{"label":"Profit Amt", "fieldname":"total_profit","fieldtype":"Currency","align":"right"},
		{"label":"Free", "fieldname":"free","fieldtype":"Check","align":"right"},
		
	]
 
 

def get_conditions(filters):
	conditions = "where s.docstatus = 1 "

	start_date = filters.start_date
	end_date = filters.end_date

	conditions += " AND s.posting_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("cashier_shift"):
		conditions += " AND s.cashier_shift = %(cashier_shift)s"
	
	return conditions

def get_report_data(filters):
	
	sql = """select
                sp.creation,
                s.closed_date,
                s.name,
                s.customer_group,
                s.customer_name,
                sp.product_category,
                sp.product_code,
                sp.product_name,
                sp.unit,
                (SELECT GROUP_CONCAT( distinct payment_type SEPARATOR ',') FROM `tabPOS Sale Payment` where parent = s.name) payment_type,
                sp.quantity,
                if(sp.is_free = 0,sp.price,sp.backup_product_price) as price,
                if(sp.is_free = 0,sp.price * sp.quantity,sp.backup_product_price * sp.quantity) sub_total,
                if(sp.is_free = 0,sp.total_discount,sp.backup_product_price * sp.quantity) total_discount,
                sp.total_tax,
                sp.total_revenue,
                sp.cost,
                (sp.total_revenue - sp.cost) as total_profit,
                sp.is_free as free
            from `tabSale` s inner join
                `tabSale Product` sp on s.name = sp.parent
                inner join 
                `tabWorking Day` wd on wd.name = s.working_day
            {}
        order by s.closed_date
                
	""".format(get_conditions(filters))	
	data = frappe.db.sql(sql,filters, as_dict=1)
	
	return data
 

