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
    // uc.organizations -> uchicago_mod_organizations_storage.organizations
    // Organization2 -> Organization
    [DisplayColumn(nameof(Name)), DisplayName("Organizations"), JsonConverter(typeof(JsonPathJsonConverter<Organization2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organizations", Schema = "uc")]
    public partial class Organization2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Organization.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("code"), Display(Order = 3), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 4), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("export_to_accounting"), Display(Name = "Export To Accounting", Order = 5), JsonProperty("exportToAccounting")]
        public virtual bool? ExportToAccounting { get; set; }

        [Column("status"), Display(Order = 6), JsonProperty("status"), RegularExpression(@"^(Active|Inactive|Pending)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("language"), Display(Order = 7), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("erp_code"), Display(Name = "Accounting Code", Order = 8), JsonProperty("erpCode"), StringLength(1024)]
        public virtual string AccountingCode { get; set; }

        [Column("payment_method"), Display(Name = "Payment Method", Order = 9), JsonProperty("paymentMethod"), StringLength(1024)]
        public virtual string PaymentMethod { get; set; }

        [Column("access_provider"), Display(Name = "Access Provider", Order = 10), JsonProperty("accessProvider")]
        public virtual bool? AccessProvider { get; set; }

        [Column("governmental"), Display(Order = 11), JsonProperty("governmental")]
        public virtual bool? Governmental { get; set; }

        [Column("licensor"), Display(Order = 12), JsonProperty("licensor")]
        public virtual bool? Licensor { get; set; }

        [Column("material_supplier"), Display(Name = "Material Supplier", Order = 13), JsonProperty("materialSupplier")]
        public virtual bool? MaterialSupplier { get; set; }

        [Column("claiming_interval"), Display(Name = "Claiming Interval", Order = 14), JsonProperty("claimingInterval")]
        public virtual int? ClaimingInterval { get; set; }

        [Column("discount_percent"), Display(Name = "Discount Percent", Order = 15), JsonProperty("discountPercent")]
        public virtual decimal? DiscountPercent { get; set; }

        [Column("expected_activation_interval"), Display(Name = "Expected Activation Interval", Order = 16), JsonProperty("expectedActivationInterval")]
        public virtual int? ExpectedActivationInterval { get; set; }

        [Column("expected_invoice_interval"), Display(Name = "Expected Invoice Interval", Order = 17), JsonProperty("expectedInvoiceInterval")]
        public virtual int? ExpectedInvoiceInterval { get; set; }

        [Column("renewal_activation_interval"), Display(Name = "Renewal Activation Interval", Order = 18), JsonProperty("renewalActivationInterval")]
        public virtual int? RenewalActivationInterval { get; set; }

        [Column("subscription_interval"), Display(Name = "Subscription Interval", Order = 19), JsonProperty("subscriptionInterval")]
        public virtual int? SubscriptionInterval { get; set; }

        [Column("expected_receipt_interval"), Display(Name = "Expected Receipt Interval", Order = 20), JsonProperty("expectedReceiptInterval")]
        public virtual int? ExpectedReceiptInterval { get; set; }

        [Column("tax_id"), Display(Name = "Tax Id", Order = 21), JsonProperty("taxId"), StringLength(1024)]
        public virtual string TaxId { get; set; }

        [Column("liable_for_vat"), Display(Name = "Liable For Vat", Order = 22), JsonProperty("liableForVat")]
        public virtual bool? LiableForVat { get; set; }

        [Column("tax_percentage"), Display(Name = "Tax Percentage", Order = 23), JsonProperty("taxPercentage")]
        public virtual decimal? TaxPercentage { get; set; }

        [Column("edi_vendor_edi_code"), Display(Name = "EDI Vendor EDI Code", Order = 24), JsonProperty("edi.vendorEdiCode"), StringLength(1024)]
        public virtual string EdiVendorEdiCode { get; set; }

        [Column("edi_vendor_edi_type"), Display(Name = "EDI Vendor EDI Type", Order = 25), JsonProperty("edi.vendorEdiType"), RegularExpression(@"^(014/EAN|31B/US-SAN|091/Vendor-assigned|092/Customer-assigned)$"), StringLength(1024)]
        public virtual string EdiVendorEdiType { get; set; }

        [Column("edi_lib_edi_code"), Display(Name = "EDI Lib EDI Code", Order = 26), JsonProperty("edi.libEdiCode"), StringLength(1024)]
        public virtual string EdiLibEdiCode { get; set; }

        [Column("edi_lib_edi_type"), Display(Name = "EDI Lib EDI Type", Order = 27), JsonProperty("edi.libEdiType"), RegularExpression(@"^(014/EAN|31B/US-SAN|091/Vendor-assigned|092/Customer-assigned)$"), StringLength(1024)]
        public virtual string EdiLibEdiType { get; set; }

        [Column("edi_prorate_tax"), Display(Name = "EDI Prorate Tax", Order = 28), JsonProperty("edi.prorateTax")]
        public virtual bool? EdiProrateTax { get; set; }

        [Column("edi_prorate_fees"), Display(Name = "EDI Prorate Fees", Order = 29), JsonProperty("edi.prorateFees")]
        public virtual bool? EdiProrateFees { get; set; }

        [Column("edi_naming_convention"), Display(Name = "EDI Naming Convention", Order = 30), JsonProperty("edi.ediNamingConvention"), StringLength(1024)]
        public virtual string EdiNamingConvention { get; set; }

        [Column("edi_send_acct_num"), Display(Name = "EDI Send Acct Num", Order = 31), JsonProperty("edi.sendAcctNum")]
        public virtual bool? EdiSendAcctNum { get; set; }

        [Column("edi_support_order"), Display(Name = "EDI Support Order", Order = 32), JsonProperty("edi.supportOrder")]
        public virtual bool? EdiSupportOrder { get; set; }

        [Column("edi_support_invoice"), Display(Name = "EDI Support Invoice", Order = 33), JsonProperty("edi.supportInvoice")]
        public virtual bool? EdiSupportInvoice { get; set; }

        [Column("edi_notes"), Display(Name = "EDI Notes", Order = 34), JsonProperty("edi.notes"), StringLength(1024)]
        public virtual string EdiNotes { get; set; }

        [Column("edi_ftp_ftp_format"), Display(Name = "EDI FTP FTP Format", Order = 35), JsonProperty("edi.ediFtp.ftpFormat"), RegularExpression(@"^(SFTP|FTP)$"), StringLength(1024)]
        public virtual string EdiFtpFtpFormat { get; set; }

        [Column("edi_ftp_server_address"), Display(Name = "EDI FTP Server Address", Order = 36), JsonProperty("edi.ediFtp.serverAddress"), StringLength(1024)]
        public virtual string EdiFtpServerAddress { get; set; }

        [Column("edi_ftp_username"), Display(Name = "EDI FTP Username", Order = 37), JsonProperty("edi.ediFtp.username"), StringLength(1024)]
        public virtual string EdiFtpUsername { get; set; }

        [Column("edi_ftp_password"), DataType(DataType.Password), Display(Name = "EDI FTP Password", Order = 38), JsonProperty("edi.ediFtp.password"), StringLength(1024)]
        public virtual string EdiFtpPassword { get; set; }

        [Column("edi_ftp_ftp_mode"), Display(Name = "EDI FTP FTP Mode", Order = 39), JsonProperty("edi.ediFtp.ftpMode"), RegularExpression(@"^(ASCII|Binary)$"), StringLength(1024)]
        public virtual string EdiFtpFtpMode { get; set; }

        [Column("edi_ftp_ftp_conn_mode"), Display(Name = "EDI FTP FTP Conn Mode", Order = 40), JsonProperty("edi.ediFtp.ftpConnMode"), RegularExpression(@"^(Active|Passive)$"), StringLength(1024)]
        public virtual string EdiFtpFtpConnMode { get; set; }

        [Column("edi_ftp_ftp_port"), Display(Name = "EDI FTP FTP Port", Order = 41), JsonProperty("edi.ediFtp.ftpPort")]
        public virtual int? EdiFtpFtpPort { get; set; }

        [Column("edi_ftp_order_directory"), Display(Name = "EDI FTP Order Directory", Order = 42), JsonProperty("edi.ediFtp.orderDirectory"), StringLength(1024)]
        public virtual string EdiFtpOrderDirectory { get; set; }

        [Column("edi_ftp_invoice_directory"), Display(Name = "EDI FTP Invoice Directory", Order = 43), JsonProperty("edi.ediFtp.invoiceDirectory"), StringLength(1024)]
        public virtual string EdiFtpInvoiceDirectory { get; set; }

        [Column("edi_ftp_notes"), Display(Name = "EDI FTP Notes", Order = 44), JsonProperty("edi.ediFtp.notes"), StringLength(1024)]
        public virtual string EdiFtpNotes { get; set; }

        [Column("edi_job_schedule_edi"), Display(Name = "EDI Job Schedule Edi", Order = 45), JsonProperty("edi.ediJob.scheduleEdi")]
        public virtual bool? EdiJobScheduleEdi { get; set; }

        [Column("edi_job_scheduling_date"), DataType(DataType.Date), Display(Name = "EDI Job Scheduling Date", Order = 46), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("edi.ediJob.schedulingDate")]
        public virtual DateTime? EdiJobSchedulingDate { get; set; }

        [Column("edi_job_time"), Display(Name = "EDI Job Time", Order = 47), JsonProperty("edi.ediJob.time"), StringLength(1024)]
        public virtual string EdiJobTime { get; set; }

        [Column("edi_job_is_monday"), Display(Name = "EDI Job Is Monday", Order = 48), JsonProperty("edi.ediJob.isMonday")]
        public virtual bool? EdiJobIsMonday { get; set; }

        [Column("edi_job_is_tuesday"), Display(Name = "EDI Job Is Tuesday", Order = 49), JsonProperty("edi.ediJob.isTuesday")]
        public virtual bool? EdiJobIsTuesday { get; set; }

        [Column("edi_job_is_wednesday"), Display(Name = "EDI Job Is Wednesday", Order = 50), JsonProperty("edi.ediJob.isWednesday")]
        public virtual bool? EdiJobIsWednesday { get; set; }

        [Column("edi_job_is_thursday"), Display(Name = "EDI Job Is Thursday", Order = 51), JsonProperty("edi.ediJob.isThursday")]
        public virtual bool? EdiJobIsThursday { get; set; }

        [Column("edi_job_is_friday"), Display(Name = "EDI Job Is Friday", Order = 52), JsonProperty("edi.ediJob.isFriday")]
        public virtual bool? EdiJobIsFriday { get; set; }

        [Column("edi_job_is_saturday"), Display(Name = "EDI Job Is Saturday", Order = 53), JsonProperty("edi.ediJob.isSaturday")]
        public virtual bool? EdiJobIsSaturday { get; set; }

        [Column("edi_job_is_sunday"), Display(Name = "EDI Job Is Sunday", Order = 54), JsonProperty("edi.ediJob.isSunday")]
        public virtual bool? EdiJobIsSunday { get; set; }

        [Column("edi_job_send_to_emails"), Display(Name = "EDI Job Send To Emails", Order = 55), JsonProperty("edi.ediJob.sendToEmails"), StringLength(1024)]
        public virtual string EdiJobSendToEmails { get; set; }

        [Column("edi_job_notify_all_edi"), Display(Name = "EDI Job Notify All Edi", Order = 56), JsonProperty("edi.ediJob.notifyAllEdi")]
        public virtual bool? EdiJobNotifyAllEdi { get; set; }

        [Column("edi_job_notify_invoice_only"), Display(Name = "EDI Job Notify Invoice Only", Order = 57), JsonProperty("edi.ediJob.notifyInvoiceOnly")]
        public virtual bool? EdiJobNotifyInvoiceOnly { get; set; }

        [Column("edi_job_notify_error_only"), Display(Name = "EDI Job Notify Error Only", Order = 58), JsonProperty("edi.ediJob.notifyErrorOnly")]
        public virtual bool? EdiJobNotifyErrorOnly { get; set; }

        [Column("edi_job_scheduling_notes"), Display(Name = "EDI Job Scheduling Notes", Order = 59), JsonProperty("edi.ediJob.schedulingNotes"), StringLength(1024)]
        public virtual string EdiJobSchedulingNotes { get; set; }

        [Column("is_vendor"), Display(Name = "Is Vendor", Order = 60), JsonProperty("isVendor")]
        public virtual bool? IsVendor { get; set; }

        [Column("san_code"), Display(Name = "San Code", Order = 61), JsonProperty("sanCode"), StringLength(1024)]
        public virtual string SanCode { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 62), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 63), InverseProperty("Organization2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 64), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 66), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 67), InverseProperty("Organization2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 68), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Organization), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 70), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Order = 71), JsonConverter(typeof(ArrayJsonConverter<List<Currency>, Currency>), "Content"), JsonProperty("vendorCurrencies")]
        public virtual ICollection<Currency> Currencies { get; set; }

        [Display(Name = "Invoices", Order = 72)]
        public virtual ICollection<Invoice2> Invoice2s { get; set; }

        [Display(Name = "Orders", Order = 73)]
        public virtual ICollection<Order2> Order2s { get; set; }

        [Display(Name = "Order Items", Order = 74)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Order Items 1", Order = 75)]
        public virtual ICollection<OrderItem2> OrderItem2s1 { get; set; }

        [Display(Name = "Organization Accounts", Order = 76), JsonProperty("accounts")]
        public virtual ICollection<OrganizationAccount> OrganizationAccounts { get; set; }

        [Display(Name = "Organization Acquisitions Units", Order = 77), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationAcquisitionsUnit>, OrganizationAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<OrganizationAcquisitionsUnit> OrganizationAcquisitionsUnits { get; set; }

        [Display(Name = "Organization Addresses", Order = 78), JsonProperty("addresses")]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

        [Display(Name = "Organization Agreements", Order = 79), JsonProperty("agreements")]
        public virtual ICollection<OrganizationAgreement> OrganizationAgreements { get; set; }

        [Display(Name = "Organization Aliases", Order = 80), JsonProperty("aliases")]
        public virtual ICollection<OrganizationAlias> OrganizationAliases { get; set; }

        [Display(Name = "Organization Changelogs", Order = 81), JsonProperty("changelogs")]
        public virtual ICollection<OrganizationChangelog> OrganizationChangelogs { get; set; }

        [Display(Name = "Organization Contacts", Order = 82), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationContact>, OrganizationContact>), "ContactId"), JsonProperty("contacts")]
        public virtual ICollection<OrganizationContact> OrganizationContacts { get; set; }

        [Display(Name = "Organization Emails", Order = 83), JsonProperty("emails")]
        public virtual ICollection<OrganizationEmail> OrganizationEmails { get; set; }

        [Display(Name = "Organization Interfaces", Order = 84), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationInterface>, OrganizationInterface>), "InterfaceId"), JsonProperty("interfaces")]
        public virtual ICollection<OrganizationInterface> OrganizationInterfaces { get; set; }

        [Display(Name = "Organization Phone Numbers", Order = 85), JsonProperty("phoneNumbers")]
        public virtual ICollection<OrganizationPhoneNumber> OrganizationPhoneNumbers { get; set; }

        [Display(Name = "Organization Tags", Order = 86), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationTag>, OrganizationTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<OrganizationTag> OrganizationTags { get; set; }

        [Display(Name = "Organization URLs", Order = 87), JsonProperty("urls")]
        public virtual ICollection<OrganizationUrl> OrganizationUrls { get; set; }

        [Display(Name = "Vouchers", Order = 88)]
        public virtual ICollection<Voucher2> Voucher2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(ExportToAccounting)} = {ExportToAccounting}, {nameof(Status)} = {Status}, {nameof(Language)} = {Language}, {nameof(AccountingCode)} = {AccountingCode}, {nameof(PaymentMethod)} = {PaymentMethod}, {nameof(AccessProvider)} = {AccessProvider}, {nameof(Governmental)} = {Governmental}, {nameof(Licensor)} = {Licensor}, {nameof(MaterialSupplier)} = {MaterialSupplier}, {nameof(ClaimingInterval)} = {ClaimingInterval}, {nameof(DiscountPercent)} = {DiscountPercent}, {nameof(ExpectedActivationInterval)} = {ExpectedActivationInterval}, {nameof(ExpectedInvoiceInterval)} = {ExpectedInvoiceInterval}, {nameof(RenewalActivationInterval)} = {RenewalActivationInterval}, {nameof(SubscriptionInterval)} = {SubscriptionInterval}, {nameof(ExpectedReceiptInterval)} = {ExpectedReceiptInterval}, {nameof(TaxId)} = {TaxId}, {nameof(LiableForVat)} = {LiableForVat}, {nameof(TaxPercentage)} = {TaxPercentage}, {nameof(EdiVendorEdiCode)} = {EdiVendorEdiCode}, {nameof(EdiVendorEdiType)} = {EdiVendorEdiType}, {nameof(EdiLibEdiCode)} = {EdiLibEdiCode}, {nameof(EdiLibEdiType)} = {EdiLibEdiType}, {nameof(EdiProrateTax)} = {EdiProrateTax}, {nameof(EdiProrateFees)} = {EdiProrateFees}, {nameof(EdiNamingConvention)} = {EdiNamingConvention}, {nameof(EdiSendAcctNum)} = {EdiSendAcctNum}, {nameof(EdiSupportOrder)} = {EdiSupportOrder}, {nameof(EdiSupportInvoice)} = {EdiSupportInvoice}, {nameof(EdiNotes)} = {EdiNotes}, {nameof(EdiFtpFtpFormat)} = {EdiFtpFtpFormat}, {nameof(EdiFtpServerAddress)} = {EdiFtpServerAddress}, {nameof(EdiFtpUsername)} = {EdiFtpUsername}, {nameof(EdiFtpPassword)} = {EdiFtpPassword}, {nameof(EdiFtpFtpMode)} = {EdiFtpFtpMode}, {nameof(EdiFtpFtpConnMode)} = {EdiFtpFtpConnMode}, {nameof(EdiFtpFtpPort)} = {EdiFtpFtpPort}, {nameof(EdiFtpOrderDirectory)} = {EdiFtpOrderDirectory}, {nameof(EdiFtpInvoiceDirectory)} = {EdiFtpInvoiceDirectory}, {nameof(EdiFtpNotes)} = {EdiFtpNotes}, {nameof(EdiJobScheduleEdi)} = {EdiJobScheduleEdi}, {nameof(EdiJobSchedulingDate)} = {EdiJobSchedulingDate}, {nameof(EdiJobTime)} = {EdiJobTime}, {nameof(EdiJobIsMonday)} = {EdiJobIsMonday}, {nameof(EdiJobIsTuesday)} = {EdiJobIsTuesday}, {nameof(EdiJobIsWednesday)} = {EdiJobIsWednesday}, {nameof(EdiJobIsThursday)} = {EdiJobIsThursday}, {nameof(EdiJobIsFriday)} = {EdiJobIsFriday}, {nameof(EdiJobIsSaturday)} = {EdiJobIsSaturday}, {nameof(EdiJobIsSunday)} = {EdiJobIsSunday}, {nameof(EdiJobSendToEmails)} = {EdiJobSendToEmails}, {nameof(EdiJobNotifyAllEdi)} = {EdiJobNotifyAllEdi}, {nameof(EdiJobNotifyInvoiceOnly)} = {EdiJobNotifyInvoiceOnly}, {nameof(EdiJobNotifyErrorOnly)} = {EdiJobNotifyErrorOnly}, {nameof(EdiJobSchedulingNotes)} = {EdiJobSchedulingNotes}, {nameof(IsVendor)} = {IsVendor}, {nameof(SanCode)} = {SanCode}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(Currencies)} = {(Currencies != null ? $"{{ {string.Join(", ", Currencies)} }}" : "")}, {nameof(OrganizationAccounts)} = {(OrganizationAccounts != null ? $"{{ {string.Join(", ", OrganizationAccounts)} }}" : "")}, {nameof(OrganizationAcquisitionsUnits)} = {(OrganizationAcquisitionsUnits != null ? $"{{ {string.Join(", ", OrganizationAcquisitionsUnits)} }}" : "")}, {nameof(OrganizationAddresses)} = {(OrganizationAddresses != null ? $"{{ {string.Join(", ", OrganizationAddresses)} }}" : "")}, {nameof(OrganizationAgreements)} = {(OrganizationAgreements != null ? $"{{ {string.Join(", ", OrganizationAgreements)} }}" : "")}, {nameof(OrganizationAliases)} = {(OrganizationAliases != null ? $"{{ {string.Join(", ", OrganizationAliases)} }}" : "")}, {nameof(OrganizationChangelogs)} = {(OrganizationChangelogs != null ? $"{{ {string.Join(", ", OrganizationChangelogs)} }}" : "")}, {nameof(OrganizationContacts)} = {(OrganizationContacts != null ? $"{{ {string.Join(", ", OrganizationContacts)} }}" : "")}, {nameof(OrganizationEmails)} = {(OrganizationEmails != null ? $"{{ {string.Join(", ", OrganizationEmails)} }}" : "")}, {nameof(OrganizationInterfaces)} = {(OrganizationInterfaces != null ? $"{{ {string.Join(", ", OrganizationInterfaces)} }}" : "")}, {nameof(OrganizationPhoneNumbers)} = {(OrganizationPhoneNumbers != null ? $"{{ {string.Join(", ", OrganizationPhoneNumbers)} }}" : "")}, {nameof(OrganizationTags)} = {(OrganizationTags != null ? $"{{ {string.Join(", ", OrganizationTags)} }}" : "")}, {nameof(OrganizationUrls)} = {(OrganizationUrls != null ? $"{{ {string.Join(", ", OrganizationUrls)} }}" : "")} }}";

        public static Organization2 FromJObject(JObject jObject) => jObject != null ? new Organization2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            ExportToAccounting = (bool?)jObject.SelectToken("exportToAccounting"),
            Status = (string)jObject.SelectToken("status"),
            Language = (string)jObject.SelectToken("language"),
            AccountingCode = (string)jObject.SelectToken("erpCode"),
            PaymentMethod = (string)jObject.SelectToken("paymentMethod"),
            AccessProvider = (bool?)jObject.SelectToken("accessProvider"),
            Governmental = (bool?)jObject.SelectToken("governmental"),
            Licensor = (bool?)jObject.SelectToken("licensor"),
            MaterialSupplier = (bool?)jObject.SelectToken("materialSupplier"),
            ClaimingInterval = (int?)jObject.SelectToken("claimingInterval"),
            DiscountPercent = (decimal?)jObject.SelectToken("discountPercent"),
            ExpectedActivationInterval = (int?)jObject.SelectToken("expectedActivationInterval"),
            ExpectedInvoiceInterval = (int?)jObject.SelectToken("expectedInvoiceInterval"),
            RenewalActivationInterval = (int?)jObject.SelectToken("renewalActivationInterval"),
            SubscriptionInterval = (int?)jObject.SelectToken("subscriptionInterval"),
            ExpectedReceiptInterval = (int?)jObject.SelectToken("expectedReceiptInterval"),
            TaxId = (string)jObject.SelectToken("taxId"),
            LiableForVat = (bool?)jObject.SelectToken("liableForVat"),
            TaxPercentage = (decimal?)jObject.SelectToken("taxPercentage"),
            EdiVendorEdiCode = (string)jObject.SelectToken("edi.vendorEdiCode"),
            EdiVendorEdiType = (string)jObject.SelectToken("edi.vendorEdiType"),
            EdiLibEdiCode = (string)jObject.SelectToken("edi.libEdiCode"),
            EdiLibEdiType = (string)jObject.SelectToken("edi.libEdiType"),
            EdiProrateTax = (bool?)jObject.SelectToken("edi.prorateTax"),
            EdiProrateFees = (bool?)jObject.SelectToken("edi.prorateFees"),
            EdiNamingConvention = (string)jObject.SelectToken("edi.ediNamingConvention"),
            EdiSendAcctNum = (bool?)jObject.SelectToken("edi.sendAcctNum"),
            EdiSupportOrder = (bool?)jObject.SelectToken("edi.supportOrder"),
            EdiSupportInvoice = (bool?)jObject.SelectToken("edi.supportInvoice"),
            EdiNotes = (string)jObject.SelectToken("edi.notes"),
            EdiFtpFtpFormat = (string)jObject.SelectToken("edi.ediFtp.ftpFormat"),
            EdiFtpServerAddress = (string)jObject.SelectToken("edi.ediFtp.serverAddress"),
            EdiFtpUsername = (string)jObject.SelectToken("edi.ediFtp.username"),
            EdiFtpPassword = (string)jObject.SelectToken("edi.ediFtp.password"),
            EdiFtpFtpMode = (string)jObject.SelectToken("edi.ediFtp.ftpMode"),
            EdiFtpFtpConnMode = (string)jObject.SelectToken("edi.ediFtp.ftpConnMode"),
            EdiFtpFtpPort = (int?)jObject.SelectToken("edi.ediFtp.ftpPort"),
            EdiFtpOrderDirectory = (string)jObject.SelectToken("edi.ediFtp.orderDirectory"),
            EdiFtpInvoiceDirectory = (string)jObject.SelectToken("edi.ediFtp.invoiceDirectory"),
            EdiFtpNotes = (string)jObject.SelectToken("edi.ediFtp.notes"),
            EdiJobScheduleEdi = (bool?)jObject.SelectToken("edi.ediJob.scheduleEdi"),
            EdiJobSchedulingDate = (DateTime?)jObject.SelectToken("edi.ediJob.schedulingDate"),
            EdiJobTime = (string)jObject.SelectToken("edi.ediJob.time"),
            EdiJobIsMonday = (bool?)jObject.SelectToken("edi.ediJob.isMonday"),
            EdiJobIsTuesday = (bool?)jObject.SelectToken("edi.ediJob.isTuesday"),
            EdiJobIsWednesday = (bool?)jObject.SelectToken("edi.ediJob.isWednesday"),
            EdiJobIsThursday = (bool?)jObject.SelectToken("edi.ediJob.isThursday"),
            EdiJobIsFriday = (bool?)jObject.SelectToken("edi.ediJob.isFriday"),
            EdiJobIsSaturday = (bool?)jObject.SelectToken("edi.ediJob.isSaturday"),
            EdiJobIsSunday = (bool?)jObject.SelectToken("edi.ediJob.isSunday"),
            EdiJobSendToEmails = (string)jObject.SelectToken("edi.ediJob.sendToEmails"),
            EdiJobNotifyAllEdi = (bool?)jObject.SelectToken("edi.ediJob.notifyAllEdi"),
            EdiJobNotifyInvoiceOnly = (bool?)jObject.SelectToken("edi.ediJob.notifyInvoiceOnly"),
            EdiJobNotifyErrorOnly = (bool?)jObject.SelectToken("edi.ediJob.notifyErrorOnly"),
            EdiJobSchedulingNotes = (string)jObject.SelectToken("edi.ediJob.schedulingNotes"),
            IsVendor = (bool?)jObject.SelectToken("isVendor"),
            SanCode = (string)jObject.SelectToken("sanCode"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            Currencies = jObject.SelectToken("vendorCurrencies")?.Select(jt => Currency.FromJObject((JValue)jt)).ToArray(),
            OrganizationAccounts = jObject.SelectToken("accounts")?.Where(jt => jt.HasValues).Select(jt => OrganizationAccount.FromJObject((JObject)jt)).ToArray(),
            OrganizationAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Select(jt => OrganizationAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            OrganizationAddresses = jObject.SelectToken("addresses")?.Where(jt => jt.HasValues).Select(jt => OrganizationAddress.FromJObject((JObject)jt)).ToArray(),
            OrganizationAgreements = jObject.SelectToken("agreements")?.Where(jt => jt.HasValues).Select(jt => OrganizationAgreement.FromJObject((JObject)jt)).ToArray(),
            OrganizationAliases = jObject.SelectToken("aliases")?.Where(jt => jt.HasValues).Select(jt => OrganizationAlias.FromJObject((JObject)jt)).ToArray(),
            OrganizationChangelogs = jObject.SelectToken("changelogs")?.Where(jt => jt.HasValues).Select(jt => OrganizationChangelog.FromJObject((JObject)jt)).ToArray(),
            OrganizationContacts = jObject.SelectToken("contacts")?.Select(jt => OrganizationContact.FromJObject((JValue)jt)).ToArray(),
            OrganizationEmails = jObject.SelectToken("emails")?.Where(jt => jt.HasValues).Select(jt => OrganizationEmail.FromJObject((JObject)jt)).ToArray(),
            OrganizationInterfaces = jObject.SelectToken("interfaces")?.Select(jt => OrganizationInterface.FromJObject((JValue)jt)).ToArray(),
            OrganizationPhoneNumbers = jObject.SelectToken("phoneNumbers")?.Where(jt => jt.HasValues).Select(jt => OrganizationPhoneNumber.FromJObject((JObject)jt)).ToArray(),
            OrganizationTags = jObject.SelectToken("tags.tagList")?.Select(jt => OrganizationTag.FromJObject((JValue)jt)).ToArray(),
            OrganizationUrls = jObject.SelectToken("urls")?.Where(jt => jt.HasValues).Select(jt => OrganizationUrl.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("description", Description),
            new JProperty("exportToAccounting", ExportToAccounting),
            new JProperty("status", Status),
            new JProperty("language", Language),
            new JProperty("erpCode", AccountingCode),
            new JProperty("paymentMethod", PaymentMethod),
            new JProperty("accessProvider", AccessProvider),
            new JProperty("governmental", Governmental),
            new JProperty("licensor", Licensor),
            new JProperty("materialSupplier", MaterialSupplier),
            new JProperty("claimingInterval", ClaimingInterval),
            new JProperty("discountPercent", DiscountPercent),
            new JProperty("expectedActivationInterval", ExpectedActivationInterval),
            new JProperty("expectedInvoiceInterval", ExpectedInvoiceInterval),
            new JProperty("renewalActivationInterval", RenewalActivationInterval),
            new JProperty("subscriptionInterval", SubscriptionInterval),
            new JProperty("expectedReceiptInterval", ExpectedReceiptInterval),
            new JProperty("taxId", TaxId),
            new JProperty("liableForVat", LiableForVat),
            new JProperty("taxPercentage", TaxPercentage),
            new JProperty("edi", new JObject(
                new JProperty("vendorEdiCode", EdiVendorEdiCode),
                new JProperty("vendorEdiType", EdiVendorEdiType),
                new JProperty("libEdiCode", EdiLibEdiCode),
                new JProperty("libEdiType", EdiLibEdiType),
                new JProperty("prorateTax", EdiProrateTax),
                new JProperty("prorateFees", EdiProrateFees),
                new JProperty("ediNamingConvention", EdiNamingConvention),
                new JProperty("sendAcctNum", EdiSendAcctNum),
                new JProperty("supportOrder", EdiSupportOrder),
                new JProperty("supportInvoice", EdiSupportInvoice),
                new JProperty("notes", EdiNotes),
                new JProperty("ediFtp", new JObject(
                    new JProperty("ftpFormat", EdiFtpFtpFormat),
                    new JProperty("serverAddress", EdiFtpServerAddress),
                    new JProperty("username", EdiFtpUsername),
                    new JProperty("password", EdiFtpPassword),
                    new JProperty("ftpMode", EdiFtpFtpMode),
                    new JProperty("ftpConnMode", EdiFtpFtpConnMode),
                    new JProperty("ftpPort", EdiFtpFtpPort),
                    new JProperty("orderDirectory", EdiFtpOrderDirectory),
                    new JProperty("invoiceDirectory", EdiFtpInvoiceDirectory),
                    new JProperty("notes", EdiFtpNotes))),
                new JProperty("ediJob", new JObject(
                    new JProperty("scheduleEdi", EdiJobScheduleEdi),
                    new JProperty("schedulingDate", EdiJobSchedulingDate?.ToLocalTime()),
                    new JProperty("time", EdiJobTime),
                    new JProperty("isMonday", EdiJobIsMonday),
                    new JProperty("isTuesday", EdiJobIsTuesday),
                    new JProperty("isWednesday", EdiJobIsWednesday),
                    new JProperty("isThursday", EdiJobIsThursday),
                    new JProperty("isFriday", EdiJobIsFriday),
                    new JProperty("isSaturday", EdiJobIsSaturday),
                    new JProperty("isSunday", EdiJobIsSunday),
                    new JProperty("sendToEmails", EdiJobSendToEmails),
                    new JProperty("notifyAllEdi", EdiJobNotifyAllEdi),
                    new JProperty("notifyInvoiceOnly", EdiJobNotifyInvoiceOnly),
                    new JProperty("notifyErrorOnly", EdiJobNotifyErrorOnly),
                    new JProperty("schedulingNotes", EdiJobSchedulingNotes))))),
            new JProperty("isVendor", IsVendor),
            new JProperty("sanCode", SanCode),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("vendorCurrencies", Currencies?.Select(c => c.ToJObject())),
            new JProperty("accounts", OrganizationAccounts?.Select(oa => oa.ToJObject())),
            new JProperty("acqUnitIds", OrganizationAcquisitionsUnits?.Select(oau => oau.ToJObject())),
            new JProperty("addresses", OrganizationAddresses?.Select(oa => oa.ToJObject())),
            new JProperty("agreements", OrganizationAgreements?.Select(oa => oa.ToJObject())),
            new JProperty("aliases", OrganizationAliases?.Select(oa => oa.ToJObject())),
            new JProperty("changelogs", OrganizationChangelogs?.Select(oc => oc.ToJObject())),
            new JProperty("contacts", OrganizationContacts?.Select(oc => oc.ToJObject())),
            new JProperty("emails", OrganizationEmails?.Select(oe => oe.ToJObject())),
            new JProperty("interfaces", OrganizationInterfaces?.Select(oi => oi.ToJObject())),
            new JProperty("phoneNumbers", OrganizationPhoneNumbers?.Select(opn => opn.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", OrganizationTags?.Select(ot => ot.ToJObject())))),
            new JProperty("urls", OrganizationUrls?.Select(ou => ou.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
