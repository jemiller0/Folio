using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.job_monitorings -> uchicago_mod_source_record_manager.job_monitoring
    // JobMonitoring2 -> JobMonitoring
    [DisplayColumn(nameof(Id)), DisplayName("Job Monitorings"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("job_monitorings", Schema = "uc")]
    public partial class JobMonitoring2
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Job Execution", Order = 2)]
        public virtual JobExecution2 JobExecution { get; set; }

        [Column("job_execution_id"), Display(Name = "Job Execution", Order = 3)]
        public virtual Guid? JobExecutionId { get; set; }

        [Column("last_event_timestamp"), Display(Name = "Last Event Timestamp", Order = 4), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public virtual DateTime? LastEventTimestamp { get; set; }

        [Column("notification_sent"), Display(Name = "Notification Sent", Order = 5)]
        public virtual bool? NotificationSent { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(JobExecutionId)} = {JobExecutionId}, {nameof(LastEventTimestamp)} = {LastEventTimestamp}, {nameof(NotificationSent)} = {NotificationSent} }}";

        public static JobMonitoring2 FromJObject(JObject jObject) => jObject != null ? new JobMonitoring2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
