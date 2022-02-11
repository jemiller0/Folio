using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.journal_records -> uchicago_mod_source_record_manager.journal_records
    // JournalRecord2 -> JournalRecord
    [DisplayColumn(nameof(Title)), DisplayName("Journal Records"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("journal_records", Schema = "uc")]
    public partial class JournalRecord2
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Job Execution", Order = 2)]
        public virtual JobExecution2 JobExecution { get; set; }

        [Column("job_execution_id"), Display(Name = "Job Execution", Order = 3)]
        public virtual Guid? JobExecutionId { get; set; }

        [Display(Order = 4)]
        public virtual Source2 Source { get; set; }

        [Column("source_id"), Display(Name = "Source", Order = 5)]
        public virtual Guid? SourceId { get; set; }

        [Column("entity_type"), Display(Name = "Entity Type", Order = 6), StringLength(1024)]
        public virtual string EntityType { get; set; }

        [Column("entity_id"), Display(Name = "Entity Id", Order = 7), StringLength(1024)]
        public virtual string EntityId { get; set; }

        [Column("entity_hrid"), Display(Name = "Entity Hrid", Order = 8), StringLength(1024)]
        public virtual string EntityHrid { get; set; }

        [Column("action_type"), Display(Name = "Action Type", Order = 9), StringLength(1024)]
        public virtual string ActionType { get; set; }

        [Column("action_status"), Display(Name = "Action Status", Order = 10), StringLength(1024)]
        public virtual string ActionStatus { get; set; }

        [Column("action_date"), DataType(DataType.Date), Display(Name = "Action Date", Order = 11), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ActionDate { get; set; }

        [Column("source_record_order"), Display(Name = "Source Record Order", Order = 12)]
        public virtual int? SourceRecordOrder { get; set; }

        [Column("error"), Display(Order = 13), StringLength(1024)]
        public virtual string Error { get; set; }

        [Column("title"), Display(Order = 14), StringLength(1024)]
        public virtual string Title { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(JobExecutionId)} = {JobExecutionId}, {nameof(SourceId)} = {SourceId}, {nameof(EntityType)} = {EntityType}, {nameof(EntityId)} = {EntityId}, {nameof(EntityHrid)} = {EntityHrid}, {nameof(ActionType)} = {ActionType}, {nameof(ActionStatus)} = {ActionStatus}, {nameof(ActionDate)} = {ActionDate}, {nameof(SourceRecordOrder)} = {SourceRecordOrder}, {nameof(Error)} = {Error}, {nameof(Title)} = {Title} }}";

        public static JournalRecord2 FromJObject(JObject jObject) => jObject != null ? new JournalRecord2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
