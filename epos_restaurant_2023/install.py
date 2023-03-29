import frappe
import datetime
import wmi
import socket

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

def create_note(name, note_list):
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

def replace_format(string,year):
     
    year_short = str(datetime.datetime.now().year)[-2:]
    month = str(datetime.datetime.now().month).zfill(2)
    digit = str(1).zfill(4)
    return string.replace('.', '').replace('YYYY', year).replace('yyyy', year).replace('YY', year_short).replace('yy', year_short).replace('MM', month).replace('#', '')


@frappe.whitelist()
def reset_sale_transaction():
   


    if frappe.session.user == 'Administrator':
        frappe.db.sql("delete from `tabCash Transaction`")
        frappe.db.sql("delete from `tabPOS Sale Payment`")
        frappe.db.sql("delete from `tabSale Payment`")
        frappe.db.sql("delete from `tabSale Product`")
        frappe.db.sql("delete from `tabSale`")
        frappe.db.sql("delete from `tabCashier Shift Cash Float`")
        frappe.db.sql("delete from `tabCashier Shift`")
        frappe.db.sql("delete from `tabWorking Day`")
        frappe.db.sql("delete from `tabComment` where reference_doctype in ('Sale','POS Sale Payment','Sale Payment','Sale Product','Cashier Shift Cash Float','Cashier Shift','Working Day')")

        

        #reset sale transaction 
        doctypes = ["Sale","Sale Payment","Cashier Shift","Working Day","Cash Transaction"]
        for d in doctypes:
            if frappe.get_meta("Sale").get_field("naming_series"):
                formats =  frappe.get_meta(d).get_field("naming_series").options
                if formats:
                    for f in formats.split("\n"):
                        for n in range(2022, 2030):
                            format_text = replace_format(f,str(n))
                            
                            frappe.db.sql("update `tabSeries` set current=  0 where name='{}'".format(format_text) )
        

        frappe.db.commit()

        return "Done"
    else:
        return "Please login as administrator"


@frappe.whitelist()
def get_unique_identification_code(server_name):
    username = "server"
    password = "eSAdmin@2023"
    connection = wmi.connect_server(server=server_name, namespace=r"root\\virtualization", user=username, password=password)
    wmi_server_connection = wmi.WMI(wmi=connection)
    system = wmi_server_connection.Msvm_ComputerSystem()[0]
    unique_identification_code = system.ElementName
    return unique_identification_code


@frappe.whitelist()
def get_server_name():
    server_name = socket.gethostname()
    return server_name