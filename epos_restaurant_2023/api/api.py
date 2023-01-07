import frappe
import base64
@frappe.whitelist(allow_guest=True)
def check_user(pin_code):
    
    return {
            "user":"admin",
            "pass":str( base64.b64encode(pin_code.encode("utf-8")).decode("utf-8"))
        } 
 

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
    

