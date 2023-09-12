import json
import frappe
import base64
from py_linq import Enumerable
from frappe.utils import today, add_to_date
from datetime import datetime, timedelta
from frappe import _
@frappe.whitelist(allow_guest=True)
def check_username(pin_code):
    if pin_code:    
        pin_code = (str( base64.b64encode(pin_code.encode("utf-8")).decode("utf-8")))
        data = frappe.db.sql("select name,full_name,pos_user_permission from `tabUser` where pos_pin_code='{}' and allow_login_to_pos=1 limit 1".format(pin_code),as_dict=1)
        if data:
            permission= frappe.get_doc("POS User Permission",data[0]["pos_user_permission"])      
            return {"username":data[0]["name"],"full_name":data[0]["full_name"],"permission":permission} 
        
    frappe.throw(_("Invalid PIN Code"))

@frappe.whitelist(allow_guest=True)
def get_user_info(name=""):
   
    if  name=="":
        name = frappe.session.user
    if name == "Guest":
        frappe.throw("Please login to start using epos system")
    data = frappe.db.sql("select name,full_name,user_image,role_profile_name,pos_user_permission from `tabUser` where name='{}'".format(name),as_dict=1)
    if data:
        permission= frappe.get_doc("POS User Permission",data[0]["pos_user_permission"])      
        return {"username":data[0]["name"],"full_name":data[0]["full_name"],"photo":data[0]["user_image"],"role":data[0]["pos_user_permission"],"permission":permission} 



@frappe.whitelist(allow_guest=True)
def get_system_settings(pos_profile="", device_name=''):
    if not frappe.db.exists("POS Profile",pos_profile):
        frappe.throw("Invalid POS Profile name")
    
    profile = frappe.get_doc("POS Profile",pos_profile)
    pos_config = frappe.get_doc("POS Config",profile.pos_config)
    pos_branding = frappe.get_doc("POS Branding", profile.pos_branding)

    sale_types = frappe.get_list("Sale Type",fields=['name', 'sale_type_name','color','is_order_use_table','sort_order'],order_by="sort_order")
    
    doc = frappe.get_doc('ePOS Settings')
    table_groups = []
    for g in profile.table_groups:
        _group = frappe.get_doc("Table Group",g.table_group,fields=["photo","table_group_name_kh"])        
        table_groups.append({
            "key":g.table_group.lower().replace(" ","_"),
            "table_group":g.table_group,
            "table_group_kh":_group.table_group_name_kh,
            "background":_group.photo,
            "tables":get_tables_number(g.table_group, device_name),
            "search_table_keyword":""
            })
    pos_menus = []
    for m in profile.pos_menus:
        pos_menus.append({"pos_menu":m.pos_menu})   
    
    #main currency information
    main_currency = frappe.get_doc("Currency",frappe.db.get_default("currency"))
    second_currency = frappe.get_doc("Currency",frappe.db.get_default("second_currency"))

    payment_types=[]
    for p in pos_config.payment_type:
        payment_types.append({ 
            "account_code":p.account_code,
            "payment_method":p.payment_type,
            "payment_type_group":p.payment_type_group,
            "currency":p.currency,
            "currency_symbol":p.currency_symbol,
            "currency_precision":p.currency_precision,
            "allow_change":p.allow_change,
            "is_single_payment_type":p.is_single_payment_type,
            "allow_cash_float":p.allow_cash_float, 
            "input_amount":0,
            "exchange_rate":p.exchange_rate if p.currency != main_currency.name else 1,
            "required_customer":p.required_customer,
            "is_foc":p.is_foc,
            "use_room_offline":p.use_room_offline,
            "rooms":p.rooms,
            "is_manual_fee":p.is_manual_fee,
            "fee_percentage":p.fee_percentage
            })
    
    #get currency
    currencies = frappe.db.sql("select name,symbol,currency_precision ,symbol_on_right,pos_currency_format from `tabCurrency` where enabled=1", as_dict=1)
    

    #get price rule
    price_rules = []
    for pr in pos_config.price_rules:
        price_rules.append({"price_rule":pr.price_rule})
    # get lang
    lang = frappe.get_list('Language',fields=['language_code', 'language_name'],filters={
        'enabled': 1
    })
    pos_setting={
        "business_branch":profile.business_branch,
        "business_name_en":pos_config.business_name_en,
        "business_name_kh":pos_config.business_name_kh,
        "address":pos_config.address,
        "address_en":pos_config.address_kh,
        "logo":pos_branding.logo,
        "phone_number":pos_config.phone_number,
        "vattin_number":pos_config.vattin_number,
        "email":pos_config.email,
        "website":pos_config.website,
        "sale_types":sale_types,
        "main_currency_name":main_currency.name,
        "exchange_rate_main_currency":frappe.db.get_default("exchange_rate_main_currency"),
        "main_currency_symbol":main_currency.symbol,
        "main_currency_format":main_currency.custom_pos_currency_format,
        "main_currency_precision":frappe.db.get_default("currency_precision"),
        "second_currency_name":second_currency.name,
        "second_currency_symbol":second_currency.symbol,
        "second_currency_format":second_currency.custom_pos_currency_format,
        "tax_1_name":doc.tax_1_name,
        "tax_2_name":doc.tax_2_name,
        "tax_3_name":doc.tax_3_name,
        "specific_business_branch":doc.specific_business_branch,
        "specific_pos_profile":doc.specific_pos_profile,
        "backend_port":doc.backend_port,
        "customer_display_slideshow": pos_branding.customer_display_slideshow,
        "thank_you_message":pos_branding.thank_you_message,
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
        "delete_bill_required_password":pos_config.delete_bill_required_password,
        "delete_bill_required_note":pos_config.delete_bill_required_note,
        "change_tax_setting_required_password":pos_config.change_tax_setting_required_password,
        "change_tax_setting_required_note":pos_config.change_tax_setting_required_note,
        "allow_change_quantity_after_submit":pos_config.allow_change_quantity_after_submit,
        "main_currency_predefine_payment_amount":pos_config.main_currency_predefine_payment_amount,
        "second_currency_predefine_payment_amount":pos_config.second_currency_predefine_payment_amount,
        "open_order_required_password":pos_config.open_order_required_password,
        "change_price_rule_require_password":pos_config.change_price_rule_require_password,
        "open_cashdrawer_require_password":pos_config.open_cashdrawer_require_password,
        "edit_closed_receipt_required_password":pos_config.edit_closed_receipt_required_password,
        "edit_closed_receipt_required_note":pos_config.edit_closed_receipt_required_note,
        "start_working_day_required_password":pos_config.start_working_day_required_password,
        "close_working_day_required_password":pos_config.close_working_day_required_password,
        "start_cashier_shift_required_password":pos_config.start_cashier_shift_required_password,
        "close_cashier_shift_required_password":pos_config.close_cashier_shift_required_password,
        "cash_in_check_out_required_password":pos_config.cash_in_check_out_required_password,
        "print_waiting_order_after_submit_order":pos_config.print_waiting_order_after_submit_order,
        "print_new_deleted_sale_product":pos_config.print_new_deleted_sale_product,
        "check_delete_item_require_passord_from_product":pos_config.check_delete_item_require_passord_from_product,
        "show_button_tip":pos_config.show_button_tip,
        
        }
    
    #get default customre
    
    if not profile.default_customer:
        frappe.throw("There is no default customer for pos profie {}".format(pos_profile))

    default_customer = frappe.get_doc("Customer", profile.default_customer)
    
    #get default print format
    print_format_query = """select 
        name,
        print_invoice_copies, 
        print_receipt_copies,
        pos_invoice_file_name, 
        pos_receipt_file_name, 
        receipt_height, 
        receipt_width,
        receipt_margin_top, 
        receipt_margin_left,
        receipt_margin_right,
        receipt_margin_bottom  
    from `tabPrint Format` 
    where doc_type='Sale' 
    and show_in_pos=1 
    and disabled=0 and name='{}'""".format(profile.default_pos_receipt)
    print_format = frappe.db.sql(print_format_query, as_dict=1)
  

    default_pos_receipt=None
    if print_format:
        default_pos_receipt = print_format[0]

    #get report list
    report_format_fields = ["name","title","doc_type","print_report_name","default_print_language","show_in_pos_report","show_in_pos","print_invoice_copies", "print_receipt_copies","pos_invoice_file_name","pos_receipt_file_name", "receipt_height", "receipt_width","receipt_margin_top", "receipt_margin_left","receipt_margin_right","receipt_margin_bottom","show_in_pos_closed_sale","report_options" ]
    reports = frappe.get_list("Print Format",fields=report_format_fields , filters={"doc_type":["in",["Cashier Shift","Working Day","Sale","POS Profile"]]},order_by="sort_order")
    letter_heads = frappe.get_list("Letter Head",fields=["name","is_default"],filters={"disabled":0})
    letter_heads.append({"name":"No Letterhead","is_default":0})

    #get tax rules 
    tax_rules = []
    for d in profile.pos_profile_tax_rule: 
        tax_rules.append({"tax_rule":d.tax_rule,"tax_rule_data":d.tax_rule_data})

    #get default tax rule
    tax_rule ={}
    if profile.tax_rule:      
        tax_rule = frappe.get_doc("Tax Rule", profile.tax_rule)

    #get shortcut key
    shortcut_keys = frappe.db.get_list('Shortcut Key',fields=['name','key','description'])

    #get shift type
    shift_types = frappe.db.sql("select name, sort from `tabShift Type`",as_dict=1)    
    

    data={
        "app_name":doc.epos_app_name,
        "specific_business_branch":doc.specific_business_branch,
        "specific_pos_profile":doc.specific_pos_profile,
        "business_branch":profile.business_branch,
        "address":pos_config.address,
        "logo":pos_branding.logo,
        "phone_number":pos_config.phone_number,
        "pos_profile":pos_profile,
        "outlet":profile.outlet,
        "close_business_day_on":pos_config.close_business_day_on,
        "alert_close_working_day_after":pos_config.alert_close_working_day_after,
        "price_rule":profile.price_rule,
        "stock_location":profile.stock_location,
        "tax_rules":tax_rules,
        "tax_rule":tax_rule,
        "login_background":pos_branding.login_background,
        "home_background":pos_branding.home_background,
        "thank_you_background":pos_branding.thank_you_background,
        "table_groups":table_groups,
        "pos_menus":pos_menus,
        "default_pos_menu":profile.default_pos_menu,
        "payment_types":payment_types,
        "tax_1_name":doc.tax_1_name,
        "tax_2_name":doc.tax_2_name,
        "tax_3_name":doc.tax_3_name,
        "use_guest_cover":pos_config.use_guest_cover,
        "sale_status":frappe.db.sql("select name,background_color from `tabSale Status`", as_dict=1),
        "print_cashier_shift_summary_after_close_shift":pos_config.print_cashier_shift_summary_after_close_shift,
        "print_cashier_shift_sale_product_summary_after_close_shift":pos_config.print_cashier_shift_sale_product_summary_after_close_shift,
        "print_working_day_summary_after_close_working_day":pos_config.print_working_day_summary_after_close_working_day,
        "print_working_day_sale_product_summary_after_close_working_day":pos_config.print_working_day_sale_product_summary_after_close_working_day,
        "print_new_deleted_sale_product":pos_config.print_new_deleted_sale_product,
        "pos_sale_order_background_image":pos_branding.pos_sale_order_background_image,
        "shift_types":shift_types,
        "show_button_tip":pos_config.show_button_tip,
        "currencies":currencies,
        "default_currency":frappe.db.get_default("currency"),
        "pos_setting":pos_setting,
        "customer":default_customer.name,
        "customer_name":default_customer.customer_name_en,
        "customer_photo":default_customer.photo,
        "customer_group":default_customer.customer_group,
        "default_sale_type":profile.default_sale_type,
        "default_payment_type":profile.default_payment_type,
        "default_pos_receipt":default_pos_receipt,
        "second_currency_payment_type":profile.second_currency_payment_type,
        "price_rules":price_rules,
        "lang": lang,
        "reports":reports,
        "letter_heads":letter_heads,
        "device_setting":frappe.get_doc("POS Station",device_name),
        "shortcut_key":shortcut_keys
    }

    return  data

@frappe.whitelist(allow_guest=True)
def get_tables_number(table_group,device_name):
    # data =  frappe.get_all("Tables Number",
    #             fields=["name as id","tbl_number as tbl_no","shape","sale_type","default_discount","height as h","width as w","price_rule","discount_type"],
    #             filters={"tbl_group":table_group}
    #         )
    data = frappe.db.sql("select name as id, shape, tbl_number as tbl_no,sale_type, default_discount,height as h, width as w, price_rule,discount_type from `tabTables Number` where tbl_group='{}' order by sort_order, tbl_number".format(table_group), as_dict=1)

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
def check_pos_profile(pos_profile_name, device_name):

    if not frappe.db.exists("POS Profile", pos_profile_name):
        frappe.throw("Invalid POS Profile")

    if not frappe.db.exists("POS Station", device_name):
        frappe.throw("Invalid POS Station")    
    station =  frappe.get_doc("POS Station",device_name)
    if station.is_used and not device_name=="Demo":
        frappe.throw("This station is already used")
    frappe.db.sql("update `tabPOS Station` set is_used = 1 where name = '{}'".format(device_name))
    
    frappe.db.commit()

    return station


@frappe.whitelist()
def get_current_working_day(business_branch):
   
    sql = "select name, posting_date, pos_profile, note from `tabWorking Day` where business_branch = '{}' and is_closed = 0 order by creation limit 1".format(business_branch)
    data =  frappe.db.sql(sql, as_dict=1) 
    if data:
        return data [0]
    return

@frappe.whitelist()
def get_current_shift_information(business_branch, pos_profile):
    return {
        "working_day":get_current_working_day(business_branch),
        "cashier_shift":get_current_cashier_shift(pos_profile)
    }

@frappe.whitelist()
def get_current_cashier_shift(pos_profile):
   
    sql = "select name,working_day, posting_date,shift_name, pos_profile, opened_note,business_branch,total_opening_amount from `tabCashier Shift` where pos_profile = '{}' and is_closed = 0 order by creation desc limit 1".format(pos_profile)
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
    sql = "select payment_type, currency,sum(input_amount + (fee_amount * exchange_rate)) as input_amount, sum(payment_amount + fee_amount) as payment_amount from `tabSale Payment` where cashier_shift='{}' and docstatus=1 group by payment_type, currency".format(cashier_shift)
    
    payments = frappe.db.sql(sql, as_dict=1)

    
    #get cash in out 
    sql = """select  
                payment_type,
                exchange_currency,
                currency,
                sum(if(transaction_status='Cash Out',input_amount*-1,input_amount)) as total_input_amount, 
                sum(if(transaction_status='Cash Out',amount*-1,amount)) as total_amount 
            from `tabCash Transaction`   
            where cashier_shift='{}'    
            group by  
                payment_type,
                currency,
                exchange_currency""".format(cashier_shift)
 
    cash_transactions = frappe.db.sql(sql, as_dict=1)
    

    #get cash float
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
    
    #get cash transaction
    for c in cash_transactions:        
        data.append({
            "payment_method":c.payment_type,
            "exchange_rate":c.exchange_currency,
            "input_amount":0,
            "opening_amount":0,
            "input_close_amount":0,
            "input_system_close_amount":c.total_input_amount,
            "system_close_amount": c.total_amount,
            "different_amount":0,
            "currency":c.currency
        })
    
 
    for p in payments:
        if not p.payment_type  in [d.payment_method for d in doc.cash_float]:
            exchange_rate =  frappe.db.get_value("Payment Type", p.payment_type, "exchange_rate")           
            data.append({
                "payment_method":p.payment_type,
                "exchange_rate":exchange_rate,
                "input_amount":0,
                "opening_amount":0,
                "input_close_amount":0,
                "input_system_close_amount": p.input_amount,
                "system_close_amount": p.payment_amount,
                "different_amount":0,
                "currency":p.currency
            })
            
    
        
    return get_cash_float(data)

#get cash float sum group by
def get_cash_float(data):
	result = []
	groups = {}
	for row in data:
		group = {
            "payment_method": row["payment_method"], 
            "exchange_rate":row["exchange_rate"],
            "currency":row["currency"]
            }
		
		input_amount = row['input_amount']
		opening_amount = row['opening_amount']
		input_close_amount = row['input_close_amount']
		input_system_close_amount = row['input_system_close_amount']
		system_close_amount = row['system_close_amount']
		different_amount = row['different_amount']
		g = json.dumps(group)	  
		if g not in groups:
			groups[g] = {
                'input_amount': [],
                'opening_amount':[],
                'input_close_amount':[],
                'input_system_close_amount':[],
                'system_close_amount':[],
                'different_amount':[],
                } 

		groups[g]['input_amount'].append(input_amount)
		groups[g]['opening_amount'].append(opening_amount)
		groups[g]['input_close_amount'].append(input_close_amount)
		groups[g]['input_system_close_amount'].append(input_system_close_amount)
		groups[g]['system_close_amount'].append(system_close_amount)
		groups[g]['different_amount'].append(different_amount)


	for group, total in groups.items():	 
		total_input_amount = sum(total['input_amount'])
		total_opening_amount = sum(total['opening_amount'])
		total_input_close_amount = sum(total['input_close_amount'])
		total_input_system_close_amount = sum(total['input_system_close_amount'])
		total_system_close_amount = sum(total['system_close_amount'])
		total_different_amount = sum(total['different_amount'])
		
		g = json.loads(group)	
		
		_result = {}
		_result.update({
                "payment_method":g['payment_method'],
                "exchange_rate":g['exchange_rate'],
                "currency":g['currency'],
                "input_amount":total_input_amount or 0,
                "opening_amount":total_opening_amount or 0,
                "input_close_amount": total_input_close_amount or 0,
                "input_system_close_amount": total_input_system_close_amount or 0,
                "system_close_amount": total_system_close_amount or 0,
                "different_amount": total_different_amount or 0
            })	
            
		result.append(_result)	
	
	return result


@frappe.whitelist()
def get_payment_cash(cashier_shift):
    sql = "select payment_type, currency, SUM(payment_amount) as payment_amount from `tabSale Payment` where cashier_shift='{}' AND payment_type_group = 'Cash' and docstatus=1 group by payment_type, currency".format(cashier_shift)
    data = frappe.db.sql(sql, as_dict=1)
    return data
@frappe.whitelist()
def get_cash_drawer_balance(cashier_shift):
    sql_system_amount = "SELECT COALESCE( SUM(payment_amount),0) AS total_amount_cash FROM `tabSale Payment` where cashier_shift='{}' AND payment_type_group = 'Cash' and docstatus=1".format(cashier_shift)
    sql_opening_amount = "SELECT total_opening_amount FROM `tabCashier Shift` WHERE name = '{}'".format(cashier_shift)
    sql_cash_out = "SELECT COALESCE( SUM(amount), 0) AS total_amount_cash_out FROM `tabCash Transaction` WHERE cashier_shift = '{}' AND transaction_status = 'Cash Out'".format(cashier_shift)
    sql_cash_in = "SELECT COALESCE( SUM(amount), 0) AS total_amount_cash_in FROM `tabCash Transaction` WHERE cashier_shift = '{}' AND transaction_status = 'Cash In'".format(cashier_shift)
    
    data_system_amount = frappe.db.sql(sql_system_amount, as_dict=1)
    total_amount_cash = data_system_amount[0].total_amount_cash
    data_opening_amount = frappe.db.sql(sql_opening_amount, as_dict=1)
    total_opening_amount = data_opening_amount[0].total_opening_amount
    data_cash_in = frappe.db.sql(sql_cash_in, as_dict=1)
    total_amount_cash_in = data_cash_in[0].total_amount_cash_in
    data_cash_out = frappe.db.sql(sql_cash_out, as_dict=1)
    total_amount_cash_out = data_cash_out[0].total_amount_cash_out
    data = {
        "total_amount_cash": total_amount_cash,
        "total_opening_amount": total_opening_amount,
        "total_amount_cash_in": total_amount_cash_in,
        "total_amount_cash_out": total_amount_cash_out,
        "total_balance": total_amount_cash - total_amount_cash_out + total_amount_cash_in + total_opening_amount
    }
    return data

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
def get_working_day_list_report(business_branch = '', pos_profile = ''): 
    days = int(frappe.db.get_default("number_of_day_cashier_can_view_report")) 
    
    date = datetime.today()
    new_date = date + timedelta(days=days*-1)    
    filters = {}
    if business_branch and not pos_profile:
        filters.update({"business_branch":["=", business_branch]})

    elif pos_profile:        
        filters.update({"pos_profile":["=", pos_profile] }) 
    
    wd = frappe.get_last_doc('Working Day',filters=filters)  
    filters.update({
        "posting_date":[">=", new_date],
        "posting_date":["<=", wd.posting_date]
    })    
        
    working_day =frappe.db.get_list('Working Day',
        filters = filters,
        fields=["name","posting_date","creation","modified_by","owner","is_closed","closed_date"],
        order_by='posting_date desc',
        page_length=100,        
    )

    for w in working_day:
        cashier_shift =frappe.db.get_list('Cashier Shift',
            filters={
                "working_day": w.name
            },
            fields=["name","pos_profile","posting_date","creation","modified_by","is_closed"]            
        )
        w.cashier_shifts = cashier_shift
    
    data = working_day
    return data


@frappe.whitelist()
def edit_sale_order(name,auth):
    #check if sale already have payment then cancel sale payment first
    payments = frappe.get_list("Sale Payment",fields=["name"], filters={"sale":name,"docstatus":1})
    for p in payments:
        sale_payment = frappe.get_doc("Sale Payment", p.name)
        sale_payment.cancel()
        sale_payment.delete()
    
    #then start to cancel sale
    sale_doc = frappe.get_doc("Sale",name)
    sale_doc.payment=[]
    sale_doc.cancel()


    #add comment to this doc to track who request to edit this sale order 
    #get user and note from pos confirm edit dialog
    

    #change status from 2 to 0 (Cancel to Draft) to allow pos can modified this doc
    sale_status_doc = frappe.get_doc("Sale Status","Submitted")
    frappe.db.sql("update `tabSale` set docstatus = 0, sale_status='Submitted', sale_status_color='{1}', sale_status_priority={2} where name='{0}'".format(name,sale_status_doc.background_color,sale_status_doc.priority))
    frappe.db.sql("update `tabSale Product` set docstatus = 0 where parent='{}'".format(name))
    #add comment
       
    doc = frappe.get_doc({
        'doctype': 'Comment',
        'subject': 'Delete sale order',
        "comment_type":"Comment",
        "reference_doctype":"Sale",
        "reference_name":sale_doc.name,
        "comment_by":auth["username"],
        "content":"User {0} edit sale order. Reason: {1}".format(auth['full_name'], auth["note"])
    })
    doc.insert()

@frappe.whitelist()
def delete_sale(name,auth):
    sale_doc = frappe.get_doc("Sale",name)
    #validate cashier shift
    cashier_shift_doc = frappe.get_doc("Cashier Shift", sale_doc.cashier_shift)
    if cashier_shift_doc.is_closed==1:
        frappe.throw(_("Cashier shift is already closed."))


    #check if sale already have payment then cancel sale payment first
    payments = frappe.get_list("Sale Payment",fields=["name"], filters={"sale":name,"docstatus":1})
    for p in payments:
        sale_payment = frappe.get_doc("Sale Payment", p.name)
        sale_payment.cancel()
        sale_payment.delete()
    
    #then start to cancel sale
    sale_doc = frappe.get_doc("Sale",name)
    sale_doc.payment=[]
    if sale_doc.docstatus ==1:
        sale_doc.cancel()
    else:        
        frappe.db.sql("update `tabSale` set docstatus = 2  where name='{0}'".format(name))
        frappe.db.sql("update `tabSale Product` set docstatus = 2 where parent='{}'".format(name))
    
    #update sale product spa deleted
    query = "update `tabSale Product SPA Commission` set is_deleted = 1  where sale = '{}'".format(name)
    frappe.db.sql(query)


    #add to comment
    doc = frappe.get_doc({
        'doctype': 'Comment',
        'subject': 'Delete sale order',
        "comment_type":"Comment",
        "reference_doctype":"Sale",
        "reference_name":sale_doc.name,
        "comment_by":auth["username"],
        "content":"User {0} delete sale order. Reason: {1}".format(auth['full_name'], auth["note"])
    })
    doc.insert()
    
@frappe.whitelist()
def get_filter_for_close_sale_list(business_branch,pos_profile):
    working_day = get_current_working_day(business_branch)
    cashier_shifts =  [{"name":'', "title":"All Cashier Shift"}]
    cashier_shifts = cashier_shifts + ( frappe.db.sql("select name, name as title from `tabCashier Shift` where working_day = '{}' order by name".format(working_day.name),as_dict=1))
    sale_types = [{"title":'All Sale Type',"name":""}]
    sale_types +=  frappe.db.sql("select name, name as title, color,is_order_use_table from `tabSale Type` order by sort_order",as_dict=1)
    outlets = [{"title":'All Outlet',"name":""}]
    outlets += frappe.db.sql("select name, name as title from `tabOutlet` where business_branch = '{}' order by name".format(business_branch),as_dict=1)
    table_groups =[{"title":'All Table Group',"name":""}]
    table_groups += frappe.db.sql("select name, name as title from `tabTable Group` where business_branch = '{}' order by name".format(business_branch),as_dict=1)

    return {
    "working_day":working_day,
    "cashier_shift":get_current_cashier_shift(pos_profile),
    "cashier_shifts":cashier_shifts,
    "sale_types":sale_types,
    "outlets":outlets,
    "table_groups":table_groups

    }




# get reservation folio
@frappe.whitelist()
def get_customer_on_membership_scan(card):
    membership = frappe.get_all('Customer Card',
								filters=[
                                    ['card_code','=',card]
                                ],
								fields=['parent','card_name','card_code','discount_type','discount','expiry'],
								limit=1
							 )
    if membership:
        ms = membership[0]
        customer = frappe.get_doc("Customer",ms["parent"]) 
        if customer:
            customer.card = customer.card
            return customer
    return {"Invalid Card"}


# get reservation folio
@frappe.whitelist()
def get_reservation_folio(property):
    room_types = frappe.db.get_list("Room Type",
                             filters=[["property",'=',property]],
                             limit=100,
                             fields=['name', 'room_type','sort_order'],
                            )


    folio = frappe.db.get_list("Reservation Folio",
                             filters=[['status','=', 'Open'], 
                                      ['reservation_status','=','In-house'] , 
                                      ["property",'=',property]],
                             limit=500,
                             fields=[
                                 'name', 
                                 'room_types',
                                 'rooms',
                                 'reservation',
                                 'guest_name',
                                 'phone_number'
                                 ],
                            )

    data = {
        "folio_data":folio,
        "room_types":room_types
        }

    return data

@frappe.whitelist()
def get_current_customer_bill_counter(pos_profile):
    pos_config = frappe.db.get_value("POS Profile",pos_profile,"pos_config" )
    prefix =  frappe.db.get_value("POS Config",pos_config,"pos_bill_number_prefix" )
    prefix = prefix.replace(".","").replace("#","")
    data = frappe.db.sql("select * from `tabSeries` where name='{}'".format(prefix),as_dict=1)
    if data:
        return data[0]["current"]
    return 0

@frappe.whitelist(methods="POST")
def update_customer_bill_counter(pos_profile, counter):
    user= (frappe.session.data.user)
    pos_user_permission = frappe.db.get_value("User", user, "pos_user_permission")
    if pos_user_permission:
        has_permission = frappe.db.get_value("POS User Permission",pos_user_permission,"reset_custom_bill_number_counter" )
        if has_permission ==0:
            frappe.throw("You don't permission to reset counter")

    pos_config = frappe.db.get_value("POS Profile",pos_profile,"pos_config" )
    prefix =  frappe.db.get_value("POS Config",pos_config,"pos_bill_number_prefix" )
    prefix = prefix.replace(".","").replace("#","")
    frappe.db.sql("update  `tabSeries` set current={} where name='{}'".format(counter, prefix))
    frappe.db.commit()

   



