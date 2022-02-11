using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.batch_voucher_batched_voucher_batched_voucher_line_fund_codes -> uchicago_mod_invoice_storage.batch_vouchers
    // BatchVoucherBatchedVoucherBatchedVoucherLineFundCode -> BatchVoucher
    [DisplayColumn(nameof(Content)), DisplayName("Batch Voucher Batched Voucher Batched Voucher Line Fund Codes"), Table("batch_voucher_batched_voucher_batched_voucher_line_fund_codes", Schema = "uc")]
    public partial class BatchVoucherBatchedVoucherBatchedVoucherLineFundCode
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Batch Voucher Batched Voucher Batched Voucher Line", Order = 2)]
        public virtual BatchVoucherBatchedVoucherBatchedVoucherLine BatchVoucherBatchedVoucherBatchedVoucherLine { get; set; }

        [Column("batch_voucher_batched_voucher_batched_voucher_line_id"), Display(Name = "Batch Voucher Batched Voucher Batched Voucher Line", Order = 3), Required, StringLength(1024)]
        public virtual string BatchVoucherBatchedVoucherBatchedVoucherLineId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchVoucherBatchedVoucherBatchedVoucherLineId)} = {BatchVoucherBatchedVoucherBatchedVoucherLineId}, {nameof(Content)} = {Content} }}";

        public static BatchVoucherBatchedVoucherBatchedVoucherLineFundCode FromJObject(JValue jObject) => jObject != null ? new BatchVoucherBatchedVoucherBatchedVoucherLineFundCode
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
