from frappe import _


def get_data():
	return {
		"fieldname": "expense_category",
		"transactions": [
			{"label": _("Link"), "items": ["Expense Code", "Expense"]},
			
		],
	}
