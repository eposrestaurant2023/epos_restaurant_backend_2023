# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import datetime
import frappe
from frappe.model.document import Document

class CheckInOut(Document):
	 pass


@frappe.whitelist()
def check_in_out(
	employee_field_value,
	timestamp,
	device_id=None,
	log_type=None,
	skip_auto_attendance=0,
	employee_fieldname="attendance_device_id",
):

 
	doc = frappe.new_doc("Check In Out")
	doc.employee = "Hello"
	doc.employee_id= employee_field_value,
	doc.log_time = timestamp
	doc.device_id = device_id
	doc.log_type = log_type
	doc.insert()
	add_to_attendance(employee_field_value,log_type,timestamp)
	return doc

def add_to_attendance(employee_id,log_type,timestamp):
	employee= frappe.get_doc("Employee",employee_id)
		
	if employee:
		data = frappe.db.exists("Attendance",{"employee": employee.name})
		time_in = None
		if log_type in [0,4]:
			time_in = timestamp
		
		time_out = None
		if log_type in [1,5]:
			time_out = timestamp
		
		doc = frappe.get_doc({
				"doctype":"Attendance",
				"date":timestamp,
				"employee" : employee.name,
				"check_in_time" : timestamp,
				"check_out_time" : time_out
			}
		)
			
		doc.insert()
	else:
		frappe.throw("No employee id {}".format(employee.employee_name))
 
@frappe.whitelist()
def xx():
	return "xxxxxxx"#frappe.enqueue("epos_restaurant_2023.employee_management.doctype.check_in_out.check_in_out.check_in_out", queue='short', self=self)