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
    // uc.invoices -> diku_mod_invoice_storage.invoices
    // Invoice2 -> Invoice
    [DisplayColumn(nameof(Number)), DisplayName("Invoices"), JsonConverter(typeof(JsonPathJsonConverter<Invoice2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoices", Schema = "uc")]
    public partial class Invoice2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Invoice.json")))
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

        [Column("adjustments_total"), Display(Name = "Adjustments Total", Order = 3), JsonProperty("adjustmentsTotal")]
        public virtual decimal? AdjustmentsTotal { get; set; }

        [Display(Name = "Approved By", Order = 4), InverseProperty("Invoice2s")]
        public virtual User2 ApprovedBy { get; set; }

        [Column("approved_by_id"), Display(Name = "Approved By", Order = 5), JsonProperty("approvedBy")]
        public virtual Guid? ApprovedById { get; set; }

        [Column("approval_date"), DataType(DataType.Date), Display(Name = "Approval Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("approvalDate")]
        public virtual DateTime? ApprovalDate { get; set; }

        [Display(Name = "Batch Group", Order = 7)]
        public virtual BatchGroup2 BatchGroup { get; set; }

        [Column("batch_group_id"), Display(Name = "Batch Group", Order = 8), JsonProperty("batchGroupId"), Required]
        public virtual Guid? BatchGroupId { get; set; }

        [Display(Name = "Bill To", Order = 9)]
        public virtual Configuration2 BillTo { get; set; }

        [Column("bill_to_id"), Display(Name = "Bill To", Order = 10), JsonProperty("billTo")]
        public virtual Guid? BillToId { get; set; }

        [Column("chk_subscription_overlap"), Display(Name = "Chk Subscription Overlap", Order = 11), JsonProperty("chkSubscriptionOverlap")]
        public virtual bool? ChkSubscriptionOverlap { get; set; }

        [Column("currency"), Display(Order = 12), JsonProperty("currency"), Required, StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("enclosure_needed"), Display(Name = "Enclosure Needed", Order = 13), JsonProperty("enclosureNeeded")]
        public virtual bool? EnclosureNeeded { get; set; }

        [Column("exchange_rate"), Display(Name = "Exchange Rate", Order = 14), JsonProperty("exchangeRate")]
        public virtual decimal? ExchangeRate { get; set; }

        [Column("export_to_accounting"), Display(Name = "Export To Accounting", Order = 15), JsonProperty("exportToAccounting")]
        public virtual bool? ExportToAccounting { get; set; }

        [Column("folio_invoice_no"), Display(Order = 16), JsonProperty("folioInvoiceNo"), StringLength(1024)]
        public virtual string Number { get; set; }

        [Column("invoice_date"), DataType(DataType.Date), Display(Name = "Invoice Date", Order = 17), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("invoiceDate"), Required]
        public virtual DateTime? InvoiceDate { get; set; }

        [Column("lock_total"), Display(Name = "Lock Total", Order = 18), JsonProperty("lockTotal")]
        public virtual bool? LockTotal { get; set; }

        [Column("note"), Display(Order = 19), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("payment_due"), Display(Name = "Payment Due", Order = 20), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("paymentDue")]
        public virtual DateTime? PaymentDue { get; set; }

        [Column("payment_terms"), Display(Name = "Payment Terms", Order = 21), JsonProperty("paymentTerms"), StringLength(1024)]
        public virtual string PaymentTerms { get; set; }

        [Column("payment_method"), Display(Name = "Payment Method", Order = 22), JsonProperty("paymentMethod"), Required, StringLength(1024)]
        public virtual string PaymentMethod { get; set; }

        [Column("status"), Display(Order = 23), JsonProperty("status"), RegularExpression(@"^(Open|Reviewed|Approved|Paid|Cancelled)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("source"), Display(Order = 24), JsonProperty("source"), RegularExpression(@"^(User|API|EDI)$"), Required, StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("sub_total"), Display(Name = "Sub Total", Order = 25), JsonProperty("subTotal")]
        public virtual decimal? SubTotal { get; set; }

        [Column("total"), Display(Order = 26), JsonProperty("total")]
        public virtual decimal? Total { get; set; }

        [Column("vendor_invoice_no"), Display(Name = "Vendor Invoice No", Order = 27), JsonProperty("vendorInvoiceNo"), Required, StringLength(1024)]
        public virtual string VendorInvoiceNo { get; set; }

        [Column("disbursement_number"), Display(Name = "Disbursement Number", Order = 28), JsonProperty("disbursementNumber"), StringLength(1024)]
        public virtual string DisbursementNumber { get; set; }

        [Column("voucher_number"), Display(Name = "Voucher Number", Order = 29), JsonProperty("voucherNumber"), StringLength(1024)]
        public virtual string VoucherNumber { get; set; }

        [Display(Order = 30)]
        public virtual Transaction2 Payment { get; set; }

        [Column("payment_id"), Display(Name = "Payment", Order = 31), JsonProperty("paymentId")]
        public virtual Guid? PaymentId { get; set; }

        [Column("disbursement_date"), DataType(DataType.Date), Display(Name = "Disbursement Date", Order = 32), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("disbursementDate")]
        public virtual DateTime? DisbursementDate { get; set; }

        [Display(Order = 33)]
        public virtual Organization2 Vendor { get; set; }

        [Column("vendor_id"), Display(Name = "Vendor", Order = 34), JsonProperty("vendorId"), Required]
        public virtual Guid? VendorId { get; set; }

        [Column("manual_payment"), Display(Name = "Manual Payment", Order = 35), JsonProperty("manualPayment")]
        public virtual bool? ManualPayment { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 36), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 37), InverseProperty("Invoice2s1")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 38), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 40), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 41), InverseProperty("Invoice2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 42), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Invoice), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 44), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Documents", Order = 45)]
        public virtual ICollection<Document2> Document2s { get; set; }

        [Display(Name = "Invoice Acquisitions Units", Order = 46), JsonConverter(typeof(ArrayJsonConverter<List<InvoiceAcquisitionsUnit>, InvoiceAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<InvoiceAcquisitionsUnit> InvoiceAcquisitionsUnits { get; set; }

        [Display(Name = "Invoice Adjustments", Order = 47), JsonProperty("adjustments")]
        public virtual ICollection<InvoiceAdjustment> InvoiceAdjustments { get; set; }

        [Display(Name = "Invoice Items", Order = 48)]
        public virtual ICollection<InvoiceItem2> InvoiceItem2s { get; set; }

        [Display(Name = "Invoice Order Numbers", Order = 49), JsonConverter(typeof(ArrayJsonConverter<List<InvoiceOrderNumber>, InvoiceOrderNumber>), "Content"), JsonProperty("poNumbers")]
        public virtual ICollection<InvoiceOrderNumber> InvoiceOrderNumbers { get; set; }

        [Display(Name = "Invoice Tags", Order = 50), JsonConverter(typeof(ArrayJsonConverter<List<InvoiceTag>, InvoiceTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<InvoiceTag> InvoiceTags { get; set; }

        [Display(Name = "Invoice Transaction Summary 2", Order = 51)]
        public virtual InvoiceTransactionSummary2 InvoiceTransactionSummary2 { get; set; }

        [Display(Name = "Order Invoices", Order = 52)]
        public virtual ICollection<OrderInvoice2> OrderInvoice2s { get; set; }

        [Display(Name = "Transactions", Order = 53)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Vouchers", Order = 54)]
        public virtual ICollection<Voucher2> Voucher2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(AdjustmentsTotal)} = {AdjustmentsTotal}, {nameof(ApprovedById)} = {ApprovedById}, {nameof(ApprovalDate)} = {ApprovalDate}, {nameof(BatchGroupId)} = {BatchGroupId}, {nameof(BillToId)} = {BillToId}, {nameof(ChkSubscriptionOverlap)} = {ChkSubscriptionOverlap}, {nameof(Currency)} = {Currency}, {nameof(EnclosureNeeded)} = {EnclosureNeeded}, {nameof(ExchangeRate)} = {ExchangeRate}, {nameof(ExportToAccounting)} = {ExportToAccounting}, {nameof(Number)} = {Number}, {nameof(InvoiceDate)} = {InvoiceDate}, {nameof(LockTotal)} = {LockTotal}, {nameof(Note)} = {Note}, {nameof(PaymentDue)} = {PaymentDue}, {nameof(PaymentTerms)} = {PaymentTerms}, {nameof(PaymentMethod)} = {PaymentMethod}, {nameof(Status)} = {Status}, {nameof(Source)} = {Source}, {nameof(SubTotal)} = {SubTotal}, {nameof(Total)} = {Total}, {nameof(VendorInvoiceNo)} = {VendorInvoiceNo}, {nameof(DisbursementNumber)} = {DisbursementNumber}, {nameof(VoucherNumber)} = {VoucherNumber}, {nameof(PaymentId)} = {PaymentId}, {nameof(DisbursementDate)} = {DisbursementDate}, {nameof(VendorId)} = {VendorId}, {nameof(ManualPayment)} = {ManualPayment}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(InvoiceAcquisitionsUnits)} = {(InvoiceAcquisitionsUnits != null ? $"{{ {string.Join(", ", InvoiceAcquisitionsUnits)} }}" : "")}, {nameof(InvoiceAdjustments)} = {(InvoiceAdjustments != null ? $"{{ {string.Join(", ", InvoiceAdjustments)} }}" : "")}, {nameof(InvoiceOrderNumbers)} = {(InvoiceOrderNumbers != null ? $"{{ {string.Join(", ", InvoiceOrderNumbers)} }}" : "")}, {nameof(InvoiceTags)} = {(InvoiceTags != null ? $"{{ {string.Join(", ", InvoiceTags)} }}" : "")} }}";

        public static Invoice2 FromJObject(JObject jObject) => jObject != null ? new Invoice2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            AccountingCode = (string)jObject.SelectToken("accountingCode"),
            AdjustmentsTotal = (decimal?)jObject.SelectToken("adjustmentsTotal"),
            ApprovedById = (Guid?)jObject.SelectToken("approvedBy"),
            ApprovalDate = ((DateTime?)jObject.SelectToken("approvalDate"))?.ToLocalTime(),
            BatchGroupId = (Guid?)jObject.SelectToken("batchGroupId"),
            BillToId = (Guid?)jObject.SelectToken("billTo"),
            ChkSubscriptionOverlap = (bool?)jObject.SelectToken("chkSubscriptionOverlap"),
            Currency = (string)jObject.SelectToken("currency"),
            EnclosureNeeded = (bool?)jObject.SelectToken("enclosureNeeded"),
            ExchangeRate = (decimal?)jObject.SelectToken("exchangeRate"),
            ExportToAccounting = (bool?)jObject.SelectToken("exportToAccounting"),
            Number = (string)jObject.SelectToken("folioInvoiceNo"),
            InvoiceDate = ((DateTime?)jObject.SelectToken("invoiceDate"))?.ToLocalTime(),
            LockTotal = (bool?)jObject.SelectToken("lockTotal"),
            Note = (string)jObject.SelectToken("note"),
            PaymentDue = ((DateTime?)jObject.SelectToken("paymentDue"))?.ToLocalTime(),
            PaymentTerms = (string)jObject.SelectToken("paymentTerms"),
            PaymentMethod = (string)jObject.SelectToken("paymentMethod"),
            Status = (string)jObject.SelectToken("status"),
            Source = (string)jObject.SelectToken("source"),
            SubTotal = (decimal?)jObject.SelectToken("subTotal"),
            Total = (decimal?)jObject.SelectToken("total"),
            VendorInvoiceNo = (string)jObject.SelectToken("vendorInvoiceNo"),
            DisbursementNumber = (string)jObject.SelectToken("disbursementNumber"),
            VoucherNumber = (string)jObject.SelectToken("voucherNumber"),
            PaymentId = (Guid?)jObject.SelectToken("paymentId"),
            DisbursementDate = ((DateTime?)jObject.SelectToken("disbursementDate"))?.ToLocalTime(),
            VendorId = (Guid?)jObject.SelectToken("vendorId"),
            ManualPayment = (bool?)jObject.SelectToken("manualPayment"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            InvoiceAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => InvoiceAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            InvoiceAdjustments = jObject.SelectToken("adjustments")?.Where(jt => jt.HasValues).Select(jt => InvoiceAdjustment.FromJObject((JObject)jt)).ToArray(),
            InvoiceOrderNumbers = jObject.SelectToken("poNumbers")?.Where(jt => jt.HasValues).Select(jt => InvoiceOrderNumber.FromJObject((JValue)jt)).ToArray(),
            InvoiceTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => InvoiceTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("accountingCode", AccountingCode),
            new JProperty("adjustmentsTotal", AdjustmentsTotal),
            new JProperty("approvedBy", ApprovedById),
            new JProperty("approvalDate", ApprovalDate?.ToUniversalTime()),
            new JProperty("batchGroupId", BatchGroupId),
            new JProperty("billTo", BillToId),
            new JProperty("chkSubscriptionOverlap", ChkSubscriptionOverlap),
            new JProperty("currency", Currency),
            new JProperty("enclosureNeeded", EnclosureNeeded),
            new JProperty("exchangeRate", ExchangeRate),
            new JProperty("exportToAccounting", ExportToAccounting),
            new JProperty("folioInvoiceNo", Number),
            new JProperty("invoiceDate", InvoiceDate?.ToUniversalTime()),
            new JProperty("lockTotal", LockTotal),
            new JProperty("note", Note),
            new JProperty("paymentDue", PaymentDue?.ToUniversalTime()),
            new JProperty("paymentTerms", PaymentTerms),
            new JProperty("paymentMethod", PaymentMethod),
            new JProperty("status", Status),
            new JProperty("source", Source),
            new JProperty("subTotal", SubTotal),
            new JProperty("total", Total),
            new JProperty("vendorInvoiceNo", VendorInvoiceNo),
            new JProperty("disbursementNumber", DisbursementNumber),
            new JProperty("voucherNumber", VoucherNumber),
            new JProperty("paymentId", PaymentId),
            new JProperty("disbursementDate", DisbursementDate?.ToUniversalTime()),
            new JProperty("vendorId", VendorId),
            new JProperty("manualPayment", ManualPayment),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", InvoiceAcquisitionsUnits?.Select(iau => iau.ToJObject())),
            new JProperty("adjustments", InvoiceAdjustments?.Select(ia => ia.ToJObject())),
            new JProperty("poNumbers", InvoiceOrderNumbers?.Select(ion => ion.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", InvoiceTags?.Select(it => it.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
