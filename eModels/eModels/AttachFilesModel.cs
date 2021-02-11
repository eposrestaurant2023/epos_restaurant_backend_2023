using eShareModel;
using System;                       
using System.ComponentModel.DataAnnotations.Schema;    
using System.Net.Http;           

namespace eModels
{

    public class AttachFileModel : CoreModel
    {
        public string file_title { get; set; }
        public string file_name { get; set; }
        public string note { get; set; }
        public string file_type { get; set; }
        public long file_size { get; set; }

        public string file_extension { get; set; }


    }


    [Table("tbl_attach_files")]
    public class AttachFilesModel : AttachFileModel
    {
        public bool is_document_file { get; set; } = false;
        

        public Guid? customer_id { get; set; }
        [ForeignKey("customer_id")]
        public CustomerModel customer { get; set; }
        public int? product_id { get; set; }
        [ForeignKey("product_id")]
        public ProductModel product { get; set; }
        
        public int? vendor_id { get; set; }
        [ForeignKey("vendor_id")]
        public VendorModel vendor { get; set; }
        
        public int? purchase_order_id { get; set; }
        [ForeignKey("purchase_order_id")]
        public PurchaseOrderModel purchase_order { get; set; }

        public int? stock_take_id { get; set; }
        [ForeignKey("stock_take_id")]
        public StockTakeModel stock_take { get; set; }

    }

    public class InputFileData
    {
        public MultipartFormDataContent multipartForm { get; set; }
        public string ImageUrl { get; set; } = "";
        public string SaveFolderPath { get; set; } = "";
    }

}
