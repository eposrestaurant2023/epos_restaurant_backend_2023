
declare @from_id nvarchar(50)='247BBAAC-5923-4289-D727-08D9BEAF0432';
declare @to_id nvarchar(50)='98A76229-1B89-4AAE-0140-08D9F83276E6';


 

if object_id('tempdb..#tbl_menu') is not null drop table #tbl_menu
select *,IDENT_CURRENT('tbl_menu') + ROW_NUMBER()  OVER(ORDER BY id ASC) as new_id, null as new_parent_id 
into #tbl_menu  
from tbl_menu 
where is_deleted = 0
and business_branch_id = @from_id 
 
 
 update m1
 set m1.new_parent_id =  (select top(1) new_id from #tbl_menu x where x.id = m1.parent_id ) 
 from #tbl_menu m
 left join #tbl_menu m1 on m.id = m1.parent_id
 where m1.id is not null
 


 --clone menu
 SET IDENTITY_INSERT dbo.tbl_menu ON; 
	insert into tbl_menu ([business_branch_id]
        ,[id]
		,[parent_id]
        ,[menu_name_en]
        ,[menu_name_kh]
        ,[created_by]
        ,[created_date]
        ,[is_deleted]
        ,[deleted_by]
        ,[deleted_date]
        ,[status]
        ,[background_color]
        ,[photo]
        ,[text_color]
        ,[menu_path]
        ,[root_menu_id]
        ,[sort_order]
        ,[is_shortcut_menu]
        ,[last_modified_by]
        ,[last_modified_date])
	select @to_id
		,[new_id]
		,[new_parent_id]
		,[menu_name_en]
		,[menu_name_kh]
		,[created_by]
		,[created_date]
		,[is_deleted]
		,[deleted_by]
		,[deleted_date]
		,[status]
		,[background_color]
		,[photo]
		,[text_color]
		,[menu_path]
		,[root_menu_id]
		,[sort_order]
		,[is_shortcut_menu]
		,[last_modified_by]
		,[last_modified_date]
	from #tbl_menu 

 SET IDENTITY_INSERT dbo.tbl_menu OFF;


 --clone menu product
 insert into tbl_product_menu(menu_id,product_id,is_deleted) 
 select m.new_id,pm.product_id, pm.is_deleted from tbl_product_menu pm
 inner join #tbl_menu m on m.id = pm.menu_id