# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document

class RestaurantBooking(Document):
	def validate(self):
		if(self.discount_type == "Percent"):
				self.total_discount = (self.total_amount or 0) * (self.discount or 0) / 100
		else:
				self.total_discount = (self.discount or 0)
		self.balance = (self.total_amount or 0) - (self.total_payment or 0) - (self.total_discount or 0)

@frappe.whitelist()
def get_meal_plan_rate(restaurant_name, meal_plan):
	data = frappe.db.sql("select coalesce(max(adult_price),0) as adult_price,coalesce(max(child_price),0) as child_price from `tabRestaurant Meal Plan` where parent = '{}' and meal_plan='{}'".format(restaurant_name,meal_plan), as_dict = 1)
	return data
