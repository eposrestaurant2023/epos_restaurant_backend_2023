# Copyright (c) 2023, Tes Pheakdey and contributors
# For license information, please see license.txt

import frappe
from frappe.model.document import Document


class POSTranslation(Document):
	pass


@frappe.whitelist()
def get_translation(lang):
	doc = frappe.get_doc("POS Translation", lang or "en")
	frappe.throw(doc.translate_text)