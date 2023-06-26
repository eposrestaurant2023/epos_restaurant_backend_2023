import frappe
import json

def add_to_inventory_transaction(data):
 
    doc = frappe.get_doc(data)
    # frappe.throw(str(json.dumps(data)))
    doc.insert()
    

def update_product_quantity(stock_location,product_code, quantity,cost,doc):
    if doc:
        doc.quantity =(doc.quantity or 0) + (quantity or 0)
        if cost != None:
                doc.cost = ((doc.total_cost or 0) + cost*quantity) / (doc.quantity if doc.quantity>0 else 1)  
                
        doc.total_cost = (doc.cost or 0) * (doc.quantity or 0)
        doc.save()
      
    else:
        doc = frappe.get_doc({
					'doctype': 'Stock Location Product',
					'product_code': product_code,
					'stock_location':stock_location,
					'quantity':quantity or 0,
					'cost':cost or 0,
					'total_cost':(quantity or 0) * (cost or 0)
				})
        doc.insert() 
        
def get_stock_location_product(stock_location,product_code):
    data = frappe.db.sql("select name from `tabStock Location Product` where stock_location='{}' and product_code='{}'".format(stock_location, product_code), as_dict=1)
    if data:
            return frappe.get_doc("Stock Location Product", data[0].name)
    else:
        return None

      
   

def get_uom_conversion(from_uom, to_uom):
    conversion =frappe.db.get_value('Unit of Measurement Conversion', {'from_uom': from_uom,"to_uom":to_uom}, ['conversion'], cache=True)
    
    return conversion or 1

def get_product_cost(stock_location, product_code):
    cost =frappe.db.get_value('Stock Location Product', {'stock_location':stock_location,"product_code":product_code}, ['cost'], cache=True)
    if (cost or 0) == 0:
        cost = frappe.db.get_value('Product',{'product_code':product_code}, ['cost'], cache=True)
    
    return cost or 0

def check_uom_conversion(from_uom, to_uom):
    conversion =frappe.db.get_value('Unit of Measurement Conversion', {'from_uom': from_uom,"to_uom":to_uom}, ['conversion'])
    return conversion