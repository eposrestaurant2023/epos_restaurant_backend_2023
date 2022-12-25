from frappe import _


def get_data():
	return {
		"fieldname": "vendor_code",
		"transactions": [
			{"label": _("Expense"), "items": ["Expense"]},
			
		],
	}
