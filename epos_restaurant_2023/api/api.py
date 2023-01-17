import json
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
def get_system_settings(pos_profile="", device_name=''):
    if not frappe.db.exists("POS Profile",pos_profile):
        frappe.throw("Invalid POS Profile name")
    
    profile = frappe.get_doc("POS Profile",pos_profile)
    pos_config = frappe.get_doc("POS Config",profile.pos_config)

    doc = frappe.get_doc('ePOS Settings')
    table_groups = []
    for g in profile.table_groups:
        
        table_groups.append({"key":g.table_group.lower().replace(" ","_"),"table_group":g.table_group,"background":frappe.get_value("Table Group",g.table_group,"photo"),"tables":get_tables_number(g.table_group, device_name),})
    pos_menus = []
    for m in profile.pos_menus:
        pos_menus.append({"pos_menu":m.pos_menu})   
        
    payment_types=[]
    for p in profile.payment_types:
        
        payment_types.append({"payment_method":p.payment_type,"allow_cash_float":p.allow_cash_float, "input_amount":0,"exchange_rate":p.exchange_rate})
    
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
        "default_pos_menu":profile.default_pos_menu,
        "payment_types":payment_types,
        "tax_1_name":doc.tax_1_name,
        "tax_2_name":doc.tax_2_name,
        "tax_3_name":doc.tax_3_name,
        "use_guest_cover":doc.use_guest_cover,
        "sale_status":frappe.db.sql("select name,background_color from `tabSale Status`", as_dict=1)
        
        
    }
    return  data

@frappe.whitelist(allow_guest=True)
def get_tables_number(table_group,device_name):
    data = frappe.get_all("Tables Number",
                fields=["name as id","tbl_number as tbl_no","shape","sale_type","default_discount","height as h","width as w","price_rule"],
                filters={"tbl_group":table_group}
            )
    background_color = frappe.db.get_default("default_table_number_background_color")
    for d in data:
        d.background_color=background_color
        position = frappe.db.sql("select x,y,h,w from `tabePOS Table Position` where device_name='{}' and tbl_number='{}' limit 1".format(device_name,d.tbl_no ), as_dict=1)
        if position:
            for p in position:
                d.x = p.x or 0
                d.y = p.y or 0
                d.w = p.w or 100
                d.h = p.h or 100
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
def get_current_cashier_shift(pos_profile):
   
    sql = "select name,working_day, posting_date, pos_profile, opened_note from `tabCashier Shift` where pos_profile = '{}' and is_closed = 0 order by creation limit 1".format(pos_profile)
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
def save_table_position(device_name, table_group):
    # frappe.throw("{}".format(table_group))
    frappe.db.sql("delete from `tabePOS Table Position` where device_name='{}'".format(device_name) )
    
    for g in table_group:
        
        for t in g['tables']:
            x = 0
            if "x" in t:
                x = t["x"]
            y = 0
            if "y" in t:
                y = t["y"]
            h = 0
            if "h" in t:
                h = t["h"]
            w = 0
            if "w" in t:
                w = t["w"]
            if not frappe.db.exists('ePOS Table Position', {'tbl_number': t['tbl_no'], 'device_name': device_name}):
                doc = frappe.get_doc({
                        'doctype': 'ePOS Table Position',
                        'device_name':device_name,
                        'tbl_number': t['tbl_no'],
                        'x':x,
                        'y':y,
                        'h':h,
                        'w':w
                    })
                doc.insert()
