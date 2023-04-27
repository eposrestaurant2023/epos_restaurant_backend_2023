from frappe import _

def get_data():
	return {
		
		"fieldname": "product_code",
		"transactions": [
			{"label": _("Sell & Purchase"), "items": ["Sale","Purchase Order"]},
            {"label": _("Inventory"), "items": ["Stock Take","Stock Transfer","Stock Adjustment"]},
            {"label": _("Transaction"), "items": ["Stock Location Product","Inventory Transaction"]},
		],
	}
