using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_fiscal_year_distribution_fund_distributions -> uchicago_mod_orders_storage.po_line
    // OrderItemFiscalYearDistributionFundDistribution -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Fiscal Year Distribution Fund Distributions"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_fiscal_year_distribution_fund_distributions", Schema = "uc")]
    public partial class OrderItemFiscalYearDistributionFundDistribution
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item Fiscal Year Distribution", Order = 2)]
        public virtual OrderItemFiscalYearDistribution OrderItemFiscalYearDistribution { get; set; }

        [Column("order_item_fiscal_year_distribution_id"), Display(Name = "Order Item Fiscal Year Distribution", Order = 3), StringLength(1024)]
        public virtual string OrderItemFiscalYearDistributionId { get; set; }

        [Column("code"), Display(Order = 4), JsonProperty("code"), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("encumbrance_id"), Display(Name = "Encumbrance Id", Order = 5), JsonProperty("encumbrance")]
        public virtual Guid? EncumbranceId { get; set; }

        [Display(Order = 6)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 7), JsonProperty("fundId")]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Expense Class", Order = 8)]
        public virtual ExpenseClass2 ExpenseClass { get; set; }

        [Column("expense_class_id"), Display(Name = "Expense Class", Order = 9), JsonProperty("expenseClassId")]
        public virtual Guid? ExpenseClassId { get; set; }

        [Column("distribution_type"), Display(Name = "Distribution Type", Order = 10), JsonProperty("distributionType"), StringLength(1024)]
        public virtual string DistributionType { get; set; }

        [Column("value"), Display(Order = 11), JsonProperty("value")]
        public virtual decimal? Value { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemFiscalYearDistributionId)} = {OrderItemFiscalYearDistributionId}, {nameof(Code)} = {Code}, {nameof(EncumbranceId)} = {EncumbranceId}, {nameof(FundId)} = {FundId}, {nameof(ExpenseClassId)} = {ExpenseClassId}, {nameof(DistributionType)} = {DistributionType}, {nameof(Value)} = {Value} }}";

        public static OrderItemFiscalYearDistributionFundDistribution FromJObject(JObject jObject) => jObject != null ? new OrderItemFiscalYearDistributionFundDistribution
        {
            Code = (string)jObject.SelectToken("code"),
            EncumbranceId = (Guid?)jObject.SelectToken("encumbrance"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            ExpenseClassId = (Guid?)jObject.SelectToken("expenseClassId"),
            DistributionType = (string)jObject.SelectToken("distributionType"),
            Value = (decimal?)jObject.SelectToken("value")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("code", Code),
            new JProperty("encumbrance", EncumbranceId),
            new JProperty("fundId", FundId),
            new JProperty("expenseClassId", ExpenseClassId),
            new JProperty("distributionType", DistributionType),
            new JProperty("value", Value)).RemoveNullAndEmptyProperties();
    }
}
