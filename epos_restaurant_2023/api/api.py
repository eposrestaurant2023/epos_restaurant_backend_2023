import json
import time
import frappe
import base64
from py_linq import Enumerable
from frappe.utils import today, add_to_date
from frappe import _
@frappe.whitelist(allow_guest=True)
def check_username(pin_code):
    if pin_code:    
        pin_code = (str( base64.b64encode(pin_code.encode("utf-8")).decode("utf-8")))
        data = frappe.db.sql("select name,full_name,pos_user_permission from `tabUser` where pos_pin_code='{}' and allow_login_to_pos=1 limit 1".format(pin_code),as_dict=1)
        if data:
            permission= frappe.get_doc("POS User Permission",data[0]["pos_user_permission"])      
            return {"username":data[0]["name"],"full_name":data[0]["full_name"],"permission":permission} 
        
    frappe.throw(_("Invalid pin code"))
 
@frappe.whitelist(allow_guest=True)
def get_system_settings(pos_profile="", device_name=''):
    if not frappe.db.exists("POS Profile",pos_profile):
        frappe.throw("Invalid POS Profile name")
    
    profile = frappe.get_doc("POS Profile",pos_profile)
    pos_config = frappe.get_doc("POS Config",profile.pos_config)
    
    doc = frappe.get_doc('ePOS Settings')
    table_groups = []
    for g in pos_config.table_groups:
        
        table_groups.append({"key":g.table_group.lower().replace(" ","_"),"table_group":g.table_group,"background":frappe.get_value("Table Group",g.table_group,"photo"),"tables":get_tables_number(g.table_group, device_name),"search_table_keyword":""})
    pos_menus = []
    for m in profile.pos_menus:
        pos_menus.append({"pos_menu":m.pos_menu})   
        
    payment_types=[]
    for p in profile.payment_types:
        
        payment_types.append({ "payment_method":p.payment_type,"currency":p.currency,"is_single_payment_type":p.is_single_payment_type,"allow_cash_float":p.allow_cash_float, "input_amount":0,"exchange_rate":p.exchange_rate,"required_customer":p.required_customer,"is_foc":p.is_foc})
    
    #get currency
    currencies = frappe.db.sql("select name,symbol,currency_precision,symbol_on_right,pos_currency_format from `tabCurrency` where enabled=1", as_dict=1)
    
    #main currency information
    main_currency = frappe.get_doc("Currency",frappe.db.get_default("currency"))
    second_currency = frappe.get_doc("Currency",frappe.db.get_default("second_currency"))

    #get price rule
    price_rules = []
    for pr in pos_config.price_rules:
        price_rules.append({"price_rule":pr.price_rule})
 
    pos_setting={
        "business_branch":profile.business_branch,
        "business_name_en":pos_config.business_name_en,
        "business_name_kh":pos_config.business_name_kh,
        "address":pos_config.address,
        "address_en":pos_config.address_kh,
        "logo":pos_config.logo,
        "phone_number":pos_config.phone_number,
        "main_currency_name":main_currency.name,
        "main_currency_symbol":main_currency.symbol,
        "main_currency_format":main_currency.pos_currency_format,
        "second_currency_name":second_currency.name,
        "second_currency_symbol":second_currency.symbol,
        "second_currency_format":second_currency.pos_currency_format,
        "thank_you_message":pos_config.thank_you_message,
        "cancel_print_bill_required_password":pos_config.cancel_print_bill_required_password,
        "cancel_print_bill_required_note":pos_config.cancel_print_bill_required_note,
        "free_item_required_password":pos_config.free_item_required_password,
        "free_item_required_note":pos_config.free_item_required_note,
        "change_item_price_required_password":pos_config.change_item_price_required_password,
        "change_item_price_required_note":pos_config.change_item_price_required_note,
        "delete_item_required_password":pos_config.delete_item_required_password,
        "delete_item_required_note":pos_config.delete_item_required_note,
        "discount_item_required_password":pos_config.discount_item_required_password,
        "discount_item_required_note":pos_config.discount_item_required_note,
        "discount_sale_required_password":pos_config.discount_sale_required_password,
        "discount_sale_required_note":pos_config.discount_sale_required_note,
        "delete_bill_require_password":pos_config.delete_bill_required_password,
        "delete_bill_required_note":pos_config.delete_bill_required_note,
        "allow_change_quantity_after_submit":pos_config.allow_change_quantity_after_submit,
        "main_currency_predefine_payment_amount":pos_config.main_currency_predefine_payment_amount,
        "second_currency_predefine_payment_amount":pos_config.second_currency_predefine_payment_amount,
        "open_order_required_password":pos_config.open_order_required_password,
        "change_price_rule_require_password":pos_config.change_price_rule_require_password,
        "open_cashdrawer_require_password":pos_config.open_cashdrawer_require_password
        }
    
    #get default customre
    
    if not profile.default_customer:
        frappe.throw("There is no default customer for pos profie {}".format(pos_profile))

    default_customer = frappe.get_doc("Customer", profile.default_customer)
    
    #get default print format
    print_format = frappe.db.sql("select name,print_invoice_copies, print_receipt_copies,pos_invoice_file_name, pos_receipt_file_name, receipt_height, receipt_width,receipt_margin_top, receipt_margin_left,receipt_margin_right,receipt_margin_bottom  from `tabPrint Format` where doc_type='Sale' and show_in_pos=1 and disabled=0 and name='{}'".format(profile.default_pos_receipt), as_dict=True)
    default_pos_receipt=None
    if print_format:
        default_pos_receipt = print_format[0]
        
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
        "sale_status":frappe.db.sql("select name,background_color from `tabSale Status`", as_dict=1),
        "print_cashier_shift_summary_after_close_shift":doc.print_cashier_shift_summary_after_close_shift,
        "print_cashier_shift_sale_product_summary_after_close_shift":doc.print_cashier_shift_sale_product_summary_after_close_shift,
        "pos_sale_order_background_image":doc.pos_sale_order_background_image,
        "currencies":currencies,
        "default_currency":frappe.db.get_default("currency"),
        "pos_setting":pos_setting,
        "customer":default_customer.name,
        "customer_name":default_customer.customer_name_en,
        "customer_photo":default_customer.photo,
        "default_sale_type":profile.default_sale_type,
        "default_payment_type":profile.default_payment_type,
        "default_pos_receipt":default_pos_receipt,
        "second_currency_payment_type":profile.second_currency_payment_type,
        "price_rules":price_rules
        
    }
    return  data

@frappe.whitelist(allow_guest=True)
def get_tables_number(table_group,device_name):
    data = frappe.get_all("Tables Number",
                fields=["name as id","tbl_number as tbl_no","shape","sale_type","default_discount","height as h","width as w","price_rule","discount_type"],
                filters={"tbl_group":table_group}
            )
    background_color = frappe.db.get_default("default_table_number_background_color")
    for d in data:
        d.background_color=background_color,
        d.default_bg_color=background_color
        position = frappe.db.sql("select x,y,h,w from `tabePOS Table Position` where device_name='{}' and tbl_number='{}' limit 1".format(device_name,d.tbl_no ), as_dict=1)
        if position:
            for p in position:
                d.x = p.x or 0
                # if d.x < 0:
                #     d.x = 0
                    
                # if d.y<0:
                #     d.y=0
                
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
def get_current_working_day(business_branch):
   
    sql = "select name, posting_date, pos_profile, note from `tabWorking Day` where business_branch = '{}' and is_closed = 0 order by creation limit 1".format(business_branch)
    data =  frappe.db.sql(sql, as_dict=1) 
    if data:
        return data [0]
    return

@frappe.whitelist()
def get_current_cashier_shift(pos_profile):
   
    sql = "select name,working_day, posting_date, pos_profile, opened_note,business_branch,total_opening_amount from `tabCashier Shift` where pos_profile = '{}' and is_closed = 0 order by creation limit 1".format(pos_profile)
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

@frappe.whitelist()
def get_pos_print_format(doctype):
    data = frappe.db.sql("select name,print_invoice_copies, print_receipt_copies,pos_invoice_file_name, pos_receipt_file_name, receipt_height, receipt_width,receipt_margin_top, receipt_margin_left,receipt_margin_right,receipt_margin_bottom  from `tabPrint Format` where doc_type='{}' and show_in_pos=1 and disabled=0".format(doctype), as_dict=True)
    
    if data:
       return data
    else:
        return [{"name":"Standard","pos_invoice_file_name":""}]

@frappe.whitelist()
def get_pos_letter_head(doctype):
    
    data = frappe.db.sql("select name from `tabLetter Head` where   disabled=0", as_dict=True)
    
    if data:
        arr =[]
        for d in data:
            arr.append(d.name)
        return arr
     


@frappe.whitelist()
def get_close_shift_summary(cashier_shift):
    data = []
    doc = frappe.get_doc("Cashier Shift",cashier_shift)
    
     
    #get close amount by payment type
    sql = "select payment_type, currency,sum(input_amount) as input_amount, sum(payment_amount) as payment_amount from `tabSale Payment` where cashier_shift='{}' and docstatus=1 group by payment_type, currency".format(cashier_shift)
    payments = frappe.db.sql(sql, as_dict=1)
    
    
    for d in doc.cash_float:
        
        data.append({
            "name":d.name,
            "payment_method":d.payment_method,
            "exchange_rate":d.exchange_rate,
            "input_amount":d.input_amount,
            "opening_amount":d.opening_amount,
            "input_close_amount":0,
            "input_system_close_amount":d.input_amount +  Enumerable(payments).where(lambda x:x.payment_type == d.payment_method).sum(lambda x: x.input_amount or 0 ),
            "system_close_amount": d.opening_amount +  Enumerable(payments).where(lambda x:x.payment_type == d.payment_method).sum(lambda x: x.payment_amount or 0 ),
            "different_amount":0,
            "currency":d.currency
        })
    
    
    for p in payments:
        if not p.payment_type  in [d.payment_method for d in doc.cash_float]:
            data.append({
                "payment_method":p.payment_type,
                "exchange_rate":frappe.db.get_value("Payment Type", p.payment_type, "exchange_rate"),
                "input_amount":0,
                "opening_amount":0,
                "input_close_amount":0,
                "input_system_close_amount": p.input_amount,
                "system_close_amount": p.payment_amount,
                "different_amount":0,
                "currency":p.currency
            })
            
    
        
    return data
@frappe.whitelist()
def get_payment_cash(cashier_shift):
    sql = "select payment_type, currency, SUM(payment_amount) as payment_amount from `tabSale Payment` where cashier_shift='{}' AND payment_type_group = 'Cash' and docstatus=1 group by payment_type, currency".format(cashier_shift)
    data = frappe.db.sql(sql, as_dict=1)
    return data
@frappe.whitelist()
def get_cash_drawer_balance(cashier_shift):
    sql = """
    SELECT SUM(total_amount) AS total_amount_cash,SUM(total_amount_cash_out) AS total_amount_cash_out,SUM(total_amount_cash_in) AS total_amount_cash_in from(
        SELECT SUM(payment_amount) as total_amount,0 total_amount_cash_out,0 total_amount_cash_in FROM `tabSale Payment` where cashier_shift='{0}' AND payment_type_group = 'Cash' and docstatus=1
        UNION all
        SELECT total_opening_amount as total_amount,0 total_amount_cash_out,0 total_amount_cash_in FROM `tabCashier Shift` WHERE name = '{0}'
        UNION all
        SELECT 0 total_amount, SUM(amount) AS total_amount_cash_out,0 total_amount_cash_in FROM `tabCash Transaction` WHERE cashier_shift = '{0}' AND transaction_status = 'Cash Out'
        UNION all
        SELECT 0 total_amount,0 total_amount_cash_out,SUM(amount) as total_amount_cash_in FROM `tabCash Transaction` WHERE cashier_shift = '{0}' AND transaction_status = 'Cash In'
		)totals
    """.format(cashier_shift)
    frappe.msgprint(sql)
    data = frappe.db.sql(sql, as_dict=1)
    return data[0]

@frappe.whitelist()
def get_meta(doctype):
    data =  frappe.get_meta(doctype)
    return data

@frappe.whitelist()
def update_print_bill_requested(name):
    doc = frappe.get_doc("Sale",name)
    doc.sale_status = 'Bill Requested'
    doc.save()
@frappe.whitelist()
def get_working_day_list_report():
    days = int(frappe.db.get_default("number_of_day_cashier_can_view_report"))
    date = add_to_date(today(),days=days*-1)
    working_day =frappe.db.get_list('Working Day',
        filters={
            "posting_date":[">=", date]
        },
        fields=["name","posting_date","creation","modified_by","total_cashier_shift","owner"],
        order_by='posting_date desc',
        page_length=100,
        
    )
    for w in working_day:
        cashier_shift =frappe.db.get_list('Cashier Shift',
            filters={
                "working_day": w.name
            },
            fields=["name","posting_date","creation","modified_by"]
            
        )
        w.cashier_shifts = cashier_shift
    
    data = working_day
    return data