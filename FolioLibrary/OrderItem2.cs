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
    // uc.order_items -> uchicago_mod_orders_storage.po_line
    // OrderItem2 -> OrderItem
    [CustomValidation(typeof(OrderItem2), nameof(ValidateOrderItem2)), DisplayColumn(nameof(Number)), DisplayName("Order Items"), JsonConverter(typeof(JsonPathJsonConverter<OrderItem2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_items", Schema = "uc")]
    public partial class OrderItem2
    {
        public static ValidationResult ValidateOrderItem2(OrderItem2 orderItem2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (orderItem2.Number != null && fsc.AnyOrderItem2s($"id <> \"{orderItem2.Id}\" and poLineNumber == \"{orderItem2.Number}\"")) return new ValidationResult("Number already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OrderItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("edition"), Display(Order = 2), JsonProperty("edition"), StringLength(1024)]
        public virtual string Edition { get; set; }

        [Column("checkin_items"), Display(Name = "Checkin Items", Order = 3), JsonProperty("checkinItems")]
        public virtual bool? CheckinItems { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement Id", Order = 4), JsonProperty("agreementId")]
        public virtual Guid? AgreementId { get; set; }

        [Column("acquisition_method"), Display(Name = "Acquisition Method", Order = 5), JsonProperty("acquisitionMethod"), RegularExpression(@"^(Approval Plan|Demand Driven Acquisitions (DDA)|Depository|Evidence Based Acquisitions (EBA)|Exchange|Gift|Purchase At Vendor System|Purchase|Technical)$"), StringLength(1024)]
        public virtual string AcquisitionMethod { get; set; }

        [Column("cancellation_restriction"), Display(Name = "Cancellation Restriction", Order = 6), JsonProperty("cancellationRestriction")]
        public virtual bool? CancellationRestriction { get; set; }

        [Column("cancellation_restriction_note"), Display(Name = "Cancellation Restriction Note", Order = 7), JsonProperty("cancellationRestrictionNote"), StringLength(1024)]
        public virtual string CancellationRestrictionNote { get; set; }

        [Column("collection"), Display(Order = 8), JsonProperty("collection")]
        public virtual bool? Collection { get; set; }

        [Column("cost_list_unit_price"), DataType(DataType.Currency), Display(Name = "Physical Unit List Price", Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("cost.listUnitPrice")]
        public virtual decimal? PhysicalUnitListPrice { get; set; }

        [Column("cost_list_unit_price_electronic"), DataType(DataType.Currency), Display(Name = "Electronic Unit List Price", Order = 10), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("cost.listUnitPriceElectronic")]
        public virtual decimal? ElectronicUnitListPrice { get; set; }

        [Column("cost_currency"), Display(Order = 11), JsonProperty("cost.currency"), Required, StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("cost_additional_cost"), DataType(DataType.Currency), Display(Name = "Additional Cost", Order = 12), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("cost.additionalCost")]
        public virtual decimal? AdditionalCost { get; set; }

        [Column("cost_discount"), Display(Order = 13), JsonProperty("cost.discount")]
        public virtual decimal? Discount { get; set; }

        [Column("cost_discount_type"), Display(Name = "Discount Type", Order = 14), JsonProperty("cost.discountType"), RegularExpression(@"^(amount|percentage)$"), StringLength(1024)]
        public virtual string DiscountType { get; set; }

        [Column("cost_exchange_rate"), Display(Name = "Exchange Rate", Order = 15), JsonProperty("cost.exchangeRate")]
        public virtual decimal? ExchangeRate { get; set; }

        [Column("cost_quantity_physical"), Display(Name = "Physical Quantity", Order = 16), JsonProperty("cost.quantityPhysical")]
        public virtual int? PhysicalQuantity { get; set; }

        [Column("cost_quantity_electronic"), Display(Name = "Electronic Quantity", Order = 17), JsonProperty("cost.quantityElectronic")]
        public virtual int? ElectronicQuantity { get; set; }

        [Column("cost_po_line_estimated_price"), DataType(DataType.Currency), Display(Name = "Estimated Price", Order = 18), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("cost.poLineEstimatedPrice")]
        public virtual decimal? EstimatedPrice { get; set; }

        [Column("cost_fyro_adjustment_amount"), DataType(DataType.Currency), Display(Name = "Fiscal Year Rollover Adjustment Amount", Order = 19), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("cost.fyroAdjustmentAmount")]
        public virtual decimal? FiscalYearRolloverAdjustmentAmount { get; set; }

        [Column("description"), Display(Name = "Internal Note", Order = 20), JsonProperty("description"), StringLength(1024)]
        public virtual string InternalNote { get; set; }

        [Column("details_receiving_note"), Display(Name = "Receiving Note", Order = 21), JsonProperty("details.receivingNote"), StringLength(1024)]
        public virtual string ReceivingNote { get; set; }

        [Column("details_subscription_from"), Display(Name = "Subscription From", Order = 22), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("details.subscriptionFrom")]
        public virtual DateTime? SubscriptionFrom { get; set; }

        [Column("details_subscription_interval"), Display(Name = "Subscription Interval", Order = 23), JsonProperty("details.subscriptionInterval")]
        public virtual int? SubscriptionInterval { get; set; }

        [Column("details_subscription_to"), Display(Name = "Subscription To", Order = 24), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("details.subscriptionTo")]
        public virtual DateTime? SubscriptionTo { get; set; }

        [Column("donor"), Display(Order = 25), JsonProperty("donor"), StringLength(1024)]
        public virtual string Donor { get; set; }

        [Column("eresource_activated"), Display(Name = "Eresource Activated", Order = 26), JsonProperty("eresource.activated")]
        public virtual bool? EresourceActivated { get; set; }

        [Column("eresource_activation_due"), Display(Name = "Eresource Activation Due", Order = 27), JsonProperty("eresource.activationDue")]
        public virtual int? EresourceActivationDue { get; set; }

        [Column("eresource_create_inventory"), Display(Name = "Eresource Create Inventory", Order = 28), JsonProperty("eresource.createInventory"), RegularExpression(@"^(Instance, Holding, Item|Instance, Holding|Instance|None)$"), StringLength(1024)]
        public virtual string EresourceCreateInventory { get; set; }

        [Column("eresource_trial"), Display(Name = "Eresource Trial", Order = 29), JsonProperty("eresource.trial")]
        public virtual bool? EresourceTrial { get; set; }

        [Column("eresource_expected_activation"), DataType(DataType.Date), Display(Name = "Eresource Expected Activation Date", Order = 30), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("eresource.expectedActivation")]
        public virtual DateTime? EresourceExpectedActivationDate { get; set; }

        [Column("eresource_user_limit"), Display(Name = "Eresource User Limit", Order = 31), JsonProperty("eresource.userLimit")]
        public virtual int? EresourceUserLimit { get; set; }

        [Display(Name = "Eresource Access Provider", Order = 32), InverseProperty("OrderItem2s")]
        public virtual Organization2 EresourceAccessProvider { get; set; }

        [Column("eresource_access_provider_id"), Display(Name = "Eresource Access Provider", Order = 33), JsonProperty("eresource.accessProvider")]
        public virtual Guid? EresourceAccessProviderId { get; set; }

        [Column("eresource_license_code"), Display(Name = "Eresource License Code", Order = 34), JsonProperty("eresource.license.code"), StringLength(1024)]
        public virtual string EresourceLicenseCode { get; set; }

        [Column("eresource_license_description"), Display(Name = "Eresource License Description", Order = 35), JsonProperty("eresource.license.description"), StringLength(1024)]
        public virtual string EresourceLicenseDescription { get; set; }

        [Column("eresource_license_reference"), Display(Name = "Eresource License Reference", Order = 36), JsonProperty("eresource.license.reference"), StringLength(1024)]
        public virtual string EresourceLicenseReference { get; set; }

        [Display(Name = "Eresource Material Type", Order = 37), InverseProperty("OrderItem2s")]
        public virtual MaterialType2 EresourceMaterialType { get; set; }

        [Column("eresource_material_type_id"), Display(Name = "Eresource Material Type", Order = 38), JsonProperty("eresource.materialType")]
        public virtual Guid? EresourceMaterialTypeId { get; set; }

        [Column("eresource_resource_url"), DataType(DataType.Url), Display(Name = "Eresource Resource URL", Order = 39), JsonProperty("eresource.resourceUrl"), StringLength(1024)]
        public virtual string EresourceResourceUrl { get; set; }

        [Display(Order = 40)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 41), JsonProperty("instanceId")]
        public virtual Guid? InstanceId { get; set; }

        [Column("is_package"), Display(Name = "Is Package", Order = 42), JsonProperty("isPackage")]
        public virtual bool? IsPackage { get; set; }

        [Column("order_format"), Display(Name = "Order Format", Order = 43), JsonProperty("orderFormat"), RegularExpression(@"^(Electronic Resource|P/E Mix|Physical Resource|Other)$"), Required, StringLength(1024)]
        public virtual string OrderFormat { get; set; }

        [Display(Name = "Package Order Item", Order = 44)]
        public virtual OrderItem2 PackageOrderItem { get; set; }

        [Column("package_po_line_id"), Display(Name = "Package Order Item", Order = 45), JsonProperty("packagePoLineId")]
        public virtual Guid? PackageOrderItemId { get; set; }

        [Column("payment_status"), Display(Name = "Payment Status", Order = 46), JsonProperty("paymentStatus"), RegularExpression(@"^(Awaiting Payment|Cancelled|Fully Paid|Partially Paid|Payment Not Required|Pending|Ongoing)$"), StringLength(1024)]
        public virtual string PaymentStatus { get; set; }

        [Column("physical_create_inventory"), Display(Name = "Physical Create Inventory", Order = 47), JsonProperty("physical.createInventory"), RegularExpression(@"^(Instance, Holding, Item|Instance, Holding|Instance|None)$"), StringLength(1024)]
        public virtual string PhysicalCreateInventory { get; set; }

        [Display(Name = "Physical Material Type", Order = 48), InverseProperty("OrderItem2s1")]
        public virtual MaterialType2 PhysicalMaterialType { get; set; }

        [Column("physical_material_type_id"), Display(Name = "Physical Material Type", Order = 49), JsonProperty("physical.materialType")]
        public virtual Guid? PhysicalMaterialTypeId { get; set; }

        [Display(Name = "Physical Material Supplier", Order = 50), InverseProperty("OrderItem2s1")]
        public virtual Organization2 PhysicalMaterialSupplier { get; set; }

        [Column("physical_material_supplier_id"), Display(Name = "Physical Material Supplier", Order = 51), JsonProperty("physical.materialSupplier")]
        public virtual Guid? PhysicalMaterialSupplierId { get; set; }

        [Column("physical_expected_receipt_date"), DataType(DataType.Date), Display(Name = "Physical Expected Receipt Date", Order = 52), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("physical.expectedReceiptDate")]
        public virtual DateTime? PhysicalExpectedReceiptDate { get; set; }

        [Column("physical_receipt_due"), Display(Name = "Physical Receipt Due", Order = 53), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("physical.receiptDue")]
        public virtual DateTime? PhysicalReceiptDue { get; set; }

        [Column("po_line_description"), Display(Order = 54), JsonProperty("poLineDescription"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("po_line_number"), Display(Order = 55), JsonProperty("poLineNumber"), StringLength(1024)]
        public virtual string Number { get; set; }

        [Column("publication_year"), Display(Name = "Publication Year", Order = 56), JsonProperty("publicationDate"), StringLength(1024)]
        public virtual string PublicationYear { get; set; }

        [Column("publisher"), Display(Order = 57), JsonProperty("publisher"), StringLength(1024)]
        public virtual string Publisher { get; set; }

        [Display(Order = 58)]
        public virtual Order2 Order { get; set; }

        [Column("order_id"), Display(Name = "Order", Order = 59), JsonProperty("purchaseOrderId"), Required]
        public virtual Guid? OrderId { get; set; }

        [Column("receipt_date"), DataType(DataType.Date), Display(Name = "Receipt Date", Order = 60), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("receiptDate")]
        public virtual DateTime? ReceiptDate { get; set; }

        [Column("receipt_status"), Display(Name = "Receipt Status", Order = 61), JsonProperty("receiptStatus"), RegularExpression(@"^(Awaiting Receipt|Cancelled|Fully Received|Partially Received|Pending|Receipt Not Required|Ongoing)$"), StringLength(1024)]
        public virtual string ReceiptStatus { get; set; }

        [Column("requester"), Display(Order = 62), JsonProperty("requester"), StringLength(1024)]
        public virtual string Requester { get; set; }

        [Column("rush"), Display(Order = 63), JsonProperty("rush")]
        public virtual bool? Rush { get; set; }

        [Column("selector"), Display(Order = 64), JsonProperty("selector"), StringLength(1024)]
        public virtual string Selector { get; set; }

        [Column("source"), Display(Order = 65), JsonProperty("source"), RegularExpression(@"^(User|API|EDI|MARC|EBSCONET)$"), Required, StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("title_or_package"), Display(Name = "Title Or Package", Order = 66), JsonProperty("titleOrPackage"), Required, StringLength(1024)]
        public virtual string TitleOrPackage { get; set; }

        [Column("vendor_detail_instructions"), Display(Name = "Vendor Instructions", Order = 67), JsonProperty("vendorDetail.instructions"), Required, StringLength(1024)]
        public virtual string VendorInstructions { get; set; }

        [Column("vendor_detail_note_from_vendor"), Display(Name = "Vendor Note", Order = 68), JsonProperty("vendorDetail.noteFromVendor"), StringLength(1024)]
        public virtual string VendorNote { get; set; }

        [Column("vendor_detail_vendor_account"), Display(Name = "Vendor Customer Id", Order = 69), JsonProperty("vendorDetail.vendorAccount"), StringLength(1024)]
        public virtual string VendorCustomerId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 70), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 71), InverseProperty("OrderItem2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 72), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 74), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 75), InverseProperty("OrderItem2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 76), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(OrderItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 78), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Invoice Items", Order = 79)]
        public virtual ICollection<InvoiceItem2> InvoiceItem2s { get; set; }

        [Display(Name = "Items", Order = 80)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Order Items", Order = 81)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Order Item Alerts", Order = 82), JsonConverter(typeof(ArrayJsonConverter<List<OrderItemAlert>, OrderItemAlert>), "AlertId"), JsonProperty("alerts")]
        public virtual ICollection<OrderItemAlert> OrderItemAlerts { get; set; }

        [Display(Name = "Order Item Claims", Order = 83), JsonProperty("claims")]
        public virtual ICollection<OrderItemClaim> OrderItemClaims { get; set; }

        [Display(Name = "Order Item Contributors", Order = 84), JsonProperty("contributors")]
        public virtual ICollection<OrderItemContributor> OrderItemContributors { get; set; }

        [Display(Name = "Order Item Funds", Order = 85), JsonProperty("fundDistribution")]
        public virtual ICollection<OrderItemFund> OrderItemFunds { get; set; }

        [Display(Name = "Order Item Locations", Order = 86), JsonProperty("locations")]
        public virtual ICollection<OrderItemLocation2> OrderItemLocation2s { get; set; }

        [Display(Name = "Order Item Notes", Order = 87)]
        public virtual ICollection<OrderItemNote> OrderItemNotes { get; set; }

        [Display(Name = "Order Item Product Ids", Order = 88), JsonProperty("details.productIds")]
        public virtual ICollection<OrderItemProductId> OrderItemProductIds { get; set; }

        [Display(Name = "Order Item Reference Numbers", Order = 89), JsonProperty("vendorDetail.referenceNumbers")]
        public virtual ICollection<OrderItemReferenceNumber> OrderItemReferenceNumbers { get; set; }

        [Display(Name = "Order Item Reporting Codes", Order = 90), JsonConverter(typeof(ArrayJsonConverter<List<OrderItemReportingCode>, OrderItemReportingCode>), "ReportingCodeId"), JsonProperty("reportingCodes")]
        public virtual ICollection<OrderItemReportingCode> OrderItemReportingCodes { get; set; }

        [Display(Name = "Order Item Tags", Order = 91), JsonConverter(typeof(ArrayJsonConverter<List<OrderItemTag>, OrderItemTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<OrderItemTag> OrderItemTags { get; set; }

        [Display(Name = "Order Item Volumes", Order = 92), JsonConverter(typeof(ArrayJsonConverter<List<OrderItemVolume>, OrderItemVolume>), "Content"), JsonProperty("physical.volumes")]
        public virtual ICollection<OrderItemVolume> OrderItemVolumes { get; set; }

        [Display(Name = "Receivings", Order = 93)]
        public virtual ICollection<Receiving2> Receiving2s { get; set; }

        [Display(Name = "Titles", Order = 94)]
        public virtual ICollection<Title2> Title2s { get; set; }

        [Display(Name = "Transactions", Order = 95)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Edition)} = {Edition}, {nameof(CheckinItems)} = {CheckinItems}, {nameof(AgreementId)} = {AgreementId}, {nameof(AcquisitionMethod)} = {AcquisitionMethod}, {nameof(CancellationRestriction)} = {CancellationRestriction}, {nameof(CancellationRestrictionNote)} = {CancellationRestrictionNote}, {nameof(Collection)} = {Collection}, {nameof(PhysicalUnitListPrice)} = {PhysicalUnitListPrice}, {nameof(ElectronicUnitListPrice)} = {ElectronicUnitListPrice}, {nameof(Currency)} = {Currency}, {nameof(AdditionalCost)} = {AdditionalCost}, {nameof(Discount)} = {Discount}, {nameof(DiscountType)} = {DiscountType}, {nameof(ExchangeRate)} = {ExchangeRate}, {nameof(PhysicalQuantity)} = {PhysicalQuantity}, {nameof(ElectronicQuantity)} = {ElectronicQuantity}, {nameof(EstimatedPrice)} = {EstimatedPrice}, {nameof(FiscalYearRolloverAdjustmentAmount)} = {FiscalYearRolloverAdjustmentAmount}, {nameof(InternalNote)} = {InternalNote}, {nameof(ReceivingNote)} = {ReceivingNote}, {nameof(SubscriptionFrom)} = {SubscriptionFrom}, {nameof(SubscriptionInterval)} = {SubscriptionInterval}, {nameof(SubscriptionTo)} = {SubscriptionTo}, {nameof(Donor)} = {Donor}, {nameof(EresourceActivated)} = {EresourceActivated}, {nameof(EresourceActivationDue)} = {EresourceActivationDue}, {nameof(EresourceCreateInventory)} = {EresourceCreateInventory}, {nameof(EresourceTrial)} = {EresourceTrial}, {nameof(EresourceExpectedActivationDate)} = {EresourceExpectedActivationDate}, {nameof(EresourceUserLimit)} = {EresourceUserLimit}, {nameof(EresourceAccessProviderId)} = {EresourceAccessProviderId}, {nameof(EresourceLicenseCode)} = {EresourceLicenseCode}, {nameof(EresourceLicenseDescription)} = {EresourceLicenseDescription}, {nameof(EresourceLicenseReference)} = {EresourceLicenseReference}, {nameof(EresourceMaterialTypeId)} = {EresourceMaterialTypeId}, {nameof(EresourceResourceUrl)} = {EresourceResourceUrl}, {nameof(InstanceId)} = {InstanceId}, {nameof(IsPackage)} = {IsPackage}, {nameof(OrderFormat)} = {OrderFormat}, {nameof(PackageOrderItemId)} = {PackageOrderItemId}, {nameof(PaymentStatus)} = {PaymentStatus}, {nameof(PhysicalCreateInventory)} = {PhysicalCreateInventory}, {nameof(PhysicalMaterialTypeId)} = {PhysicalMaterialTypeId}, {nameof(PhysicalMaterialSupplierId)} = {PhysicalMaterialSupplierId}, {nameof(PhysicalExpectedReceiptDate)} = {PhysicalExpectedReceiptDate}, {nameof(PhysicalReceiptDue)} = {PhysicalReceiptDue}, {nameof(Description)} = {Description}, {nameof(Number)} = {Number}, {nameof(PublicationYear)} = {PublicationYear}, {nameof(Publisher)} = {Publisher}, {nameof(OrderId)} = {OrderId}, {nameof(ReceiptDate)} = {ReceiptDate}, {nameof(ReceiptStatus)} = {ReceiptStatus}, {nameof(Requester)} = {Requester}, {nameof(Rush)} = {Rush}, {nameof(Selector)} = {Selector}, {nameof(Source)} = {Source}, {nameof(TitleOrPackage)} = {TitleOrPackage}, {nameof(VendorInstructions)} = {VendorInstructions}, {nameof(VendorNote)} = {VendorNote}, {nameof(VendorCustomerId)} = {VendorCustomerId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(OrderItemAlerts)} = {(OrderItemAlerts != null ? $"{{ {string.Join(", ", OrderItemAlerts)} }}" : "")}, {nameof(OrderItemClaims)} = {(OrderItemClaims != null ? $"{{ {string.Join(", ", OrderItemClaims)} }}" : "")}, {nameof(OrderItemContributors)} = {(OrderItemContributors != null ? $"{{ {string.Join(", ", OrderItemContributors)} }}" : "")}, {nameof(OrderItemFunds)} = {(OrderItemFunds != null ? $"{{ {string.Join(", ", OrderItemFunds)} }}" : "")}, {nameof(OrderItemLocation2s)} = {(OrderItemLocation2s != null ? $"{{ {string.Join(", ", OrderItemLocation2s)} }}" : "")}, {nameof(OrderItemProductIds)} = {(OrderItemProductIds != null ? $"{{ {string.Join(", ", OrderItemProductIds)} }}" : "")}, {nameof(OrderItemReferenceNumbers)} = {(OrderItemReferenceNumbers != null ? $"{{ {string.Join(", ", OrderItemReferenceNumbers)} }}" : "")}, {nameof(OrderItemReportingCodes)} = {(OrderItemReportingCodes != null ? $"{{ {string.Join(", ", OrderItemReportingCodes)} }}" : "")}, {nameof(OrderItemTags)} = {(OrderItemTags != null ? $"{{ {string.Join(", ", OrderItemTags)} }}" : "")}, {nameof(OrderItemVolumes)} = {(OrderItemVolumes != null ? $"{{ {string.Join(", ", OrderItemVolumes)} }}" : "")} }}";

        public static OrderItem2 FromJObject(JObject jObject) => jObject != null ? new OrderItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Edition = (string)jObject.SelectToken("edition"),
            CheckinItems = (bool?)jObject.SelectToken("checkinItems"),
            AgreementId = (Guid?)jObject.SelectToken("agreementId"),
            AcquisitionMethod = (string)jObject.SelectToken("acquisitionMethod"),
            CancellationRestriction = (bool?)jObject.SelectToken("cancellationRestriction"),
            CancellationRestrictionNote = (string)jObject.SelectToken("cancellationRestrictionNote"),
            Collection = (bool?)jObject.SelectToken("collection"),
            PhysicalUnitListPrice = (decimal?)jObject.SelectToken("cost.listUnitPrice"),
            ElectronicUnitListPrice = (decimal?)jObject.SelectToken("cost.listUnitPriceElectronic"),
            Currency = (string)jObject.SelectToken("cost.currency"),
            AdditionalCost = (decimal?)jObject.SelectToken("cost.additionalCost"),
            Discount = (decimal?)jObject.SelectToken("cost.discount"),
            DiscountType = (string)jObject.SelectToken("cost.discountType"),
            ExchangeRate = (decimal?)jObject.SelectToken("cost.exchangeRate"),
            PhysicalQuantity = (int?)jObject.SelectToken("cost.quantityPhysical"),
            ElectronicQuantity = (int?)jObject.SelectToken("cost.quantityElectronic"),
            EstimatedPrice = (decimal?)jObject.SelectToken("cost.poLineEstimatedPrice"),
            FiscalYearRolloverAdjustmentAmount = (decimal?)jObject.SelectToken("cost.fyroAdjustmentAmount"),
            InternalNote = (string)jObject.SelectToken("description"),
            ReceivingNote = (string)jObject.SelectToken("details.receivingNote"),
            SubscriptionFrom = (DateTime?)jObject.SelectToken("details.subscriptionFrom"),
            SubscriptionInterval = (int?)jObject.SelectToken("details.subscriptionInterval"),
            SubscriptionTo = (DateTime?)jObject.SelectToken("details.subscriptionTo"),
            Donor = (string)jObject.SelectToken("donor"),
            EresourceActivated = (bool?)jObject.SelectToken("eresource.activated"),
            EresourceActivationDue = (int?)jObject.SelectToken("eresource.activationDue"),
            EresourceCreateInventory = (string)jObject.SelectToken("eresource.createInventory"),
            EresourceTrial = (bool?)jObject.SelectToken("eresource.trial"),
            EresourceExpectedActivationDate = ((DateTime?)jObject.SelectToken("eresource.expectedActivation"))?.ToUniversalTime(),
            EresourceUserLimit = (int?)jObject.SelectToken("eresource.userLimit"),
            EresourceAccessProviderId = (Guid?)jObject.SelectToken("eresource.accessProvider"),
            EresourceLicenseCode = (string)jObject.SelectToken("eresource.license.code"),
            EresourceLicenseDescription = (string)jObject.SelectToken("eresource.license.description"),
            EresourceLicenseReference = (string)jObject.SelectToken("eresource.license.reference"),
            EresourceMaterialTypeId = (Guid?)jObject.SelectToken("eresource.materialType"),
            EresourceResourceUrl = (string)jObject.SelectToken("eresource.resourceUrl"),
            InstanceId = (Guid?)jObject.SelectToken("instanceId"),
            IsPackage = (bool?)jObject.SelectToken("isPackage"),
            OrderFormat = (string)jObject.SelectToken("orderFormat"),
            PackageOrderItemId = (Guid?)jObject.SelectToken("packagePoLineId"),
            PaymentStatus = (string)jObject.SelectToken("paymentStatus"),
            PhysicalCreateInventory = (string)jObject.SelectToken("physical.createInventory"),
            PhysicalMaterialTypeId = (Guid?)jObject.SelectToken("physical.materialType"),
            PhysicalMaterialSupplierId = (Guid?)jObject.SelectToken("physical.materialSupplier"),
            PhysicalExpectedReceiptDate = ((DateTime?)jObject.SelectToken("physical.expectedReceiptDate"))?.ToUniversalTime(),
            PhysicalReceiptDue = (DateTime?)jObject.SelectToken("physical.receiptDue"),
            Description = (string)jObject.SelectToken("poLineDescription"),
            Number = (string)jObject.SelectToken("poLineNumber"),
            PublicationYear = (string)jObject.SelectToken("publicationDate"),
            Publisher = (string)jObject.SelectToken("publisher"),
            OrderId = (Guid?)jObject.SelectToken("purchaseOrderId"),
            ReceiptDate = ((DateTime?)jObject.SelectToken("receiptDate"))?.ToUniversalTime(),
            ReceiptStatus = (string)jObject.SelectToken("receiptStatus"),
            Requester = (string)jObject.SelectToken("requester"),
            Rush = (bool?)jObject.SelectToken("rush"),
            Selector = (string)jObject.SelectToken("selector"),
            Source = (string)jObject.SelectToken("source"),
            TitleOrPackage = (string)jObject.SelectToken("titleOrPackage"),
            VendorInstructions = (string)jObject.SelectToken("vendorDetail.instructions"),
            VendorNote = (string)jObject.SelectToken("vendorDetail.noteFromVendor"),
            VendorCustomerId = (string)jObject.SelectToken("vendorDetail.vendorAccount"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            OrderItemAlerts = jObject.SelectToken("alerts")?.Select(jt => OrderItemAlert.FromJObject((JValue)jt)).ToArray(),
            OrderItemClaims = jObject.SelectToken("claims")?.Where(jt => jt.HasValues).Select(jt => OrderItemClaim.FromJObject((JObject)jt)).ToArray(),
            OrderItemContributors = jObject.SelectToken("contributors")?.Where(jt => jt.HasValues).Select(jt => OrderItemContributor.FromJObject((JObject)jt)).ToArray(),
            OrderItemFunds = jObject.SelectToken("fundDistribution")?.Where(jt => jt.HasValues).Select(jt => OrderItemFund.FromJObject((JObject)jt)).ToArray(),
            OrderItemLocation2s = jObject.SelectToken("locations")?.Where(jt => jt.HasValues).Select(jt => OrderItemLocation2.FromJObject((JObject)jt)).ToArray(),
            OrderItemProductIds = jObject.SelectToken("details.productIds")?.Where(jt => jt.HasValues).Select(jt => OrderItemProductId.FromJObject((JObject)jt)).ToArray(),
            OrderItemReferenceNumbers = jObject.SelectToken("vendorDetail.referenceNumbers")?.Where(jt => jt.HasValues).Select(jt => OrderItemReferenceNumber.FromJObject((JObject)jt)).ToArray(),
            OrderItemReportingCodes = jObject.SelectToken("reportingCodes")?.Select(jt => OrderItemReportingCode.FromJObject((JValue)jt)).ToArray(),
            OrderItemTags = jObject.SelectToken("tags.tagList")?.Select(jt => OrderItemTag.FromJObject((JValue)jt)).ToArray(),
            OrderItemVolumes = jObject.SelectToken("physical.volumes")?.Select(jt => OrderItemVolume.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("edition", Edition),
            new JProperty("checkinItems", CheckinItems),
            new JProperty("agreementId", AgreementId),
            new JProperty("acquisitionMethod", AcquisitionMethod),
            new JProperty("cancellationRestriction", CancellationRestriction),
            new JProperty("cancellationRestrictionNote", CancellationRestrictionNote),
            new JProperty("collection", Collection),
            new JProperty("cost", new JObject(
                new JProperty("listUnitPrice", PhysicalUnitListPrice),
                new JProperty("listUnitPriceElectronic", ElectronicUnitListPrice),
                new JProperty("currency", Currency),
                new JProperty("additionalCost", AdditionalCost),
                new JProperty("discount", Discount),
                new JProperty("discountType", DiscountType),
                new JProperty("exchangeRate", ExchangeRate),
                new JProperty("quantityPhysical", PhysicalQuantity),
                new JProperty("quantityElectronic", ElectronicQuantity),
                new JProperty("poLineEstimatedPrice", EstimatedPrice),
                new JProperty("fyroAdjustmentAmount", FiscalYearRolloverAdjustmentAmount))),
            new JProperty("description", InternalNote),
            new JProperty("details", new JObject(
                new JProperty("receivingNote", ReceivingNote),
                new JProperty("subscriptionFrom", SubscriptionFrom?.ToLocalTime()),
                new JProperty("subscriptionInterval", SubscriptionInterval),
                new JProperty("subscriptionTo", SubscriptionTo?.ToLocalTime()),
                new JProperty("productIds", OrderItemProductIds?.Select(oipi => oipi.ToJObject())))),
            new JProperty("donor", Donor),
            new JProperty("eresource", new JObject(
                new JProperty("activated", EresourceActivated),
                new JProperty("activationDue", EresourceActivationDue),
                new JProperty("createInventory", EresourceCreateInventory),
                new JProperty("trial", EresourceTrial),
                new JProperty("expectedActivation", EresourceExpectedActivationDate?.ToLocalTime()),
                new JProperty("userLimit", EresourceUserLimit),
                new JProperty("accessProvider", EresourceAccessProviderId),
                new JProperty("license", new JObject(
                    new JProperty("code", EresourceLicenseCode),
                    new JProperty("description", EresourceLicenseDescription),
                    new JProperty("reference", EresourceLicenseReference))),
                new JProperty("materialType", EresourceMaterialTypeId),
                new JProperty("resourceUrl", EresourceResourceUrl))),
            new JProperty("instanceId", InstanceId),
            new JProperty("isPackage", IsPackage),
            new JProperty("orderFormat", OrderFormat),
            new JProperty("packagePoLineId", PackageOrderItemId),
            new JProperty("paymentStatus", PaymentStatus),
            new JProperty("physical", new JObject(
                new JProperty("createInventory", PhysicalCreateInventory),
                new JProperty("materialType", PhysicalMaterialTypeId),
                new JProperty("materialSupplier", PhysicalMaterialSupplierId),
                new JProperty("expectedReceiptDate", PhysicalExpectedReceiptDate?.ToLocalTime()),
                new JProperty("receiptDue", PhysicalReceiptDue?.ToLocalTime()),
                new JProperty("volumes", OrderItemVolumes?.Select(oiv => oiv.ToJObject())))),
            new JProperty("poLineDescription", Description),
            new JProperty("poLineNumber", Number),
            new JProperty("publicationDate", PublicationYear),
            new JProperty("publisher", Publisher),
            new JProperty("purchaseOrderId", OrderId),
            new JProperty("receiptDate", ReceiptDate?.ToLocalTime()),
            new JProperty("receiptStatus", ReceiptStatus),
            new JProperty("requester", Requester),
            new JProperty("rush", Rush),
            new JProperty("selector", Selector),
            new JProperty("source", Source),
            new JProperty("titleOrPackage", TitleOrPackage),
            new JProperty("vendorDetail", new JObject(
                new JProperty("instructions", VendorInstructions),
                new JProperty("noteFromVendor", VendorNote),
                new JProperty("vendorAccount", VendorCustomerId),
                new JProperty("referenceNumbers", OrderItemReferenceNumbers?.Select(oirn => oirn.ToJObject())))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("alerts", OrderItemAlerts?.Select(oia => oia.ToJObject())),
            new JProperty("claims", OrderItemClaims?.Select(oic => oic.ToJObject())),
            new JProperty("contributors", OrderItemContributors?.Select(oic => oic.ToJObject())),
            new JProperty("fundDistribution", OrderItemFunds?.Select(oif => oif.ToJObject())),
            new JProperty("locations", OrderItemLocation2s?.Select(oil2 => oil2.ToJObject())),
            new JProperty("reportingCodes", OrderItemReportingCodes?.Select(oirc => oirc.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", OrderItemTags?.Select(oit => oit.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
