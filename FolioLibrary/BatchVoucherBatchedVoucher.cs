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
    [DisplayColumn(nameof(Id)), DisplayName("Batch Voucher Batched Vouchers"), JsonConverter(typeof(JsonPathJsonConverter<BatchVoucherBatchedVoucher>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_voucher_batched_vouchers", Schema = "uc")]
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

        [Column("account_no"), Display(Name = "Account Number", Order = 5), JsonProperty("accountNo"), StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 6), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Column("disbursement_number"), Display(Name = "Disbursement Number", Order = 7), JsonProperty("disbursementNumber"), StringLength(1024)]
        public virtual string DisbursementNumber { get; set; }

        [Column("disbursement_date"), DataType(DataType.Date), Display(Name = "Disbursement Date", Order = 8), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("disbursementDate")]
        public virtual DateTime? DisbursementDate { get; set; }

        [Column("disbursement_amount"), DataType(DataType.Currency), Display(Name = "Disbursement Amount", Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("disbursementAmount")]
        public virtual decimal? DisbursementAmount { get; set; }

        [Column("enclosure_needed"), Display(Name = "Enclosure Needed", Order = 10), JsonProperty("enclosureNeeded")]
        public virtual bool? EnclosureNeeded { get; set; }

        [Column("exchange_rate"), Display(Name = "Exchange Rate", Order = 11), JsonProperty("exchangeRate")]
        public virtual decimal? ExchangeRate { get; set; }

        [Column("folio_invoice_no"), Display(Name = "Folio Invoice No", Order = 12), JsonProperty("folioInvoiceNo"), Required, StringLength(1024)]
        public virtual string FolioInvoiceNo { get; set; }

        [Column("invoice_currency"), Display(Name = "Invoice Currency", Order = 13), JsonProperty("invoiceCurrency"), Required, StringLength(1024)]
        public virtual string InvoiceCurrency { get; set; }

        [Column("invoice_note"), Display(Name = "Invoice Note", Order = 14), JsonProperty("invoiceNote"), StringLength(1024)]
        public virtual string InvoiceNote { get; set; }

        [Column("status"), Display(Order = 15), JsonProperty("status"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("system_currency"), Display(Name = "System Currency", Order = 16), JsonProperty("systemCurrency"), Required, StringLength(1024)]
        public virtual string SystemCurrency { get; set; }

        [Column("type"), Display(Order = 17), JsonProperty("type"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("vendor_invoice_no"), Display(Name = "Vendor Invoice No", Order = 18), JsonProperty("vendorInvoiceNo"), Required, StringLength(1024)]
        public virtual string VendorInvoiceNo { get; set; }

        [Column("vendor_name"), Display(Name = "Vendor Name", Order = 19), JsonProperty("vendorName"), Required, StringLength(1024)]
        public virtual string VendorName { get; set; }

        [Column("voucher_date"), DataType(DataType.Date), Display(Name = "Voucher Date", Order = 20), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("voucherDate"), Required]
        public virtual DateTime? VoucherDate { get; set; }

        [Column("voucher_number"), Display(Name = "Voucher Number", Order = 21), JsonProperty("voucherNumber"), Required, StringLength(1024)]
        public virtual string VoucherNumber { get; set; }

        [Column("vendor_address_address_line1"), Display(Name = "Vendor Street Address 1", Order = 22), JsonProperty("vendorAddress.addressLine1"), StringLength(1024)]
        public virtual string VendorStreetAddress1 { get; set; }

        [Column("vendor_address_address_line2"), Display(Name = "Vendor Street Address 2", Order = 23), JsonProperty("vendorAddress.addressLine2"), StringLength(1024)]
        public virtual string VendorStreetAddress2 { get; set; }

        [Column("vendor_address_city"), Display(Name = "Vendor City", Order = 24), JsonProperty("vendorAddress.city"), StringLength(1024)]
        public virtual string VendorCity { get; set; }

        [Column("vendor_address_state_region"), Display(Name = "Vendor State", Order = 25), JsonProperty("vendorAddress.stateRegion"), StringLength(1024)]
        public virtual string VendorState { get; set; }

        [Column("vendor_address_zip_code"), Display(Name = "Vendor Postal Code", Order = 26), JsonProperty("vendorAddress.zipCode"), RegularExpression(@"^\d{5}(-\d{4})?$"), StringLength(1024)]
        public virtual string VendorPostalCode { get; set; }

        [Column("vendor_address_country"), Display(Name = "Vendor Country Code", Order = 27), JsonProperty("vendorAddress.country"), StringLength(1024)]
        public virtual string VendorCountryCode { get; set; }

        [Display(Name = "Batch Voucher Batched Voucher Batched Voucher Lines", Order = 28), JsonProperty("batchedVoucherLines")]
        public virtual ICollection<BatchVoucherBatchedVoucherBatchedVoucherLine> BatchVoucherBatchedVoucherBatchedVoucherLines { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchVoucherId)} = {BatchVoucherId}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(Amount)} = {Amount}, {nameof(DisbursementNumber)} = {DisbursementNumber}, {nameof(DisbursementDate)} = {DisbursementDate}, {nameof(DisbursementAmount)} = {DisbursementAmount}, {nameof(EnclosureNeeded)} = {EnclosureNeeded}, {nameof(ExchangeRate)} = {ExchangeRate}, {nameof(FolioInvoiceNo)} = {FolioInvoiceNo}, {nameof(InvoiceCurrency)} = {InvoiceCurrency}, {nameof(InvoiceNote)} = {InvoiceNote}, {nameof(Status)} = {Status}, {nameof(SystemCurrency)} = {SystemCurrency}, {nameof(Type)} = {Type}, {nameof(VendorInvoiceNo)} = {VendorInvoiceNo}, {nameof(VendorName)} = {VendorName}, {nameof(VoucherDate)} = {VoucherDate}, {nameof(VoucherNumber)} = {VoucherNumber}, {nameof(VendorStreetAddress1)} = {VendorStreetAddress1}, {nameof(VendorStreetAddress2)} = {VendorStreetAddress2}, {nameof(VendorCity)} = {VendorCity}, {nameof(VendorState)} = {VendorState}, {nameof(VendorPostalCode)} = {VendorPostalCode}, {nameof(VendorCountryCode)} = {VendorCountryCode}, {nameof(BatchVoucherBatchedVoucherBatchedVoucherLines)} = {(BatchVoucherBatchedVoucherBatchedVoucherLines != null ? $"{{ {string.Join(", ", BatchVoucherBatchedVoucherBatchedVoucherLines)} }}" : "")} }}";

        public static BatchVoucherBatchedVoucher FromJObject(JObject jObject) => jObject != null ? new BatchVoucherBatchedVoucher
        {
            AccountingCode = (string)jObject.SelectToken("accountingCode"),
            AccountNumber = (string)jObject.SelectToken("accountNo"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            DisbursementNumber = (string)jObject.SelectToken("disbursementNumber"),
            DisbursementDate = (DateTime?)jObject.SelectToken("disbursementDate"),
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
            VoucherDate = (DateTime?)jObject.SelectToken("voucherDate"),
            VoucherNumber = (string)jObject.SelectToken("voucherNumber"),
            VendorStreetAddress1 = (string)jObject.SelectToken("vendorAddress.addressLine1"),
            VendorStreetAddress2 = (string)jObject.SelectToken("vendorAddress.addressLine2"),
            VendorCity = (string)jObject.SelectToken("vendorAddress.city"),
            VendorState = (string)jObject.SelectToken("vendorAddress.stateRegion"),
            VendorPostalCode = (string)jObject.SelectToken("vendorAddress.zipCode"),
            VendorCountryCode = (string)jObject.SelectToken("vendorAddress.country"),
            BatchVoucherBatchedVoucherBatchedVoucherLines = jObject.SelectToken("batchedVoucherLines")?.Where(jt => jt.HasValues).Select(jt => BatchVoucherBatchedVoucherBatchedVoucherLine.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("accountingCode", AccountingCode),
            new JProperty("accountNo", AccountNumber),
            new JProperty("amount", Amount),
            new JProperty("disbursementNumber", DisbursementNumber),
            new JProperty("disbursementDate", DisbursementDate),
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
            new JProperty("voucherDate", VoucherDate),
            new JProperty("voucherNumber", VoucherNumber),
            new JProperty("vendorAddress", new JObject(
                new JProperty("addressLine1", VendorStreetAddress1),
                new JProperty("addressLine2", VendorStreetAddress2),
                new JProperty("city", VendorCity),
                new JProperty("stateRegion", VendorState),
                new JProperty("zipCode", VendorPostalCode),
                new JProperty("country", VendorCountryCode))),
            new JProperty("batchedVoucherLines", BatchVoucherBatchedVoucherBatchedVoucherLines?.Select(bvbvbvl => bvbvbvl.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
