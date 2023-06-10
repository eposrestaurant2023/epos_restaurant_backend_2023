import json
import time
import frappe
import base64
from py_linq import Enumerable
from frappe.utils import today, add_to_date
from datetime import datetime, timedelta
from frappe import _


@frappe.whitelist()
def on_save(data,current_sale_id ): 
    for d in data:       
        if d["temp_deleted"]:    
            if d["name"]:    
                frappe.delete_doc("Sale", str(d["name"]))
                
        else:
            doc = frappe.get_doc(d)
            doc.save() 

    frappe.db.commit()

    _current_sale = frappe.get_doc("Sale",str(current_sale_id))

    return _current_sale


@frappe.whitelist()
def get_sales(sale_id ): 
    sale = frappe.get_doc("Sale", str(sale_id))
    sales = []
     
    sale_list = frappe.get_list("Sale",
                                limit_page_length=100,
                                filters={
                                    'name':['!=',sale.name], 
                                    'table_id':sale.table_id,
                                    'cashier_shift':sale.cashier_shift,
                                    'docstatus':0
                                })
    for s in sale_list:
        doc = frappe.get_doc("Sale", str(s.name))
        sales.append(doc)

    return sales