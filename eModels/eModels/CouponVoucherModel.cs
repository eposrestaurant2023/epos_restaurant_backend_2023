using eShareModel;         
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;          

namespace eModels
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

        public List<RefundTransactionModel> refund_transactions { get; set; }
    }
    [Table("tbl_refund_transaction")]
    public class RefundTransactionModel : RefundTransactionShareModel
    {
        [ForeignKey("coupon_transaction_id")]
        public CouponVoucherTransactionModel coupon_transaction { get; set; }
    }
}
