using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_fund_distributions -> uchicago_mod_orders_storage.po_line
    // OrderItemFund -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Funds"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_fund_distributions", Schema = "uc")]
    public partial class OrderItemFund
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Column("code"), JsonProperty("code"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string FundCode { get; set; }

        [Display(Order = 5)]
        public virtual Transaction2 Encumbrance { get; set; }

        [Column("encumbrance_id"), Display(Name = "Encumbrance", Order = 6), JsonProperty("encumbrance")]
        public virtual Guid? EncumbranceId { get; set; }

        [Display(Order = 7)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 8), JsonProperty("fundId"), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Expense Class", Order = 9)]
        public virtual ExpenseClass2 ExpenseClass { get; set; }

        [Column("expense_class_id"), Display(Name = "Expense Class", Order = 10), JsonProperty("expenseClassId")]
        public virtual Guid? ExpenseClassId { get; set; }

        [Column("distribution_type"), Display(Name = "Distribution Type", Order = 11), JsonProperty("distributionType"), Required, StringLength(1024)]
        public virtual string DistributionType { get; set; }

        [Column("value"), Display(Order = 12), JsonProperty("value"), Required]
        public virtual decimal? Value { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(FundCode)} = {FundCode}, {nameof(EncumbranceId)} = {EncumbranceId}, {nameof(FundId)} = {FundId}, {nameof(ExpenseClassId)} = {ExpenseClassId}, {nameof(DistributionType)} = {DistributionType}, {nameof(Value)} = {Value} }}";

        public static OrderItemFund FromJObject(JObject jObject) => jObject != null ? new OrderItemFund
        {
            FundCode = (string)jObject.SelectToken("code"),
            EncumbranceId = (Guid?)jObject.SelectToken("encumbrance"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            ExpenseClassId = (Guid?)jObject.SelectToken("expenseClassId"),
            DistributionType = (string)jObject.SelectToken("distributionType"),
            Value = (decimal?)jObject.SelectToken("value")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("code", FundCode),
            new JProperty("encumbrance", EncumbranceId),
            new JProperty("fundId", FundId),
            new JProperty("expenseClassId", ExpenseClassId),
            new JProperty("distributionType", DistributionType),
            new JProperty("value", Value)).RemoveNullAndEmptyProperties();
    }
}
