﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ePOSPrintingService.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("yqvCU6H7jCxODH8z49Lq1DgjjsUjy+CGLHCPbk5dc2ans4lapfIy7YC/1k7slsjwEep3Ba9dKaGyMDB5c" +
            "2xe5tC9Anp2K/s8azqJiPMytqghMI2swRsRHvrfbqmmWsfg/FSnHdrXacyQwtwJEsZTJyDbzsZhVnfLp" +
            "g+rmpIPEvR9KP9/h9Viwke488+Qdb9z")]
        public string DBConnection {
            get {
                return ((string)(this["DBConnection"]));
            }
            set {
                this["DBConnection"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\deleteme\\")]
        public string FileWatcherPath {
            get {
                return ((string)(this["FileWatcherPath"]));
            }
            set {
                this["FileWatcherPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{\"PageWidth\":1.5,\"PageHeight\":1,\"MarginTop\":0,\"MarginRight\":0,\"MarginBottom\":0,\"M" +
            "arginLeft\":0}")]
        public string LabelPageSetup {
            get {
                return ((string)(this["LabelPageSetup"]));
            }
            set {
                this["LabelPageSetup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OneNote")]
        public string LabelPrinterName {
            get {
                return ((string)(this["LabelPrinterName"]));
            }
            set {
                this["LabelPrinterName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[\r\n    {\r\n        \"ReceiptName\": \"Invoice En\",\r\n        \"ReceiptFileName\": \"rpt_r" +
            "eceipt_en\",\r\n        \"InvoiceFileName\": \"rpt_invoice_en\",\r\n        \"number_invoi" +
            "ce_copies\": 1,\r\n        \"number_receipt_copies\": 2,\r\n        \"PageWidth\": 2.85,\r" +
            "\n        \"PageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n" +
            "        \"MarginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    },\r\n    {\r\n        \"Rec" +
            "eiptName\": \"Invoice Kh\",\r\n        \"ReceiptFileName\": \"rpt_receipt_kh\",\r\n        " +
            "\"InvoiceFileName\": \"rpt_invoice_kh\",\r\n        \"number_invoice_copies\": 2,\r\n     " +
            "   \"number_receipt_copies\": 1,\r\n        \"PageWidth\": 2.85,\r\n        \"PageHeight\"" +
            ": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n        \"MarginBottom" +
            "\": 0,\r\n        \"MarginLeft\": 0\r\n    },\r\n    {\r\n        \"ReceiptName\": \"Kitchen O" +
            "rder\",\r\n        \"ReceiptFileName\": \"rpt_kitchen_order\",\r\n        \"InvoiceFileNam" +
            "e\": \"rpt_kitchen_order\",\r\n        \"number_invoice_copies\": 1,\r\n        \"number_r" +
            "eceipt_copies\": 1,\r\n        \"PageWidth\": 2.85,\r\n        \"PageHeight\": 11,\r\n     " +
            "   \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n        \"MarginBottom\": 0,\r\n     " +
            "   \"MarginLeft\": 0\r\n    },\r\n {\r\n        \"ReceiptName\": \"Close Working Day\",\r\n   " +
            "     \"ReceiptFileName\": \"rpt_close_working_day_summary\",\r\n        \"PageWidth\": 2" +
            ".85,\r\n        \"PageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight\":" +
            " 0,\r\n        \"MarginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    },\r\n {\r\n        \"R" +
            "eceiptName\": \"Close Working Day Sale Product\",\r\n        \"ReceiptFileName\": \"rpt_" +
            "close_working_day_sale_product\",\r\n        \"PageWidth\": 2.85,\r\n        \"PageHeigh" +
            "t\": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n        \"MarginBott" +
            "om\": 0,\r\n        \"MarginLeft\": 0\r\n    }\r\n,\r\n {\r\n        \"ReceiptName\": \"Close Wo" +
            "rking Day Sale Transaction\",\r\n        \"ReceiptFileName\": \"rpt_close_working_day_" +
            "sale_transaction\",\r\n        \"PageWidth\": 2.85,\r\n        \"PageHeight\": 11,\r\n     " +
            "   \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n        \"MarginBottom\": 0,\r\n     " +
            "   \"MarginLeft\": 0\r\n    },\r\n {\r\n        \"ReceiptName\": \"Close Cashier Shift Summ" +
            "ary\",\r\n        \"ReceiptFileName\": \"rpt_close_cashier_shift_summary\",\r\n        \"P" +
            "ageWidth\": 2.85,\r\n        \"PageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n        \"M" +
            "arginRight\": 0,\r\n        \"MarginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    }\r\n,\r\n" +
            " {\r\n        \"ReceiptName\": \"Close Cashier Shift Sale Transaction\",\r\n        \"Rec" +
            "eiptFileName\": \"rpt_close_cashier_shift_sale_transaction\",\r\n        \"PageWidth\":" +
            " 2.85,\r\n        \"PageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight" +
            "\": 0,\r\n        \"MarginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    }\r\n,\r\n {\r\n      " +
            "  \"ReceiptName\": \"Close Cashier Shift Sale Product\",\r\n        \"ReceiptFileName\":" +
            " \"rpt_close_cashier_shift_sale_product\",\r\n        \"PageWidth\": 2.85,\r\n        \"P" +
            "ageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n        \"MarginRight\": 0,\r\n        \"Ma" +
            "rginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    }\r\n,\r\n {\r\n        \"ReceiptName\": \"" +
            "Deleted Sale Order\",\r\n        \"InvoiceFileName\": \"rpt_deleted_invoice\",\r\n       " +
            " \"PageWidth\": 2.85,\r\n        \"PageHeight\": 11,\r\n        \"MarginTop\": 0,\r\n       " +
            " \"MarginRight\": 0,\r\n        \"MarginBottom\": 0,\r\n        \"MarginLeft\": 0\r\n    }\r\n" +
            "\r\n\r\n]")]
        public string ReceiptSettings {
            get {
                return ((string)(this["ReceiptSettings"]));
            }
            set {
                this["ReceiptSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Cashier Printer")]
        public string CashierPrinter {
            get {
                return ((string)(this["CashierPrinter"]));
            }
            set {
                this["CashierPrinter"] = value;
            }
        }
    }
}
