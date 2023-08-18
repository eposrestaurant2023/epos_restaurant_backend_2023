import frappe
import datetime
import json
import frappe
import os, shutil
import shlex, subprocess
from frappe.utils import cstr
import asyncio
from frappe import conf
from frappe.utils import get_url
from http.server import BaseHTTPRequestHandler, HTTPServer


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
            notes.append({"note":n})
        
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
def run_backup_command():
    site_name = cstr(frappe.local.site)
    folder = frappe.utils.get_site_path(conf.get("backup_path", "private/backups"))
    for filename in os.listdir(folder):
        file_path = os.path.join(folder, filename)
        try:
            if os.path.isfile(file_path) or os.path.islink(file_path):
                os.unlink(file_path)
            elif os.path.isdir(file_path):
                shutil.rmtree(file_path)
        except Exception as e:
            print('Failed to delete %s. Reason: %s' % (file_path, e))

    asyncio.run(run_bench_command("bench --site " + site_name + " backup --with-files"))


async def run_bench_command(command, kwargs=None):
    site = {"site": frappe.local.site}
    cmd_input = None
    if kwargs:
        cmd_input = kwargs.get("cmd_input", None)
        if cmd_input:
            if not isinstance(cmd_input, bytes):
                raise Exception(f"The input should be of type bytes, not {type(cmd_input).name}")
            del kwargs["cmd_input"]
        kwargs.update(site)
    else:
        kwargs = site
    command = " ".join(command.split()).format(**kwargs)
    command = shlex.split(command)
    subprocess.run(command, input=cmd_input, capture_output=True)

## RESET SALE TRANSACTION
@frappe.whitelist()
def reset_sale_transaction():
    # backupd db first
    run_backup_command()

    if frappe.local.request.method == "POST":
        if frappe.session.user == 'Administrator':
            frappe.db.sql("delete from `tabCash Transaction`")
            frappe.db.sql("delete from `tabSale Product Deleted`")
            frappe.db.sql("delete from `tabSale Product SPA Commission`")            
            frappe.db.sql("delete from `tabInventory Transaction`")
            frappe.db.sql("delete from `tabPOS Sale Payment`")
            frappe.db.sql("delete from `tabSale Payment`")
            frappe.db.sql("delete from `tabSale Product`")
            frappe.db.sql("delete from `tabSale`")
            frappe.db.sql("delete from `tabCashier Shift Cash Float`")
            frappe.db.sql("delete from `tabCashier Shift`")
            frappe.db.sql("delete from `tabWorking Day`")
            frappe.db.sql("delete from `tabPromotion Products`")
            frappe.db.sql("delete from `tabPromotion Customer Group`")
            frappe.db.sql("delete from `tabHappy Hours Promotion`")
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

            return {"You was reset sale transaction."}
        else:
            return {"Please contact to system's Administrator for reset sale transaction.(Permission denied)"}
    
    else:
        return {"Invalid Method."}

## END RESET SALE TRANSACTION

## RESET DATABASE Method
@frappe.whitelist()
def reset_database():
    if frappe.local.request.method == "POST":
        if frappe.session.user == 'Administrator':
            #step 1 reset data
            reset_data()
            #step 2 create predefine data
            create_predefine_data()
            return {"You was reset system to new setup."}
        else:
            return {"Please contact to system's Administrator for reset sale transaction.(Permission denied)"}
        
    else:
        return {"Invalid Method."}
## END RESET DATABASE

## RESET DATA Method
@frappe.whitelist()
def reset_data():
    if frappe.local.request.method == "POST":
        if frappe.session.user == 'Administrator':        
            #step 1 reset sale transaction
            reset_sale_transaction()

            # update 
            frappe.db.sql("update `tabSeries` set current = 0")
            frappe.db.sql("update `tabLanguage` set enabled =0 where name not in ('kh','en')")
            frappe.db.sql(" update `tabRole` set desk_access = 0 where name = 'Sales User'")
        

            # delete 
            frappe.db.sql("delete from `tabInventory Transaction`")
            frappe.db.sql("delete from `tabUnit of Measurement Conversion`")
            frappe.db.sql("delete from `tabStock Location Product`")
            frappe.db.sql("delete from `tabStock Adjustment Product`")
            frappe.db.sql("delete from `tabStock Adjustment`")
            frappe.db.sql("delete from `tabStock Transfer Products`")
            frappe.db.sql("delete from `tabStock Transfer`")
            frappe.db.sql("delete from `tabStock Take Products`")
            frappe.db.sql("delete from `tabStock Take`")
            frappe.db.sql("delete from `tabStock Location`")
            frappe.db.sql("delete from `tabModifiers`")
            frappe.db.sql("delete from `tabModifier Code`")
            frappe.db.sql("delete from `tabModifier Category`")
            frappe.db.sql("delete from `tabProduct Printer`")
            frappe.db.sql("delete from `tabProduct Menu`")
            frappe.db.sql("delete from `tabProduct Price`")
            frappe.db.sql("delete from `tabPurchase Order Payment`")
            frappe.db.sql("delete from `tabPurchase Order Products`")
            frappe.db.sql("delete from `tabPurchase Order`")
            frappe.db.sql("delete from `tabVendor`")
            frappe.db.sql("delete from `tabVendor Group`")
            frappe.db.sql("delete from `tabCustomer`")
            frappe.db.sql("delete from `tabCustomer Group`")
            frappe.db.sql("delete from `tabSale Quotation Product`")
            frappe.db.sql("delete from `tabSale Quotation`")
            frappe.db.sql("delete from `tabEmployee Emergency Contact`")
            frappe.db.sql("delete from `tabEmployee Exit Type`")
            frappe.db.sql("delete from `tabCheck In Out`")
            frappe.db.sql("delete from `tabDepartment`")
            frappe.db.sql("delete from `tabPosition`")
            frappe.db.sql("delete from `tabAttendance`")
            frappe.db.sql("delete from `tabShift Type`")
            frappe.db.sql("delete from `tabEmployee Type`")
            frappe.db.sql("delete from `tabEmployee`")
            frappe.db.sql("delete from `tabExpense Payment`")
            frappe.db.sql("delete from `tabExpense Item`")
            frappe.db.sql("delete from `tabExpense`")
            frappe.db.sql("delete from `tabExpense Category`")
            frappe.db.sql("delete from `tabPayment Type`")
            frappe.db.sql("delete from `tabCurrency Exchange`")
            frappe.db.sql("delete from `tabePOS Table Position`")
            frappe.db.sql("delete from `tabPOS Menu`")
            frappe.db.sql("delete from `tabUnit Of Measurement`")
            frappe.db.sql("delete from `tabUnit Category`")
            frappe.db.sql("delete from `tabTables Number`")
            frappe.db.sql("delete from `tabTable Group`")
            frappe.db.sql("delete from `tabOutlet`")
            frappe.db.sql("delete from `tabPrinter`")
            frappe.db.sql("delete from `tabKitchen Group`")
            frappe.db.sql("delete from `tabPrice Rule`")
            frappe.db.sql("delete from `tabRevenue Group`")
            frappe.db.sql("delete from `tabPayment Type Group`")
            # frappe.db.sql("delete from `tabPOS Profile Payment Type`")
            frappe.db.sql("delete from `tabProduct`")
            frappe.db.sql("delete from `tabProduct Category`")
            frappe.db.sql("delete from `tabPOS Price Rule`")
            frappe.db.sql("delete from `tabPOS Table Group`")
            frappe.db.sql("delete from `tabPOS Profile Root Menu`")
            frappe.db.sql("delete from `tabTax Rule`")
            frappe.db.sql("delete from `tabTemp Product Menu`")
            frappe.db.sql("delete from `tabCashier Notes`")
            frappe.db.sql("delete from `tabCategory Note`")
            frappe.db.sql("delete from `tabPOS Config Payment Type`")
            frappe.db.sql("delete from `tabPOS Profile`")
            frappe.db.sql("delete from `tabPOS Config`")
            frappe.db.sql("delete from `tabUser` where LOWER(full_name) not in ('Administrator','Guest','admin','cashier')")
            frappe.db.sql("delete from `tabRole Profile`")
            frappe.db.sql("delete from `tabModule Profile`")
            frappe.db.sql("delete from `tabNavbar Item`")
            frappe.db.sql("delete from `tabDiscount Code`")
            frappe.db.sql("delete from `tabPOS User Permission`")

            # frappe.db.sql("delete from `tabPOS Profile Table Group`")
            # frappe.db.sql("delete from `tabRestaurant Table`")
            frappe.db.sql("delete from `tabBusiness Branch`")
            frappe.db.sql("delete from `tabCurrency` where name not in('USD','KHR','RIEL')")
            frappe.db.sql("delete from `tabPrint Format`")
            frappe.db.sql("delete from `tabPOS Branding`")
            frappe.db.sql("delete from `tabPOS Station`")

            #update 
            frappe.db.sql("update `tabCurrency` set enabled = 1, name='RIEL', currency_name='RIEL' where name='KHR'")
            frappe.db.sql("update `tabCurrency` set pos_currency_format='#,###,##0. ៛', currency_precision=0 where name in ('KHR','RIEL')")
            frappe.db.sql("update `tabCurrency` set pos_currency_format = '$ #,###,##0.00',currency_precision=2  where name='USD'")

            frappe.db.commit()
            return {"You was cleared all data and configuration."}
        else:
            return {"Please contact to system's Administrator for reset sale transaction.(Permission denied)"}
        
    else:
        return {"Invalid Method."}
## END RESET DATA Method

## CREATE PREDEFINE DATA Method
@frappe.whitelist()
def create_predefine_data(): 
    if frappe.local.request.method == "POST":
        if frappe.session.user == 'Administrator':
            data = frappe.db.get_list('Predefine Data',fields=["*"], order_by='sort asc')     
            for d in data:    
                if d.is_single == 0 :   
                    if not frappe.db.exists(d.doc_type, d.doc_name): 
                        doc = frappe.get_doc(json.loads(d.data))
                        doc.insert()    
                        frappe.db.commit()
                else:
                    doc = frappe.get_doc(json.loads(d.data))  
                    doc.save()
                    frappe.db.commit()
            return {"You was created predefine data."}
        else:
            return {"Please contact to system's Administrator for reset sale transaction.(Permission denied)"}
        
    else:
        return {"Invalid Method."}
    
        
## END CREATE PREDEFINE DATA Method

@frappe.whitelist()
def get_server_name():
    server_name = socket.gethostname()
    return server_name

@frappe.whitelist()
def my_test():
    if frappe.local.request.method == "POST":
        return "Yes"
    else:
        return "No"