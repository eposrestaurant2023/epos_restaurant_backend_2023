import frappe

def after_install():
    # ceate table group
    if not frappe.db.exists("Table Group", "Main Group"):
        doc = frappe.get_doc(
          {
            "name": "Main Group",
            "outlet": "Main Outlet",
            "business_branch": "Main Branch",
            "table_group_name_en": "Main Group",
            "table_group_name_kh": "Main Group",
            "disabled": 0,
            "is_standard": 1,
            "doctype": "Table Group",
            "photo": "/images/defaulttable_bg.jpg"
        }
        )
        doc.insert()

    #create table 
    create_table("01")
    create_table("02")
    create_table("03")
    create_table("04")
    create_table("05")
    create_table("06")
    create_table("07")
    create_table("08")
    create_table("09")
    create_table("10")


    #create note
    create_note("Edit Closed Receipt",["Wrong Payment","Wrong Product","Guest Cancel","Cashier Error"])

def create_note(name, notes):
    if not frappe.db.exists("Category Note", note_list):
        notes = []
        for n in note_list:
            notes.append(n)
        
        doc = frappe.get_doc(
        {
            "name": name,
            "category_note_name_en": name,
            "category_note_name_kh": name,
            "disabled": 0,
            "multiple_selected": 0,
            "doctype": "Category Note",
            "notes": notes
        }
        )
        doc.insert()

#create table
def create_table(tbl_no):
    if not frappe.db.exists("Table Group", "Main Group"):
        doc = frappe.get_doc(
         {
            "tbl_number": tbl_no,
            "tbl_group": "Main Group",
            "width": 100,
            "height": 85,
            "shape": "Rectangle",
            "discount_type": "Percent",
            "default_discount": 0,
            "price_rule": "",
            "sale_type": "Dine In",
            "require_check_in": 0,
            "doctype": "Tables Number"
            }
        )
        doc.insert()


