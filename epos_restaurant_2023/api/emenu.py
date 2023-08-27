import json
import frappe
import base64
from py_linq import Enumerable
from frappe.utils import today, add_to_date
from datetime import datetime, timedelta
from frappe import _

@frappe.whitelist(allow_guest=True)
def get_emenu_settings(business_branch = ''):
    doc = frappe.get_doc('ePOS Settings') 
    emenus = Enumerable(doc.emenu).where(lambda x:(x.business_branch or "") == (business_branch or ""))   
    emenu = frappe.get_doc('eMenu', emenus[0].emenu)  
    pos_menu = frappe.db.sql("SELECT `name`, pos_menu_name_en, pos_menu_name_kh,parent_pos_menu, is_main_emenu FROM `tabPOS Menu` WHERE parent_pos_menu = '{}' ORDER BY sort_order".format(emenu.pos_menu), as_dict=1)
   
    #get currency
    currencies = frappe.db.sql("select name,symbol,currency_precision,symbol_on_right,pos_currency_format from `tabCurrency` where enabled=1", as_dict=1)
 
    main_currency = frappe.get_doc("Currency",frappe.db.get_default("currency"))
    second_currency = frappe.get_doc("Currency",frappe.db.get_default("second_currency"))

    exchange_rate_main_currency = frappe.db.get_default("exchange_rate_main_currency")

    to_currency = second_currency.name
    if exchange_rate_main_currency != main_currency.name:
        to_currency = main_currency.name   

    
    _query = "select exchange_rate from `tabCurrency Exchange` where from_currency = '{}' and to_currency = '{}' limit 1".format(exchange_rate_main_currency,to_currency) 
    _exchange_rate = frappe.db.sql(_query, as_dict=1)
    exchange_rate = 1
    if _exchange_rate:
        exchange_rate = _exchange_rate[0].exchange_rate
   
    return { 
        "title": emenu.title,
        "welcome_title": emenu.welcome_title,
        "welcome_description": emenu.welcome_description,
        "slideshow": emenu.slideshow,
        "pos_menu": pos_menu,
        "exchange_rate":exchange_rate,
        "currencies":currencies, 
        "default_currency":frappe.db.get_default("currency"),
        "allow_make_order":emenus[0].allow_make_order,
        "template_style": {

            "title_color": emenu.title_color,
            "title_class": emenu.title_class,
            "subtitle_color": emenu.subtitle_color,
            "text_color": emenu.text_color,
            "text_class": emenu.text_class,
            "shortcut_color": emenu.shortcut_color,
            "shortcut_active_color": emenu.shortcut_active_color,
            "shortcut_background": emenu.shortcut_background,
            "shortcut_active_background": emenu.shortcut_active_background,
            "height_banner": emenu.height_banner,
            "background_color_banner": emenu.background_color_banner,
            "background_image_banner": emenu.background_image_banner,
        }
    }

@frappe.whitelist(allow_guest=True)
def get_pos_profile(name):
    if not frappe.db.exists("POS Profile",name):
        frappe.throw("Invalid QR")   

    return frappe.get_doc("POS Profile",name)

@frappe.whitelist(allow_guest=True)
def get_pos_table(name,groups):
    sql = "SELECT `name`,tbl_number FROM `tabTables Number` where tbl_number = '{}' and tbl_group in ({}) limit 1".format(name,groups)
    table  = frappe.db.sql(sql, as_dict=1)
    if table:
        return table[0]

    return

@frappe.whitelist(allow_guest=True)
def get_emenu_menu(shortcut,is_main_emenu = False):
    if is_main_emenu:
        data = get_loop_menu(shortcut)
        return data
    
    return []



@frappe.whitelist(allow_guest=True)
def get_loop_menu(parent,is_child=False):
    data = get_parent_menu(parent,is_child)
    for d in data:
        if d.is_group :
            d.child = get_loop_menu(d.name, True) 
    return data

@frappe.whitelist(allow_guest=True)
def get_parent_menu(parent,is_child=False):

    filter_main_emenu =  ""
    if not is_child:
        filter_main_emenu = "or position_main_menu = 1"

    query = """SELECT `name`, 
        title_en,
        title_kh,
        description,
        show_description, 
        parent_pos_menu,
        background_image ,
        is_group
    FROM `tabPOS Menu` 
    WHERE is_emenu = 1 and (parent_pos_menu = '{0}' {1}) 
    ORDER BY sort_order""".format(parent,filter_main_emenu)
    data = frappe.db.sql(query, as_dict=1)
    return data


@frappe.whitelist(allow_guest=True)
def get_emenu_product(menu):
    sql = """select 
                name as menu_product_name,
                product_code as name,
                product_name_en as name_en,
                product_name_kh as name_kh,
                '{0}' as parent,
                price,
                unit,
                allow_discount,
                allow_change_price,
                allow_free,
                is_open_product,
                is_inventory_product,
                is_require_employee,
                prices,
                printers,
                modifiers,
                photo,
                'product' as type,
                append_quantity,
                is_combo_menu,
                use_combo_group,
                combo_menu_data,
                combo_group_data,
                tax_rule,
                sort_order,
                tax_rule_data,
                revenue_group,
                sort_order
            from  `tabTemp Product Menu` 
            where 
                pos_menu='{0}' 
            order by sort_order
            """.format(menu)
    data = frappe.db.sql(sql,as_dict=1)

    return data




@frappe.whitelist(allow_guest=True)
def get_current_working_day(business_branch):
   
    sql = "select name, posting_date, pos_profile, note from `tabWorking Day` where business_branch = '{}' and is_closed = 0 order by creation limit 1".format(business_branch)
    data =  frappe.db.sql(sql, as_dict=1) 
    if data:
        return data [0]
    return

@frappe.whitelist(allow_guest=True)
def get_current_shift_information(business_branch, pos_profile,customer):

    default_customer = frappe.get_doc("Customer", customer)
    return {
        "working_day":get_current_working_day(business_branch),
        "cashier_shift":get_current_cashier_shift(pos_profile),
        "default_customer":default_customer
    }

@frappe.whitelist(allow_guest=True)
def get_current_cashier_shift(pos_profile):   
    sql = "select name,working_day, posting_date,shift_name, pos_profile, opened_note,business_branch,total_opening_amount from `tabCashier Shift` where pos_profile = '{}' and is_closed = 0 order by creation desc limit 1".format(pos_profile)
 
    data =  frappe.db.sql(sql, as_dict=1) 
    if data:
        return data [0]
    return



@frappe.whitelist(allow_guest=True)
def on_emenu_initialize(pos_profile=''): 
    _sql =  """select 
                `name`,
                business_branch 
                from `tabPOS Profile` 
            where `name`= if('{0}'='',`name`,'{0}')""".format(pos_profile)
    doc = frappe.db.sql(_sql,as_dict=1)

    if len(doc) > 1:
        return {"_error":"Plesase scan QR"}
    
    elif len(doc)==1:
        pos_setting = frappe.get_doc('ePOS Settings') 
        emenus = Enumerable(pos_setting.emenu) 

        emenu_templates = emenus.where(lambda x:(x.business_branch or "") == doc[0].business_branch and x.default == 1)
        if emenu_templates.count()>0:
            ## get emenu template
            return on_get_emenu_template(emenu_templates[0].emenu)
             
        else:
            emenu_templates = emenus.where(lambda x:(x.business_branch or "") == doc[0].business_branch)
            if emenu_templates.count()>0:
                ## get emenu template
                return on_get_emenu_template(emenu_templates[0].emenu)   
                          
            return {"_error":"Invalid QR"}

    return {"_error":"Invalid QR"}

@frappe.whitelist(allow_guest=True)
def on_get_emenu_template(name):
    _app_setting = frappe.get_doc('eMenu', name)  
 
    _menu_tree = on_get_emenu_tree(_app_setting.pos_menu,True)

    return {
        "app_setting":_app_setting,
        "menus":_menu_tree
    }


@frappe.whitelist(allow_guest=True)
def on_get_emenu_tree(parent, is_root = False):
    _parent = "parent_pos_menu = '{}'".format(parent)  
    is_main_emenu = "is_main_emenu"
    if is_root  : 
        is_main_emenu = "1"
        _parent ="`name` = '{}'".format(parent)  
        
    query = """SELECT `name`, 
        title_en,
        title_kh,
        description,
        show_description, 
        parent_pos_menu,
        background_image ,
        is_group,
        {1} as is_main_emenu,
        {2} as is_root
    FROM `tabPOS Menu` 
    WHERE is_emenu = 1 and {0} 
    ORDER BY sort_order""".format(_parent,is_main_emenu ,is_root) 
 
    data = frappe.db.sql(query, as_dict=1) 
    for c in data: 
        c.children = on_get_emenu_tree(c.name)


    return data
 