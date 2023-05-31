
import frappe
import json
from frappe.utils.response import json_handler

@frappe.whitelist(allow_guest=True)
def get_product_by_menu(root_menu=""):
    if root_menu=="":
        return []
    else:
        menus = []
        sql = """select 
                    name,
                    pos_menu_name_en as name_en,
                    pos_menu_name_kh as name_kh,
                    parent_pos_menu as parent,
                    photo,
                    text_color,
                    background_color,
                    shortcut_menu,
                    price_rule,
                    photo,
                    'menu' as type,
                    sort_order
                from `tabPOS Menu` 
                where 
                    parent_pos_menu='{}' and
                    disabled = 0 
                order by sort_order, name
                """.format(root_menu)
        data = frappe.db.sql(sql,as_dict=1)
        for d in data:
            menus.append(d)
            
            for m in get_child_menus(d.name):
                menus.append(m)
            
            for m in get_products(d.name):
                menus.append(m)
             
        return menus

def get_child_menus(parent_menu):
    menus = []
    menus.append({"type":"back","parent":parent_menu})
    sql = """select 
                name,
                pos_menu_name_en as name_en,
                pos_menu_name_kh as name_kh,
                parent_pos_menu as parent,
                photo,
                text_color,
                background_color,
                shortcut_menu,
                price_rule,
                'menu' as type,
                sort_order
            from `tabPOS Menu` 
            where 
                parent_pos_menu='{}' and
                disabled = 0 
            order by sort_order, name
            """.format(parent_menu)
    data = frappe.db.sql(sql,as_dict=1)
    for d in data:
        
        menus.append(d)
     
        for m in get_child_menus(d.name):
            menus.append(m)
        
        for m in get_products(d.name):
            menus.append(m)
        
        
    return menus


def get_products(parent_menu):
     
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
                sort_order
            from  `tabTemp Product Menu` 
            where 
                pos_menu='{0}' 
            order by sort_order
            """.format(parent_menu)
    data = frappe.db.sql(sql,as_dict=1)
   
    return data

@frappe.whitelist()
def get_product_variants(parent):
    data  = frappe.db.sql("select name from `tabProduct Variants` where parent='{}'".format(parent),as_dict=1)
    if data :
        return data

@frappe.whitelist()
def get_product_by_barcode(barcode):
    #check if barcode have in product
    data  = frappe.db.sql("select name from `tabProduct` where name='{}'".format(barcode),as_dict=1)
    if data:
            if data[0].name:
                p = frappe.get_doc('Product', data[0].name)
                return {
                    "menu_product_name": barcode,
                    "name": p.name,
                    "name_en": p.product_name_en,
                    "name_kh": p.product_name_kh,
                    "parent": p.product_category,
                    "price": p.price,
                    "unit": p.unit,
                    "allow_discount": p.allow_discount,
                    "allow_change_price": p.allow_change_price,
                    "allow_free": p.allow_free,
                    "is_open_product": p.is_open_product,
                    "is_inventory_product": p.is_inventory_product,
                    "prices": json.dumps(([pr.price,pr.business_branch,pr.price_rule,pr.portion] for pr in  p.product_price),default=json_handler),
                    "printers":json.dumps(([pr.printer,pr.group_item_type] for pr in p.printers),default=json_handler),
                    "modifiers": "",
                    "photo": p.photo,
                    "type": "product",
                    "append_quantity": 1,
                    "modifiers_data": json.dumps(([pr.business_branch,pr.modifier_category,pr.prefix,pr.modifier_code,pr.price] for pr in p.product_modifiers),default=json_handler),
                    "sort_order":p.sort_order
                }
            else:
                frappe.throw("Item No Name ?")
    else:
        frappe.throw("Product Not Found")