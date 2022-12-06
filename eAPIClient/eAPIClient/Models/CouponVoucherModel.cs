using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using eShareModel;

namespace eAPIClient.Models
{
    [Table("tbl_coupon_voucher")]
    public class CouponVoucherModel : CouponVoucherShareModel
    {
        public List<CouponVoucherTransactionModel> coupon_vouchers { get; set; }
    }


    [Table("tbl_coupon_voucher_transaction")]
    public class CouponVoucherTransactionModel : CouponVoucherTransactionShareModel
    {
        [ForeignKey("coupon_voucher_id")]
        public CouponVoucherModel coupon_voucher { get; set; }


    }

}
