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
    // uc.holdings -> diku_mod_inventory_storage.holdings_record
    // Holding2 -> Holding
    [DisplayColumn(nameof(ShortId)), DisplayName("Holdings"), JsonConverter(typeof(JsonPathJsonConverter<Holding2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("holdings", Schema = "uc")]
    public partial class Holding2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Holding.json")))
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

        [Display(Name = "Holding Type", Order = 3)]
        public virtual HoldingType2 HoldingType { get; set; }

        [Column("holding_type_id"), Display(Name = "Holding Type", Order = 4), JsonProperty("holdingsTypeId")]
        public virtual Guid? HoldingTypeId { get; set; }

        [Display(Order = 5)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 6), JsonProperty("instanceId"), Required]
        public virtual Guid? InstanceId { get; set; }

        [Display(Order = 7), InverseProperty("Holding2s")]
        public virtual Location2 Location { get; set; }

        [Column("permanent_location_id"), Display(Name = "Location", Order = 8), JsonProperty("permanentLocationId"), Required]
        public virtual Guid? LocationId { get; set; }

        [Display(Name = "Temporary Location", Order = 9), InverseProperty("Holding2s1")]
        public virtual Location2 TemporaryLocation { get; set; }

        [Column("temporary_location_id"), Display(Name = "Temporary Location", Order = 10), JsonProperty("temporaryLocationId")]
        public virtual Guid? TemporaryLocationId { get; set; }

        [Display(Name = "Call Number Type", Order = 11)]
        public virtual CallNumberType2 CallNumberType { get; set; }

        [Column("call_number_type_id"), Display(Name = "Call Number Type", Order = 12), JsonProperty("callNumberTypeId")]
        public virtual Guid? CallNumberTypeId { get; set; }

        [Column("call_number_prefix"), Display(Name = "Call Number Prefix", Order = 13), JsonProperty("callNumberPrefix"), StringLength(1024)]
        public virtual string CallNumberPrefix { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 14), JsonProperty("callNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("call_number_suffix"), Display(Name = "Call Number Suffix", Order = 15), JsonProperty("callNumberSuffix"), StringLength(1024)]
        public virtual string CallNumberSuffix { get; set; }

        [Column("shelving_title"), Display(Name = "Shelving Title", Order = 16), JsonProperty("shelvingTitle"), StringLength(1024)]
        public virtual string ShelvingTitle { get; set; }

        [Column("acquisition_format"), Display(Name = "Acquisition Format", Order = 17), JsonProperty("acquisitionFormat"), StringLength(1024)]
        public virtual string AcquisitionFormat { get; set; }

        [Column("acquisition_method"), Display(Name = "Acquisition Method", Order = 18), JsonProperty("acquisitionMethod"), StringLength(1024)]
        public virtual string AcquisitionMethod { get; set; }

        [Column("receipt_status"), Display(Name = "Receipt Status", Order = 19), JsonProperty("receiptStatus"), StringLength(1024)]
        public virtual string ReceiptStatus { get; set; }

        [Display(Name = "Ill Policy", Order = 20)]
        public virtual IllPolicy2 IllPolicy { get; set; }

        [Column("ill_policy_id"), Display(Name = "Ill Policy", Order = 21), JsonProperty("illPolicyId")]
        public virtual Guid? IllPolicyId { get; set; }

        [Column("retention_policy"), Display(Name = "Retention Policy", Order = 22), JsonProperty("retentionPolicy"), StringLength(1024)]
        public virtual string RetentionPolicy { get; set; }

        [Column("digitization_policy"), Display(Name = "Digitization Policy", Order = 23), JsonProperty("digitizationPolicy"), StringLength(1024)]
        public virtual string DigitizationPolicy { get; set; }

        [Column("copy_number"), Display(Name = "Copy Number", Order = 24), JsonProperty("copyNumber"), StringLength(1024)]
        public virtual string CopyNumber { get; set; }

        [Column("number_of_items"), Display(Name = "Item Count", Order = 25), JsonProperty("numberOfItems"), StringLength(1024)]
        public virtual string ItemCount { get; set; }

        [Column("receiving_history_display_type"), Display(Name = "Receiving History Display Type", Order = 26), JsonProperty("receivingHistory.displayType"), StringLength(1024)]
        public virtual string ReceivingHistoryDisplayType { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 27), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 28), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 29), InverseProperty("Holding2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 30), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 32), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 33), InverseProperty("Holding2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 34), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Order = 36)]
        public virtual Source2 Source { get; set; }

        [Column("source_id"), Display(Name = "Source", Order = 37), JsonProperty("sourceId")]
        public virtual Guid? SourceId { get; set; }

        [Column("content"), CustomValidation(typeof(Holding), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 38), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Order = 39), JsonProperty("holdingsStatements")]
        public virtual ICollection<Extent> Extents { get; set; }

        [Display(Name = "Fees", Order = 40)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Holding Electronic Accesses", Order = 41), JsonProperty("electronicAccess")]
        public virtual ICollection<HoldingElectronicAccess> HoldingElectronicAccesses { get; set; }

        [Display(Name = "Holding Entries", Order = 42), JsonProperty("receivingHistory.entries")]
        public virtual ICollection<HoldingEntry> HoldingEntries { get; set; }

        [Display(Name = "Holding Former Ids", Order = 43), JsonConverter(typeof(ArrayJsonConverter<List<HoldingFormerId>, HoldingFormerId>), "Content"), JsonProperty("formerIds")]
        public virtual ICollection<HoldingFormerId> HoldingFormerIds { get; set; }

        [Display(Name = "Holding Notes", Order = 44), JsonProperty("notes")]
        public virtual ICollection<HoldingNote> HoldingNotes { get; set; }

        [Display(Name = "Holding Statistical Codes", Order = 45), JsonConverter(typeof(ArrayJsonConverter<List<HoldingStatisticalCode>, HoldingStatisticalCode>), "StatisticalCodeId"), JsonProperty("statisticalCodeIds")]
        public virtual ICollection<HoldingStatisticalCode> HoldingStatisticalCodes { get; set; }

        [Display(Name = "Holding Tags", Order = 46), JsonConverter(typeof(ArrayJsonConverter<List<HoldingTag>, HoldingTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<HoldingTag> HoldingTags { get; set; }

        [Display(Name = "Index Statements", Order = 47), JsonProperty("holdingsStatementsForIndexes")]
        public virtual ICollection<IndexStatement> IndexStatements { get; set; }

        [Display(Name = "Items", Order = 48)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Supplement Statements", Order = 49), JsonProperty("holdingsStatementsForSupplements")]
        public virtual ICollection<SupplementStatement> SupplementStatements { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ShortId)} = {ShortId}, {nameof(HoldingTypeId)} = {HoldingTypeId}, {nameof(InstanceId)} = {InstanceId}, {nameof(LocationId)} = {LocationId}, {nameof(TemporaryLocationId)} = {TemporaryLocationId}, {nameof(CallNumberTypeId)} = {CallNumberTypeId}, {nameof(CallNumberPrefix)} = {CallNumberPrefix}, {nameof(CallNumber)} = {CallNumber}, {nameof(CallNumberSuffix)} = {CallNumberSuffix}, {nameof(ShelvingTitle)} = {ShelvingTitle}, {nameof(AcquisitionFormat)} = {AcquisitionFormat}, {nameof(AcquisitionMethod)} = {AcquisitionMethod}, {nameof(ReceiptStatus)} = {ReceiptStatus}, {nameof(IllPolicyId)} = {IllPolicyId}, {nameof(RetentionPolicy)} = {RetentionPolicy}, {nameof(DigitizationPolicy)} = {DigitizationPolicy}, {nameof(CopyNumber)} = {CopyNumber}, {nameof(ItemCount)} = {ItemCount}, {nameof(ReceivingHistoryDisplayType)} = {ReceivingHistoryDisplayType}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(SourceId)} = {SourceId}, {nameof(Content)} = {Content}, {nameof(Extents)} = {(Extents != null ? $"{{ {string.Join(", ", Extents)} }}" : "")}, {nameof(HoldingElectronicAccesses)} = {(HoldingElectronicAccesses != null ? $"{{ {string.Join(", ", HoldingElectronicAccesses)} }}" : "")}, {nameof(HoldingEntries)} = {(HoldingEntries != null ? $"{{ {string.Join(", ", HoldingEntries)} }}" : "")}, {nameof(HoldingFormerIds)} = {(HoldingFormerIds != null ? $"{{ {string.Join(", ", HoldingFormerIds)} }}" : "")}, {nameof(HoldingNotes)} = {(HoldingNotes != null ? $"{{ {string.Join(", ", HoldingNotes)} }}" : "")}, {nameof(HoldingStatisticalCodes)} = {(HoldingStatisticalCodes != null ? $"{{ {string.Join(", ", HoldingStatisticalCodes)} }}" : "")}, {nameof(HoldingTags)} = {(HoldingTags != null ? $"{{ {string.Join(", ", HoldingTags)} }}" : "")}, {nameof(IndexStatements)} = {(IndexStatements != null ? $"{{ {string.Join(", ", IndexStatements)} }}" : "")}, {nameof(SupplementStatements)} = {(SupplementStatements != null ? $"{{ {string.Join(", ", SupplementStatements)} }}" : "")} }}";

        public static Holding2 FromJObject(JObject jObject) => jObject != null ? new Holding2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            ShortId = (int?)jObject.SelectToken("hrid"),
            HoldingTypeId = (Guid?)jObject.SelectToken("holdingsTypeId"),
            InstanceId = (Guid?)jObject.SelectToken("instanceId"),
            LocationId = (Guid?)jObject.SelectToken("permanentLocationId"),
            TemporaryLocationId = (Guid?)jObject.SelectToken("temporaryLocationId"),
            CallNumberTypeId = (Guid?)jObject.SelectToken("callNumberTypeId"),
            CallNumberPrefix = (string)jObject.SelectToken("callNumberPrefix"),
            CallNumber = (string)jObject.SelectToken("callNumber"),
            CallNumberSuffix = (string)jObject.SelectToken("callNumberSuffix"),
            ShelvingTitle = (string)jObject.SelectToken("shelvingTitle"),
            AcquisitionFormat = (string)jObject.SelectToken("acquisitionFormat"),
            AcquisitionMethod = (string)jObject.SelectToken("acquisitionMethod"),
            ReceiptStatus = (string)jObject.SelectToken("receiptStatus"),
            IllPolicyId = (Guid?)jObject.SelectToken("illPolicyId"),
            RetentionPolicy = (string)jObject.SelectToken("retentionPolicy"),
            DigitizationPolicy = (string)jObject.SelectToken("digitizationPolicy"),
            CopyNumber = (string)jObject.SelectToken("copyNumber"),
            ItemCount = (string)jObject.SelectToken("numberOfItems"),
            ReceivingHistoryDisplayType = (string)jObject.SelectToken("receivingHistory.displayType"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            SourceId = (Guid?)jObject.SelectToken("sourceId"),
            Content = jObject.ToString(),
            Extents = jObject.SelectToken("holdingsStatements")?.Where(jt => jt.HasValues).Select(jt => Extent.FromJObject((JObject)jt)).ToArray(),
            HoldingElectronicAccesses = jObject.SelectToken("electronicAccess")?.Where(jt => jt.HasValues).Select(jt => HoldingElectronicAccess.FromJObject((JObject)jt)).ToArray(),
            HoldingEntries = jObject.SelectToken("receivingHistory.entries")?.Where(jt => jt.HasValues).Select(jt => HoldingEntry.FromJObject((JObject)jt)).ToArray(),
            HoldingFormerIds = jObject.SelectToken("formerIds")?.Where(jt => jt.HasValues).Select(jt => HoldingFormerId.FromJObject((JValue)jt)).ToArray(),
            HoldingNotes = jObject.SelectToken("notes")?.Where(jt => jt.HasValues).Select(jt => HoldingNote.FromJObject((JObject)jt)).ToArray(),
            HoldingStatisticalCodes = jObject.SelectToken("statisticalCodeIds")?.Where(jt => jt.HasValues).Select(jt => HoldingStatisticalCode.FromJObject((JValue)jt)).ToArray(),
            HoldingTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => HoldingTag.FromJObject((JValue)jt)).ToArray(),
            IndexStatements = jObject.SelectToken("holdingsStatementsForIndexes")?.Where(jt => jt.HasValues).Select(jt => IndexStatement.FromJObject((JObject)jt)).ToArray(),
            SupplementStatements = jObject.SelectToken("holdingsStatementsForSupplements")?.Where(jt => jt.HasValues).Select(jt => SupplementStatement.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("hrid", ShortId?.ToString()),
            new JProperty("holdingsTypeId", HoldingTypeId),
            new JProperty("instanceId", InstanceId),
            new JProperty("permanentLocationId", LocationId),
            new JProperty("temporaryLocationId", TemporaryLocationId),
            new JProperty("callNumberTypeId", CallNumberTypeId),
            new JProperty("callNumberPrefix", CallNumberPrefix),
            new JProperty("callNumber", CallNumber),
            new JProperty("callNumberSuffix", CallNumberSuffix),
            new JProperty("shelvingTitle", ShelvingTitle),
            new JProperty("acquisitionFormat", AcquisitionFormat),
            new JProperty("acquisitionMethod", AcquisitionMethod),
            new JProperty("receiptStatus", ReceiptStatus),
            new JProperty("illPolicyId", IllPolicyId),
            new JProperty("retentionPolicy", RetentionPolicy),
            new JProperty("digitizationPolicy", DigitizationPolicy),
            new JProperty("copyNumber", CopyNumber),
            new JProperty("numberOfItems", ItemCount),
            new JProperty("receivingHistory", new JObject(
                new JProperty("displayType", ReceivingHistoryDisplayType),
                new JProperty("entries", HoldingEntries?.Select(he => he.ToJObject())))),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("sourceId", SourceId),
            new JProperty("holdingsStatements", Extents?.Select(e2 => e2.ToJObject())),
            new JProperty("electronicAccess", HoldingElectronicAccesses?.Select(hea => hea.ToJObject())),
            new JProperty("formerIds", HoldingFormerIds?.Select(hfi => hfi.ToJObject())),
            new JProperty("notes", HoldingNotes?.Select(hn => hn.ToJObject())),
            new JProperty("statisticalCodeIds", HoldingStatisticalCodes?.Select(hsc => hsc.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", HoldingTags?.Select(ht => ht.ToJObject())))),
            new JProperty("holdingsStatementsForIndexes", IndexStatements?.Select(@is => @is.ToJObject())),
            new JProperty("holdingsStatementsForSupplements", SupplementStatements?.Select(ss => ss.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
