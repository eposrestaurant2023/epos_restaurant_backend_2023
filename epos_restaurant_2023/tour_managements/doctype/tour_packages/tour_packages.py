# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from py_linq import Enumerable
from frappe.model.document import Document

class TourPackages(Document):
	def validate(self):
		self.total_cost =   Enumerable(self.cost_breakdowns).sum(lambda x: (x.rate or 0))
		self.total_fix_cost=   Enumerable(self.fix_cost).sum(lambda x: (x.rate or 0))
		for d in self.transportation_cost:
			d.rate =( (d.daily_rate or 0) * (d.days or 1) ) + (d.extra_charge or 0)

		if self.total_cost:
			for d in self.tour_package_prices:
				d.fix_cost = self.total_fix_cost
				transportation_cost = get_transportation_cost(self,d.number_of_person)
				d.transportation_type = transportation_cost["transportation_type"]
				d.transportation_cost = transportation_cost["cost"] / (d.number_of_person or 1)
				d.additional_cost = ( self.total_cost / (d.number_of_person or 1))
				if d.number_of_person>= (self.total_pax_include_tour_leader or 1):
					d.additional_cost = d.additional_cost + (self.tour_leader_cost  / (d.number_of_person or 1))
				d.price = self.total_fix_cost + d.transportation_cost  + d.additional_cost 


def get_transportation_cost(self,pax):
	
	for d in self.transportation_cost:
		
		if pax in range(d.min_pax,d.max_pax+1):
			
			return {
				"transportation_type":d.transportation_type,
				"cost":d.rate or 0
			}
	return {
				"transportation_type":"",
				"cost":0
			}