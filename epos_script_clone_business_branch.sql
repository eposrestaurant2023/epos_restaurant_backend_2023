


declare @from_id nvarchar(50)='696C71BE-ECA2-40FC-2748-08DA37EA03BE'; 
declare @to_id nvarchar(50)='027A149C-64B0-44BB-8A5D-08DAB172713D';

--select * from [tbl_business_branch_currency] where business_branch_id = @to_id;
--select * from tbl_business_branch_payment_type where business_branch_id =@to_id;
--select * from tbl_business_branch_role where business_branch_id = @to_id
--select * from tbl_business_branch_setting where business_branch_id = @to_id
--select * from tbl_discount_code where business_branch_id = @to_id
--select * from tbl_sale_type where business_branch_id = @to_id
--select * from tbl_discount_code where business_branch_id = @to_id


--return;


if exists (select id from tbl_business_branch where id = @to_id )
begin
	--clone business branch currency
	if not exists(select id from [tbl_business_branch_currency] where business_branch_id = @to_id)
	begin 
		INSERT INTO [dbo].[tbl_business_branch_currency]
	           ([business_branch_id]
	           ,[currency_id]
	           ,[exchange_rate]
	           ,[exchange_rate_input]
	           ,[change_exchange_rate]
	           ,[change_exchange_rate_input]
	           ,[is_deleted]
	           ,[id])
		select @to_id
			   ,[currency_id]
			   ,[exchange_rate]
			   ,[exchange_rate_input]
			   ,[change_exchange_rate]
			   ,[change_exchange_rate_input]
			   ,0
			   ,NEWID()
		from tbl_business_branch_currency where business_branch_id = @from_id
	end

	--clone business branch payment type
	if not exists (select business_branch_id from tbl_business_branch_payment_type where business_branch_id =@to_id)
	begin
		insert into tbl_business_branch_payment_type(business_branch_id,payment_type_id,[status])
		select @to_id,payment_type_id,[status] from tbl_business_branch_payment_type where business_branch_id = @from_id
	end

	-- clone business branch role 
	if not exists (select role_id from tbl_business_branch_role where business_branch_id = @to_id)
	begin 
		insert into tbl_business_branch_role (role_id, business_branch_id, is_delete)
		select id, @to_id,0 from tbl_role where id = 1
	end 

	-- clone business branch setting
	if not exists(select setting_id from tbl_business_branch_setting where business_branch_id = @to_id)
	begin
		insert into tbl_business_branch_setting(business_branch_id,setting_id,setting_value)
		select @to_id,setting_id,setting_value from tbl_business_branch_setting where business_branch_id = @from_id
	end 

	--clone discount code
	if not exists( select id from tbl_discount_code where business_branch_id = @to_id)
	begin 
		insert into tbl_discount_code([business_branch_id]
			  ,[discount_code]
			  ,[discount_value]
			  ,[created_by]
			  ,[created_date]
			  ,[is_deleted]
			  ,[deleted_by]
			  ,[deleted_date]
			  ,[status]
			  ,[discount_type]
			  ,[last_modified_by]
			  ,[last_modified_date])	
		 select @to_id
			  ,[discount_code]
			  ,[discount_value]
			  ,[created_by]
			  ,[created_date]
			  ,[is_deleted]
			  ,[deleted_by]
			  ,[deleted_date]
			  ,[status]
			  ,[discount_type]
			  ,[last_modified_by]
			  ,[last_modified_date]
		  from [dbo].[tbl_discount_code]
		  where business_branch_id = @from_id
	end

	-- clone sale type
	if not exists(select id from tbl_sale_type where business_branch_id = @to_id)
	begin
		insert into tbl_sale_type(
			id,
			business_branch_id,
			is_build_in,
			sale_type_name,
			is_order_use_table,
			is_deleted,
			sort_order, 
			color)
		select 
			NEWID(),
			@to_id,
			is_build_in,
			sale_type_name,
			is_order_use_table,
			is_deleted,
			sort_order,
			color 
		from tbl_sale_type 
		where business_branch_id = @from_id
		 
	end 
end  

--*** Manaul update and create ** --
--- run update sp_update_admin_role 1
--- create price rule (for new branch configure)
--- verify price (of new branch configure)
--- create table group and table
--- check printer for product printer setup
---  create product tax base on business_branch
--- update product on field product tax base on business_branch


--create product tax that not exists in @from_id
		if OBJECT_ID('tempdb..#tbl_product') is not null drop table #tbl_product
		select id into #tbl_product from tbl_product
		insert into tbl_product_tax
			([product_id]
			,[business_branch_id]
			,[tax_1_rate]
			,[tax_2_rate]
			,[tax_3_rate]
			,[created_by]
			,[created_date]
			,[is_deleted]
			,[deleted_by]
			,[deleted_date]
			,[status]
			,[last_modified_by]
			,[last_modified_date])
		select 
			[product_id]
			,@from_id
			,[tax_1_rate]
			,[tax_2_rate]
			,[tax_3_rate]
			,[created_by]
			,[created_date]
			,[is_deleted]
			,[deleted_by]
			,[deleted_date]
			,[status]
			,'System'
			,getdate()
		from tbl_product_tax pt
		where pt.business_branch_id = @from_id
		and pt.product_id not in(select id from #tbl_product);



		--create product tax that not exists in @to_id
		if OBJECT_ID('tempdb..#tbl_product_tax') is not null drop table #tbl_product_tax
		select product_id into #tbl_product_tax from tbl_product_tax where business_branch_id = @to_id;

		insert into tbl_product_tax
			([product_id]
			,[business_branch_id]
			,[tax_1_rate]
			,[tax_2_rate]
			,[tax_3_rate]
			,[created_by]
			,[created_date]
			,[is_deleted]
			,[deleted_by]
			,[deleted_date]
			,[status]
			,[last_modified_by]
			,[last_modified_date])

		select 
			[product_id]
			,@to_id
			,[tax_1_rate]
			,[tax_2_rate]
			,[tax_3_rate]
			,[created_by]
			,[created_date]
			,[is_deleted]
			,[deleted_by]
			,[deleted_date]
			,[status]
			,'System'
			,getdate()
		from tbl_product_tax 
		where business_branch_id = @from_id
		and product_id not in (select product_id from #tbl_product_tax)


		--after create product tax to all business branch, run script below to update
		--update tbl_product on field product tax 
		update p 
			set p.product_tax_value  = (select x.business_branch_id,x.tax_1_rate,x.tax_2_rate,x.tax_3_rate from tbl_product_tax x where x.product_id = p.id for json path)
		from tbl_product p
 


--- update product on field business_branch_ids (permission on product specifect by business branch)
--***** Code here *****

		if OBJECT_ID('tempdb..#product') is not null drop table #product
		select id into #product from tbl_product 
 
		declare @name nvarchar(max);
		declare @id int;
		while exists(select * from #product)
		begin			
			set @id =(select top(1) id from #product );
			set @name = null;
			select 
				@name = COALESCE(@name + ', ' + cast( m.business_branch_id as nvarchar(50)), cast( m.business_branch_id as nvarchar(50)))
			from tbl_menu m
			inner join tbl_product_menu pm on m.id = pm.menu_id
			where product_id = @id
 
			update tbl_product set business_branch_ids = @name where id = @id;

			delete #product where id = @id;
		end 


--- 