using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.invoice_item_adjustment_fund_distributions -> uchicago_mod_invoice_storage.invoice_lines
    // InvoiceItemAdjustmentFund -> InvoiceItem
    [DisplayColumn(nameof(Id)), DisplayName("Invoice Item Adjustment Funds"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoice_item_adjustment_fund_distributions", Schema = "uc")]
    public partial class InvoiceItemAdjustmentFund
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Column("invoice_item_adjustment_id"), Display(Name = "Invoice Item Adjustment Id", Order = 2), Required, StringLength(1024)]
        public virtual string InvoiceItemAdjustmentId { get; set; }

        [Column("code"), Display(Name = "Fund Code", Order = 3), JsonProperty("code"), StringLength(1024)]
        public virtual string FundCode { get; set; }

        [Display(Order = 4)]
        public virtual Transaction2 Encumbrance { get; set; }

        [Column("encumbrance_id"), Display(Name = "Encumbrance", Order = 5), JsonProperty("encumbrance")]
        public virtual Guid? EncumbranceId { get; set; }

        [Display(Order = 6)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 7), JsonProperty("fundId"), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Invoice Item", Order = 8)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("invoice_item_id"), Display(Name = "Invoice Item", Order = 9), JsonProperty("invoiceLineId")]
        public virtual Guid? InvoiceItemId { get; set; }

        [Column("distribution_type"), Display(Name = "Distribution Type", Order = 10), JsonProperty("distributionType"), Required, StringLength(1024)]
        public virtual string DistributionType { get; set; }

        [Display(Name = "Expense Class", Order = 11)]
        public virtual ExpenseClass2 ExpenseClass { get; set; }

        [Column("expense_class_id"), Display(Name = "Expense Class", Order = 12), JsonProperty("expenseClassId")]
        public virtual Guid? ExpenseClassId { get; set; }

        [Column("value"), Display(Order = 13), JsonProperty("value"), Required]
        public virtual decimal? Value { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceItemAdjustmentId)} = {InvoiceItemAdjustmentId}, {nameof(FundCode)} = {FundCode}, {nameof(EncumbranceId)} = {EncumbranceId}, {nameof(FundId)} = {FundId}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(DistributionType)} = {DistributionType}, {nameof(ExpenseClassId)} = {ExpenseClassId}, {nameof(Value)} = {Value} }}";

        public static InvoiceItemAdjustmentFund FromJObject(JObject jObject) => jObject != null ? new InvoiceItemAdjustmentFund
        {
            FundCode = (string)jObject.SelectToken("code"),
            EncumbranceId = (Guid?)jObject.SelectToken("encumbrance"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            InvoiceItemId = (Guid?)jObject.SelectToken("invoiceLineId"),
            DistributionType = (string)jObject.SelectToken("distributionType"),
            ExpenseClassId = (Guid?)jObject.SelectToken("expenseClassId"),
            Value = (decimal?)jObject.SelectToken("value")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("code", FundCode),
            new JProperty("encumbrance", EncumbranceId),
            new JProperty("fundId", FundId),
            new JProperty("invoiceLineId", InvoiceItemId),
            new JProperty("distributionType", DistributionType),
            new JProperty("expenseClassId", ExpenseClassId),
            new JProperty("value", Value)).RemoveNullAndEmptyProperties();
    }
}
