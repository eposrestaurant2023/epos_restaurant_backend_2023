using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;
namespace eModels
{
    [Table("tbl_business_branch")]
    public class BusinessBranchModel   : eShareModel.BusinessBranchModel
    {
        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();
            stock_locations = new List<StockLocationModel>();
            contacts = new List<ContactModel>();
        }

        
        public  Guid project_id { get; set; }
        [ForeignKey("project_id")]
        public ProjectModel Project { get; set; }
         
        public List<OutletModel>  outlets { get; set; }


        public List<StockLocationModel> stock_locations { get; set; }

        public List<ContactModel> contacts { get; set; }

    }
}
