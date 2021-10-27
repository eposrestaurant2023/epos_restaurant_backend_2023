﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eShareModel;
namespace eModels
{
    [Table("tbl_business_branch")]
    public class BusinessBranchModel   : BusinessBranchShareModel
    {
        public BusinessBranchModel()
        {
            outlets = new List<OutletModel>();
            stock_locations = new List<StockLocationModel>();
            cash_drawers = new List<CashDrawerModel>();
            contacts = new List<ContactRelatedModel>();
            business_branch_system_features = new List<BusinessBranchSystemFeatureModel>();
        }
         
        public  Guid project_id { get; set; }
        [ForeignKey("project_id")]
        public ProjectModel Project { get; set; }

        public DateTime? start_date { get; set; } = DateTime.Now.AddMonths(1);
        public DateTime? expired_date { get; set; } = DateTime.Now.AddMonths(1);

        public List<OutletModel>  outlets { get; set; }


       public List<StockLocationModel> stock_locations { get; set; }
       public List<CashDrawerModel> cash_drawers { get; set; }

        public List<ContactRelatedModel> contacts { get; set; }

        public List<BusinessBranchSystemFeatureModel> business_branch_system_features{ get; set; }

    }
}
