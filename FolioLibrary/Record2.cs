using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.records -> diku_mod_source_record_storage.records_lb
    // Record2 -> Record
    [DisplayColumn(nameof(Id)), DisplayName("Records"), JsonConverter(typeof(JsonPathJsonConverter<Record2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("records", Schema = "uc")]
    public partial class Record2
    {
        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Snapshot2 Snapshot { get; set; }

        [Column("snapshot_id"), Display(Name = "Snapshot", Order = 3), JsonProperty("snapshotId"), Required]
        public virtual Guid? SnapshotId { get; set; }

        [Column("matched_id"), Display(Name = "Matched Id", Order = 4), JsonProperty("matchedId"), Required]
        public virtual Guid? MatchedId { get; set; }

        [Column("generation"), Display(Order = 5), JsonProperty("generation"), Required]
        public virtual int? Generation { get; set; }

        [Column("record_type"), Display(Name = "Record Type", Order = 6), JsonProperty("recordType"), RegularExpression(@"^(MARC)$"), Required, StringLength(1024)]
        public virtual string RecordType { get; set; }

        [Display(Order = 7)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 8), JsonProperty("externalIdsHolder.instanceId")]
        public virtual Guid? InstanceId { get; set; }

        [Column("state"), Display(Order = 9), JsonProperty("state"), Required, StringLength(1024)]
        public virtual string State { get; set; }

        [Column("leader_record_status"), Display(Name = "Leader Record Status", Order = 10), JsonProperty("leaderRecordStatus"), StringLength(1)]
        public virtual string LeaderRecordStatus { get; set; }

        [Column("order"), Display(Order = 11), JsonProperty("order")]
        public virtual int? Order { get; set; }

        [Column("suppress_discovery"), Display(Name = "Suppress Discovery", Order = 12), JsonProperty("additionalInfo.suppressDiscovery")]
        public virtual bool? SuppressDiscovery { get; set; }

        [Display(Name = "Creation User", Order = 13), InverseProperty("Record2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("creation_user_id"), Display(Name = "Creation User", Order = 14), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("Record2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("last_write_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 18), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("instance_hrid"), Display(Name = "Instance Hrid", Order = 19), StringLength(1024)]
        public virtual string InstanceHrid { get; set; }

        [Display(Name = "Error Record 2", Order = 20)]
        public virtual ErrorRecord2 ErrorRecord2 { get; set; }

        [Display(Name = "Marc Record 2", Order = 21)]
        public virtual MarcRecord2 MarcRecord2 { get; set; }

        [Display(Name = "Raw Record 2", Order = 22)]
        public virtual RawRecord2 RawRecord2 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(SnapshotId)} = {SnapshotId}, {nameof(MatchedId)} = {MatchedId}, {nameof(Generation)} = {Generation}, {nameof(RecordType)} = {RecordType}, {nameof(InstanceId)} = {InstanceId}, {nameof(State)} = {State}, {nameof(LeaderRecordStatus)} = {LeaderRecordStatus}, {nameof(Order)} = {Order}, {nameof(SuppressDiscovery)} = {SuppressDiscovery}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(InstanceHrid)} = {InstanceHrid} }}";

        public static Record2 FromJObject(JObject jObject) => jObject != null ? new Record2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            SnapshotId = (Guid?)jObject.SelectToken("snapshotId"),
            MatchedId = (Guid?)jObject.SelectToken("matchedId"),
            Generation = (int?)jObject.SelectToken("generation"),
            RecordType = (string)jObject.SelectToken("recordType"),
            InstanceId = (Guid?)jObject.SelectToken("externalIdsHolder.instanceId"),
            State = (string)jObject.SelectToken("state"),
            LeaderRecordStatus = (string)jObject.SelectToken("leaderRecordStatus"),
            Order = (int?)jObject.SelectToken("order"),
            SuppressDiscovery = (bool?)jObject.SelectToken("additionalInfo.suppressDiscovery"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("snapshotId", SnapshotId),
            new JProperty("matchedId", MatchedId),
            new JProperty("generation", Generation),
            new JProperty("recordType", RecordType),
            new JProperty("externalIdsHolder", new JObject(
                new JProperty("instanceId", InstanceId))),
            new JProperty("state", State),
            new JProperty("leaderRecordStatus", LeaderRecordStatus),
            new JProperty("order", Order),
            new JProperty("additionalInfo", new JObject(
                new JProperty("suppressDiscovery", SuppressDiscovery))),
            new JProperty("metadata", new JObject(
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime())))).RemoveNullAndEmptyProperties();
    }
}
