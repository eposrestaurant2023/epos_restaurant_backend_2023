import frappe
from frappe.utils import today, get_time, now, format_time
from py_linq import Enumerable
from datetime import datetime, timedelta
@frappe.whitelist(allow_guest=True)
def check_promotion(business_branch = '', check_time = False):
    date = datetime.today()
    filters=[
            ['docstatus','=',1],
            ['start_date','<=', date],
            ['end_date', '>=', date]
    ]
    filters = list(filters)
    if check_time: 
        filters.append(['start_time','<=', format_time(date,'HH:mm:ss')])
        filters.append(['end_time','>=', format_time(date,'HH:mm:ss')])
  
    promotions = frappe.get_list("Happy Hours Promotion", 
                                fields=['name', 'promotion_name','business_branch','start_time','end_time','percentage_discount','note','number_discount'], 
                                filters=filters,
                                order_by="priority")
    
    data =  Enumerable(promotions).where(lambda x:x.business_branch == business_branch or '')
    if data[0]:
        if check_time:
            return True
        else:
            # check availible on customer
            promotion = data[0]
            customer_promotion = frappe.get_doc("Happy Hours Promotion",promotion.name)
            customer_groups = []
            if len(customer_promotion.customer_group) > 0:
                for c in customer_promotion.customer_group:
                    customer_groups.append({'name': c.name, 'customer_group_name_en':c.customer_group_name_en, 'customer_group_name_kh':c.customer_group_name_kh})
            result = {
                'info': promotion,
                'customer_groups': customer_groups
            }
            return result
    return False

@frappe.whitelist(allow_guest=True)
def check_promotion_product(promotion_name = '', product_name = ''):
    product_promotion = frappe.db.sql("SELECT `name` FROM `tabPromotion Products` WHERE parent = '{0}' AND product_code = '{1}'".format(promotion_name, product_name), as_dict=1)
    return product_promotion[0] if len(product_promotion) > 0  else ''

@frappe.whitelist(allow_guest=True)
def get_promotion_products(promotion_name,products = []):
    filter_products = "', '".join(products)
    
    product_promotions = frappe.db.sql("SELECT `product_code` FROM `tabPromotion Products` WHERE parent = '{0}' AND product_code IN ('{1}')".format(promotion_name, filter_products), as_dict=1)
    return product_promotions