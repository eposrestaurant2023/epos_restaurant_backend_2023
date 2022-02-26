
declare @from_id nvarchar(50)='247BBAAC-5923-4289-D727-08D9BEAF0432';
declare @to_id nvarchar(50)='98A76229-1B89-4AAE-0140-08D9F83276E6';

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