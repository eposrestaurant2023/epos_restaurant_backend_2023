import frappe
from frappe.utils import date_diff 
from frappe.utils.data import strip
import datetime

def execute(filters=None): 
	if filters.filter_based_on =="Fiscal Year":
		if not filters.from_fiscal_year:
			filters.from_fiscal_year = datetime.date.today().year
		
		filters.start_date = '{}-01-01'.format(filters.from_fiscal_year)
		filters.end_date = '{}-12-31'.format(filters.from_fiscal_year) 

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
	if not filters.from_business_branch:
		filters.from_business_branch = frappe.db.get_list("Business Branch",pluck='name')
  
	if not filters.from_stock_location:
		filters.from_stock_location = frappe.db.get_list("Stock Location",pluck='name')
  
	if not filters.to_business_branch:
		filters.to_business_branch = frappe.db.get_list("Business Branch",pluck='name')
  
	if not filters.to_stock_location:
		filters.to_stock_location = frappe.db.get_list("Stock Location",pluck='name')
  

	if filters.start_date and filters.end_date:
		if filters.start_date > filters.end_date:

			frappe.throw("The 'Start Date' ({}) must be before the 'End Date' ({})".format(filters.start_date, filters.end_date))

	
	if filters.column_group=="Daily":
		n = date_diff(filters.end_date, filters.start_date)
		if n>30:
			frappe.throw("Date range cannot greater than 30 days")

	if filters.row_group and filters.parent_row_group:
		if(filters.row_group == filters.parent_row_group):
			frappe.throw("Parent row group and row group can not be the same")
 


				

def get_columns(filters):
	
	columns = []
	columns.append({'fieldname':'row_group','label':filters.row_group,'fieldtype':'Data','align':'left','width':350})
	# if filters.row_group == "Product":
	# 	columns.append({"label":"Item Code","fieldname":"item_code","fieldtype":"Data","align":"left",'width':130})
	
	hide_columns = filters.get("hide_columns")
	 
	if filters.column_group !="None" and filters.row_group not in ["Date","Month","Year"]:
		 
		for c in get_dynamic_columns(filters):
			columns.append(c)

	#add total to last column

	fields = get_report_field(filters)
	for f in fields:
		if not hide_columns or  f["label"] not in hide_columns:
			columns.append({
					'fieldname':"total_" +  f['fieldname'],
					'label':"Total " + f["label"],
					'fieldtype':f['fieldtype'],
					'precision': f["precision"],
					'align':f['align']
					}
				)
	if (filters.row_group == "Stock Transfer Transaction" or filters.parent_row_group == "Stock Transfer Transaction") and filters.get("include_cancelled") == True:
		columns.append({"label":"Status","fieldname":"docstatus","fieldtype":"Data","align":"center",'width':100})
	return columns
 
def get_dynamic_columns(filters):
	hide_columns = filters.get("hide_columns")
	#dynmic report file
	fields = get_fields(filters)
	#static report field
	report_fields = get_report_field(filters)
	columns=[]
	for f in fields:
		for rf in report_fields:
			if not hide_columns or  rf["label"] not in hide_columns:
				columns.append({
					'fieldname':f["fieldname"] + "_" + rf["fieldname"],
					'label': f["label"] + " "  + rf["short_label"],
					'fieldtype':rf["fieldtype"],
					'precision': rf["precision"],
					'align':rf["align"]}
				)

		
	return columns

def get_fields(filters):
	sql=""
	
	if filters.column_group=="Daily":
		sql = """
			select 
				concat('col_',date_format(date,'%d_%m')) as fieldname, 
				date_format(date,'%d') as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',date_format(date,'%d_%m')) , 
				date_format(date,'%d')  	
		""".format(filters.start_date, filters.end_date)
	elif filters.column_group =="Monthly":
		sql = """
			select 
				concat('col_',date_format(date,'%m_%Y')) as fieldname, 
				date_format(date,'%b %y') as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',date_format(date,'%m_%Y')) , 
				date_format(date,'%b %y')  	
		""".format(filters.start_date, filters.end_date)
	elif filters.column_group=="Weekly":
		sql = """
			select 
				concat('col_',date_format(date,'%v_%Y')) as fieldname, 
				concat('WK ',date_format(date,'%v %y')) as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',date_format(date,'%v_%Y')), 
				concat('WK ',date_format(date,'%v %y')) 
		""".format(filters.start_date, filters.end_date)
	elif filters.column_group=="Quarterly":
		sql = """
			select 
				concat('col_',QUARTER(date)) as fieldname, 
				concat('Q',QUARTER(date),' ',date_format(date,'%y')) as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',QUARTER(date)),
				concat('Q',QUARTER(date),' ',date_format(date,'%y')) 
		""".format(filters.start_date, filters.end_date)
	elif filters.column_group=="Half Yearly":
		sql = """
			select 
				concat('col_',if(month(date) between 1 and 6,'jan_jun','jul_dec'),date_format(date,'%y')) as fieldname, 
				concat(if(month(date) between 1 and 6,'Jan-Jun','Jul-Dec'),' ',date_format(date,'%y')) as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',if(month(date) between 1 and 6,'jan_jun','jul_dec'),date_format(date,'%y')), 
				concat(if(month(date) between 1 and 6,'Jan-Jun','Jul-Dec'),' ',date_format(date,'%y')) 
		""".format(filters.start_date, filters.end_date)
	elif filters.column_group=="Yearly":
		sql = """
			select 
				concat('col_',date_format(date,'%Y')) as fieldname, 
				date_format(date,'%Y') as label ,
				min(date) as start_date,
				max(date) as end_date
			from `tabDates` 
			where date between '{}' and '{}'
			group by
				concat('col_',date_format(date,'%Y')),
				date_format(date,'%Y')
		""".format(filters.start_date, filters.end_date)

	fields = frappe.db.sql(sql,as_dict=1)
	 
	return fields
 
def get_conditions(filters,group_filter=None):
	conditions = " 1 = 1 "
	start_date = filters.start_date
	end_date = filters.end_date


	if(group_filter!=None):
		conditions += " and {} ='{}'".format(group_filter["field"],group_filter["value"].replace("'","''").replace("%","%%"))

	conditions += " AND b.posting_date between '{}' AND '{}'".format(start_date,end_date)

	if filters.get("product_group"):
		conditions += " AND a.product_group in %(product_group)s"

	if filters.get("product_category"):
		conditions += " AND a.product_category in %(product_category)s"
 
	conditions += " AND b.from_business_branch in %(from_business_branch)s"
	conditions += " AND b.from_stock_location in %(from_stock_location)s"
	
	conditions += " AND b.to_business_branch in %(to_business_branch)s"
	conditions += " AND b.to_stock_location in %(to_stock_location)s"
	
	return conditions

def get_report_data(filters,parent_row_group=None,indent=0,group_filter=None):
	
	hide_columns = filters.get("hide_columns")
	row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==filters.row_group][0]
	
	if(parent_row_group!=None):
		row_group = [d["fieldname"] for d in get_row_groups() if d["label"]==parent_row_group][0]

	report_fields = get_report_field(filters)
	
	sql = "select {} as row_group, {} as indent ".format(row_group, indent)
	if filters.column_group != "None":
		fields = get_fields(filters)
		for f in fields:
			
			sql = strip(sql)
			if sql[-1]!=",":
				sql = sql + ','
			
			for rf in report_fields:
				
				if not hide_columns or  rf["label"] not in hide_columns:
					sql = sql +	"SUM(if(b.posting_date between '{}' AND '{}',{},0)) as '{}_{}',".format(f["start_date"],f["end_date"],rf["sql_expression"],f["fieldname"],rf["fieldname"])
			#end for
	# total last column
	item_code = ""
	groupdocstatus = ""
	normal_filter = "b.docstatus in (1) AND"
	# if ((indent > 0) and ( filters.row_group == "Product" or filters.parent_row_group == "Product")):
	# 	item_code = ",a.item_code"
	
	for rf in report_fields:
		#check sql variable if last character is , then remove it
		sql = strip(sql)
		if sql[-1]==",":
			sql = sql[0:len(sql)-1]
		if not hide_columns or  rf["label"] not in hide_columns:
			sql = sql + " ,SUM({}) AS 'total_{}' ".format(rf["sql_expression"],rf["fieldname"])
	sql = sql + """ {2}
		FROM `tabStock Transfer Products` AS a
			INNER JOIN `tabStock Transfer` b on b.name = a.parent
		WHERE
			{4}
			{0}
		GROUP BY 
		{1} {2} {3}
	""".format(get_conditions(filters,group_filter), row_group,item_code,groupdocstatus,normal_filter)	
	
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
	report_summary=[]
	if filters.parent_row_group==None:
		if not filters.is_ticket:
			report_summary =[{"label":"Total " + filters.row_group ,"value":len(data)}]
	
	fields = get_report_field(filters)

	for f in fields:
		if not hide_columns or  f["label"] not in hide_columns:
			value=sum(d["total_" + f["fieldname"]] for d in data if d["indent"]==0)
			if f["fieldtype"] == "Currency":
				value = frappe.utils.fmt_money(value)
			elif f["fieldtype"] =="Float":
				value = "{:.2f}".format(value)
			report_summary.append({"label":"Total {}".format(f["label"]),"value":value,"indicator":f["indicator"]})	

	return report_summary

def get_report_chart(filters,data):
	columns = []
	hide_columns = filters.get("hide_columns")
	dataset = []
	colors = []

	report_fields = get_report_field(filters)

	if filters.column_group != "None":
		fields = get_fields(filters)
		for f in fields:
			columns.append(f["label"])
		for rf in report_fields:
			if not hide_columns or  rf["label"] not in hide_columns:
				#loop sum dynamic column data data set value
				dataset_values = []
				for f in fields:
					dataset_values.append(sum(d["{}_{}".format(f["fieldname"],rf["fieldname"])] for d in data if d["indent"]==0))
					
				dataset.append({'name':rf["label"],'values':dataset_values})
				colors.append(rf["chart_color"])

	else: # if column group is none
		for d in data:
			if d["indent"] ==0:
				columns.append(d["row_group"])

		myds = []
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
	return [
		{"label":"Quantity","short_label":"Qty", "fieldname":"quantity","fieldtype":"Float","indicator":"Grey","precision":2, "align":"center","chart_color":"#FF8A65","sql_expression":"a.quantity"},
		{"label":"Amount", "short_label":"Amt", "fieldname":"amount","fieldtype":"Currency","indicator":"Red","precision":None, "align":"right","chart_color":"#2E7D32","sql_expression":"a.amount"},
 ]
 

def get_row_groups():
	return [
		{
			"fieldname":"a.parent",
			"label":"Stock Transfer Transaction",
			"parent_row_group_filter_field":"row_group",
			
		},
		{
			"fieldname":"a.product_category",
			"label":"Category",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"a.product_group",
			"label":"Product Group",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"b.from_business_branch",
			"label":"From Business Branch",
			"parent_row_group_filter_field":"row_group"
		},
  		{
			"fieldname":"b.from_stock_location",
			"label":"From Stock Location",
			"parent_row_group_filter_field":"row_group"
		},
    	{
			"fieldname":"b.to_business_branch",
			"label":"To Business Branch",
			"parent_row_group_filter_field":"row_group"
		},
  		{
			"fieldname":"b.to_stock_location",
			"label":"To Stock Location",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%d/%%m/%%Y')",
			"label":"Date",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%m/%%Y')",
			"label":"Month",
			"parent_row_group_filter_field":"row_group"
		},
		{
			"fieldname":"date_format(b.posting_date,'%%Y')",
			"label":"Year",
			"parent_row_group_filter_field":"row_group"
		},
  
		{
			"fieldname":"concat(a.product_code,'-',a.product_name)",
			"label":"Product"
		}
	]