
if OBJECT_ID('tempdb..#tbl_product') is not null drop table #tbl_product

select 
	m.menu_name_en,
	m.menu_name_kh,
	pc.category_name_en,
	pc.category_name_kh,
	p.product_name_en,
	coalesce(p.product_name_kh,p.product_name_en) as product_name_kh,
	p.allow_discount,
	p.allow_change_price,
	p.allow_free,
	pp.portion_name, 
	mp.price,
	mp.menu_id
	into #tbl_product
from tbl_product p   
inner join tbl_product_category pc on pc.id = p.product_category_id
inner join tbl_menu_product mp on mp.product_id = p.id
inner join tbl_menu m on m.id = mp.menu_id
inner join tbl_product_portion pp on mp.portion_id = pp.id
where coalesce(p.[status],0) = 1 and coalesce(p.is_voided,0) = 0
order by 
	m.menu_name_en,
	p.product_name_en



if object_id('tempdb..#tbl_menu_tree') is not null drop table #tbl_menu_tree
;with menu as
(select m.id,
	m.parent_id,
	cast(m.menu_name_en AS nvarchar(max)) level_menu,
	cast ('' as nvarchar(max)) tree_menu,
	cast (m.menu_name_en AS nvarchar(max)) menu_name
from tbl_menu m
where m.parent_id=0
union all 
select 
	m2.id,
	m2.parent_id,
	c.level_menu + '=>' + cast(m2.menu_name_en AS nvarchar(max)) AS level_menu,
	c.tree_menu + '==' as tree_menu,
	cast (m2.menu_name_en AS nvarchar(max)) AS menu_name
from tbl_menu m2
inner join menu c on m2.parent_id = c.id)


select id as menu_id,
	level_menu,
	tree_menu+menu_name as menu 
	into #tbl_menu_tree 
from menu
order by level_menu option (recompile)

select 
	p.*,
	mt.menu,
	mt.level_menu
from #tbl_menu_tree mt
inner join #tbl_product p on mt.menu_id = p.menu_id
order by mt.level_menu
 

