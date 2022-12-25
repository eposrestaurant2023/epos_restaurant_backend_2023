from frappe import _

def get_data():
	return {
		"fieldname": "product_code",
		"transactions": [
			{"label": _("Sell"), "items": ["Sale"]},
            {"label": _("Inventory"), "items": ["Stock Take","Stock Transfer","Stock Location Product","Inventory Transaction"]},
		],
	}
