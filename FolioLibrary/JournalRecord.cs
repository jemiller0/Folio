using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("journal_records", Schema = "diku_mod_source_record_manager")]
    public partial class JournalRecord
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Job Execution", Order = 2)]
        public virtual JobExecution JobExecution { get; set; }

        [Column("job_execution_id"), Display(Name = "Job Execution", Order = 3), Editable(false)]
        public virtual Guid? JobExecutionId { get; set; }

        [Column("source_id"), Display(Name = "Source Id", Order = 4), Editable(false)]
        public virtual Guid? SourceId { get; set; }

        [Column("entity_type"), Display(Name = "Entity Type", Order = 5), Editable(false), StringLength(1024)]
        public virtual string EntityType { get; set; }

        [Column("entity_id"), Display(Name = "Entity Id", Order = 6), Editable(false), StringLength(1024)]
        public virtual string EntityId { get; set; }

        [Column("entity_hrid"), Display(Name = "Entity Hrid", Order = 7), Editable(false), StringLength(1024)]
        public virtual string EntityHrid { get; set; }

        [Column("action_type"), Display(Name = "Action Type", Order = 8), Editable(false), StringLength(1024)]
        public virtual string ActionType { get; set; }

        [Column("action_status"), Display(Name = "Action Status", Order = 9), Editable(false), StringLength(1024)]
        public virtual string ActionStatus { get; set; }

        [Column("action_date"), DataType(DataType.Date), Display(Name = "Action Date", Order = 10), DisplayFormat(DataFormatString = "{0:d}"), Editable(false)]
        public virtual DateTime? ActionDate { get; set; }

        [Column("source_record_order"), Display(Name = "Source Record Order", Order = 11), Editable(false)]
        public virtual int? SourceRecordOrder { get; set; }

        [Column("error"), Display(Order = 12), Editable(false), StringLength(1024)]
        public virtual string Error { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(JobExecutionId)} = {JobExecutionId}, {nameof(SourceId)} = {SourceId}, {nameof(EntityType)} = {EntityType}, {nameof(EntityId)} = {EntityId}, {nameof(EntityHrid)} = {EntityHrid}, {nameof(ActionType)} = {ActionType}, {nameof(ActionStatus)} = {ActionStatus}, {nameof(ActionDate)} = {ActionDate}, {nameof(SourceRecordOrder)} = {SourceRecordOrder}, {nameof(Error)} = {Error} }}";
    }
}
