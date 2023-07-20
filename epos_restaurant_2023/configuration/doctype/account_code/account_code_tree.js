frappe.treeview_settings['Account Code'] = {
    breadcrumb: 'Account Code',
    title: 'Account Code',
    fields: [
        {
            fieldtype: 'Data', fieldname: 'code',
            label: 'Account Code', reqd: true
        },
       
        {
            fieldtype: 'Data', fieldname: 'account_name',
            label: 'New Account Name', reqd: true
        },
        {
            fieldtype: 'Column Break',
        },
        {
            fieldtype: 'Select', fieldname: 'type',
            label: 'Type', options:"Debit\nCredit" ,default:"Debit"
        },
        {
            fieldtype: 'Check', fieldname: 'is_group',
            label: 'Is Group', 
        },
        {fieldtype: 'Section Break',label:"Discount",   "collapsible": 1,},
        {
            fieldtype: 'Check', fieldname: 'allow_discount',
            label: 'Allow Discount', 
        },
        { fieldtype: 'Column Break',},
        { fieldtype: 'Link', fieldname: 'discount_account',label: 'Discount Account', options:"Account Code" ,"mandatory_depends_on": "eval:doc.allow_discount ==1",},
        {fieldtype: 'Section Break',label:"Taxes",   "collapsible": 1,},
        {
            "default": "0",
            "fieldname": "allow_tax",
            "fieldtype": "Check",
            "label": "Apply Tax on this Account"
           },
           {
            "default": "0",
            "fieldname": "allow_user_to_change_tax",
            "fieldtype": "Check",
            "label": "Allow User to Change Tax"
           },
           {
            "fieldname": "column_break_17",
            "fieldtype": "Column Break"
           },
           {
            "fieldname": "tax_rule",
            "fieldtype": "Link",
            "label": "Tax Rule",
            "mandatory_depends_on": "eval:doc.allow_tax ==1",
            "options": "Tax Rule"
           },
           {
            "default": "No",
            "fieldname": "rate_include_tax",
            "fieldtype": "Select",
            "label": "Rate Include Tax",
            "options": "\nYes\nNo"
           },
           {
            "fieldname": "bank_fee_section",
            "fieldtype": "Section Break",
            "label": "Bank Fee",
            "collapsible": 1,
           },
           {
            "default": "0",
            "fieldname": "allow_bank_fee",
            "fieldtype": "Check",
            "label": "Bank Fee"
           },
           {
            "default": "0",
            "fieldname": "allow_user_to_change_bank_fee",
            "fieldtype": "Check",
            "label": "Allow User to Change Bank Fee"
           },
           {
            "fieldname": "column_break_22",
            "fieldtype": "Column Break"
           },
           {
            "fieldname": "bank_fee_account",
            "fieldtype": "Link",
            "label": "Bank Fee Account",
            "mandatory_depends_on": "eval:doc.allow_bank_fee == 1",
            "options": "Account Code"
           },
           {
            "fieldname": "bank_fee",
            "fieldtype": "Percent",
            "label": "Bank Fee"
           },
           {
            "fieldname": "section_break_24",
            "fieldtype": "Section Break",
            "label": "City Ledger",
            "collapsible": 1,
           },


           {
            "fieldname": "require_city_ledger_account",
            "fieldtype": "Check",
            "label": "Require select a city ledger account",
            "default":0
           }
        

        
       
    ],
    // ignore fields even if mandatory

    onload: function(treeview) {
        
    },
    post_render: function(treeview) {
        // triggered when tree is instanciated
    },
    onrender: function(node) {
        // triggered when a node is instanciated
    },
    on_get_node: function(nodes) {
        // triggered when `get_tree_nodes` returns nodes
    },
    // enable custom buttons beside each node
    extend_toolbar: true,

}
