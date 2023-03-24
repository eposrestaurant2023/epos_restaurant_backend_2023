import frappe
def after_migrate():
    frappe.db.sql("delete from `tabPrint Format` where name in ('Close Sale Summary','Close Sale Summary - UI') and standard='Yes'")