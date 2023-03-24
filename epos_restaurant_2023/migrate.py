import frappe
def after_migrate():
    frappe.db.sql("delete from `tabPrint Format` where name = 'Close Sale Summary' and standard='Yes'")