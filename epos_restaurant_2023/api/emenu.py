import json
import frappe
import base64
from py_linq import Enumerable
from frappe.utils import today, add_to_date
from datetime import datetime, timedelta
from frappe import _


@frappe.whitelist(allow_guest=True)
def get_emenu_category(shortcut,is_main_emenu = False):
    filter_main_emenu = ""
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
def get_emenu_settings(business_branch = ''):
    doc = frappe.get_doc('ePOS Settings')
    emenus = Enumerable(doc.emenu).where(lambda x:x.business_branch == business_branch or '')
    emenu = frappe.get_doc('eMenu', emenus[0].emenu)
  
    pos_menu = frappe.db.sql("SELECT `name`, pos_menu_name_en, pos_menu_name_kh,parent_pos_menu, is_main_emenu FROM `tabPOS Menu` WHERE parent_pos_menu = '{}' ORDER BY sort_order".format(emenu.pos_menu), as_dict=1)
    return { 
        "title": emenu.title,
        "welcome_title": emenu.welcome_title,
        "welcome_description": emenu.welcome_description,
        "slideshow": emenu.slideshow,
        "pos_menu": pos_menu,
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
def get_emenu_product(menu):
    pos_menu = frappe.db.sql("""
        SELECT p.product_code  FROM `tabProduct Menu` AS pm
        INNER JOIN `tabProduct` AS p
        ON pm.parent = p.name
        WHERE pm.pos_menu = '{}'
    """.format(menu), as_dict=1)
    data = []
    if pos_menu:
        for d in pos_menu:
            product = frappe.get_doc("Product", d.product_code)
            data.append(product)
    return data