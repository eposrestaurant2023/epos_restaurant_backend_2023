import json
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
                                fields=['name', 'promotion_name','priority','business_branch','start_date','end_date','start_time','end_time','percentage_discount','note','number_discount'], 
                                filters=filters,
                                order_by="priority")
    
    data =  Enumerable(promotions).where(lambda x:x.business_branch == business_branch or x.business_branch == None or x.business_branch == '')
    if len(data) > 0:
        if check_time:
            return True
        else:
            # check availible on customer
            promotions = []
            for d in data:
                pro = frappe.get_doc("Happy Hours Promotion",d.name)
                customer_groups = []
                for cg in pro.customer_group:
                    customer_groups.append({'customer_group_name_en':cg.customer_group_name_en, 'customer_group_name_kh':cg.customer_group_name_en})
                d.customer_groups = customer_groups
                promotions.append(d)
            # customer_promotion = frappe.get_doc("Happy Hours Promotion",promotion.name)
            # customer_groups = []
            # if len(customer_promotion.customer_group) > 0:
            #     for c in customer_promotion.customer_group:
            #         customer_groups.append({'name': c.name, 'customer_group_name_en':c.customer_group_name_en, 'customer_group_name_kh':c.customer_group_name_kh})
            # result = {
            #     'info': promotion,
            #     'customer_groups': customer_groups
            # }
            return promotions
    return False

@frappe.whitelist(allow_guest=True)
def check_promotion_product(promotions = [], product_name = ''):
    for p in promotions:
        product_promotion = frappe.db.sql("SELECT `name` FROM `tabPromotion Products` WHERE parent = '{0}' AND product_code = '{1}' AND docstatus = 1".format(p['name'], product_name), as_dict=1)
        if not product_promotion == []:
            return p
        
@frappe.whitelist(allow_guest=True)
def get_promotion_products(promotions = [],products = []):
    
    if len(promotions) > 0:
        promotion_list = []
        for p in json.loads(json.dumps(promotions)):
            promotion_data = frappe.get_doc('Happy Hours Promotion',p.name)
            promotion_list.append(promotion_data)
        return promotion_list