--epos_dev_client_db is dev
--epos_lum_orng_client_db is pro

--Step 1 Check new table Dev Database 
SELECT * from epos_dev_client_db.INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME not in (SELECT TABLE_NAME from epos_lum_orng_client_db.INFORMATION_SCHEMA.TABLES)
--Generate only table dont have in pro db
-- contrain name, schema and data, use dB false, collation, permission

--Step 2 generate table from dev db that not exist in pro db



--STEP 3 if table exist in pro db and dev db not have
SELECT 'drop table ' +  TABLE_NAME from epos_lum_orng_client_db.INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME not in (SELECT TABLE_NAME from epos_dev_client_db.INFORMATION_SCHEMA.TABLES)




--STEP 4 Check new field have in dev DB but not have in Pro DB
--check with data type decimal
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
 dev.COLUMN_DEFAULT,
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE + '(' + cast(dev.NUMERIC_PRECISION as nvarchar(50)) + ',' + cast(dev.NUMERIC_SCALE as nvarchar(50)) + ')'
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'decimal' 

--check with data type float
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
 dev.COLUMN_DEFAULT,
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE + ' DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'''''') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'float' 

--check with data type nvarchar  
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE +  '(' + cast(dev.CHARACTER_MAXIMUM_LENGTH as nvarchar(50)) +  ') DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'''''') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'nvarchar' and dev.CHARACTER_MAXIMUM_LENGTH>=0


--check with data type nvarchar(max)  
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE +  '(max) DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'''''') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'nvarchar'  and dev.CHARACTER_MAXIMUM_LENGTH = -1

--check data have diferent lenght for nvarchar only
SELECT 
	dev.TABLE_NAME as dev_TABLE_NAME,
	dev.COLUMN_NAME as dev_COLUMN_NAME, 
	dev.DATA_TYPE as dev_DATA_TYPE,
	pro.TABLE_NAME as pro_TABLE_NAME, 
	pro.COLUMN_NAME as pro_COLUMN_NAME,
	pro.DATA_TYPE as pro_DATA_TYPE ,
	pro.character_maximum_length as pro_character_maximum_length,
	dev.character_maximum_length as dev_character_maximum_length,
	'ALTER TABLE ' + pro.TABLE_NAME + ' ALTER COLUMN ' + pro.COLUMN_NAME + ' NVARCHAR (' + case when cast(dev.character_maximum_length as nvarchar(50))='-1' then 'MAX' else cast(dev.character_maximum_length as nvarchar(50)) end + ') NULL;' as [modify]
from epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro
inner join epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev  on pro.DATA_TYPE = dev.DATA_TYPE and pro.TABLE_NAME = dev.TABLE_NAME and pro.COLUMN_NAME = dev.COLUMN_NAME and pro.character_maximum_length <> dev.character_maximum_length



--check with data type text 
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE +  ' DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'''''') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'text'   


--check with data type ntext 
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE +  ' DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'''''') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'ntext' 

--check with data type int
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE  + ' DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'0') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'int' 


--check with data type bit
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE  + ' DEFAULT ' + coalesce(dev.COLUMN_DEFAULT ,'0') 
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE = 'bit' 



--check with data type date & datetime
SELECT 
 pro.COLUMN_NAME, 
 dev.TABLE_NAME, 
 dev.COLUMN_NAME ,
  
 'alter table ' + dev.TABLE_NAME + ' add ' + dev.COLUMN_NAME +  ' ' + dev.DATA_TYPE  
from epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev
left join  epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE pro.COLUMN_NAME is null and dev.DATA_TYPE in ('date' , 'datetime','time','datetime2')








--STEP 5 check if field in pro db exist but not exist in dev db
SELECT 
 dev.COLUMN_NAME, 
 pro.TABLE_NAME, 
 pro.COLUMN_NAME ,
 'ALTER TABLE ' + pro.TABLE_NAME + ' DROP COLUMN ' + pro.COLUMN_NAME
  
from epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro
left join  epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev on dev.TABLE_NAME = pro.TABLE_NAME and dev.COLUMN_NAME = pro.COLUMN_NAME
WHERE dev.COLUMN_NAME is null 


--STEP 6 check if data type differenct
SELECT 
 dev.TABLE_NAME as dev_TABLE_NAME,
 dev.COLUMN_NAME as dev_COLUMN_NAME, 
 dev.DATA_TYPE as dev_DATA_TYPE,
 pro.TABLE_NAME as pro_TABLE_NAME, 
 pro.COLUMN_NAME as pro_COLUMN_NAME,
 pro.DATA_TYPE as pro_DATA_TYPE ,
 'ALTER TABLE ' + pro.TABLE_NAME + ' ALTER COLUMN ' + pro.COLUMN_NAME + ' ' + dev.DATA_TYPE + '(' + cast(dev.NUMERIC_PRECISION as nvarchar(50)) + ',' + cast(dev.NUMERIC_SCALE as nvarchar(50)) + ')'
from epos_lum_orng_client_db.INFORMATION_SCHEMA.COLUMNS pro
inner join epos_dev_client_db.INFORMATION_SCHEMA.COLUMNS dev  on pro.DATA_TYPE <> dev.DATA_TYPE and pro.TABLE_NAME = dev.TABLE_NAME and pro.COLUMN_NAME = dev.COLUMN_NAME



--STEP 7 do manual update datatype on table that have different field datatype

-- STEP 8 check collation in field

--Step 9 genderate ALL function and store procedure from dev db to replace in pro db

-- Step 10 Check tbl_setting for new record
-- step 11 check tbl_document_setting
-- Step 12 Check permission

-- Step 13 Check Collation 
select 'ALTER TABLE ' + TABLE_NAME + ' ALTER COLUMN ' + COLUMN_NAME + ' nvarchar(' + case when cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(50))='-1' then 'MAX' else cast(CHARACTER_MAXIMUM_LENGTH as nvarchar(50)) end + ') COLLATE Khmer_100_BIN NULL;' from INFORMATION_SCHEMA.COLUMNS where DATA_TYPE='nvarchar'   and COLLATION_NAME<>'Khmer_100_BIN'


-- Step 14 Enabl Broker Rollback
alter database epos_lum_orng_client_db set enable_broker with rollback immediate;
-- Or 
ALTER DATABASE epos_lum_orng_client_db SET NEW_BROKER;