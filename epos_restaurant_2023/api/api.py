import time
import frappe
import base64
from frappe import _
@frappe.whitelist(allow_guest=True)
def check_username(pin_code):
    if pin_code:    
        pin_code = (str( base64.b64encode(pin_code.encode("utf-8")).decode("utf-8")))
        data = frappe.db.sql("select name from `tabUser` where pos_pin_code='{}' and allow_login_to_pos=1 limit 1".format(pin_code),as_dict=1)
        if data:
            return {"username":data[0]["name"]} 
        
    frappe.throw(_("Invalid pin code"))
    

@frappe.whitelist(allow_guest=True)
def get_system_settings():
   
    return  frappe.get_doc('ePOS Settings')

@frappe.whitelist()
def get_shortcut_menu():
    return frappe.get_list("Product", fields=["name"])
@frappe.whitelist()
def get_product_menu(pos_menu=''):
    return frappe.db.sql(
		"""
		SELECT * FROM `tabProduct` p
        INNER JOIN `tabProduct Menu` m
        ON p.name = m.parent
        WHERE m.pos_menu = '{}'
		""".format(pos_menu),
		as_dict=True
	)
    

