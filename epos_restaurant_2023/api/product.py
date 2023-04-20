
import frappe

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
                    'menu' as type
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
                'menu' as type
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
                append_quantity
            from  `tabTemp Product Menu` 
            where 
                pos_menu='{0}' 
            order by  product_name_en
            """.format(parent_menu)
    data = frappe.db.sql(sql,as_dict=1)
   
    return data


@frappe.whitelist()
def get_product_by_barcode(barcode):
    #check if barcode have in product
    sql = "select * from `tabProduct` where name='{}'".format(barcode)
    data  = frappe.db.sql(sql,as_dict=1)
    if data:
            p = frappe.get_doc('Product', data[0].name)
            frappe.msgprint(p.product_name)
            return {
                "menu_product_name": barcode,
                "name": p.name,
                "name_en": p.product_name_en,
                "name_kh": p.product_name,
                "parent": "",
                "price": 0,
                "unit": "Unit",
                "allow_discount": 0,
                "allow_change_price": 1,
                "allow_free": 1,
                "is_open_product": 0,
                "is_inventory_product": 1,
                "prices": "[{\"price\": 1.84, \"branch\": \"\", \"price_rule\": \"Normal Rate\", \"portion\": \"Normal\"}, {\"price\": 2, \"branch\": \"\", \"price_rule\": \"Khmer New Year Rate\", \"portion\": \"Normal\"}]",
                "printers": "[]",
                "modifiers": "",
                "photo": "",
                "type": "product",
                "append_quantity": 1,
                "modifiers_data": "[]"
            }
    
    frappe.throw("Not Found")
    return {
        "menu_product_name": "f345104b55",
        "name": "9696",
        "name_en": "Coffee Frappe",
        "name_kh": "កាហ្វេក្រឡុក",
        "parent": "កាហ្វេត្រជាក់-Ice Coffee",
        "price": 1.75,
        "unit": "Unit",
        "allow_discount": 0,
        "allow_change_price": 1,
        "allow_free": 1,
        "is_open_product": 0,
        "is_inventory_product": 1,
        "prices": "[{\"price\": 1.84, \"branch\": \"\", \"price_rule\": \"Normal Rate\", \"portion\": \"Normal\"}, {\"price\": 2, \"branch\": \"\", \"price_rule\": \"Khmer New Year Rate\", \"portion\": \"Normal\"}]",
        "printers": "[]",
        "modifiers": "",
        "photo": "",
        "type": "product",
        "append_quantity": 1,
        "modifiers_data": "[]"
    }