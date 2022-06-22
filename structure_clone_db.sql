

----reset sale transaction
--exec epos_mane_hariharalay_db.dbo.sp_reset_sale_transaction 1	

----delete tbl_default_stock_location_product
--truncate table epos_mane_hariharalay_db.dbo.tbl_default_stock_location_product

declare @project_id nvarchar(50) = 'bbe7baf8-00bd-4012-cdb1-08da533b8ecc'

if (select count(id) from epos_mane_hariharalay_db.dbo.tbl_business_branch ) =1
begin

	if(select count(id) from esoftix_backend_db.dbo.tbl_business_branch where project_id = @project_id) > 1
	begin
		select 'New project has multiple business branch.'
		return;
	end
	

	update epos_mane_hariharalay_db.dbo.tbl_setting set setting_value = @project_id where id = 57


	-- temp old data for clear
	if OBJECT_ID('tempdb..#tbl_business_branch_old') is not null drop table #tbl_business_branch_old
	select id,business_branch_name_en,business_branch_name_kh into #tbl_business_branch_old from epos_mane_hariharalay_db.dbo.tbl_business_branch

	if OBJECT_ID('tempdb..#tbl_outlet_old') is not null drop table #tbl_outlet_old
	select id,outlet_name_en,outlet_name_kh into #tbl_outlet_old from epos_mane_hariharalay_db.dbo.tbl_outlet

	if OBJECT_ID('tempdb..#tbl_station_old') is not null drop table #tbl_station_old
	select id,station_name_en,station_name_kh into #tbl_station_old from epos_mane_hariharalay_db.dbo.tbl_station


	if OBJECT_ID('tempdb..#tbl_cash_drawer_old') is not null drop table #tbl_cash_drawer_old
	select id,cash_drawer_name into #tbl_cash_drawer_old from epos_mane_hariharalay_db.dbo.tbl_cash_drawer

	if object_id('tempdb..#tbl_business_branch_new') is not null drop table #tbl_business_branch_new
	select 
		id,
		business_branch_code,
		business_branch_name_en,
		business_branch_name_kh,
		address_en,
		address_kh,
		contact_name,
		email,
		phone_1,
		phone_2,
		website,
		note,
		logo,
		status,
		color,
		created_by,
		created_date,
		is_deleted,
		deleted_by,
		deleted_date,
		last_modified_by,
		last_modified_date
		into #tbl_business_branch_new
	from esoftix_backend_db.dbo.tbl_business_branch 
	where project_id = @project_id

	if object_id('tempdb..#tbl_outlet_new') is not null drop table #tbl_outlet_new
	select 
		o.* 
		into #tbl_outlet_new
	from esoftix_backend_db.dbo.tbl_outlet o
	inner join #tbl_business_branch_new b on o.business_branch_id = b.id

	if object_id('tempdb..#tbl_station_new') is not null drop table #tbl_station_new
	select
		s.*  
		into #tbl_station_new
	from esoftix_backend_db.dbo.tbl_station s
	inner join #tbl_outlet_new o on s.outlet_id = o.id

	if object_id('tempdb..#tbl_cash_drawer_new') is not null drop table #tbl_cash_drawer_new
	select 
		c.id,
		c.cash_drawer_name,
		c.created_by,
		c.created_date,
		c.last_modified_by,
		c.last_modified_date,
		c.is_deleted,
		c.deleted_by,
		c.deleted_date,
		c.status
		into #tbl_cash_drawer_new
	from esoftix_backend_db.dbo.tbl_cash_drawer c
	inner join #tbl_station_new s on c.id = s.cash_drawer_id

	update s
	set 
		s.cash_drawer_name = c.cash_drawer_name
	from #tbl_station_new s
	inner join #tbl_cash_drawer_new c on s.cash_drawer_id = c.id


	--create business branch
	insert into epos_mane_hariharalay_db.dbo.tbl_business_branch(
		id, 
		business_branch_code,
		business_branch_name_en, 
		business_branch_name_kh, 
		address_en, 
		address_kh, 
		email, 
		phone_1, 
		phone_2, 
		website, 
		note, 
		logo, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		color, 
		contact_name, 
		last_modified_by, 
		last_modified_date
	)
	select 
		id, 
		business_branch_code,
		business_branch_name_en, 
		business_branch_name_kh, 
		address_en, 
		address_kh, 
		email, 
		phone_1, 
		phone_2, 
		website, 
		note, 
		logo, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		color, 
		contact_name, 
		last_modified_by, 
		last_modified_date
	from #tbl_business_branch_new 

	--delete old data
	delete
		c
	from epos_mane_hariharalay_db.dbo.tbl_cash_drawer c
	inner join #tbl_cash_drawer_old cc on c.id = cc.id
	
	delete
		s
	from epos_mane_hariharalay_db.dbo.tbl_station s
	inner join #tbl_station_old ss on s.id = ss.id
	
	delete
		o
	from epos_mane_hariharalay_db.dbo.tbl_outlet o
	inner join #tbl_outlet_old oo on o.id = oo.id

	delete
		b
	from epos_mane_hariharalay_db.dbo.tbl_business_branch b
	inner join #tbl_business_branch_old bb on b.id = bb.id 
	 

	--update business branch id & ids 

	declare @tbl table(id nvarchar(50)) 
	insert into @tbl
	select id from #tbl_business_branch_new
	while exists (select id from @tbl)
	begin
		declare @id nvarchar(50);
		set @id = (select top(1) id from @tbl) 

		 declare @update table(val nvarchar(max))
		 insert into @update
		  select 
			'update ' + TABLE_NAME + ' set ' + column_name +' ='''+@id+''''
		  from epos_mane_hariharalay_db.INFORMATION_SCHEMA.columns 
		  where column_name in('business_branch_id','business_branch_ids') 
		  order by column_name;

		 declare @val nvarchar(max);
		 select @val = coalesce(@val +'; '+ val,val) from @update
		 exec(@val)

		delete @tbl where id = @id;
	end


	--create outlet 
	insert into  epos_mane_hariharalay_db.dbo.tbl_outlet
	(
		id, 
		business_branch_id, 
		outlet_name_en, 
		outlet_name_kh, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		last_modified_by, 
		last_modified_date
	)
	select 
		id, 
		business_branch_id, 
		outlet_name_en, 
		outlet_name_kh, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		last_modified_by, 
		last_modified_date
	from #tbl_outlet_new

	-- create station
	insert into epos_mane_hariharalay_db.dbo.tbl_station
	(
		id, 
		station_name_en, 
		station_name_kh, 
		is_already_config, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		outlet_id, 
		is_full_license,
		expired_date, 
		tax_1_rate, 
		tax_1_taxable_rate, 
		tax_2_rate,
		tax_2_taxable_rate, 
		tax_3_rate, 
		tax_3_taxable_rate, 
		last_modified_by, 
		last_modified_date, 
		cash_drawer_id, 
		cash_drawer_name,
		is_order_station
	 )
	select
		id, 
		station_name_en, 
		station_name_kh, 
		is_already_config, 
		created_by, 
		created_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status, 
		outlet_id, 
		is_full_license,
		expired_date, 
		0 as tax_1_rate, 
		1 as tax_1_taxable_rate, 
		0.1 as tax_2_rate,
		1 as tax_2_taxable_rate, 
		0.1 as tax_3_rate, 
		1 as tax_3_taxable_rate, 
		last_modified_by, 
		last_modified_date, 
		cash_drawer_id, 
		cash_drawer_name,
		is_order_station
	from #tbl_station_new

	--create cash drawer
	 insert into  epos_mane_hariharalay_db.dbo.tbl_cash_drawer
	 (
		 id, 
		 cash_drawer_name, 
		 created_by, 
		 created_date, 
		 last_modified_by, 
		 last_modified_date, 
		 is_deleted, 
		 deleted_by, 
		 deleted_date, 
		 status
	)
	 select 
		id, 
		cash_drawer_name, 
		created_by, 
		created_date, 
		last_modified_by, 
		last_modified_date, 
		is_deleted, 
		deleted_by, 
		deleted_date, 
		status
	from #tbl_cash_drawer_new 


	
end
else
begin
	select 'Business Branch more then one.'
end


