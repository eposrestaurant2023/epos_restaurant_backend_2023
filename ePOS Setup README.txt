Setup POS Client 
** Database Setup **
1. SQL Server Express 2016 + SSMS 
2. Restore Database 

** API Setup **
1. IIS Management (Setup & Config)
2. Deploy API Client in IIS 
3. Config API Client in appsettings.json file 
=== i. Database connection
=== ii.  Server API URL
=== iii. Autherize Key (if server api url Unautherized please key this Team-Work  )
=== iv. Business ID (Request to Team work) 
=== v. Print Request Path (Config with path for watcher service print location. Ex: "print_request_path":"C:\\Program Files\\ePOS\\watcher\\print" ) 
=== vi. Telegram Token & ChatID (Can be keep, if need telegram alert let check with Mr.Pheakdey for get token and chatId )
=== vii. BackEnd Telegram (same number vi.  )

*** POS Setup **
1. Create Folder Structure for Project 
  Ex. C:\Program Files\ePOS\
------- 1. client_api
--------2. epos_desktop
------- 3. services
----------3.1.  print
------- 4. watcher
-----------4.1 print

2.Install Printing Service 
**** Confige Services in Printing Service Folder
-------- Edit [ePOSPrintingService.exe.config] file
--------i. Change value of FileWatcherPath 
	 <setting name="FileWatcherPath" serializeAs="String">
                		<value>C:\Program Files\ePOS\watcher\print\</value>
            	</setting>
-------ii. check and change value of ReceiptSettings
------iii. Chagne value of CashierPrinter
            <setting name="CashierPrinter" serializeAs="String">
                <value>Cashier Printer</value>
            </setting>
-------iv. check and config telegram setting 
            <setting name="telegram" serializeAs="String">
                <value>{
  	  	"telegram_alert_url": "https://api.telegram.org/",
    		"telegram_alert_token": "593655431:AAH6x4ncKNsxwj2wlaV2WPS4iIB0O2qRPmw",
    		"telegram_chat_id": "-545894511",
    		"image_path":"C:\\Program Files\\ePOS\\client_api\\uploads\\telegram_images\\"
	}</value>
            </setting>
          ******* Note*****
	the valuen of "image_path" is in location of client_api folder 
--------v. Change value of api_url
 	<setting name="api_url" serializeAs="String">
                		<value>http://localhost:1111/api/</value>
           	 </setting>

************************************************************

--- i. Run cmd as Administrator
------ Type  : 
	sc create "ePOSPrinting" binpath="C:\Program Files\ePOS\services\print\ePOSPrintingService.exe" start=auto



---ii. Start Service in windows services
 (services.msc)


