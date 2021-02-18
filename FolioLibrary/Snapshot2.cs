using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.snapshots -> diku_mod_source_record_storage.snapshots_lb
    // Snapshot2 -> Snapshot
    [DisplayColumn(nameof(Id)), DisplayName("Snapshots"), JsonConverter(typeof(JsonPathJsonConverter<Snapshot2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("snapshots", Schema = "uc")]
    public partial class Snapshot2
    {
        [Column("id"), JsonProperty("jobExecutionId"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("status"), Display(Order = 2), JsonProperty("status"), RegularExpression(@"^(PARENT|NEW|FILE_UPLOADED|PARSING_IN_PROGRESS|PARSING_FINISHED|PROCESSING_IN_PROGRESS|PROCESSING_FINISHED|COMMIT_IN_PROGRESS|COMMITTED|ERROR|DISCARDED)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("processing_started_date"), DataType(DataType.Date), Display(Name = "Processing Started Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("processingStartedDate")]
        public virtual DateTime? ProcessingStartedDate { get; set; }

        [Display(Name = "Creation User", Order = 4), InverseProperty("Snapshot2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("creation_user_id"), Display(Name = "Creation User", Order = 5), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Last Write User", Order = 7), InverseProperty("Snapshot2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("last_write_user_id"), Display(Name = "Last Write User", Order = 8), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Records", Order = 10)]
        public virtual ICollection<Record2> Record2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Status)} = {Status}, {nameof(ProcessingStartedDate)} = {ProcessingStartedDate}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime} }}";

        public static Snapshot2 FromJObject(JObject jObject) => jObject != null ? new Snapshot2
        {
            Id = (Guid?)jObject.SelectToken("jobExecutionId"),
            Status = (string)jObject.SelectToken("status"),
            ProcessingStartedDate = ((DateTime?)jObject.SelectToken("processingStartedDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("jobExecutionId", Id),
            new JProperty("status", Status),
            new JProperty("processingStartedDate", ProcessingStartedDate?.ToUniversalTime()),
            new JProperty("metadata", new JObject(
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime())))).RemoveNullAndEmptyProperties();
    }
}
