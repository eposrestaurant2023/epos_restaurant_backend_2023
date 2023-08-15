# Copyright (c) 2022, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe import _
from frappe.utils.nestedset import NestedSet

class POSMenu(NestedSet):
    def validate(self):
        # if self.pos_menu_name_en == "Root Menu":
        #     frappe.throw("Root Menu is not allow to change.")

        if not self.pos_menu_name_kh:
            self.pos_menu_name_kh = self.pos_menu_name_en

        self.pos_menu_path = get_menu_path(self.pos_menu_name_en)

        

        if self.is_emenu:
            if self.is_main_emenu:
                filter_emenu_business_branch = 'IS NULL'
                if self.emenu_business_branch:
                    filter_emenu_business_branch = '= ' + "'{}'".format(self.emenu_business_branch)

                main_emnu = frappe.db.sql("SELECT count(`name`) AS total FROM `tabPOS Menu` WHERE is_main_emenu = 1 and emenu_business_branch {}".format(filter_emenu_business_branch), as_dict=1)
                if main_emnu[0].total > 0:
                    frappe.throw(_("Main eMenu has already."))
                    
            if self.is_new():
                if not self.background_image:
                    placeholder = "/assets/frappe/images/emenu_placeholder.jpg"
                    self.background_image = placeholder
                    self.banner = placeholder
                else:
                    if not self.banner:
                        self.banner = self.background_image

            if not self.title_en:
                self.title_en = self.pos_menu_name_kh
                if not self.title_kh:                   
                    self.title_kh = self.title_en
              


            if self.is_main_emenu and self.position_main_menu:
                frappe.throw(_("Main eMenu cannot become sub menu of itselft."))

    def on_trash(self):
        if self.pos_menu_name_en == "Root Menu":
            frappe.throw("Root Menu is not allow to change.")

    def after_rename(self, old_name,new_name,merge):
        update_pos_menu_path(old_name)
        frappe.db.set_value("POS Menu",new_name,"pos_menu_path",get_menu_path(new_name))


def update_pos_menu_path(old_name):
    data = frappe.db.sql("select lower(name) as name from `tabPOS Menu` where pos_menu_path like '%{}%'".format(old_name.lower()))
    if data:
        for d in data:
            frappe.db.set_value("POS Menu",d[0],"pos_menu_path",get_menu_path(d[0]))
        
        #update to tbl_product_menu
        frappe.db.sql("update `tabProduct Menu` a set menu_path = (select pos_menu_path from `tabPOS Menu` b where b.name= a.pos_menu)")    
            

def  get_menu_path(pos_menu_name):
    
    pos_menu = pos_menu_name
    pos_menu_paths = []
    while pos_menu:
        pos_menu_paths.insert(0,pos_menu)
        pos_menu =   frappe.db.get_value('POS Menu', pos_menu, 'parent_pos_menu')
    
    path = ' > '.join(pos_menu_paths)
   
        
       
        
    return path