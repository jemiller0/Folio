using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fixed_due_date_schedule_schedules -> diku_mod_circulation_storage.fixed_due_date_schedule
    // FixedDueDateScheduleSchedule -> FixedDueDateSchedule
    [DisplayColumn(nameof(Id)), DisplayName("Fixed Due Date Schedule Schedules"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fixed_due_date_schedule_schedules", Schema = "uc")]
    public partial class FixedDueDateScheduleSchedule
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Fixed Due Date Schedule", Order = 2)]
        public virtual FixedDueDateSchedule2 FixedDueDateSchedule { get; set; }

        [Column("fixed_due_date_schedule_id"), Display(Name = "Fixed Due Date Schedule", Order = 3), Required]
        public virtual Guid? FixedDueDateScheduleId { get; set; }

        [Column("from"), Display(Order = 4), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("from"), Required]
        public virtual DateTime? From { get; set; }

        [Column("to"), Display(Order = 5), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("to"), Required]
        public virtual DateTime? To { get; set; }

        [Column("due"), Display(Order = 6), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("due"), Required]
        public virtual DateTime? Due { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FixedDueDateScheduleId)} = {FixedDueDateScheduleId}, {nameof(From)} = {From}, {nameof(To)} = {To}, {nameof(Due)} = {Due} }}";

        public static FixedDueDateScheduleSchedule FromJObject(JObject jObject) => jObject != null ? new FixedDueDateScheduleSchedule
        {
            From = ((DateTime?)jObject.SelectToken("from"))?.ToLocalTime(),
            To = ((DateTime?)jObject.SelectToken("to"))?.ToLocalTime(),
            Due = ((DateTime?)jObject.SelectToken("due"))?.ToLocalTime()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("from", From?.ToUniversalTime()),
            new JProperty("to", To?.ToUniversalTime()),
            new JProperty("due", Due?.ToUniversalTime())).RemoveNullAndEmptyProperties();
    }
}
