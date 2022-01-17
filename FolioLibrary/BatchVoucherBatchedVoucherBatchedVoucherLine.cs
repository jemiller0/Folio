using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FolioLibrary
{
    // uc.batch_voucher_batched_voucher_batched_voucher_lines -> diku_mod_invoice_storage.batch_vouchers
    // BatchVoucherBatchedVoucherBatchedVoucherLine -> BatchVoucher
    [DisplayColumn(nameof(Id)), DisplayName("Batch Voucher Batched Voucher Batched Voucher Lines"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_voucher_batched_voucher_batched_voucher_lines", Schema = "uc")]
    public partial class BatchVoucherBatchedVoucherBatchedVoucherLine
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Batch Voucher Batched Voucher", Order = 2)]
        public virtual BatchVoucherBatchedVoucher BatchVoucherBatchedVoucher { get; set; }

        [Column("batch_voucher_batched_voucher_id"), Display(Name = "Batch Voucher Batched Voucher", Order = 3), Required, StringLength(1024)]
        public virtual string BatchVoucherBatchedVoucherId { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 4), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Column("external_account_number"), Display(Name = "Account Number", Order = 5), JsonProperty("externalAccountNumber"), Required, StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Display(Name = "Batch Voucher Batched Voucher Batched Voucher Line Fund Codes", Order = 6), JsonConverter(typeof(ArrayJsonConverter<List<BatchVoucherBatchedVoucherBatchedVoucherLineFundCode>, BatchVoucherBatchedVoucherBatchedVoucherLineFundCode>), "Content"), JsonProperty("fundCodes")]
        public virtual ICollection<BatchVoucherBatchedVoucherBatchedVoucherLineFundCode> BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchVoucherBatchedVoucherId)} = {BatchVoucherBatchedVoucherId}, {nameof(Amount)} = {Amount}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes)} = {(BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes != null ? $"{{ {string.Join(", ", BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes)} }}" : "")} }}";

        public static BatchVoucherBatchedVoucherBatchedVoucherLine FromJObject(JObject jObject) => jObject != null ? new BatchVoucherBatchedVoucherBatchedVoucherLine
        {
            Amount = (decimal?)jObject.SelectToken("amount"),
            AccountNumber = (string)jObject.SelectToken("externalAccountNumber"),
            BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes = jObject.SelectToken("fundCodes")?.Select(jt => BatchVoucherBatchedVoucherBatchedVoucherLineFundCode.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("amount", Amount),
            new JProperty("externalAccountNumber", AccountNumber),
            new JProperty("fundCodes", BatchVoucherBatchedVoucherBatchedVoucherLineFundCodes?.Select(bvbvbvlfc => bvbvbvlfc.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
