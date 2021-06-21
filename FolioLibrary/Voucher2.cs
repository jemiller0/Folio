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
    // uc.vouchers -> diku_mod_invoice_storage.vouchers
    // Voucher2 -> Voucher
    [DisplayColumn(nameof(Number)), DisplayName("Vouchers"), JsonConverter(typeof(JsonPathJsonConverter<Voucher2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("vouchers", Schema = "uc")]
    public partial class Voucher2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Voucher.json")))
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

        [Column("account_no"), Display(Name = "Account Number", Order = 3), JsonProperty("accountNo"), StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 4), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Display(Name = "Batch Group", Order = 5)]
        public virtual BatchGroup2 BatchGroup { get; set; }

        [Column("batch_group_id"), Display(Name = "Batch Group", Order = 6), JsonProperty("batchGroupId"), Required]
        public virtual Guid? BatchGroupId { get; set; }

        [Column("disbursement_number"), Display(Name = "Disbursement Number", Order = 7), JsonProperty("disbursementNumber"), StringLength(1024)]
        public virtual string DisbursementNumber { get; set; }

        [Column("disbursement_date"), DataType(DataType.Date), Display(Name = "Disbursement Date", Order = 8), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("disbursementDate")]
        public virtual DateTime? DisbursementDate { get; set; }

        [Column("disbursement_amount"), DataType(DataType.Currency), Display(Name = "Disbursement Amount", Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("disbursementAmount")]
        public virtual decimal? DisbursementAmount { get; set; }

        [Column("enclosure_needed"), Display(Name = "Enclosure Needed", Order = 10), JsonProperty("enclosureNeeded")]
        public virtual bool? EnclosureNeeded { get; set; }

        [Column("invoice_currency"), Display(Name = "Invoice Currency", Order = 11), JsonProperty("invoiceCurrency"), Required, StringLength(1024)]
        public virtual string InvoiceCurrency { get; set; }

        [Display(Order = 12)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("invoice_id"), Display(Name = "Invoice", Order = 13), JsonProperty("invoiceId"), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Column("exchange_rate"), Display(Name = "Exchange Rate", Order = 14), JsonProperty("exchangeRate")]
        public virtual decimal? ExchangeRate { get; set; }

        [Column("export_to_accounting"), Display(Name = "Export To Accounting", Order = 15), JsonProperty("exportToAccounting")]
        public virtual bool? ExportToAccounting { get; set; }

        [Column("status"), Display(Order = 16), JsonProperty("status"), RegularExpression(@"^(Awaiting payment|Paid)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("system_currency"), Display(Name = "System Currency", Order = 17), JsonProperty("systemCurrency"), Required, StringLength(1024)]
        public virtual string SystemCurrency { get; set; }

        [Column("type"), Display(Order = 18), JsonProperty("type"), RegularExpression(@"^(Payment|Pre-payment|Credit|Voucher)$"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("voucher_date"), DataType(DataType.Date), Display(Name = "Voucher Date", Order = 19), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("voucherDate")]
        public virtual DateTime? VoucherDate { get; set; }

        [Column("voucher_number"), Display(Order = 20), JsonProperty("voucherNumber"), Required, StringLength(1024)]
        public virtual string Number { get; set; }

        [Display(Order = 21)]
        public virtual Organization2 Vendor { get; set; }

        [Column("vendor_id"), Display(Name = "Vendor", Order = 22), JsonProperty("vendorId")]
        public virtual Guid? VendorId { get; set; }

        [Column("vendor_address_address_line1"), Display(Name = "Vendor Street Address 1", Order = 23), JsonProperty("vendorAddress.addressLine1"), StringLength(1024)]
        public virtual string VendorStreetAddress1 { get; set; }

        [Column("vendor_address_address_line2"), Display(Name = "Vendor Street Address 2", Order = 24), JsonProperty("vendorAddress.addressLine2"), StringLength(1024)]
        public virtual string VendorStreetAddress2 { get; set; }

        [Column("vendor_address_city"), Display(Name = "Vendor City", Order = 25), JsonProperty("vendorAddress.city"), StringLength(1024)]
        public virtual string VendorCity { get; set; }

        [Column("vendor_address_state_region"), Display(Name = "Vendor State", Order = 26), JsonProperty("vendorAddress.stateRegion"), StringLength(1024)]
        public virtual string VendorState { get; set; }

        [Column("vendor_address_zip_code"), Display(Name = "Vendor Postal Code", Order = 27), JsonProperty("vendorAddress.zipCode"), RegularExpression(@"^\d{5}(-\d{4})?$"), StringLength(1024)]
        public virtual string VendorPostalCode { get; set; }

        [Column("vendor_address_country"), Display(Name = "Vendor Country Code", Order = 28), JsonProperty("vendorAddress.country"), StringLength(1024)]
        public virtual string VendorCountryCode { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 29), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 30), InverseProperty("Voucher2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 31), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 33), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 34), InverseProperty("Voucher2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 35), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Voucher), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 37), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Voucher Acquisitions Units", Order = 38), JsonConverter(typeof(ArrayJsonConverter<List<VoucherAcquisitionsUnit>, VoucherAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<VoucherAcquisitionsUnit> VoucherAcquisitionsUnits { get; set; }

        [Display(Name = "Voucher Items", Order = 39)]
        public virtual ICollection<VoucherItem2> VoucherItem2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(Amount)} = {Amount}, {nameof(BatchGroupId)} = {BatchGroupId}, {nameof(DisbursementNumber)} = {DisbursementNumber}, {nameof(DisbursementDate)} = {DisbursementDate}, {nameof(DisbursementAmount)} = {DisbursementAmount}, {nameof(EnclosureNeeded)} = {EnclosureNeeded}, {nameof(InvoiceCurrency)} = {InvoiceCurrency}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(ExchangeRate)} = {ExchangeRate}, {nameof(ExportToAccounting)} = {ExportToAccounting}, {nameof(Status)} = {Status}, {nameof(SystemCurrency)} = {SystemCurrency}, {nameof(Type)} = {Type}, {nameof(VoucherDate)} = {VoucherDate}, {nameof(Number)} = {Number}, {nameof(VendorId)} = {VendorId}, {nameof(VendorStreetAddress1)} = {VendorStreetAddress1}, {nameof(VendorStreetAddress2)} = {VendorStreetAddress2}, {nameof(VendorCity)} = {VendorCity}, {nameof(VendorState)} = {VendorState}, {nameof(VendorPostalCode)} = {VendorPostalCode}, {nameof(VendorCountryCode)} = {VendorCountryCode}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(VoucherAcquisitionsUnits)} = {(VoucherAcquisitionsUnits != null ? $"{{ {string.Join(", ", VoucherAcquisitionsUnits)} }}" : "")} }}";

        public static Voucher2 FromJObject(JObject jObject) => jObject != null ? new Voucher2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            AccountingCode = (string)jObject.SelectToken("accountingCode"),
            AccountNumber = (string)jObject.SelectToken("accountNo"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            BatchGroupId = (Guid?)jObject.SelectToken("batchGroupId"),
            DisbursementNumber = (string)jObject.SelectToken("disbursementNumber"),
            DisbursementDate = ((DateTime?)jObject.SelectToken("disbursementDate"))?.ToLocalTime(),
            DisbursementAmount = (decimal?)jObject.SelectToken("disbursementAmount"),
            EnclosureNeeded = (bool?)jObject.SelectToken("enclosureNeeded"),
            InvoiceCurrency = (string)jObject.SelectToken("invoiceCurrency"),
            InvoiceId = (Guid?)jObject.SelectToken("invoiceId"),
            ExchangeRate = (decimal?)jObject.SelectToken("exchangeRate"),
            ExportToAccounting = (bool?)jObject.SelectToken("exportToAccounting"),
            Status = (string)jObject.SelectToken("status"),
            SystemCurrency = (string)jObject.SelectToken("systemCurrency"),
            Type = (string)jObject.SelectToken("type"),
            VoucherDate = ((DateTime?)jObject.SelectToken("voucherDate"))?.ToLocalTime(),
            Number = (string)jObject.SelectToken("voucherNumber"),
            VendorId = (Guid?)jObject.SelectToken("vendorId"),
            VendorStreetAddress1 = (string)jObject.SelectToken("vendorAddress.addressLine1"),
            VendorStreetAddress2 = (string)jObject.SelectToken("vendorAddress.addressLine2"),
            VendorCity = (string)jObject.SelectToken("vendorAddress.city"),
            VendorState = (string)jObject.SelectToken("vendorAddress.stateRegion"),
            VendorPostalCode = (string)jObject.SelectToken("vendorAddress.zipCode"),
            VendorCountryCode = (string)jObject.SelectToken("vendorAddress.country"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            VoucherAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => VoucherAcquisitionsUnit.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("accountingCode", AccountingCode),
            new JProperty("accountNo", AccountNumber),
            new JProperty("amount", Amount),
            new JProperty("batchGroupId", BatchGroupId),
            new JProperty("disbursementNumber", DisbursementNumber),
            new JProperty("disbursementDate", DisbursementDate?.ToUniversalTime()),
            new JProperty("disbursementAmount", DisbursementAmount),
            new JProperty("enclosureNeeded", EnclosureNeeded),
            new JProperty("invoiceCurrency", InvoiceCurrency),
            new JProperty("invoiceId", InvoiceId),
            new JProperty("exchangeRate", ExchangeRate),
            new JProperty("exportToAccounting", ExportToAccounting),
            new JProperty("status", Status),
            new JProperty("systemCurrency", SystemCurrency),
            new JProperty("type", Type),
            new JProperty("voucherDate", VoucherDate?.ToUniversalTime()),
            new JProperty("voucherNumber", Number),
            new JProperty("vendorId", VendorId),
            new JProperty("vendorAddress", new JObject(
                new JProperty("addressLine1", VendorStreetAddress1),
                new JProperty("addressLine2", VendorStreetAddress2),
                new JProperty("city", VendorCity),
                new JProperty("stateRegion", VendorState),
                new JProperty("zipCode", VendorPostalCode),
                new JProperty("country", VendorCountryCode))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", VoucherAcquisitionsUnits?.Select(vau => vau.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
