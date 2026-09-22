using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FolioLibrary
{
    // uc.order_item_fiscal_year_distributions -> uchicago_mod_orders_storage.po_line
    // OrderItemFiscalYearDistribution -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Fiscal Year Distributions"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_fiscal_year_distributions", Schema = "uc")]
    public partial class OrderItemFiscalYearDistribution
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Name = "Fiscal Year", Order = 4)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 5), JsonProperty("fiscalYearId")]
        public virtual Guid? FiscalYearId { get; set; }

        [Display(Name = "Order Item Fiscal Year Distribution Fund Distributions", Order = 6), JsonProperty("fundDistributions")]
        public virtual ICollection<OrderItemFiscalYearDistributionFundDistribution> OrderItemFiscalYearDistributionFundDistributions { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(OrderItemFiscalYearDistributionFundDistributions)} = {(OrderItemFiscalYearDistributionFundDistributions != null ? $"{{ {string.Join(", ", OrderItemFiscalYearDistributionFundDistributions)} }}" : "")} }}";

        public static OrderItemFiscalYearDistribution FromJObject(JObject jObject) => jObject != null ? new OrderItemFiscalYearDistribution
        {
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            OrderItemFiscalYearDistributionFundDistributions = jObject.SelectToken("fundDistributions")?.Where(jt => jt.HasValues).Select(jt => OrderItemFiscalYearDistributionFundDistribution.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("fundDistributions", OrderItemFiscalYearDistributionFundDistributions?.Select(oifydfd => oifydfd.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
