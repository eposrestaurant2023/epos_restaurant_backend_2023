import frappe
def before_migrate ():
    frappe.db.sql("delete from `tabPrint Format` where name in ('Sale','Sale Receipt Test','Close Sale Summary','Close Sale Summary - UI') and standard='Yes'")


    frappe.db.sql("delete from `tabUnit of Measurement Conversion` where name in ('e7c28b72f2')")
    frappe.db.sql("delete from `tabPrinter` where printer_name in ('Kitchen Printer','Cashier Printer', 'Bar Printer')")
    
def after_migrate():
    frappe.db.sql("delete from `tabPrint Format` where name in ('Close Sale Summary','Close Sale Summary - UI') and standard='Yes'")
    frappe.db.sql("delete from `tabUnit of Measurement Conversion` where name in ('e7c28b72f2')")
    frappe.db.sql("delete from `tabPrinter` where printer_name in ('Kitchen Printer','Cashier Printer', 'Bar Printer')")
    