import json
import frappe
from frappe.utils import today, get_time, now, format_time
from py_linq import Enumerable
from datetime import datetime, timedelta

def str2bool(value):
    return str(value).lower() in ('true','yes','t','1')

@frappe.whitelist(allow_guest=True)
def check_promotion(business_branch = '', check_time = False):
    date = datetime.today()
    filters=[
            ['docstatus','=',1],
            ['start_date','<=', date],
            ['end_date', '>=', date]
    ]
    filters = list(filters)
    if str2bool(check_time): 
        filters.append(['start_time','<=', format_time(date,'HH:mm:ss')])
        filters.append(['end_time','>=', format_time(date,'HH:mm:ss')])
    
  
    promotions = frappe.get_list("Happy Hours Promotion", 
                                fields=['name', 'promotion_name','priority','business_branch','start_date','end_date','start_time','end_time','percentage_discount','note','number_discount'], 
                                filters=filters,
                                order_by="priority")  
   
    
    data =  Enumerable(promotions).where(lambda x:x.business_branch == business_branch or x.business_branch == None or x.business_branch == '')    
    if len(data) > 0:
        # check availible on customer
        promotions = []
        for d in data:
            pro = frappe.get_doc("Happy Hours Promotion",d.name)
            customer_groups = []
            for cg in pro.customer_group:
                customer_groups.append({'customer_group_name_en':cg.customer_group_name_en, 'customer_group_name_kh':cg.customer_group_name_en})
            d.customer_groups = customer_groups
            promotions.append(d) 
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
    date = datetime.today()
    

    result = []
    if len(promotions) > 0:
        promotion_list = Enumerable(promotions).order_by(lambda x:x['priority']).to_list()
        expire_promotions = []
        for p in promotion_list:
            # check promotion expire
            if p['end_time'] >= format_time(date,'HH:mm:ss'):
                promotion_data = frappe.get_doc('Happy Hours Promotion',p['name'])
                for product in products:
                    order_time = datetime.strftime(datetime.strptime(product['order_time'][11:], '%H:%M:%S.%f'), "%H:%M:%S")
                    if order_time >= datetime.strftime(datetime.strptime(str(promotion_data.start_time), '%H:%M:%S'), '%H:%M:%S'):
                        t = Enumerable(promotion_data.products).where(lambda x: x.product_code == product['product_code'])
                        if t[0]:
                            result.append({
                                'product_code':product['product_code'],
                                'order_time': order_time,
                                'promotion_name': p['name'],
                                'promotion_title': p['promotion_name'],
                                'percentage_discount': p['percentage_discount']
                            })
                            products.remove(product)
            else:
                expire_promotions.append(p)
    return {
        'product_promotions': result,
        'expire_promotion': expire_promotions
    }