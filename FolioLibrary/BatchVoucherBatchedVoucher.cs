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
    // uc.batch_voucher_batched_vouchers -> diku_mod_invoice_storage.batch_vouchers
    // BatchVoucherBatchedVoucher -> BatchVoucher
    [DisplayColumn(nameof(Id)), DisplayName("Batch Voucher Batched Vouchers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_voucher_batched_vouchers", Schema = "uc")]
    public partial class BatchVoucherBatchedVoucher
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Batch Voucher", Order = 2)]
        public virtual BatchVoucher2 BatchVoucher { get; set; }

        [Column("batch_voucher_id"), Display(Name = "Batch Voucher", Order = 3), Required]
        public virtual Guid? BatchVoucherId { get; set; }

        [Column("accounting_code"), Display(Name = "Accounting Code", Order = 4), JsonProperty("accountingCode"), Required, StringLength(1024)]
        public virtual string AccountingCode { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 5), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Column("disbursement_number"), Display(Name = "Disbursement Number", Order = 6), JsonProperty("disbursementNumber"), StringLength(1024)]
        public virtual string DisbursementNumber { get; set; }

        [Column("disbursement_date"), DataType(DataType.Date), Display(Name = "Disbursement Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("disbursementDate")]
        public virtual DateTime? DisbursementDate { get; set; }

        [Column("disbursement_amount"), DataType(DataType.Currency), Display(Name = "Disbursement Amount", Order = 8), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("disbursementAmount")]
        public virtual decimal? DisbursementAmount { get; set; }

        [Column("enclosure_needed"), Display(Name = "Enclosure Needed", Order = 9), JsonProperty("enclosureNeeded")]
        public virtual bool? EnclosureNeeded { get; set; }

        [Column("exchange_rate"), Display(Name = "Exchange Rate", Order = 10), JsonProperty("exchangeRate")]
        public virtual decimal? ExchangeRate { get; set; }

        [Column("folio_invoice_no"), Display(Name = "Folio Invoice No", Order = 11), JsonProperty("folioInvoiceNo"), Required, StringLength(1024)]
        public virtual string FolioInvoiceNo { get; set; }

        [Column("invoice_currency"), Display(Name = "Invoice Currency", Order = 12), JsonProperty("invoiceCurrency"), Required, StringLength(1024)]
        public virtual string InvoiceCurrency { get; set; }

        [Column("invoice_note"), Display(Name = "Invoice Note", Order = 13), JsonProperty("invoiceNote"), StringLength(1024)]
        public virtual string InvoiceNote { get; set; }

        [Column("status"), Display(Order = 14), JsonProperty("status"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("system_currency"), Display(Name = "System Currency", Order = 15), JsonProperty("systemCurrency"), Required, StringLength(1024)]
        public virtual string SystemCurrency { get; set; }

        [Column("type"), Display(Order = 16), JsonProperty("type"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("vendor_invoice_no"), Display(Name = "Vendor Invoice No", Order = 17), JsonProperty("vendorInvoiceNo"), Required, StringLength(1024)]
        public virtual string VendorInvoiceNo { get; set; }

        [Column("vendor_name"), Display(Name = "Vendor Name", Order = 18), JsonProperty("vendorName"), Required, StringLength(1024)]
        public virtual string VendorName { get; set; }

        [Column("voucher_date"), DataType(DataType.Date), Display(Name = "Voucher Date", Order = 19), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("voucherDate"), Required]
        public virtual DateTime? VoucherDate { get; set; }

        [Column("voucher_number"), Display(Name = "Voucher Number", Order = 20), JsonProperty("voucherNumber"), Required, StringLength(1024)]
        public virtual string VoucherNumber { get; set; }

        [Display(Name = "Batch Voucher Batched Voucher Batched Voucher Lines", Order = 21), JsonProperty("batchedVoucherLines")]
        public virtual ICollection<BatchVoucherBatchedVoucherBatchedVoucherLine> BatchVoucherBatchedVoucherBatchedVoucherLines { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchVoucherId)} = {BatchVoucherId}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(Amount)} = {Amount}, {nameof(DisbursementNumber)} = {DisbursementNumber}, {nameof(DisbursementDate)} = {DisbursementDate}, {nameof(DisbursementAmount)} = {DisbursementAmount}, {nameof(EnclosureNeeded)} = {EnclosureNeeded}, {nameof(ExchangeRate)} = {ExchangeRate}, {nameof(FolioInvoiceNo)} = {FolioInvoiceNo}, {nameof(InvoiceCurrency)} = {InvoiceCurrency}, {nameof(InvoiceNote)} = {InvoiceNote}, {nameof(Status)} = {Status}, {nameof(SystemCurrency)} = {SystemCurrency}, {nameof(Type)} = {Type}, {nameof(VendorInvoiceNo)} = {VendorInvoiceNo}, {nameof(VendorName)} = {VendorName}, {nameof(VoucherDate)} = {VoucherDate}, {nameof(VoucherNumber)} = {VoucherNumber}, {nameof(BatchVoucherBatchedVoucherBatchedVoucherLines)} = {(BatchVoucherBatchedVoucherBatchedVoucherLines != null ? $"{{ {string.Join(", ", BatchVoucherBatchedVoucherBatchedVoucherLines)} }}" : "")} }}";

        public static BatchVoucherBatchedVoucher FromJObject(JObject jObject) => jObject != null ? new BatchVoucherBatchedVoucher
        {
            AccountingCode = (string)jObject.SelectToken("accountingCode"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            DisbursementNumber = (string)jObject.SelectToken("disbursementNumber"),
            DisbursementDate = ((DateTime?)jObject.SelectToken("disbursementDate"))?.ToLocalTime(),
            DisbursementAmount = (decimal?)jObject.SelectToken("disbursementAmount"),
            EnclosureNeeded = (bool?)jObject.SelectToken("enclosureNeeded"),
            ExchangeRate = (decimal?)jObject.SelectToken("exchangeRate"),
            FolioInvoiceNo = (string)jObject.SelectToken("folioInvoiceNo"),
            InvoiceCurrency = (string)jObject.SelectToken("invoiceCurrency"),
            InvoiceNote = (string)jObject.SelectToken("invoiceNote"),
            Status = (string)jObject.SelectToken("status"),
            SystemCurrency = (string)jObject.SelectToken("systemCurrency"),
            Type = (string)jObject.SelectToken("type"),
            VendorInvoiceNo = (string)jObject.SelectToken("vendorInvoiceNo"),
            VendorName = (string)jObject.SelectToken("vendorName"),
            VoucherDate = ((DateTime?)jObject.SelectToken("voucherDate"))?.ToLocalTime(),
            VoucherNumber = (string)jObject.SelectToken("voucherNumber"),
            BatchVoucherBatchedVoucherBatchedVoucherLines = jObject.SelectToken("batchedVoucherLines")?.Where(jt => jt.HasValues).Select(jt => BatchVoucherBatchedVoucherBatchedVoucherLine.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("accountingCode", AccountingCode),
            new JProperty("amount", Amount),
            new JProperty("disbursementNumber", DisbursementNumber),
            new JProperty("disbursementDate", DisbursementDate?.ToUniversalTime()),
            new JProperty("disbursementAmount", DisbursementAmount),
            new JProperty("enclosureNeeded", EnclosureNeeded),
            new JProperty("exchangeRate", ExchangeRate),
            new JProperty("folioInvoiceNo", FolioInvoiceNo),
            new JProperty("invoiceCurrency", InvoiceCurrency),
            new JProperty("invoiceNote", InvoiceNote),
            new JProperty("status", Status),
            new JProperty("systemCurrency", SystemCurrency),
            new JProperty("type", Type),
            new JProperty("vendorInvoiceNo", VendorInvoiceNo),
            new JProperty("vendorName", VendorName),
            new JProperty("voucherDate", VoucherDate?.ToUniversalTime()),
            new JProperty("voucherNumber", VoucherNumber),
            new JProperty("batchedVoucherLines", BatchVoucherBatchedVoucherBatchedVoucherLines?.Select(bvbvbvl => bvbvbvl.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
