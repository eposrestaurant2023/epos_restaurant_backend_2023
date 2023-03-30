import frappe
import datetime


def after_install():
    # ceate table group
    if not frappe.db.exists("Table Group", "Main Group"):
        doc = frappe.get_doc(
          {
            "name": "Main Group",
            "outlet": "Main Outlet",
            "business_branch": "Main Branch",
            "table_group_name_en": "Main Group",
            "table_group_name_kh": "Main Group",
            "disabled": 0,
            "is_standard": 1,
            "doctype": "Table Group",
            "photo": "/images/defaulttable_bg.jpg"
        }
        )
        doc.insert()

    #create table 
    create_table("01")
    create_table("02")
    create_table("03")
    create_table("04")
    create_table("05")
    create_table("06")
    create_table("07")
    create_table("08")
    create_table("09")
    create_table("10")


    #create note
    create_note("Edit Closed Receipt",["Wrong Payment","Wrong Product","Guest Cancel","Cashier Error"])

def create_note(name, note_list):
    if not frappe.db.exists("Category Note", note_list):
        notes = []
        for n in note_list:
            notes.append({"note":n})
        
        doc = frappe.get_doc(
        {
            "name": name,
            "category_note_name_en": name,
            "category_note_name_kh": name,
            "disabled": 0,
            "multiple_selected": 0,
            "doctype": "Category Note",
            "notes": notes
        }
        )
        doc.insert()

#create table
def create_table(tbl_no):
    if not frappe.db.exists("Table Group", "Main Group"):
        doc = frappe.get_doc(
         {
            "tbl_number": tbl_no,
            "tbl_group": "Main Group",
            "width": 100,
            "height": 85,
            "shape": "Rectangle",
            "discount_type": "Percent",
            "default_discount": 0,
            "price_rule": "",
            "sale_type": "Dine In",
            "require_check_in": 0,
            "doctype": "Tables Number"
            }
        )
        doc.insert()

def replace_format(string,year):
     
    year_short = str(datetime.datetime.now().year)[-2:]
    month = str(datetime.datetime.now().month).zfill(2)
    digit = str(1).zfill(4)
    return string.replace('.', '').replace('YYYY', year).replace('yyyy', year).replace('YY', year_short).replace('yy', year_short).replace('MM', month).replace('#', '')


@frappe.whitelist()
def reset_sale_transaction():
    # backupd db first


    if frappe.session.user == 'Administrator':
        frappe.db.sql("delete from `tabCash Transaction`")
        frappe.db.sql("delete from `tabPOS Sale Payment`")
        frappe.db.sql("delete from `tabSale Payment`")
        frappe.db.sql("delete from `tabSale Product`")
        frappe.db.sql("delete from `tabSale`")
        frappe.db.sql("delete from `tabCashier Shift Cash Float`")
        frappe.db.sql("delete from `tabCashier Shift`")
        frappe.db.sql("delete from `tabWorking Day`")
        frappe.db.sql("delete from `tabComment` where reference_doctype in ('Sale','POS Sale Payment','Sale Payment','Sale Product','Cashier Shift Cash Float','Cashier Shift','Working Day')")

        

        #reset sale transaction 
        doctypes = ["Sale","Sale Payment","Cashier Shift","Working Day","Cash Transaction"]
        for d in doctypes:
            if frappe.get_meta("Sale").get_field("naming_series"):
                formats =  frappe.get_meta(d).get_field("naming_series").options
                if formats:
                    for f in formats.split("\n"):
                        for n in range(2022, 2030):
                            format_text = replace_format(f,str(n))
                            
                            frappe.db.sql("update `tabSeries` set current=  0 where name='{}'".format(format_text) )
        

        frappe.db.commit()

        return "Done"
    else:
        return "Please login as administrator"



@frappe.whitelist()
def reset_database():
    if frappe.session.user == 'Administrator':
        create_sale_receipt_print_format()
        create_sale_invoice_a4_print_format()




def create_sale_receipt_print_format():
    
    data = {
        "name": "Sale Receipt",
        "title": "Sale Receipt",
        "doc_type": "Sale",
        "module": "Selling",
        "default_print_language": "en",
        "standard": "Yes",
        "custom_format": 0,
        "show_in_pos": 1,
        "show_in_pos_report": 1,
        "show_in_pos_closed_sale": 0,
        "sort_order": 1,
        "disabled": 0,
        "print_format_type": "Jinja",
        "raw_printing": 0,
        "margin_top": 0,
        "margin_bottom": 0,
        "margin_left": 0,
        "margin_right": 0,
        "align_labels_right": 0,
        "show_section_headings": 0,
        "line_breaks": 0,
        "absolute_value": 0,
        "font_size": 3,
        "page_number": "Bottom Center",
        "css": "\r\n@media print {\r\n    .print-format{\r\n    padding: 1px !important;\r\n    }\r\n    \r\n}\r\n\r\n \r\n.report-list {\r\n    margin-top: 10px !important;\r\n    margin-bottom: 5px !important;\r\n}\r\n.print-format{\r\n    padding: 2px !important;\r\n}\r\n.print-format td{\r\n    padding: 4px !important;\r\n    border: none !important;\r\n  \r\n}\r\n.report-list td,.report-list th {\r\n    padding: 3px !important;\r\n    font-size: 13px !important;\r\n    \r\n}\r\n.tbl-list tr td {\r\n    padding: 4px 0px !important;\r\n    font-size: 10px !important;\r\n    border: none !important;\r\n}\r\n.tbl-list tr td:nth-child(2){\r\n    padding-left: 1px !important;\r\n    padding-right: 1px !important;\r\n    \r\n}\r\n.tbl-list-right tr td:nth-child(3){\r\n    text-align: right;\r\n}\r\n\r\n.report-list thead tr th {\r\n    color: #000;\r\n    font-weight: 600;\r\n    padding-top: 4px !important;\r\n    padding-bottom: 4px !important;\r\nborder-bottom: solid 1px black!important;\r\n \r\n}\r\n\r\n.tbl-list-summary{\r\n    margin-top:10px;\r\n}\r\n.tbl-list-summary td{\r\n    font-size: 12px;\r\n    padding:0px;\r\n}\r\n.summary-value{\r\n    text-align:right;\r\n}\r\n.payment p{\r\n    font-size: 10px;\r\n}\r\n.center {\r\n  text-align: center;\r\n  margin-top: 20px !important;\r\n}\r\n.print-format .row:not(.section-break){\r\n    margin-top:0px!important;\r\n}\r\n.tbl-list-summary tr td:nth-child(2){\r\n  text-align:right!important;\r\n}\r\n.grand_total{\r\nfont-weight:bold; font-size:13px; margin-top:0px; border:solid 2px #000000;\r\n\r\n}\r\n.grand_total tr td:nth-child(2){\r\n text-align:right!important;\r\n\r\n}\r\n.table-bill-header tr td{\r\n    margin:0px!important;\r\n    padding:0px!important;\r\n    font-size:13px;\r\n}\r\n*{\r\n    color:#000000!important;\r\n}",
        "format_data": "[{\"fieldname\": \"print_heading_template\", \"fieldtype\": \"Custom HTML\", \"options\": \"{% set date_format = frappe.db.sql(\\\"select TIME_FORMAT(creation, '%H:%i %p') as ptime,TIME_FORMAT(modified, '%H:%i %p') as ctime from `tabSale` where name='{}'\\\".format(doc.name),as_dict=1) %}\\n\\n{% if doc.docstatus == 1 %}\\n    <h3 class=\\\"text-center\\\">INVOICE</h3>\\n{% else %}\\n    <h3 class=\\\"text-center\\\">DRAFT ITEM</h3>\\n{% endif%}\\n<table style=\\\"width:100%\\\" class=\\\"table-bill-header\\\">\\n    <tr>\\n        <td>{{_(\\\"Date\\\")}}: {{frappe.format_value(doc.posting_date,\\\"Date\\\")}}</td>\\n        <td class=\\\"text-right\\\" style=\\\"white-space:nowrap\\\">{{_(\\\"Invoice No\\\")}}: {{doc.name}}</td>\\n    </tr>\\n    <tr>\\n        <td style=\\\"white-space:nowrap\\\">{{_(\\\"Customer\\\")}}: {{doc.customer_name}}</td>\\n        <td class=\\\"text-right\\\">{{_(\\\"Table No\\\")}}: {{doc.tbl_number}}</td>\\n    </tr>\\n    {% if docstatus == 0 %}\\n    <tr>\\n        <td>{{_(\\\"Opened\\\")}}: {{date_format[0].ptime}}</td>\\n        <td class=\\\"text-right\\\" style=\\\"white-space:nowrap\\\">{{_(\\\"Cashier\\\")}}: {{doc.created_by}}</td>\\n    </tr>\\n    {% else %}\\n    <tr>\\n        <td>{{_(\\\"Opened\\\")}}: {{date_format[0].ptime}}</td>\\n        <td class=\\\"text-right\\\" style=\\\"white-space:nowrap\\\">{{_(\\\"Closed\\\")}}: {{date_format[0].ctime}}</td>\\n    </tr>\\n    <tr>\\n        <td>{{_(\\\"Cashier\\\")}}: {{doc.closed_by}}</td>\\n    </tr>\\n    {%endif%}\\n    {% if doc.note %}\\n    <tr>\\n        <td colspan=\\\"2\\\">{{_(\\\"Note\\\")}}: {{doc.note}}</td>\\n        \\n    </tr>\\n    {% endif %}\\n    \\n    \\n    \\n</table>\"}, {\"fieldtype\": \"Section Break\", \"label\": \"\"}, {\"fieldtype\": \"Column Break\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"{% \\r\\n    set sale_transactions = frappe.db.sql(\\\"\\r\\n    select \\r\\n    p.product_code,\\r\\n    sum(p.quantity) as qty, \\r\\n    p.price,\\r\\n    p.discount,\\r\\n    p.discount_amount,\\r\\n    p.note,\\r\\n    concat(p.product_name,if(coalesce(p.portion,'')='','',concat('( ', p.portion, ' )')),\\r\\n    if(coalesce(p.modifiers,'')='','',concat(' - ', p.modifiers)),\\r\\n    if(p.is_free=1,' -Free',''))\\r\\n    as product_name,\\r\\n    if(p.discount_type = 'Amount','','%') as discount_type,\\r\\n    sum(p.sub_total) as amount,\\r\\n    s.customer_name \\r\\n    from `tabSale Product` p \\r\\n    inner join\\r\\n        `tabSale` s on s.name = p.parent \\r\\n    where \\r\\n        s.name = '{}' \\r\\n    group by \\r\\n    p.product_name,\\r\\n    p.price,\\r\\n    p.portion,\\r\\n    p.modifiers,\\r\\n    p.discount,\\r\\n    p.discount_amount,\\r\\n    p.note,\\r\\n    p.discount_type\\r\\n    \\\".format(doc.name),as_dict=1)%}\\r\\n    \\r\\n{%set main_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"currency\\\"))%}\\r\\n{%set second_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"second_currency\\\"))%}\\r\\n\\r\\n<table class=\\\"table report-list\\\">\\r\\n    <thead>\\r\\n        <tr>\\r\\n            <th class=\\\"text-left\\\">{{_(\\\"DESCRIPTION\\\")}}</th>\\r\\n            <th class=\\\"text-left\\\">{{_(\\\"QTY\\\")}}</th>\\r\\n            <th class=\\\"text-right\\\">{{_(\\\"PRICE\\\")}}</th>\\r\\n            <th class=\\\"text-right\\\">{{_(\\\"AMOUNT\\\")}}</th>\\r\\n        </tr>\\r\\n    </thead>\\r\\n    <tbody>\\r\\n        {% for p in sale_transactions %}\\r\\n        <tr>\\r\\n            <td class=\\\"text-left\\\">{{p.product_code}} - {{p.product_name}}</td>\\r\\n            <td class=\\\"text-left\\\">{{p.qty}}</td>\\r\\n            <td class=\\\"text-right\\\" style=\\\"white-space: nowrap;\\\">{{ frappe.utils.fmt_money(p.price,currency=main_currency.name, precision=main_currency.currency_precision) }}</td>\\r\\n            <td class=\\\"text-right\\\" style=\\\"white-space: nowrap;\\\">{{frappe.utils.fmt_money(p.amount - p.discount_amount,currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n            {% if p.discount > 0 %}\\r\\n                <tr>\\r\\n                    <td colspin='4'>{{_(\\\"Disc. \\\")}}( {{p.discount}} {{p.discount_type}}) : {{p.discount_amount}}</td>\\r\\n                </tr>\\r\\n            {%endif%}\\r\\n            {% if p.note != '' %}\\r\\n                <tr>\\r\\n                    <td colspin='4'>{{_(\\\"Note: \\\")}}{{p.note}}</td>\\r\\n                </tr>\\r\\n            {%endif%}\\r\\n        </tr>\\r\\n       \\r\\n        {% endfor %}\\r\\n    </tbody>\\r\\n</table>\\r\\n<div style=\\\"border-bottom: 1px solid #000000; margin-top: 12px\\\"></div>\\r\\n         \\r\\n     \"}, {\"fieldtype\": \"Section Break\", \"label\": \"\"}, {\"fieldtype\": \"Column Break\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"{%set main_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"currency\\\"))%}\\r\\n{%set second_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"second_currency\\\"))%}\\r\\n\\r\\n<div class=\\\"tbl-list-summary\\\" style=\\\"margin-top:5px\\\">\\r\\n    <table class=\\\"ml-auto\\\">\\r\\n        {% if doc.sub_total != doc.grand_total %}\\r\\n        <tr>\\r\\n                <td style=\\\"width:150px\\\">{{_(\\\"Sub Total\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.sub_total, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        {% if doc.product_discount>0 %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Item Discount\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.product_discount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        {% if doc.sale_discount>0 %}\\r\\n        <tr>\\r\\n                <td>\\r\\n                    {{_(\\\"Sale Discount\\\")}} \\r\\n                    {% if doc.discount_type == \\\"Percent\\\"%}\\r\\n                        ({{doc.discount}} %) \\r\\n                    {% endif %}\\r\\n                </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.sale_discount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        {% if doc.product_discount>0 and doc.sale_discount >0 %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Total Discount\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.total_discount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        \\r\\n        {% if doc.tax_1_amount > 0 %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Tax 1\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.tax_1_amount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        {% if doc.tax_2_amount > 0 %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Tax 2\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.tax_2_amount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        {% if doc.tax_3_amount > 0 %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Tax 3\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.tax_3_amount, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        {% if (1 if doc.tax_1_amount> 0 else 0) + (1 if doc.tax_2_amount> 0 else 0)  + (1 if doc.tax_3_amount> 0 else 0)  >=2  %}\\r\\n        <tr>\\r\\n                <td>{{_(\\\"Total Tax\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.total_tax, currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        \\r\\n        \\r\\n    </table>\\r\\n</div>\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"{%set main_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"currency\\\"))%}\\r\\n{%set second_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"second_currency\\\"))%}\\r\\n\\r\\n<div class=\\\"grand_total\\\" >\\r\\n    <table class=\\\"ml-auto\\\" style=\\\"margin-top:0px\\\">\\r\\n       \\r\\n        <tr>\\r\\n               <td style=\\\"width:138px\\\">{{_(\\\"Grand Total\\\")}}({{main_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.grand_total,currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n           <tr>\\r\\n                <td>{{_(\\\"Grand Total\\\")}}({{second_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.grand_total * doc.exchange_rate,currency=second_currency.name, precision=second_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        \\r\\n        \\r\\n    </table>\\r\\n</div>\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"{%set main_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"currency\\\"))%}\\r\\n{%set second_currency = frappe.get_doc(\\\"Currency\\\",frappe.db.get_default(\\\"second_currency\\\"))%}\\r\\n\\r\\n<div class=\\\"tbl-list-summary\\\" style=\\\"margin-top:5px\\\">\\r\\n    <table class=\\\"ml-auto\\\">\\r\\n        \\r\\n        {% for p in doc.payment %}\\r\\n         <tr>\\r\\n                <td style=\\\"width:150px\\\">{{p.payment_type}}</td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(p.input_amount, currency=p.currency,precision=p.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endfor %}\\r\\n        \\r\\n        {% if (doc.payment | length) > 1 %}\\r\\n         <tr>\\r\\n                <td>{{_(\\\"Total Paid\\\")}}</td>\\r\\n                \\r\\n                <td>{{frappe.format_value(doc.total_paid, \\\"Currency\\\")}}</td>\\r\\n        </tr>\\r\\n        \\r\\n        {% endif %}\\r\\n        \\r\\n        {% if doc.balance > 0 and doc.docstatus==1 %}\\r\\n             <tr>\\r\\n                <td>{{_(\\\"Balance\\\")}}({{main_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.balance,currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n           <tr>\\r\\n                <td>{{_(\\\"Balance\\\")}}({{second_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.balance * doc.exchange_rate,currency=second_currency.name, precision=second_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        {% if doc.changed_amount > 0 %}\\r\\n             <tr>\\r\\n                <td>{{_(\\\"Changed\\\")}}({{main_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.changed_amount,currency=main_currency.name, precision=main_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n           <tr>\\r\\n                <td>{{_(\\\"Changed\\\")}}({{second_currency.name}}) </td>\\r\\n                \\r\\n                <td>{{frappe.utils.fmt_money(doc.changed_amount * doc.exchange_rate,currency=second_currency.name, precision=second_currency.currency_precision)}}</td>\\r\\n        </tr>\\r\\n        {% endif %}\\r\\n        \\r\\n        \\r\\n    </table>\\r\\n</div>\"}]",
        "print_format_builder": 0,
        "print_format_builder_beta": 0,
        "pos_invoice_file_name": "rpt_invoice_en",
        "pos_receipt_file_name": "rpt_receipt_en",
        "receipt_height": 11,
        "receipt_width": 3.14,
        "receipt_margin_top": 0,
        "receipt_margin_bottom": 0,
        "receipt_margin_left": 0,
        "receipt_margin_right": 0,
        "print_invoice_copies": 1,
        "print_receipt_copies": 1,
        "doctype": "Print Format"

    }
    if not frappe.db.exists("Print Format", "Sale Receipt"):
        doc = frappe.get_doc(data)
        doc.insert()


def create_sale_invoice_a4_print_format():
    data = {
        "name": "Sale Invoice A4",
        "doc_type": "Sale",
        "module": "Selling",
        "default_print_language": "en",
        "standard": "No",
        "custom_format": 0,
        "disabled": 0,
        "print_format_type": "Jinja",
        "raw_printing": 0,
        "margin_top": 5,
        "margin_bottom": 5,
        "margin_left": 10,
        "margin_right": 10,
        "align_labels_right": 0,
        "show_section_headings": 0,
        "line_breaks": 0,
        "absolute_value": 0,
        "font_size": 14,
        "page_number": "Bottom Center",
        "css": "\r\n.report-list {\r\n    margin-top: 10px !important;\r\n    margin-bottom: 10px !important;\r\n}\r\n.report-list td,.tbl-payment-list td {\r\n    padding-top: 6px!important;\r\n    padding-bottom: 6px!important;\r\n}\r\n.tbl-list tr td,.tbl-list-summary table tr td {\r\n    padding: 4px 0px !important;\r\n    font-size: 13px !important;\r\n}\r\n.tbl-list tr td:nth-child(2), .tbl-list-summary table tr td:nth-child(2){\r\n    padding-left: 4px !important;\r\n    padding-right: 4px !important;\r\n}\r\n.tbl-list-right tr td:nth-child(3), .tbl-list-summary table tr td:nth-child(3){\r\n    text-align: right;\r\n}\r\n.report-list thead {\r\n    background: #f3f3f3;\r\n}\r\n.report-list thead tr th {\r\n    color: #000;\r\n    font-weight: 600;\r\n}\r\n.report-list th, .report-list td {\r\n    border: 1px solid #d9d9d9 !important;\r\n}\r\n.tbl-list-summary table tr td:nth-child(3) {\r\n    min-width: 100px;\r\n}\r\n.tbl-payment-list tbody td {\r\n    border-top: none !important;\r\n}\r\n.tbl-payment-list thead {\r\n    border-bottom: 1px solid #d9d9d9;\r\n    font-weight: 600;\r\n}",
        "format_data": "[{\"fieldname\": \"print_heading_template\", \"fieldtype\": \"Custom HTML\", \"options\": \"<h4 class=\\\"text-center\\\">{{_(\\\"Sale\\\")}}</h4>\\r\\n\\r\\n<div class=\\\"text-center\\\" style=\\\"text-transform: uppercase;\\\">\\r\\n    {% if doc.docstatus == 1 %}\\r\\n        <span>\\r\\n        {% if doc.balance == 0 and doc.is_free == 1%}\\r\\n            <span><strong>{{_(\\\"Paid\\\")}}</strong></span>\\r\\n        {% endif %}\\r\\n        {% if doc.total_paid > 0 and doc.balance > 0 %}\\r\\n            <span><strong>{{_(\\\"Partially Paid\\\")}}</strong></span>\\r\\n        {% endif %}\\r\\n        {%if doc.is_free == 0%}\\r\\n            {% if doc.total_paid == 0 %}\\r\\n                <span><strong>{{_(\\\"Unpaid\\\")}}</strong></span>\\r\\n            {% endif %}\\r\\n        {%endif%}\\r\\n        </span>\\r\\n    {% endif %}\\r\\n</div>\\r\\n<div class=\\\"row\\\">\\r\\n    <div class=\\\"col-xs-6\\\">\\r\\n{% if doc.customer %}\\r\\n{% set customer = frappe.get_doc('Customer',doc.customer,['*']) %}\\r\\n            {% if customer %}\\r\\n        <table class=\\\"tbl-list\\\">\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Customer Code\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{customer.name}}</strong></td>\\r\\n            </tr>\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Customer Name\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{customer.customer_name_en}}</strong></td>\\r\\n            </tr>\\r\\n            {% if customer.company_name %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Company\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{customer.company_name}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            {% if customer.phone_number %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Phone Number\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{customer.phone_number}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n        </table>\\r\\n            {% endif %}\\r\\n  {% endif %}\\r\\n   </div>\\r\\n    <div class=\\\"col-xs-4\\\">\\r\\n        \\r\\n        </div>\\r\\n    <div class=\\\"col-xs-6\\\">\\r\\n        <table class=\\\"tbl-list ml-auto tbl-list-right\\\">\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Sale #\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.name}}</strong></td>\\r\\n            </tr>\\r\\n            {% if doc.referance %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Ref #\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.referance}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Date\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.posting_date}}</strong></td>\\r\\n            </tr>\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Branch\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.business_branch}}</strong></td>\\r\\n            </tr>\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Stock Location\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.stock_location}}</strong></td>\\r\\n            </tr>\\r\\n        </table>\\r\\n    </div>\\r\\n</div>\"}, {\"fieldtype\": \"Section Break\", \"label\": \"Product List\"}, {\"fieldtype\": \"Column Break\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"{% \\r\\n    set sale_transactions = frappe.db.sql(\\\"\\r\\n    select \\r\\n        p.product_code,\\r\\n        sum(p.quantity) as qty, \\r\\n        p.price, \\r\\n        p.unit,\\r\\n        concat(p.product_name,if(coalesce(p.portion,'')='','',concat('-', p.portion)),\\r\\n        if(coalesce(p.modifiers,'')='','',concat('-', p.modifiers)),\\r\\n        if(p.is_free=1,'-Free','')) \\r\\n        as product_name,\\r\\n        sum(p.sub_total) as amount,\\r\\n        s.customer_name \\r\\n    from \\r\\n        `tabSale Product` p \\r\\n    inner join\\r\\n        `tabSale` s on s.name = p.parent \\r\\n    where \\r\\n        s.name = '{}' \\r\\n    group by \\r\\n        p.product_name,\\r\\n        p.price,\\r\\n        p.portion,\\r\\n        p.modifiers,\\r\\n        p.unit\\r\\n    \\\".format(doc.name),as_dict=1)%}\\r\\n<table class=\\\"table table-bordered report-list\\\">\\r\\n    <thead>\\r\\n        <tr>\\r\\n            <th>{{_(\\\"No\\\")}}</th>\\r\\n            <th>{{_(\\\"Description\\\")}}</th>\\r\\n            <th class=\\\"text-center\\\">{{_(\\\"QTY\\\")}}</th>\\r\\n            <th class=\\\"text-center\\\">{{_(\\\"Unit\\\")}}</th>\\r\\n            <th class=\\\"text-right\\\">{{_(\\\"Price\\\")}}</th>\\r\\n            <th class=\\\"text-right\\\">{{_(\\\"Amount\\\")}}</th>\\r\\n        </tr>\\r\\n    </thead>\\r\\n    <tbody>\\r\\n        {% for p in sale_transactions %}\\r\\n        <tr>\\r\\n            <td>{{sale_transactions.index(p)+1}}</td>\\r\\n            <td>{{p.product_code}} - {{p.product_name}}</td>\\r\\n            <td class=\\\"text-center\\\">{{p.qty}}</td>\\r\\n            <td class=\\\"text-center\\\">{{p.unit}}</td>\\r\\n            <td class=\\\"text-right\\\">{{ frappe.format_value(p.price,\\\"Currency\\\") }}</td>\\r\\n            <td class=\\\"text-right\\\">{{frappe.format_value(p.amount,\\\"Currency\\\")}}</td>\\r\\n        {% endfor %}\\r\\n    </tbody>\\r\\n</table>\"}, {\"fieldname\": \"_custom_html\", \"print_hide\": 0, \"label\": \"Custom HTML\", \"fieldtype\": \"HTML\", \"options\": \"    <div class=\\\"row\\\">\\r\\n        <div class=\\\"col-xs-6\\\">\\r\\n            {% set payments = frappe.get_list('Sale Payment',filters={\\r\\n            'sale': doc.name,'docstatus':1},fields=['*'],order_by='posting_date asc') %}\\r\\n            {% if payments %}\\r\\n            {% for payment in payments %}\\r\\n            <p>Paid on <b>{{payment.posting_date}}</b> by {{payment.payment_type}} : <b>{{frappe.format_value(payment.payment_amount, \\\"Currency\\\")}}</b></p>\\r\\n            {% endfor %}\\r\\n            {% endif %}\\r\\n        </div>\\r\\n    <div class=\\\"col-xs-6\\\">\\r\\n    <div class=\\\"tbl-list-summary\\\">\\r\\n        <table class=\\\"ml-auto\\\">\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Total Quantity\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{doc.total_quantity}}</strong></td>\\r\\n            </tr>\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Sub Total\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{frappe.format_value(doc.sub_total,\\\"Currency\\\")}}</strong></td>\\r\\n            </tr>\\r\\n            {% if doc.total_discount > 0 %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Total Discount\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{ frappe.format_value(doc.total_discount,\\\"Currency\\\") }}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            {% if doc.total_tax > 0 %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Total Tax\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{ frappe.format_value(doc.total_tax,\\\"Currency\\\") }}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Grand Total\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{frappe.format_value(doc.grand_total,\\\"Currency\\\")}}</strong></td>\\r\\n            </tr>\\r\\n            {% if doc.total_paid > 0 %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Total Paid\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{frappe.format_value(doc.total_paid,\\\"Currency\\\")}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            {% if doc.balance > 0 %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Balance\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{frappe.format_value(doc.balance,\\\"Currency\\\")}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n            {% if doc.changed_amount > 0 %}\\r\\n            <tr>\\r\\n                <td><strong>{{_(\\\"Change\\\")}}</strong></td>\\r\\n                <td>:</td>\\r\\n                <td><strong>{{frappe.format_value(doc.changed_amount,\\\"Currency\\\")}}</strong></td>\\r\\n            </tr>\\r\\n            {% endif %}\\r\\n        </table>\\r\\n    </div>\\r\\n</div>\\r\\n</div>\"}]",
        "print_format_builder": 0,
        "print_format_builder_beta": 0,
        "show_in_pos": 1,
        "receipt_width": 3.14,
        "receipt_margin_top": 0,
        "receipt_margin_bottom": 0,
        "receipt_margin_left": 0,
        "receipt_margin_right": 0,
        "receipt_height": 11,
        "print_invoice_copies": 0,
        "print_receipt_copies": 0,
        "show_in_pos_report": 1,
        "title": "Sale Invoice",
        "sort_order": 0,
        "show_in_pos_closed_sale": 0,
        "doctype": "Print Format",
        }
    if not frappe.db.exists("Print Format", "Sale Invoice A4"):
        doc = frappe.get_doc(data)
        doc.insert()

@frappe.whitelist()
def get_server_name():
    server_name = socket.gethostname()
    return server_name