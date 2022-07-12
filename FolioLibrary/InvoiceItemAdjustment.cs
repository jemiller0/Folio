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
    // uc.invoice_item_adjustments -> uchicago_mod_invoice_storage.invoice_lines
    // InvoiceItemAdjustment -> InvoiceItem
    [DisplayColumn(nameof(Id)), DisplayName("Invoice Item Adjustments"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoice_item_adjustments", Schema = "uc")]
    public partial class InvoiceItemAdjustment
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Invoice Item", Order = 2)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("invoice_item_id"), Display(Name = "Invoice Item", Order = 3), Required]
        public virtual Guid? InvoiceItemId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("adjustment_id"), JsonProperty("adjustmentId"), ScaffoldColumn(false)]
        public virtual Guid? AdjustmentId { get; set; }

        [Column("description"), Display(Order = 6), JsonProperty("description"), Required, StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("export_to_accounting"), Display(Name = "Export To Accounting", Order = 7), JsonProperty("exportToAccounting")]
        public virtual bool? ExportToAccounting { get; set; }

        [Column("prorate"), Display(Order = 8), JsonProperty("prorate"), Required, StringLength(1024)]
        public virtual string Prorate { get; set; }

        [Column("relation_to_total"), Display(Name = "Relation To Total", Order = 9), JsonProperty("relationToTotal"), Required, StringLength(1024)]
        public virtual string RelationToTotal { get; set; }

        [Column("type"), Display(Order = 10), JsonProperty("type"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("value"), Display(Order = 11), JsonProperty("value"), Required]
        public virtual decimal? Value { get; set; }

        [Column("total_amount"), DataType(DataType.Currency), Display(Name = "Total Amount", Order = 12), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("totalAmount")]
        public virtual decimal? TotalAmount { get; set; }

        [Display(Name = "Invoice Item Adjustment Funds", Order = 13), JsonProperty("fundDistributions")]
        public virtual ICollection<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(Id2)} = {Id2}, {nameof(AdjustmentId)} = {AdjustmentId}, {nameof(Description)} = {Description}, {nameof(ExportToAccounting)} = {ExportToAccounting}, {nameof(Prorate)} = {Prorate}, {nameof(RelationToTotal)} = {RelationToTotal}, {nameof(Type)} = {Type}, {nameof(Value)} = {Value}, {nameof(TotalAmount)} = {TotalAmount}, {nameof(InvoiceItemAdjustmentFunds)} = {(InvoiceItemAdjustmentFunds != null ? $"{{ {string.Join(", ", InvoiceItemAdjustmentFunds)} }}" : "")} }}";

        public static InvoiceItemAdjustment FromJObject(JObject jObject) => jObject != null ? new InvoiceItemAdjustment
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            AdjustmentId = (Guid?)jObject.SelectToken("adjustmentId"),
            Description = (string)jObject.SelectToken("description"),
            ExportToAccounting = (bool?)jObject.SelectToken("exportToAccounting"),
            Prorate = (string)jObject.SelectToken("prorate"),
            RelationToTotal = (string)jObject.SelectToken("relationToTotal"),
            Type = (string)jObject.SelectToken("type"),
            Value = (decimal?)jObject.SelectToken("value"),
            TotalAmount = (decimal?)jObject.SelectToken("totalAmount"),
            InvoiceItemAdjustmentFunds = jObject.SelectToken("fundDistributions")?.Where(jt => jt.HasValues).Select(jt => InvoiceItemAdjustmentFund.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("adjustmentId", AdjustmentId),
            new JProperty("description", Description),
            new JProperty("exportToAccounting", ExportToAccounting),
            new JProperty("prorate", Prorate),
            new JProperty("relationToTotal", RelationToTotal),
            new JProperty("type", Type),
            new JProperty("value", Value),
            new JProperty("totalAmount", TotalAmount),
            new JProperty("fundDistributions", InvoiceItemAdjustmentFunds?.Select(iiaf => iiaf.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
