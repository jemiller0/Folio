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

        [Column("hrid"), Display(Name = "Short Id", Order = 2), Editable(false), JsonConverter(typeof(StringJsonConverter<int?>)), JsonProperty("hrid")]
        public virtual int? ShortId { get; set; }

        [Display(Order = 3)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 4), JsonProperty("holdingsRecordId"), Required]
        public virtual Guid? HoldingId { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 5), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("accession_number"), Display(Name = "Accession Number", Order = 6), JsonProperty("accessionNumber"), StringLength(1024)]
        public virtual string AccessionNumber { get; set; }

        [Column("barcode"), Display(Order = 7), JsonProperty("barcode"), StringLength(1024)]
        public virtual string Barcode { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 8), JsonProperty("itemLevelCallNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("call_number_prefix"), Display(Name = "Call Number Prefix", Order = 9), JsonProperty("itemLevelCallNumberPrefix"), StringLength(1024)]
        public virtual string CallNumberPrefix { get; set; }

        [Column("call_number_suffix"), Display(Name = "Call Number Suffix", Order = 10), JsonProperty("itemLevelCallNumberSuffix"), StringLength(1024)]
        public virtual string CallNumberSuffix { get; set; }

        [Display(Name = "Call Number Type", Order = 11), InverseProperty("Item2s1")]
        public virtual CallNumberType2 CallNumberType { get; set; }

        [Column("call_number_type_id"), Display(Name = "Call Number Type", Order = 12), JsonProperty("itemLevelCallNumberTypeId")]
        public virtual Guid? CallNumberTypeId { get; set; }

        [Column("effective_call_number"), Display(Name = "Effective Call Number", Order = 13), Editable(false), JsonProperty("effectiveCallNumberComponents.callNumber"), StringLength(1024)]
        public virtual string EffectiveCallNumber { get; set; }

        [Column("effective_call_number_prefix"), Display(Name = "Effective Call Number Prefix", Order = 14), Editable(false), JsonProperty("effectiveCallNumberComponents.prefix"), StringLength(1024)]
        public virtual string EffectiveCallNumberPrefix { get; set; }

        [Column("effective_call_number_suffix"), Display(Name = "Effective Call Number Suffix", Order = 15), Editable(false), JsonProperty("effectiveCallNumberComponents.suffix"), StringLength(1024)]
        public virtual string EffectiveCallNumberSuffix { get; set; }

        [Display(Name = "Effective Call Number Type", Order = 16), InverseProperty("Item2s")]
        public virtual CallNumberType2 EffectiveCallNumberType { get; set; }

        [Column("effective_call_number_type_id"), Display(Name = "Effective Call Number Type", Order = 17), JsonProperty("effectiveCallNumberComponents.typeId")]
        public virtual Guid? EffectiveCallNumberTypeId { get; set; }

        [Column("volume"), Display(Order = 18), JsonProperty("volume"), StringLength(1024)]
        public virtual string Volume { get; set; }

        [Column("enumeration"), Display(Order = 19), JsonProperty("enumeration"), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 20), JsonProperty("chronology"), StringLength(1024)]
        public virtual string Chronology { get; set; }

        [Column("item_identifier"), Display(Name = "Item Identifier", Order = 21), JsonProperty("itemIdentifier"), StringLength(1024)]
        public virtual string ItemIdentifier { get; set; }

        [Column("copy_number"), Display(Name = "Copy Number", Order = 22), JsonProperty("copyNumber"), StringLength(1024)]
        public virtual string CopyNumber { get; set; }

        [Column("number_of_pieces"), Display(Name = "Pieces Count", Order = 23), JsonProperty("numberOfPieces"), StringLength(1024)]
        public virtual string PiecesCount { get; set; }

        [Column("description_of_pieces"), Display(Name = "Pieces Description", Order = 24), JsonProperty("descriptionOfPieces"), StringLength(1024)]
        public virtual string PiecesDescription { get; set; }

        [Column("number_of_missing_pieces"), Display(Name = "Missing Pieces Count", Order = 25), JsonProperty("numberOfMissingPieces"), StringLength(1024)]
        public virtual string MissingPiecesCount { get; set; }

        [Column("missing_pieces"), Display(Name = "Missing Pieces Description", Order = 26), JsonProperty("missingPieces"), StringLength(1024)]
        public virtual string MissingPiecesDescription { get; set; }

        [Column("missing_pieces_date"), DataType(DataType.DateTime), Display(Name = "Missing Pieces Time", Order = 27), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("missingPiecesDate")]
        public virtual DateTime? MissingPiecesTime { get; set; }

        [Display(Name = "Damaged Status", Order = 28)]
        public virtual ItemDamagedStatus2 DamagedStatus { get; set; }

        [Column("item_damaged_status_id"), Display(Name = "Damaged Status", Order = 29), JsonProperty("itemDamagedStatusId")]
        public virtual Guid? DamagedStatusId { get; set; }

        [Column("item_damaged_status_date"), DataType(DataType.DateTime), Display(Name = "Damaged Status Time", Order = 30), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("itemDamagedStatusDate")]
        public virtual DateTime? DamagedStatusTime { get; set; }

        [Column("status_name"), Display(Name = "Status Name", Order = 31), JsonProperty("status.name"), RegularExpression(@"^(Available|Awaiting pickup|Awaiting delivery|Checked out|In process|In transit|Missing|On order|Paged|Declared lost|Order closed|Claimed returned|Unknown|Withdrawn|Lost and paid|Aged to lost)$"), Required, StringLength(1024)]
        public virtual string StatusName { get; set; }

        [Column("status_date"), DataType(DataType.Date), Display(Name = "Status Date", Order = 32), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("status.date")]
        public virtual DateTime? StatusDate { get; set; }

        [Display(Name = "Material Type", Order = 33)]
        public virtual MaterialType2 MaterialType { get; set; }

        [Column("material_type_id"), Display(Name = "Material Type", Order = 34), JsonProperty("materialTypeId"), Required]
        public virtual Guid? MaterialTypeId { get; set; }

        [Display(Name = "Permanent Loan Type", Order = 35), InverseProperty("Item2s")]
        public virtual LoanType2 PermanentLoanType { get; set; }

        [Column("permanent_loan_type_id"), Display(Name = "Permanent Loan Type", Order = 36), JsonProperty("permanentLoanTypeId"), Required]
        public virtual Guid? PermanentLoanTypeId { get; set; }

        [Display(Name = "Temporary Loan Type", Order = 37), InverseProperty("Item2s1")]
        public virtual LoanType2 TemporaryLoanType { get; set; }

        [Column("temporary_loan_type_id"), Display(Name = "Temporary Loan Type", Order = 38), JsonProperty("temporaryLoanTypeId")]
        public virtual Guid? TemporaryLoanTypeId { get; set; }

        [Display(Name = "Permanent Location", Order = 39), InverseProperty("Item2s1")]
        public virtual Location2 PermanentLocation { get; set; }

        [Column("permanent_location_id"), Display(Name = "Permanent Location", Order = 40), JsonProperty("permanentLocationId")]
        public virtual Guid? PermanentLocationId { get; set; }

        [Display(Name = "Temporary Location", Order = 41), InverseProperty("Item2s2")]
        public virtual Location2 TemporaryLocation { get; set; }

        [Column("temporary_location_id"), Display(Name = "Temporary Location", Order = 42), JsonProperty("temporaryLocationId")]
        public virtual Guid? TemporaryLocationId { get; set; }

        [Display(Name = "Effective Location", Order = 43), InverseProperty("Item2s")]
        public virtual Location2 EffectiveLocation { get; set; }

        [Column("effective_location_id"), Display(Name = "Effective Location", Order = 44), JsonProperty("effectiveLocationId")]
        public virtual Guid? EffectiveLocationId { get; set; }

        [Display(Name = "In Transit Destination Service Point", Order = 45), InverseProperty("Item2s1")]
        public virtual ServicePoint2 InTransitDestinationServicePoint { get; set; }

        [Column("in_transit_destination_service_point_id"), Display(Name = "In Transit Destination Service Point", Order = 46), JsonProperty("inTransitDestinationServicePointId")]
        public virtual Guid? InTransitDestinationServicePointId { get; set; }

        [Display(Name = "Order Item", Order = 47)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 48), JsonProperty("purchaseOrderLineIdentifier")]
        public virtual Guid? OrderItemId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 49), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 50), InverseProperty("Item2s1")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 51), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 53), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 54), InverseProperty("Item2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 55), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("last_check_in_date_time"), DataType(DataType.DateTime), Display(Name = "Last Check In Date Time", Order = 57), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastCheckIn.dateTime")]
        public virtual DateTime? LastCheckInDateTime { get; set; }

        [Display(Name = "Last Check In Service Point", Order = 58), InverseProperty("Item2s")]
        public virtual ServicePoint2 LastCheckInServicePoint { get; set; }

        [Column("last_check_in_service_point_id"), Display(Name = "Last Check In Service Point", Order = 59), JsonProperty("lastCheckIn.servicePointId")]
        public virtual Guid? LastCheckInServicePointId { get; set; }

        [Display(Name = "Last Check In Staff Member", Order = 60), InverseProperty("Item2s")]
        public virtual User2 LastCheckInStaffMember { get; set; }

        [Column("last_check_in_staff_member_id"), Display(Name = "Last Check In Staff Member", Order = 61), JsonProperty("lastCheckIn.staffMemberId")]
        public virtual Guid? LastCheckInStaffMemberId { get; set; }

        [Column("content"), CustomValidation(typeof(Item), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 62), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Check Ins", Order = 63)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Circulation Notes", Order = 64), JsonProperty("circulationNotes")]
        public virtual ICollection<CirculationNote> CirculationNotes { get; set; }

        [Display(Name = "Fees", Order = 65)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Item Electronic Accesses", Order = 66), JsonProperty("electronicAccess")]
        public virtual ICollection<ItemElectronicAccess> ItemElectronicAccesses { get; set; }

        [Display(Name = "Item Former Ids", Order = 67), JsonConverter(typeof(ArrayJsonConverter<List<ItemFormerId>, ItemFormerId>), "Content"), JsonProperty("formerIds")]
        public virtual ICollection<ItemFormerId> ItemFormerIds { get; set; }

        [Display(Name = "Item Notes", Order = 68), JsonProperty("notes")]
        public virtual ICollection<ItemNote> ItemNotes { get; set; }

        [Display(Name = "Item Statistical Codes", Order = 69), JsonConverter(typeof(ArrayJsonConverter<List<ItemStatisticalCode>, ItemStatisticalCode>), "StatisticalCodeId"), JsonProperty("statisticalCodeIds")]
        public virtual ICollection<ItemStatisticalCode> ItemStatisticalCodes { get; set; }

        [Display(Name = "Item Tags", Order = 70), JsonConverter(typeof(ArrayJsonConverter<List<ItemTag>, ItemTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<ItemTag> ItemTags { get; set; }

        [Display(Name = "Item Year Captions", Order = 71), JsonConverter(typeof(ArrayJsonConverter<List<ItemYearCaption>, ItemYearCaption>), "Content"), JsonProperty("yearCaption")]
        public virtual ICollection<ItemYearCaption> ItemYearCaptions { get; set; }

        [Display(Name = "Loans", Order = 72)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Receivings", Order = 73)]
        public virtual ICollection<Receiving2> Receiving2s { get; set; }

        [Display(Name = "Requests", Order = 74)]
        public virtual ICollection<Request2> Request2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ShortId)} = {ShortId}, {nameof(HoldingId)} = {HoldingId}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(AccessionNumber)} = {AccessionNumber}, {nameof(Barcode)} = {Barcode}, {nameof(CallNumber)} = {CallNumber}, {nameof(CallNumberPrefix)} = {CallNumberPrefix}, {nameof(CallNumberSuffix)} = {CallNumberSuffix}, {nameof(CallNumberTypeId)} = {CallNumberTypeId}, {nameof(EffectiveCallNumber)} = {EffectiveCallNumber}, {nameof(EffectiveCallNumberPrefix)} = {EffectiveCallNumberPrefix}, {nameof(EffectiveCallNumberSuffix)} = {EffectiveCallNumberSuffix}, {nameof(EffectiveCallNumberTypeId)} = {EffectiveCallNumberTypeId}, {nameof(Volume)} = {Volume}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology}, {nameof(ItemIdentifier)} = {ItemIdentifier}, {nameof(CopyNumber)} = {CopyNumber}, {nameof(PiecesCount)} = {PiecesCount}, {nameof(PiecesDescription)} = {PiecesDescription}, {nameof(MissingPiecesCount)} = {MissingPiecesCount}, {nameof(MissingPiecesDescription)} = {MissingPiecesDescription}, {nameof(MissingPiecesTime)} = {MissingPiecesTime}, {nameof(DamagedStatusId)} = {DamagedStatusId}, {nameof(DamagedStatusTime)} = {DamagedStatusTime}, {nameof(StatusName)} = {StatusName}, {nameof(StatusDate)} = {StatusDate}, {nameof(MaterialTypeId)} = {MaterialTypeId}, {nameof(PermanentLoanTypeId)} = {PermanentLoanTypeId}, {nameof(TemporaryLoanTypeId)} = {TemporaryLoanTypeId}, {nameof(PermanentLocationId)} = {PermanentLocationId}, {nameof(TemporaryLocationId)} = {TemporaryLocationId}, {nameof(EffectiveLocationId)} = {EffectiveLocationId}, {nameof(InTransitDestinationServicePointId)} = {InTransitDestinationServicePointId}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(LastCheckInDateTime)} = {LastCheckInDateTime}, {nameof(LastCheckInServicePointId)} = {LastCheckInServicePointId}, {nameof(LastCheckInStaffMemberId)} = {LastCheckInStaffMemberId}, {nameof(Content)} = {Content}, {nameof(CirculationNotes)} = {(CirculationNotes != null ? $"{{ {string.Join(", ", CirculationNotes)} }}" : "")}, {nameof(ItemElectronicAccesses)} = {(ItemElectronicAccesses != null ? $"{{ {string.Join(", ", ItemElectronicAccesses)} }}" : "")}, {nameof(ItemFormerIds)} = {(ItemFormerIds != null ? $"{{ {string.Join(", ", ItemFormerIds)} }}" : "")}, {nameof(ItemNotes)} = {(ItemNotes != null ? $"{{ {string.Join(", ", ItemNotes)} }}" : "")}, {nameof(ItemStatisticalCodes)} = {(ItemStatisticalCodes != null ? $"{{ {string.Join(", ", ItemStatisticalCodes)} }}" : "")}, {nameof(ItemTags)} = {(ItemTags != null ? $"{{ {string.Join(", ", ItemTags)} }}" : "")}, {nameof(ItemYearCaptions)} = {(ItemYearCaptions != null ? $"{{ {string.Join(", ", ItemYearCaptions)} }}" : "")} }}";

        public static Item2 FromJObject(JObject jObject) => jObject != null ? new Item2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            ShortId = (int?)jObject.SelectToken("hrid"),
            HoldingId = (Guid?)jObject.SelectToken("holdingsRecordId"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            AccessionNumber = (string)jObject.SelectToken("accessionNumber"),
            Barcode = (string)jObject.SelectToken("barcode"),
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
            MissingPiecesTime = ((DateTime?)jObject.SelectToken("missingPiecesDate"))?.ToLocalTime(),
            DamagedStatusId = (Guid?)jObject.SelectToken("itemDamagedStatusId"),
            DamagedStatusTime = ((DateTime?)jObject.SelectToken("itemDamagedStatusDate"))?.ToLocalTime(),
            StatusName = (string)jObject.SelectToken("status.name"),
            StatusDate = ((DateTime?)jObject.SelectToken("status.date"))?.ToLocalTime(),
            MaterialTypeId = (Guid?)jObject.SelectToken("materialTypeId"),
            PermanentLoanTypeId = (Guid?)jObject.SelectToken("permanentLoanTypeId"),
            TemporaryLoanTypeId = (Guid?)jObject.SelectToken("temporaryLoanTypeId"),
            PermanentLocationId = (Guid?)jObject.SelectToken("permanentLocationId"),
            TemporaryLocationId = (Guid?)jObject.SelectToken("temporaryLocationId"),
            EffectiveLocationId = (Guid?)jObject.SelectToken("effectiveLocationId"),
            InTransitDestinationServicePointId = (Guid?)jObject.SelectToken("inTransitDestinationServicePointId"),
            OrderItemId = (Guid?)jObject.SelectToken("purchaseOrderLineIdentifier"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            LastCheckInDateTime = ((DateTime?)jObject.SelectToken("lastCheckIn.dateTime"))?.ToLocalTime(),
            LastCheckInServicePointId = (Guid?)jObject.SelectToken("lastCheckIn.servicePointId"),
            LastCheckInStaffMemberId = (Guid?)jObject.SelectToken("lastCheckIn.staffMemberId"),
            Content = jObject.ToString(),
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
            new JProperty("hrid", ShortId?.ToString()),
            new JProperty("holdingsRecordId", HoldingId),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("accessionNumber", AccessionNumber),
            new JProperty("barcode", Barcode),
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
            new JProperty("missingPiecesDate", MissingPiecesTime?.ToUniversalTime()),
            new JProperty("itemDamagedStatusId", DamagedStatusId),
            new JProperty("itemDamagedStatusDate", DamagedStatusTime?.ToUniversalTime()),
            new JProperty("status", new JObject(
                new JProperty("name", StatusName),
                new JProperty("date", StatusDate?.ToUniversalTime()))),
            new JProperty("materialTypeId", MaterialTypeId),
            new JProperty("permanentLoanTypeId", PermanentLoanTypeId),
            new JProperty("temporaryLoanTypeId", TemporaryLoanTypeId),
            new JProperty("permanentLocationId", PermanentLocationId),
            new JProperty("temporaryLocationId", TemporaryLocationId),
            new JProperty("effectiveLocationId", EffectiveLocationId),
            new JProperty("inTransitDestinationServicePointId", InTransitDestinationServicePointId),
            new JProperty("purchaseOrderLineIdentifier", OrderItemId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("lastCheckIn", new JObject(
                new JProperty("dateTime", LastCheckInDateTime?.ToUniversalTime()),
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
