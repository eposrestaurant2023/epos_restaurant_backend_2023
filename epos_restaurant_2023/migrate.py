import frappe
def after_migrate():
    frappe.db.sql("delete from `tabPrint Format` where name in ('Close Sale Summary','Close Sale Summary - UI') and standard='Yes'")
    frappe.db.sql("delete from `tabUnit of Measurement Conversion` where name in ('e7c28b72f2')")
    