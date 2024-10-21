using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.receivings -> uchicago_mod_orders_storage.pieces
    // Receiving2 -> Receiving
    [DisplayColumn(nameof(Id)), DisplayName("Receivings"), JsonConverter(typeof(JsonPathJsonConverter<Receiving2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("receivings", Schema = "uc")]
    public partial class Receiving2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Receiving.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("display_summary"), Display(Name = "Display Summary", Order = 2), JsonProperty("displaySummary"), StringLength(1024)]
        public virtual string DisplaySummary { get; set; }

        [Column("comment"), Display(Order = 3), JsonProperty("comment"), StringLength(1024)]
        public virtual string Comment { get; set; }

        [Column("format"), Display(Order = 4), JsonProperty("format"), RegularExpression(@"^(Physical|Electronic|Other)$"), Required, StringLength(1024)]
        public virtual string Format { get; set; }

        [Display(Order = 5)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 6), JsonProperty("itemId")]
        public virtual Guid? ItemId { get; set; }

        [Column("bind_item_id"), Display(Name = "Bind Item Id", Order = 7), JsonProperty("bindItemId")]
        public virtual Guid? BindItemId { get; set; }

        [Column("bind_item_tenant_id"), Display(Name = "Bind Item Tenant Id", Order = 8), JsonProperty("bindItemTenantId")]
        public virtual Guid? BindItemTenantId { get; set; }

        [Display(Order = 9)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 10), JsonProperty("locationId")]
        public virtual Guid? LocationId { get; set; }

        [Display(Name = "Order Item", Order = 11)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("po_line_id"), Display(Name = "Order Item", Order = 12), JsonProperty("poLineId"), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 13)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 14), JsonProperty("titleId"), Required]
        public virtual Guid? TitleId { get; set; }

        [Display(Order = 15)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 16), JsonProperty("holdingId")]
        public virtual Guid? HoldingId { get; set; }

        [Column("receiving_tenant_id"), Display(Name = "Receiving Tenant Id", Order = 17), JsonProperty("receivingTenantId")]
        public virtual Guid? ReceivingTenantId { get; set; }

        [Column("display_on_holding"), Display(Name = "Display On Holding", Order = 18), JsonProperty("displayOnHolding")]
        public virtual bool? DisplayOnHolding { get; set; }

        [Column("display_to_public"), Display(Name = "Display To Public", Order = 19), JsonProperty("displayToPublic")]
        public virtual bool? DisplayToPublic { get; set; }

        [Column("enumeration"), Display(Order = 20), JsonProperty("enumeration"), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 21), JsonProperty("chronology"), StringLength(1024)]
        public virtual string Chronology { get; set; }

        [Column("barcode"), Display(Order = 22), JsonProperty("barcode"), StringLength(1024)]
        public virtual string Barcode { get; set; }

        [Column("accession_number"), Display(Name = "Accession Number", Order = 23), JsonProperty("accessionNumber"), StringLength(1024)]
        public virtual string AccessionNumber { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 24), JsonProperty("callNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 25), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("copy_number"), Display(Name = "Copy Number", Order = 26), JsonProperty("copyNumber"), StringLength(1024)]
        public virtual string CopyNumber { get; set; }

        [Column("receiving_status"), Display(Name = "Receiving Status", Order = 27), JsonProperty("receivingStatus"), RegularExpression(@"^(Received|Expected|Late|Claim delayed|Claim sent|Unreceivable)$"), Required, StringLength(1024)]
        public virtual string ReceivingStatus { get; set; }

        [Column("supplement"), Display(Order = 28), JsonProperty("supplement")]
        public virtual bool? Supplement { get; set; }

        [Column("is_bound"), Display(Name = "Is Bound", Order = 29), JsonProperty("isBound")]
        public virtual bool? IsBound { get; set; }

        [Column("receipt_date"), DataType(DataType.DateTime), Display(Name = "Receipt Time", Order = 30), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("receiptDate")]
        public virtual DateTime? ReceiptTime { get; set; }

        [Column("received_date"), DataType(DataType.DateTime), Display(Name = "Receive Time", Order = 31), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("receivedDate")]
        public virtual DateTime? ReceiveTime { get; set; }

        [Column("status_updated_date"), DataType(DataType.Date), Display(Name = "Status Updated Date", Order = 32), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("statusUpdatedDate")]
        public virtual DateTime? StatusUpdatedDate { get; set; }

        [Column("claiming_interval"), Display(Name = "Claiming Interval", Order = 33), JsonProperty("claimingInterval")]
        public virtual int? ClaimingInterval { get; set; }

        [Column("internal_note"), Display(Name = "Internal Note", Order = 34), JsonProperty("internalNote"), StringLength(1024)]
        public virtual string InternalNote { get; set; }

        [Column("external_note"), Display(Name = "External Note", Order = 35), JsonProperty("externalNote"), StringLength(1024)]
        public virtual string ExternalNote { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 36), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 37), InverseProperty("Receiving2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 38), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 40), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 41), InverseProperty("Receiving2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 42), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Receiving), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 44), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(DisplaySummary)} = {DisplaySummary}, {nameof(Comment)} = {Comment}, {nameof(Format)} = {Format}, {nameof(ItemId)} = {ItemId}, {nameof(BindItemId)} = {BindItemId}, {nameof(BindItemTenantId)} = {BindItemTenantId}, {nameof(LocationId)} = {LocationId}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(TitleId)} = {TitleId}, {nameof(HoldingId)} = {HoldingId}, {nameof(ReceivingTenantId)} = {ReceivingTenantId}, {nameof(DisplayOnHolding)} = {DisplayOnHolding}, {nameof(DisplayToPublic)} = {DisplayToPublic}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology}, {nameof(Barcode)} = {Barcode}, {nameof(AccessionNumber)} = {AccessionNumber}, {nameof(CallNumber)} = {CallNumber}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(CopyNumber)} = {CopyNumber}, {nameof(ReceivingStatus)} = {ReceivingStatus}, {nameof(Supplement)} = {Supplement}, {nameof(IsBound)} = {IsBound}, {nameof(ReceiptTime)} = {ReceiptTime}, {nameof(ReceiveTime)} = {ReceiveTime}, {nameof(StatusUpdatedDate)} = {StatusUpdatedDate}, {nameof(ClaimingInterval)} = {ClaimingInterval}, {nameof(InternalNote)} = {InternalNote}, {nameof(ExternalNote)} = {ExternalNote}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Receiving2 FromJObject(JObject jObject) => jObject != null ? new Receiving2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            DisplaySummary = (string)jObject.SelectToken("displaySummary"),
            Comment = (string)jObject.SelectToken("comment"),
            Format = (string)jObject.SelectToken("format"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            BindItemId = (Guid?)jObject.SelectToken("bindItemId"),
            BindItemTenantId = (Guid?)jObject.SelectToken("bindItemTenantId"),
            LocationId = (Guid?)jObject.SelectToken("locationId"),
            OrderItemId = (Guid?)jObject.SelectToken("poLineId"),
            TitleId = (Guid?)jObject.SelectToken("titleId"),
            HoldingId = (Guid?)jObject.SelectToken("holdingId"),
            ReceivingTenantId = (Guid?)jObject.SelectToken("receivingTenantId"),
            DisplayOnHolding = (bool?)jObject.SelectToken("displayOnHolding"),
            DisplayToPublic = (bool?)jObject.SelectToken("displayToPublic"),
            Enumeration = (string)jObject.SelectToken("enumeration"),
            Chronology = (string)jObject.SelectToken("chronology"),
            Barcode = (string)jObject.SelectToken("barcode"),
            AccessionNumber = (string)jObject.SelectToken("accessionNumber"),
            CallNumber = (string)jObject.SelectToken("callNumber"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            CopyNumber = (string)jObject.SelectToken("copyNumber"),
            ReceivingStatus = (string)jObject.SelectToken("receivingStatus"),
            Supplement = (bool?)jObject.SelectToken("supplement"),
            IsBound = (bool?)jObject.SelectToken("isBound"),
            ReceiptTime = (DateTime?)jObject.SelectToken("receiptDate"),
            ReceiveTime = (DateTime?)jObject.SelectToken("receivedDate"),
            StatusUpdatedDate = ((DateTime?)jObject.SelectToken("statusUpdatedDate"))?.ToUniversalTime(),
            ClaimingInterval = (int?)jObject.SelectToken("claimingInterval"),
            InternalNote = (string)jObject.SelectToken("internalNote"),
            ExternalNote = (string)jObject.SelectToken("externalNote"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("displaySummary", DisplaySummary),
            new JProperty("comment", Comment),
            new JProperty("format", Format),
            new JProperty("itemId", ItemId),
            new JProperty("bindItemId", BindItemId),
            new JProperty("bindItemTenantId", BindItemTenantId),
            new JProperty("locationId", LocationId),
            new JProperty("poLineId", OrderItemId),
            new JProperty("titleId", TitleId),
            new JProperty("holdingId", HoldingId),
            new JProperty("receivingTenantId", ReceivingTenantId),
            new JProperty("displayOnHolding", DisplayOnHolding),
            new JProperty("displayToPublic", DisplayToPublic),
            new JProperty("enumeration", Enumeration),
            new JProperty("chronology", Chronology),
            new JProperty("barcode", Barcode),
            new JProperty("accessionNumber", AccessionNumber),
            new JProperty("callNumber", CallNumber),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("copyNumber", CopyNumber),
            new JProperty("receivingStatus", ReceivingStatus),
            new JProperty("supplement", Supplement),
            new JProperty("isBound", IsBound),
            new JProperty("receiptDate", ReceiptTime?.ToLocalTime()),
            new JProperty("receivedDate", ReceiveTime?.ToLocalTime()),
            new JProperty("statusUpdatedDate", StatusUpdatedDate?.ToLocalTime()),
            new JProperty("claimingInterval", ClaimingInterval),
            new JProperty("internalNote", InternalNote),
            new JProperty("externalNote", ExternalNote),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
