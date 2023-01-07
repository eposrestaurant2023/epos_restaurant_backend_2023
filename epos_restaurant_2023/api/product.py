
import frappe

@frappe.whitelist()
def get_product_by_menu(root_menu):
    menus = []
    sql = """select 
                name,
                pos_menu_name_en,
                pos_menu_name_kh,
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
            """.format(root_menu)
    data = frappe.db.sql(sql,as_dict=1)
    for d in data:
        children = []
        children +=get_child_menus(d.name)
        children +=get_products(d.name)
        d["children"]=children
        menus.append(d)
    return menus

def get_child_menus(parent_menu):
    menus = []
    sql = """select 
                name,
                pos_menu_name_en as name_en,
                pos_menu_name_kh as name_kh,
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
        children = []
        children +=get_child_menus(d.name)
        children +=get_products(d.name)
        d["children"]=children
        menus.append(d)
    return menus


def get_products(parent_menu):
     
    sql = """select 
                product_name_en as name_en,
                product_name_kh as name_kh,
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
                'product' as type
            from  `tabTemp Product Menu` 
            where 
                pos_menu='{}' 
            order by  product_name_en
            """.format(parent_menu)
    data = frappe.db.sql(sql,as_dict=1)
   
    return data
