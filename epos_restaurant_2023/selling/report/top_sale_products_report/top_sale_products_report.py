import frappe
from frappe import _
from frappe.utils import date_diff,today ,add_months, add_days
from frappe.utils.data import strip
import datetime

def execute(filters=None): 
	if filters.filter_based_on =="Fiscal Year":
		if not filters.from_fiscal_year:
			filters.from_fiscal_year = datetime.date.today().year
		
		filters.start_date = '{}-01-01'.format(filters.from_fiscal_year)
		filters.end_date = '{}-12-31'.format(filters.from_fiscal_year) 
	elif filters.filter_based_on =="This Month":
		filters.start_date = datetime.date.today().replace(day=1)
		filters.end_date =add_days(  add_months(filters.start_date ,1),-1)
		 

	validate(filters)
	#run this to update parent_product_group in table sales invoice item

	report_data = []
	skip_total_row=False
	message=None
	if filters.get("parent_row_group"):
		report_data = get_report_group_data(filters)
		message="Enable <strong>Parent Row Group</strong> making report loading slower. Please try  to select some report filter to reduce record from database "
		skip_total_row = True
	else:
		report_data = get_report_data(filters) 
	report_chart = None
	if filters.chart_type !="None" and len(report_data)<=100:
		report_chart = get_report_chart(filters,report_data) 
	return get_columns(filters), report_data, message, report_chart, get_report_summary(report_data,filters),skip_total_row
 
def validate(filters):
	if not filters.business_branch:
		filters.business_branch = frappe.db.get_list("Business Branch",pluck='name')
  
	if not filters.outlet:
		filters.outlet = frappe.db.get_list("Outlet",pluck='name')
  

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:

			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))

	
 
	if filters.row_group and filters.parent_row_group:
		if(filters.row_group == filters.parent_row_group):
			frappe.throw("Parent row group and row group can not be the same")
 


				

def get_columns(filters):	
	columns = []
	row_group = [d for d in get_row_groups() if d["label"]==filters.row_group][0]
 
 
	if filters.row_group == 'Sale Invoice':
		columns.append({'fieldname':'row_group','label':filters.row_group,'fieldtype':'Link',"options":"Sale",'align':'left','width':250})
	else:
		columns.append({'fieldname':'row_group','label':filters.row_group,'fieldtype':'Data','align':'left','width':250})
	# if filters.row_group == "Product":
	# 	columns.append({"label":"Item Code","fieldname":"item_code","fieldtype":"Data","align":"left",'width':130})
	
	hide_columns = filters.get("hide_columns")


	#add total to last column

	fields = get_report_field(filters)
	for f in fields:
		if (not hide_columns or  f["label"] not in hide_columns)  :
			 
			if f['fieldname'] =='commission' or f['fieldname'] =='net_sale' :
				if  row_group["show_commission"]:
					columns.append({
						'fieldname':"total_" +  f['fieldname'],
						'label':"Total " + f["label"],
						'fieldtype':f['fieldtype'],
						'precision': f["precision"],
						'align':f['align']
						}
					)
			else:
				columns.append({
						'fieldname':"total_" +  f['fieldname'],
						'label':"Total " + f["label"],
						'fieldtype':f['fieldtype'],
						'precision': f["precision"],
						'align':f['align']
						}
					)
    
	if (filters.row_group == "Sale Invoice" or filters.parent_row_group == "Sale Invoice") and filters.get("include_cancelled") == True:
		columns.append({"label":"Status","fieldname":"docstatus","fieldtype":"Data","align":"center",'width':100})
	return columns
 
 
def get_conditions(filters,group_filter=None):
	conditions = " 1 = 1 and b.is_foc = 0"

	start_date = filters.start_date
	end_date = filters.end_date


	if(group_filter!=None):
		 
		conditions += " and {} ='{}'".format(group_filter["field"],group_filter["value"].replace("'","''").replace("%","%%"))

	conditions += " AND b.posting_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("product_group"):
		conditions += " AND a.product_group in %(product_group)s"

	if filters.get("product_category"):
		conditions += " AND a.product_category in %(product_category)s"

	if filters.get("customer_group"):
		conditions += " AND b.customer_group in %(customer_group)s"
 
	conditions += " AND b.business_branch in %(business_branch)s"
	conditions += " AND b.outlet in %(outlet)s"

	if filters.get("pos_profile"):
		conditions += " AND b.pos_profile in %(pos_profile)s"
	
	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):

	hide_columns = filters.get("hide_columns")
	row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==filters.row_group][0]
	
	if(parent_row_group!=None):
		row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==parent_row_group][0]
		
	report_fields = get_report_field(filters)
	

	sql = "select {} as row_group, {} as indent ".format(row_group, indent)
	
	# total last column
	item_code = ""
	groupdocstatus = ""
	normal_filter = "b.docstatus in (1) AND"

	#get order by 
	order_by = ""
	if filters.order_by=="Quantity":
		order_by="sum(a.quantity) {}".format(filters.order_type)
	else:
		order_by="sum(a.total_revenue) {}".format(filters.order_type)
	
	for rf in report_fields:
		#check sql variable if last character is , then remove it
		sql = strip(sql)
		if sql[-1]==",":
			sql = sql[0:len(sql)-1]
		if not hide_columns or  rf["label"] not in hide_columns:
			sql = sql + " ,{} AS 'total_{}' ".format(rf["sql_expression"],rf["fieldname"])
	sql = sql + """ {2}
		FROM `tabSale Product` AS a
			INNER JOIN `tabSale` b on b.name = a.parent
		WHERE
			{4}
			{0}
		GROUP BY 
		{1} {2} {3}
		order by 
		{5}
		limit %(top)s
	""".format(get_conditions(filters,group_filter), row_group,item_code,groupdocstatus,normal_filter,order_by)
 
	data = frappe.db.sql(sql,filters, as_dict=1)
	return data
 
def get_report_group_data(filters):
	parent = get_report_data(filters, filters.parent_row_group, 0)
	data=[] 
	for p in parent:
		p["is_group"] = 1
		data.append(p)

		row_group = [d for d in get_row_groups() if d["label"]==filters.parent_row_group][0]
		children = get_report_data(filters, None, 1, group_filter={"field":row_group["fieldname"],"value":p[row_group["parent_row_group_filter_field"]]})
		for c in children:
			data.append(c)
	return data

def get_report_summary(data,filters):
	hide_columns = filters.get("hide_columns")
	row_group = [d for d in get_row_groups() if d["label"]==filters.row_group][0]
	report_summary=[]
	if filters.parent_row_group==None:
		if not filters.is_ticket:
			report_summary =[{"label":"Total " + filters.row_group ,"value":len(data)}]
	fields = get_report_field(filters)

	for f in fields:
		if not hide_columns or  f["label"] not in hide_columns:
			if f["fieldname"] == 'commission':
				if row_group["show_commission"] == True:
					value=sum(d["total_" + f["fieldname"]] for d in data if d["indent"]==0)
					if f["fieldtype"] == "Currency":
						value = frappe.utils.fmt_money(value)
					elif f["fieldtype"] =="Float":
						value = "{:.2f}".format(value)
					report_summary.append({"label":"Total {}".format(f["label"]),"value":value,"indicator":f["indicator"]})
			else:
				value=sum(d["total_" + f["fieldname"]] for d in data if d["indent"]==0)
				if f["fieldtype"] == "Currency":
					value = frappe.utils.fmt_money(value)
				elif f["fieldtype"] =="Float":
					value = "{:.2f}".format(value)

				report_summary.append({"label":"Total {}".format(f["label"]),"value":value,"indicator":f["indicator"]})

	return  report_summary

def get_report_chart(filters,data):
	columns = []
	hide_columns = filters.get("hide_columns")
	dataset = []
	colors = []

	report_fields = get_report_field(filters)

	
	for d in data:
		if d["indent"] ==0:
			columns.append(d["row_group"])


	for rf in report_fields:
		if not hide_columns or  rf["label"] not in hide_columns:
			fieldname = 'total_'+rf["fieldname"]
			if(fieldname=="total_qty"):
				dataset.append({'name':rf["label"],'values':(d["total_qty"] for d in data if d["indent"]==0)})
			elif(fieldname=="total_sub_total"):
				dataset.append({'name':rf["label"],'values':(d["total_sub_total"] for d in data if d["indent"]==0)})
			elif(fieldname=="total_cost"):
				dataset.append({'name':rf["label"],'values':(d["total_cost"] for d in data if d["indent"]==0)})
			elif(fieldname=="total_amount"):
				dataset.append({'name':rf["label"],'values':(d["total_amount"] for d in data if d["indent"]==0)})
			elif(fieldname=="total_profit"):
				dataset.append({'name':rf["label"],'values':(d["total_profit"] for d in data if d["indent"]==0)})

		 

	chart = {
		'data':{
			'labels':columns,
			'datasets':dataset
		},
		"type": filters.chart_type,
		"lineOptions": {
			"regionFill": 1,
		},
		"axisOptions": {"xIsSeries": 1}
	}
	return chart

def get_report_field(filters):
	row_group = [d for d in get_row_groups() if d["label"]==filters.row_group][0]
	fields = []
	fields.append({"label":"Quantity","short_label":"Qty", "fieldname":"quantity","fieldtype":"Float","indicator":"Grey","precision":2, "align":"center","chart_color":"#FF8A65","sql_expression":"SUM(a.quantity)"})
	fields.append({"label":"Sub Total", "short_label":"Sub To.", "fieldname":"sub_total","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.sub_total)"})
	fields.append({"label":"Discount", "short_label":"Disc.", "fieldname":"discount_amount","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_discount)"})
	fields.append({"label":"Tax", "short_label":"Tax", "fieldname":"total_tax","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_tax)"})
	fields.append({"label":"Amount", "short_label":"Amt", "fieldname":"amount","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"})
	fields.append({"label":"Cost", "short_label":"Cost", "fieldname":"cost","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.cost)"})
	if row_group['show_commission'] :
		fields.append({"label":"Commission", "short_label":"commission", "fieldname":"commission","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(b.commission_amount)/Count(a.name)"})
		fields.append({"label":"Net Sale", "short_label":"net_sale", "fieldname":"net_sale","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue) - SUM(b.commission_amount)/Count(a.name)"})
		fields.append({"label":"Profit", "short_label":"Profit", "fieldname":"profit","fieldtype":"Currency","indicator":"Green","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue - a.cost) - SUM(b.commission_amount)/Count(a.name)"})
	else:
		fields.append({"label":"Net Sale", "short_label":"net_sale", "fieldname":"net_sale","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"})
		fields.append({"label":"Profit", "short_label":"Profit", "fieldname":"profit","fieldtype":"Currency","indicator":"Green","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue - a.cost)"})
 	# return [
	# 	{"label":"Quantity","short_label":"Qty", "fieldname":"quantity","fieldtype":"Float","indicator":"Grey","precision":2, "align":"center","chart_color":"#FF8A65","sql_expression":"SUM(a.quantity)"},
	# 	{"label":"Sub Total", "short_label":"Sub To.", "fieldname":"sub_total","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.sub_total)"},
	# 	{"label":"Discount", "short_label":"Disc.", "fieldname":"discount_amount","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_discount)"},
	# 	{"label":"Tax", "short_label":"Tax", "fieldname":"total_tax","fieldtype":"Currency","indicator":"Grey","precision":None, "align":"right","chart_color":"#dd5574","sql_expression":"SUM(a.total_tax)"},
	# 	{"label":"Amount", "short_label":"Amt", "fieldname":"amount","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"},
	# 	{"label":"Commission", "short_label":"commission", "fieldname":"commission","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(b.commission_amount)/Count(a.name)"},
	# 	{"label":"Net Sale", "short_label":"net_sale", "fieldname":"net_sale","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue)"},
	# 	{"label":"Cost", "short_label":"Cost", "fieldname":"cost","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.cost)"},
	# 	{"label":"Profit", "short_label":"Profit", "fieldname":"profit","fieldtype":"Currency","indicator":"Green","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"SUM(a.total_revenue - a.cost)"},
	# ]
	return fields
 

def get_row_groups():
	return [
		{
			"fieldname":"a.parent",
			"label":"Sale Invoice",
			"parent_row_group_filter_field":"row_group",
			"show_commission":True
		},
		{
			"fieldname":"a.product_category",
			"label":"Category",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(a.product_group,'')='','Not Set',a.product_group)",
			"label":"Product Group",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"a.revenue_group",
			"label":"Revenue Group",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"b.outlet",
			"label":"Outlet",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.tbl_group,'')='','Not Set',b.tbl_group)",
			"label":_("Table Group"),
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.tbl_number,'')='','Not Set',b.tbl_number)",
			"label":_("Table"),
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"b.business_branch",
			"label":"Business Branch",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.pos_profile,'')='','Not Set',b.pos_profile)",
			"label":"POS Profile",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.customer,'')='','Not Set',concat(b.customer,'-',b.customer_name))",
			"label":"Customer",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.customer_group,'')='','Not Set',b.customer_group)",
			"label":"Customer Group",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},		
		{
			"fieldname":"ifnull(b.stock_location,'Not Set')",
			"label":"Stock Location",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"date_format(b.posting_date,'%%d/%%m/%%Y')",
			"label":"Date",
			"parent_row_group_filter_field":"row_group",
			"show_commission":True
		},
		{
			"fieldname":"date_format(b.posting_date,'%%m/%%Y')",
			"label":"Month",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"date_format(b.posting_date,'%%Y')",
			"label":"Year",
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},

		{
			"fieldname":"concat(a.product_code,'-',a.product_name)",
			"label":"Product",
			"show_commission":False
		},
  		{
			"fieldname":"if(ifnull(b.working_day,'')='','Not Set',b.working_day)",
			"label":_("Working Day"),
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.cashier_shift,'')='','Not Set',b.cashier_shift)",
			"label":_("Cashier Shift"),
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
		{
			"fieldname":"if(ifnull(b.sale_type,'')='','Not Set',b.sale_type)",
			"label":_("Sale Type"),
			"parent_row_group_filter_field":"row_group",
			"show_commission":False
		},
	]