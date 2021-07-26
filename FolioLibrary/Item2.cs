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
    // uc.items -> diku_mod_inventory_storage.item
    // Item2 -> Item
    [DisplayColumn(nameof(ShortId)), DisplayName("Items"), JsonConverter(typeof(JsonPathJsonConverter<Item2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("items", Schema = "uc")]
    public partial class Item2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Item.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), Display(Order = 2), JsonProperty("_version")]
        public virtual int? Version { get; set; }

        [Column("hrid"), Display(Name = "Short Id", Order = 3), Editable(false), JsonConverter(typeof(StringJsonConverter<int?>)), JsonProperty("hrid")]
        public virtual int? ShortId { get; set; }

        [Display(Order = 4)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 5), JsonProperty("holdingsRecordId"), Required]
        public virtual Guid? HoldingId { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 6), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("accession_number"), Display(Name = "Accession Number", Order = 7), JsonProperty("accessionNumber"), StringLength(1024)]
        public virtual string AccessionNumber { get; set; }

        [Column("barcode"), Display(Order = 8), JsonProperty("barcode"), StringLength(1024)]
        public virtual string Barcode { get; set; }

        [Column("effective_shelving_order"), Display(Name = "Effective Shelving Order", Order = 9), Editable(false), JsonProperty("effectiveShelvingOrder"), StringLength(1024)]
        public virtual string EffectiveShelvingOrder { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 10), JsonProperty("itemLevelCallNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("call_number_prefix"), Display(Name = "Call Number Prefix", Order = 11), JsonProperty("itemLevelCallNumberPrefix"), StringLength(1024)]
        public virtual string CallNumberPrefix { get; set; }

        [Column("call_number_suffix"), Display(Name = "Call Number Suffix", Order = 12), JsonProperty("itemLevelCallNumberSuffix"), StringLength(1024)]
        public virtual string CallNumberSuffix { get; set; }

        [Display(Name = "Call Number Type", Order = 13), InverseProperty("Item2s1")]
        public virtual CallNumberType2 CallNumberType { get; set; }

        [Column("call_number_type_id"), Display(Name = "Call Number Type", Order = 14), JsonProperty("itemLevelCallNumberTypeId")]
        public virtual Guid? CallNumberTypeId { get; set; }

        [Column("effective_call_number"), Display(Name = "Effective Call Number", Order = 15), Editable(false), JsonProperty("effectiveCallNumberComponents.callNumber"), StringLength(1024)]
        public virtual string EffectiveCallNumber { get; set; }

        [Column("effective_call_number_prefix"), Display(Name = "Effective Call Number Prefix", Order = 16), Editable(false), JsonProperty("effectiveCallNumberComponents.prefix"), StringLength(1024)]
        public virtual string EffectiveCallNumberPrefix { get; set; }

        [Column("effective_call_number_suffix"), Display(Name = "Effective Call Number Suffix", Order = 17), Editable(false), JsonProperty("effectiveCallNumberComponents.suffix"), StringLength(1024)]
        public virtual string EffectiveCallNumberSuffix { get; set; }

        [Display(Name = "Effective Call Number Type", Order = 18), InverseProperty("Item2s")]
        public virtual CallNumberType2 EffectiveCallNumberType { get; set; }

        [Column("effective_call_number_type_id"), Display(Name = "Effective Call Number Type", Order = 19), JsonProperty("effectiveCallNumberComponents.typeId")]
        public virtual Guid? EffectiveCallNumberTypeId { get; set; }

        [Column("volume"), Display(Order = 20), JsonProperty("volume"), StringLength(1024)]
        public virtual string Volume { get; set; }

        [Column("enumeration"), Display(Order = 21), JsonProperty("enumeration"), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 22), JsonProperty("chronology"), StringLength(1024)]
        public virtual string Chronology { get; set; }

        [Column("item_identifier"), Display(Name = "Item Identifier", Order = 23), JsonProperty("itemIdentifier"), StringLength(1024)]
        public virtual string ItemIdentifier { get; set; }

        [Column("copy_number"), Display(Name = "Copy Number", Order = 24), JsonProperty("copyNumber"), StringLength(1024)]
        public virtual string CopyNumber { get; set; }

        [Column("number_of_pieces"), Display(Name = "Pieces Count", Order = 25), JsonProperty("numberOfPieces"), StringLength(1024)]
        public virtual string PiecesCount { get; set; }

        [Column("description_of_pieces"), Display(Name = "Pieces Description", Order = 26), JsonProperty("descriptionOfPieces"), StringLength(1024)]
        public virtual string PiecesDescription { get; set; }

        [Column("number_of_missing_pieces"), Display(Name = "Missing Pieces Count", Order = 27), JsonProperty("numberOfMissingPieces"), StringLength(1024)]
        public virtual string MissingPiecesCount { get; set; }

        [Column("missing_pieces"), Display(Name = "Missing Pieces Description", Order = 28), JsonProperty("missingPieces"), StringLength(1024)]
        public virtual string MissingPiecesDescription { get; set; }

        [Column("missing_pieces_date"), DataType(DataType.DateTime), Display(Name = "Missing Pieces Time", Order = 29), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("missingPiecesDate")]
        public virtual DateTime? MissingPiecesTime { get; set; }

        [Display(Name = "Damaged Status", Order = 30)]
        public virtual ItemDamagedStatus2 DamagedStatus { get; set; }

        [Column("item_damaged_status_id"), Display(Name = "Damaged Status", Order = 31), JsonProperty("itemDamagedStatusId")]
        public virtual Guid? DamagedStatusId { get; set; }

        [Column("item_damaged_status_date"), DataType(DataType.DateTime), Display(Name = "Damaged Status Time", Order = 32), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("itemDamagedStatusDate")]
        public virtual DateTime? DamagedStatusTime { get; set; }

        [Column("status_name"), Display(Name = "Status Name", Order = 33), JsonProperty("status.name"), RegularExpression(@"^(Aged to lost|Available|Awaiting pickup|Awaiting delivery|Checked out|Claimed returned|Declared lost|In process|In process (non-requestable)|In transit|Intellectual item|Long missing|Lost and paid|Missing|On order|Paged|Restricted|Order closed|Unavailable|Unknown|Withdrawn)$"), Required, StringLength(1024)]
        public virtual string StatusName { get; set; }

        [Column("status_date"), DataType(DataType.Date), Display(Name = "Status Date", Order = 34), DisplayFormat(DataFormatString = "{0:d}"), Editable(false), JsonProperty("status.date")]
        public virtual DateTime? StatusDate { get; set; }

        [Display(Name = "Material Type", Order = 35)]
        public virtual MaterialType2 MaterialType { get; set; }

        [Column("material_type_id"), Display(Name = "Material Type", Order = 36), JsonProperty("materialTypeId"), Required]
        public virtual Guid? MaterialTypeId { get; set; }

        [Display(Name = "Permanent Loan Type", Order = 37), InverseProperty("Item2s")]
        public virtual LoanType2 PermanentLoanType { get; set; }

        [Column("permanent_loan_type_id"), Display(Name = "Permanent Loan Type", Order = 38), JsonProperty("permanentLoanTypeId"), Required]
        public virtual Guid? PermanentLoanTypeId { get; set; }

        [Display(Name = "Temporary Loan Type", Order = 39), InverseProperty("Item2s1")]
        public virtual LoanType2 TemporaryLoanType { get; set; }

        [Column("temporary_loan_type_id"), Display(Name = "Temporary Loan Type", Order = 40), JsonProperty("temporaryLoanTypeId")]
        public virtual Guid? TemporaryLoanTypeId { get; set; }

        [Display(Name = "Permanent Location", Order = 41), InverseProperty("Item2s1")]
        public virtual Location2 PermanentLocation { get; set; }

        [Column("permanent_location_id"), Display(Name = "Permanent Location", Order = 42), JsonProperty("permanentLocationId")]
        public virtual Guid? PermanentLocationId { get; set; }

        [Display(Name = "Temporary Location", Order = 43), InverseProperty("Item2s2")]
        public virtual Location2 TemporaryLocation { get; set; }

        [Column("temporary_location_id"), Display(Name = "Temporary Location", Order = 44), JsonProperty("temporaryLocationId")]
        public virtual Guid? TemporaryLocationId { get; set; }

        [Display(Name = "Effective Location", Order = 45), InverseProperty("Item2s")]
        public virtual Location2 EffectiveLocation { get; set; }

        [Column("effective_location_id"), Display(Name = "Effective Location", Order = 46), JsonProperty("effectiveLocationId")]
        public virtual Guid? EffectiveLocationId { get; set; }

        [Display(Name = "In Transit Destination Service Point", Order = 47), InverseProperty("Item2s1")]
        public virtual ServicePoint2 InTransitDestinationServicePoint { get; set; }

        [Column("in_transit_destination_service_point_id"), Display(Name = "In Transit Destination Service Point", Order = 48), JsonProperty("inTransitDestinationServicePointId")]
        public virtual Guid? InTransitDestinationServicePointId { get; set; }

        [Display(Name = "Order Item", Order = 49)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 50), JsonProperty("purchaseOrderLineIdentifier")]
        public virtual Guid? OrderItemId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 51), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 52), InverseProperty("Item2s1")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 53), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 55), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 56), InverseProperty("Item2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 57), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("last_check_in_date_time"), DataType(DataType.DateTime), Display(Name = "Last Check In Date Time", Order = 59), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastCheckIn.dateTime")]
        public virtual DateTime? LastCheckInDateTime { get; set; }

        [Display(Name = "Last Check In Service Point", Order = 60), InverseProperty("Item2s")]
        public virtual ServicePoint2 LastCheckInServicePoint { get; set; }

        [Column("last_check_in_service_point_id"), Display(Name = "Last Check In Service Point", Order = 61), JsonProperty("lastCheckIn.servicePointId")]
        public virtual Guid? LastCheckInServicePointId { get; set; }

        [Display(Name = "Last Check In Staff Member", Order = 62), InverseProperty("Item2s")]
        public virtual User2 LastCheckInStaffMember { get; set; }

        [Column("last_check_in_staff_member_id"), Display(Name = "Last Check In Staff Member", Order = 63), JsonProperty("lastCheckIn.staffMemberId")]
        public virtual Guid? LastCheckInStaffMemberId { get; set; }

        [Column("content"), CustomValidation(typeof(Item), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 64), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Check Ins", Order = 65)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Circulation Notes", Order = 66), JsonProperty("circulationNotes")]
        public virtual ICollection<CirculationNote> CirculationNotes { get; set; }

        [Display(Name = "Fees", Order = 67)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Item Electronic Accesses", Order = 68), JsonProperty("electronicAccess")]
        public virtual ICollection<ItemElectronicAccess> ItemElectronicAccesses { get; set; }

        [Display(Name = "Item Former Ids", Order = 69), JsonConverter(typeof(ArrayJsonConverter<List<ItemFormerId>, ItemFormerId>), "Content"), JsonProperty("formerIds")]
        public virtual ICollection<ItemFormerId> ItemFormerIds { get; set; }

        [Display(Name = "Item Notes", Order = 70), JsonProperty("notes")]
        public virtual ICollection<ItemNote> ItemNotes { get; set; }

        [Display(Name = "Item Statistical Codes", Order = 71), JsonConverter(typeof(ArrayJsonConverter<List<ItemStatisticalCode>, ItemStatisticalCode>), "StatisticalCodeId"), JsonProperty("statisticalCodeIds")]
        public virtual ICollection<ItemStatisticalCode> ItemStatisticalCodes { get; set; }

        [Display(Name = "Item Tags", Order = 72), JsonConverter(typeof(ArrayJsonConverter<List<ItemTag>, ItemTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<ItemTag> ItemTags { get; set; }

        [Display(Name = "Item Year Captions", Order = 73), JsonConverter(typeof(ArrayJsonConverter<List<ItemYearCaption>, ItemYearCaption>), "Content"), JsonProperty("yearCaption")]
        public virtual ICollection<ItemYearCaption> ItemYearCaptions { get; set; }

        [Display(Name = "Loans", Order = 74)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Receivings", Order = 75)]
        public virtual ICollection<Receiving2> Receiving2s { get; set; }

        [Display(Name = "Requests", Order = 76)]
        public virtual ICollection<Request2> Request2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(ShortId)} = {ShortId}, {nameof(HoldingId)} = {HoldingId}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(AccessionNumber)} = {AccessionNumber}, {nameof(Barcode)} = {Barcode}, {nameof(EffectiveShelvingOrder)} = {EffectiveShelvingOrder}, {nameof(CallNumber)} = {CallNumber}, {nameof(CallNumberPrefix)} = {CallNumberPrefix}, {nameof(CallNumberSuffix)} = {CallNumberSuffix}, {nameof(CallNumberTypeId)} = {CallNumberTypeId}, {nameof(EffectiveCallNumber)} = {EffectiveCallNumber}, {nameof(EffectiveCallNumberPrefix)} = {EffectiveCallNumberPrefix}, {nameof(EffectiveCallNumberSuffix)} = {EffectiveCallNumberSuffix}, {nameof(EffectiveCallNumberTypeId)} = {EffectiveCallNumberTypeId}, {nameof(Volume)} = {Volume}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology}, {nameof(ItemIdentifier)} = {ItemIdentifier}, {nameof(CopyNumber)} = {CopyNumber}, {nameof(PiecesCount)} = {PiecesCount}, {nameof(PiecesDescription)} = {PiecesDescription}, {nameof(MissingPiecesCount)} = {MissingPiecesCount}, {nameof(MissingPiecesDescription)} = {MissingPiecesDescription}, {nameof(MissingPiecesTime)} = {MissingPiecesTime}, {nameof(DamagedStatusId)} = {DamagedStatusId}, {nameof(DamagedStatusTime)} = {DamagedStatusTime}, {nameof(StatusName)} = {StatusName}, {nameof(StatusDate)} = {StatusDate}, {nameof(MaterialTypeId)} = {MaterialTypeId}, {nameof(PermanentLoanTypeId)} = {PermanentLoanTypeId}, {nameof(TemporaryLoanTypeId)} = {TemporaryLoanTypeId}, {nameof(PermanentLocationId)} = {PermanentLocationId}, {nameof(TemporaryLocationId)} = {TemporaryLocationId}, {nameof(EffectiveLocationId)} = {EffectiveLocationId}, {nameof(InTransitDestinationServicePointId)} = {InTransitDestinationServicePointId}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(LastCheckInDateTime)} = {LastCheckInDateTime}, {nameof(LastCheckInServicePointId)} = {LastCheckInServicePointId}, {nameof(LastCheckInStaffMemberId)} = {LastCheckInStaffMemberId}, {nameof(Content)} = {Content}, {nameof(CirculationNotes)} = {(CirculationNotes != null ? $"{{ {string.Join(", ", CirculationNotes)} }}" : "")}, {nameof(ItemElectronicAccesses)} = {(ItemElectronicAccesses != null ? $"{{ {string.Join(", ", ItemElectronicAccesses)} }}" : "")}, {nameof(ItemFormerIds)} = {(ItemFormerIds != null ? $"{{ {string.Join(", ", ItemFormerIds)} }}" : "")}, {nameof(ItemNotes)} = {(ItemNotes != null ? $"{{ {string.Join(", ", ItemNotes)} }}" : "")}, {nameof(ItemStatisticalCodes)} = {(ItemStatisticalCodes != null ? $"{{ {string.Join(", ", ItemStatisticalCodes)} }}" : "")}, {nameof(ItemTags)} = {(ItemTags != null ? $"{{ {string.Join(", ", ItemTags)} }}" : "")}, {nameof(ItemYearCaptions)} = {(ItemYearCaptions != null ? $"{{ {string.Join(", ", ItemYearCaptions)} }}" : "")} }}";

        public static Item2 FromJObject(JObject jObject) => jObject != null ? new Item2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            ShortId = (int?)jObject.SelectToken("hrid"),
            HoldingId = (Guid?)jObject.SelectToken("holdingsRecordId"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            AccessionNumber = (string)jObject.SelectToken("accessionNumber"),
            Barcode = (string)jObject.SelectToken("barcode"),
            EffectiveShelvingOrder = (string)jObject.SelectToken("effectiveShelvingOrder"),
            CallNumber = (string)jObject.SelectToken("itemLevelCallNumber"),
            CallNumberPrefix = (string)jObject.SelectToken("itemLevelCallNumberPrefix"),
            CallNumberSuffix = (string)jObject.SelectToken("itemLevelCallNumberSuffix"),
            CallNumberTypeId = (Guid?)jObject.SelectToken("itemLevelCallNumberTypeId"),
            EffectiveCallNumber = (string)jObject.SelectToken("effectiveCallNumberComponents.callNumber"),
            EffectiveCallNumberPrefix = (string)jObject.SelectToken("effectiveCallNumberComponents.prefix"),
            EffectiveCallNumberSuffix = (string)jObject.SelectToken("effectiveCallNumberComponents.suffix"),
            EffectiveCallNumberTypeId = (Guid?)jObject.SelectToken("effectiveCallNumberComponents.typeId"),
            Volume = (string)jObject.SelectToken("volume"),
            Enumeration = (string)jObject.SelectToken("enumeration"),
            Chronology = (string)jObject.SelectToken("chronology"),
            ItemIdentifier = (string)jObject.SelectToken("itemIdentifier"),
            CopyNumber = (string)jObject.SelectToken("copyNumber"),
            PiecesCount = (string)jObject.SelectToken("numberOfPieces"),
            PiecesDescription = (string)jObject.SelectToken("descriptionOfPieces"),
            MissingPiecesCount = (string)jObject.SelectToken("numberOfMissingPieces"),
            MissingPiecesDescription = (string)jObject.SelectToken("missingPieces"),
            MissingPiecesTime = (DateTime?)jObject.SelectToken("missingPiecesDate"),
            DamagedStatusId = (Guid?)jObject.SelectToken("itemDamagedStatusId"),
            DamagedStatusTime = (DateTime?)jObject.SelectToken("itemDamagedStatusDate"),
            StatusName = (string)jObject.SelectToken("status.name"),
            StatusDate = (DateTime?)jObject.SelectToken("status.date"),
            MaterialTypeId = (Guid?)jObject.SelectToken("materialTypeId"),
            PermanentLoanTypeId = (Guid?)jObject.SelectToken("permanentLoanTypeId"),
            TemporaryLoanTypeId = (Guid?)jObject.SelectToken("temporaryLoanTypeId"),
            PermanentLocationId = (Guid?)jObject.SelectToken("permanentLocationId"),
            TemporaryLocationId = (Guid?)jObject.SelectToken("temporaryLocationId"),
            EffectiveLocationId = (Guid?)jObject.SelectToken("effectiveLocationId"),
            InTransitDestinationServicePointId = (Guid?)jObject.SelectToken("inTransitDestinationServicePointId"),
            OrderItemId = (Guid?)jObject.SelectToken("purchaseOrderLineIdentifier"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            LastCheckInDateTime = (DateTime?)jObject.SelectToken("lastCheckIn.dateTime"),
            LastCheckInServicePointId = (Guid?)jObject.SelectToken("lastCheckIn.servicePointId"),
            LastCheckInStaffMemberId = (Guid?)jObject.SelectToken("lastCheckIn.staffMemberId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CirculationNotes = jObject.SelectToken("circulationNotes")?.Where(jt => jt.HasValues).Select(jt => CirculationNote.FromJObject((JObject)jt)).ToArray(),
            ItemElectronicAccesses = jObject.SelectToken("electronicAccess")?.Where(jt => jt.HasValues).Select(jt => ItemElectronicAccess.FromJObject((JObject)jt)).ToArray(),
            ItemFormerIds = jObject.SelectToken("formerIds")?.Where(jt => jt.HasValues).Select(jt => ItemFormerId.FromJObject((JValue)jt)).ToArray(),
            ItemNotes = jObject.SelectToken("notes")?.Where(jt => jt.HasValues).Select(jt => ItemNote.FromJObject((JObject)jt)).ToArray(),
            ItemStatisticalCodes = jObject.SelectToken("statisticalCodeIds")?.Where(jt => jt.HasValues).Select(jt => ItemStatisticalCode.FromJObject((JValue)jt)).ToArray(),
            ItemTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => ItemTag.FromJObject((JValue)jt)).ToArray(),
            ItemYearCaptions = jObject.SelectToken("yearCaption")?.Where(jt => jt.HasValues).Select(jt => ItemYearCaption.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("hrid", ShortId?.ToString()),
            new JProperty("holdingsRecordId", HoldingId),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("accessionNumber", AccessionNumber),
            new JProperty("barcode", Barcode),
            new JProperty("effectiveShelvingOrder", EffectiveShelvingOrder),
            new JProperty("itemLevelCallNumber", CallNumber),
            new JProperty("itemLevelCallNumberPrefix", CallNumberPrefix),
            new JProperty("itemLevelCallNumberSuffix", CallNumberSuffix),
            new JProperty("itemLevelCallNumberTypeId", CallNumberTypeId),
            new JProperty("effectiveCallNumberComponents", new JObject(
                new JProperty("callNumber", EffectiveCallNumber),
                new JProperty("prefix", EffectiveCallNumberPrefix),
                new JProperty("suffix", EffectiveCallNumberSuffix),
                new JProperty("typeId", EffectiveCallNumberTypeId))),
            new JProperty("volume", Volume),
            new JProperty("enumeration", Enumeration),
            new JProperty("chronology", Chronology),
            new JProperty("itemIdentifier", ItemIdentifier),
            new JProperty("copyNumber", CopyNumber),
            new JProperty("numberOfPieces", PiecesCount),
            new JProperty("descriptionOfPieces", PiecesDescription),
            new JProperty("numberOfMissingPieces", MissingPiecesCount),
            new JProperty("missingPieces", MissingPiecesDescription),
            new JProperty("missingPiecesDate", MissingPiecesTime),
            new JProperty("itemDamagedStatusId", DamagedStatusId),
            new JProperty("itemDamagedStatusDate", DamagedStatusTime),
            new JProperty("status", new JObject(
                new JProperty("name", StatusName),
                new JProperty("date", StatusDate))),
            new JProperty("materialTypeId", MaterialTypeId),
            new JProperty("permanentLoanTypeId", PermanentLoanTypeId),
            new JProperty("temporaryLoanTypeId", TemporaryLoanTypeId),
            new JProperty("permanentLocationId", PermanentLocationId),
            new JProperty("temporaryLocationId", TemporaryLocationId),
            new JProperty("effectiveLocationId", EffectiveLocationId),
            new JProperty("inTransitDestinationServicePointId", InTransitDestinationServicePointId),
            new JProperty("purchaseOrderLineIdentifier", OrderItemId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("lastCheckIn", new JObject(
                new JProperty("dateTime", LastCheckInDateTime),
                new JProperty("servicePointId", LastCheckInServicePointId),
                new JProperty("staffMemberId", LastCheckInStaffMemberId))),
            new JProperty("circulationNotes", CirculationNotes?.Select(cn => cn.ToJObject())),
            new JProperty("electronicAccess", ItemElectronicAccesses?.Select(iea => iea.ToJObject())),
            new JProperty("formerIds", ItemFormerIds?.Select(ifi => ifi.ToJObject())),
            new JProperty("notes", ItemNotes?.Select(@in => @in.ToJObject())),
            new JProperty("statisticalCodeIds", ItemStatisticalCodes?.Select(isc => isc.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", ItemTags?.Select(it => it.ToJObject())))),
            new JProperty("yearCaption", ItemYearCaptions?.Select(iyc => iyc.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
