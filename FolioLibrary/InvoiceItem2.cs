using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.invoice_items -> diku_mod_invoice_storage.invoice_lines
    // InvoiceItem2 -> InvoiceItem
    [DisplayColumn(nameof(Number)), DisplayName("Invoice Items"), JsonConverter(typeof(JsonPathJsonConverter<InvoiceItem2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoice_items", Schema = "uc")]
    public partial class InvoiceItem2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InvoiceItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("accounting_code"), Display(Name = "Accounting Code", Order = 2), JsonProperty("accountingCode"), StringLength(1024)]
        public virtual string AccountingCode { get; set; }

        [Column("account_number"), Display(Name = "Account Number", Order = 3), JsonProperty("accountNumber"), StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Column("adjustments_total"), Display(Name = "Adjustments Total", Order = 4), JsonProperty("adjustmentsTotal")]
        public virtual decimal? AdjustmentsTotal { get; set; }

        [Column("comment"), Display(Order = 5), JsonProperty("comment"), StringLength(1024)]
        public virtual string Comment { get; set; }

        [Column("description"), Display(Order = 6), JsonProperty("description"), Required, StringLength(1024)]
        public virtual string Description { get; set; }

        [Display(Order = 7)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("invoice_id"), Display(Name = "Invoice", Order = 8), JsonProperty("invoiceId"), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Column("invoice_line_number"), Display(Order = 9), JsonProperty("invoiceLineNumber"), StringLength(1024)]
        public virtual string Number { get; set; }

        [Column("invoice_line_status"), Display(Name = "Invoice Line Status", Order = 10), JsonProperty("invoiceLineStatus"), RegularExpression(@"^(Open|Reviewed|Approved|Paid|Cancelled|Error)$"), Required, StringLength(1024)]
        public virtual string InvoiceLineStatus { get; set; }

        [Display(Name = "Order Item", Order = 11)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("po_line_id"), Display(Name = "Order Item", Order = 12), JsonProperty("poLineId")]
        public virtual Guid? OrderItemId { get; set; }

        [Column("product_id"), Display(Name = "Product Id", Order = 13), JsonProperty("productId"), StringLength(1024)]
        public virtual string ProductId { get; set; }

        [Display(Name = "Product Id Type", Order = 14)]
        public virtual IdType2 ProductIdType { get; set; }

        [Column("product_id_type_id"), Display(Name = "Product Id Type", Order = 15), JsonProperty("productIdType")]
        public virtual Guid? ProductIdTypeId { get; set; }

        [Column("quantity"), Display(Order = 16), JsonProperty("quantity"), Required]
        public virtual int? Quantity { get; set; }

        [Column("release_encumbrance"), Display(Name = "Release Encumbrance", Order = 17), JsonProperty("releaseEncumbrance")]
        public virtual bool? ReleaseEncumbrance { get; set; }

        [Column("subscription_info"), Display(Name = "Subscription Info", Order = 18), JsonProperty("subscriptionInfo"), StringLength(1024)]
        public virtual string SubscriptionInfo { get; set; }

        [Column("subscription_start"), Display(Name = "Subscription Start", Order = 19), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("subscriptionStart")]
        public virtual DateTime? SubscriptionStart { get; set; }

        [Column("subscription_end"), Display(Name = "Subscription End", Order = 20), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("subscriptionEnd")]
        public virtual DateTime? SubscriptionEnd { get; set; }

        [Column("sub_total"), Display(Name = "Sub Total", Order = 21), JsonProperty("subTotal"), Required]
        public virtual decimal? SubTotal { get; set; }

        [Column("total"), Display(Order = 22), JsonProperty("total")]
        public virtual decimal? Total { get; set; }

        [Column("vendor_ref_no"), Display(Name = "Vendor Ref No", Order = 23), JsonProperty("vendorRefNo"), StringLength(1024)]
        public virtual string VendorRefNo { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 24), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 25), InverseProperty("InvoiceItem2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 26), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 28), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 29), InverseProperty("InvoiceItem2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 30), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(InvoiceItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 32), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Invoice Adjustment Funds", Order = 33)]
        public virtual ICollection<InvoiceAdjustmentFund> InvoiceAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Adjustment Funds", Order = 34), JsonProperty("fundDistributions")]
        public virtual ICollection<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Adjustments", Order = 35), JsonProperty("adjustments")]
        public virtual ICollection<InvoiceItemAdjustment> InvoiceItemAdjustments { get; set; }

        [Display(Name = "Invoice Item Funds", Order = 36), JsonProperty("fundDistributions")]
        public virtual ICollection<InvoiceItemFund> InvoiceItemFunds { get; set; }

        [Display(Name = "Invoice Item Tags", Order = 37), JsonConverter(typeof(ArrayJsonConverter<List<InvoiceItemTag>, InvoiceItemTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<InvoiceItemTag> InvoiceItemTags { get; set; }

        [Display(Name = "Transactions", Order = 38)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Voucher Item Funds", Order = 39)]
        public virtual ICollection<VoucherItemFund> VoucherItemFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(AdjustmentsTotal)} = {AdjustmentsTotal}, {nameof(Comment)} = {Comment}, {nameof(Description)} = {Description}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(Number)} = {Number}, {nameof(InvoiceLineStatus)} = {InvoiceLineStatus}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(ProductId)} = {ProductId}, {nameof(ProductIdTypeId)} = {ProductIdTypeId}, {nameof(Quantity)} = {Quantity}, {nameof(ReleaseEncumbrance)} = {ReleaseEncumbrance}, {nameof(SubscriptionInfo)} = {SubscriptionInfo}, {nameof(SubscriptionStart)} = {SubscriptionStart}, {nameof(SubscriptionEnd)} = {SubscriptionEnd}, {nameof(SubTotal)} = {SubTotal}, {nameof(Total)} = {Total}, {nameof(VendorRefNo)} = {VendorRefNo}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(InvoiceItemAdjustmentFunds)} = {(InvoiceItemAdjustmentFunds != null ? $"{{ {string.Join(", ", InvoiceItemAdjustmentFunds)} }}" : "")}, {nameof(InvoiceItemAdjustments)} = {(InvoiceItemAdjustments != null ? $"{{ {string.Join(", ", InvoiceItemAdjustments)} }}" : "")}, {nameof(InvoiceItemFunds)} = {(InvoiceItemFunds != null ? $"{{ {string.Join(", ", InvoiceItemFunds)} }}" : "")}, {nameof(InvoiceItemTags)} = {(InvoiceItemTags != null ? $"{{ {string.Join(", ", InvoiceItemTags)} }}" : "")} }}";

        public static InvoiceItem2 FromJObject(JObject jObject) => jObject != null ? new InvoiceItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            AccountingCode = (string)jObject.SelectToken("accountingCode"),
            AccountNumber = (string)jObject.SelectToken("accountNumber"),
            AdjustmentsTotal = (decimal?)jObject.SelectToken("adjustmentsTotal"),
            Comment = (string)jObject.SelectToken("comment"),
            Description = (string)jObject.SelectToken("description"),
            InvoiceId = (Guid?)jObject.SelectToken("invoiceId"),
            Number = (string)jObject.SelectToken("invoiceLineNumber"),
            InvoiceLineStatus = (string)jObject.SelectToken("invoiceLineStatus"),
            OrderItemId = (Guid?)jObject.SelectToken("poLineId"),
            ProductId = (string)jObject.SelectToken("productId"),
            ProductIdTypeId = (Guid?)jObject.SelectToken("productIdType"),
            Quantity = (int?)jObject.SelectToken("quantity"),
            ReleaseEncumbrance = (bool?)jObject.SelectToken("releaseEncumbrance"),
            SubscriptionInfo = (string)jObject.SelectToken("subscriptionInfo"),
            SubscriptionStart = ((DateTime?)jObject.SelectToken("subscriptionStart"))?.ToLocalTime(),
            SubscriptionEnd = ((DateTime?)jObject.SelectToken("subscriptionEnd"))?.ToLocalTime(),
            SubTotal = (decimal?)jObject.SelectToken("subTotal"),
            Total = (decimal?)jObject.SelectToken("total"),
            VendorRefNo = (string)jObject.SelectToken("vendorRefNo"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            InvoiceItemAdjustmentFunds = jObject.SelectToken("fundDistributions")?.Where(jt => jt.HasValues).Select(jt => InvoiceItemAdjustmentFund.FromJObject((JObject)jt)).ToArray(),
            InvoiceItemAdjustments = jObject.SelectToken("adjustments")?.Where(jt => jt.HasValues).Select(jt => InvoiceItemAdjustment.FromJObject((JObject)jt)).ToArray(),
            InvoiceItemFunds = jObject.SelectToken("fundDistributions")?.Where(jt => jt.HasValues).Select(jt => InvoiceItemFund.FromJObject((JObject)jt)).ToArray(),
            InvoiceItemTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => InvoiceItemTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("accountingCode", AccountingCode),
            new JProperty("accountNumber", AccountNumber),
            new JProperty("adjustmentsTotal", AdjustmentsTotal),
            new JProperty("comment", Comment),
            new JProperty("description", Description),
            new JProperty("invoiceId", InvoiceId),
            new JProperty("invoiceLineNumber", Number),
            new JProperty("invoiceLineStatus", InvoiceLineStatus),
            new JProperty("poLineId", OrderItemId),
            new JProperty("productId", ProductId),
            new JProperty("productIdType", ProductIdTypeId),
            new JProperty("quantity", Quantity),
            new JProperty("releaseEncumbrance", ReleaseEncumbrance),
            new JProperty("subscriptionInfo", SubscriptionInfo),
            new JProperty("subscriptionStart", SubscriptionStart?.ToUniversalTime()),
            new JProperty("subscriptionEnd", SubscriptionEnd?.ToUniversalTime()),
            new JProperty("subTotal", SubTotal),
            new JProperty("total", Total),
            new JProperty("vendorRefNo", VendorRefNo),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("fundDistributions", InvoiceItemFunds?.Select(iif => iif.ToJObject())),
            new JProperty("adjustments", InvoiceItemAdjustments?.Select(iia => iia.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", InvoiceItemTags?.Select(iit => iit.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
