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
def get_system_settings(pos_profile=""):
    if not frappe.db.exists("POS Profile",pos_profile):
        frappe.throw("Invalid POS Profile name")
    
    profile = frappe.get_doc("POS Profile",pos_profile)
    pos_config = frappe.get_doc("POS Config",profile.pos_config)

    doc = frappe.get_doc('ePOS Settings')
    table_groups = []
    for g in profile.table_groups:
        
        table_groups.append({"table_group":g.table_group,"background":frappe.get_value("Table Group",g.table_group,"photo"),"tables":get_tables_number(g.table_group)})
    pos_menus = []
    for m in profile.pos_menus:
        pos_menus.append({"pos_menu":m.pos_menu})   
        
    payment_types=[]
    for p in profile.payment_types:
        payment_types.append({"payment_type":p.payment_type})
    data={
        "app_name":doc.epos_app_name,
        "business_branch":profile.business_branch,
        "address":pos_config.address,
        "logo":pos_config.logo,
        "phone_number":pos_config.phone_number,
        "pos_profile":pos_profile,
        "outlet":profile.outlet,
        "price_rule":profile.price_rule,
        "stock_location":profile.stock_location,
        "tax_rule":profile.tax_rule,
        "login_background":pos_config.login_background,
        "home_background":pos_config.home_background,
        "thank_you_background":pos_config.thank_you_background,
        "table_groups":table_groups,
        "pos_menus":pos_menus,
        "payment_type":payment_types
        
    }
    return  data

def get_tables_number(table_group):
    data = frappe.get_all("Tables Number",
                fields=["tbl_number","shape","sale_type","default_discount","height","width","price_rule"],
                filters={"tbl_group":table_group}
            )
    return data

@frappe.whitelist(allow_guest=True)
def check_pos_profile(pos_profile_name):
    if not frappe.db.exists("POS Profile", pos_profile_name):
        frappe.throw("Invalid POS Profile")
    return


@frappe.whitelist()
def get_current_working_day(pos_profile):
   
    sql = "select name, posting_date, pos_profile, note from `tabWorking Day` where pos_profile = '{}' and is_closed = 0 order by creation limit 1".format(pos_profile)
    data =  frappe.db.sql(sql, as_dict=1) 
    if data:
        return data [0]
    return

@frappe.whitelist()
def get_user_information():
    data = frappe.get_doc("User",frappe.session.user)
    return {
        "name":data.name,
        "full_name":data.full_name,
        "role":data.role_profile_name,
        "phone_number":data.phone,
        "photo":data.user_image
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
    
