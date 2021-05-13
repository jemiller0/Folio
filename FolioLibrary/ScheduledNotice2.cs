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
    // uc.scheduled_notices -> diku_mod_circulation_storage.scheduled_notice
    // ScheduledNotice2 -> ScheduledNotice
    [DisplayColumn(nameof(Id)), DisplayName("Scheduled Notices"), JsonConverter(typeof(JsonPathJsonConverter<ScheduledNotice2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("scheduled_notices", Schema = "uc")]
    public partial class ScheduledNotice2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ScheduledNotice.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Loan2 Loan { get; set; }

        [Column("loan_id"), Display(Name = "Loan", Order = 3), JsonProperty("loanId")]
        public virtual Guid? LoanId { get; set; }

        [Display(Order = 4)]
        public virtual Request2 Request { get; set; }

        [Column("request_id"), Display(Name = "Request", Order = 5), JsonProperty("requestId")]
        public virtual Guid? RequestId { get; set; }

        [Display(Order = 6)]
        public virtual Payment2 Payment { get; set; }

        [Column("payment_id"), Display(Name = "Payment", Order = 7), JsonProperty("feeFineActionId")]
        public virtual Guid? PaymentId { get; set; }

        [Display(Name = "Recipient User", Order = 8), InverseProperty("ScheduledNotice2s")]
        public virtual User2 RecipientUser { get; set; }

        [Column("recipient_user_id"), Display(Name = "Recipient User", Order = 9), JsonProperty("recipientUserId")]
        public virtual Guid? RecipientUserId { get; set; }

        [Column("next_run_time"), DataType(DataType.DateTime), Display(Name = "Next Run Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("nextRunTime"), Required]
        public virtual DateTime? NextRunTime { get; set; }

        [Column("triggering_event"), Display(Name = "Triggering Event", Order = 11), JsonProperty("triggeringEvent"), RegularExpression(@"^(Hold expiration|Request expiration|Due date|Overdue fine returned|Overdue fine renewed|Aged to lost|Aged to lost - fine charged|Aged to lost & item returned - fine adjusted|Aged to lost & item replaced - fine adjusted)$"), StringLength(1024)]
        public virtual string TriggeringEvent { get; set; }

        [Column("notice_config_timing"), Display(Name = "Notice Config Timing", Order = 12), JsonProperty("noticeConfig.timing"), RegularExpression(@"^(Upon At|Before|After)$"), Required, StringLength(1024)]
        public virtual string NoticeConfigTiming { get; set; }

        [Column("notice_config_recurring_period_duration"), Display(Name = "Notice Config Recurring Period Duration", Order = 13), JsonProperty("noticeConfig.recurringPeriod.duration"), Required]
        public virtual int? NoticeConfigRecurringPeriodDuration { get; set; }

        [Column("notice_config_recurring_period_interval_id"), Display(Name = "Notice Config Recurring Period Interval", Order = 14), JsonProperty("noticeConfig.recurringPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string NoticeConfigRecurringPeriodInterval { get; set; }

        [Display(Name = "Notice Config Template", Order = 15)]
        public virtual Template2 NoticeConfigTemplate { get; set; }

        [Column("notice_config_template_id"), Display(Name = "Notice Config Template", Order = 16), JsonProperty("noticeConfig.templateId"), Required]
        public virtual Guid? NoticeConfigTemplateId { get; set; }

        [Column("notice_config_format"), Display(Name = "Notice Config Format", Order = 17), JsonProperty("noticeConfig.format"), RegularExpression(@"^(Email|SMS|Print)$"), Required, StringLength(1024)]
        public virtual string NoticeConfigFormat { get; set; }

        [Column("notice_config_send_in_real_time"), Display(Name = "Notice Config Send In Real Time", Order = 18), JsonProperty("noticeConfig.sendInRealTime")]
        public virtual bool? NoticeConfigSendInRealTime { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 20), InverseProperty("ScheduledNotice2s1")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 21), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 23), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 24), InverseProperty("ScheduledNotice2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 25), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ScheduledNotice), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 27), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LoanId)} = {LoanId}, {nameof(RequestId)} = {RequestId}, {nameof(PaymentId)} = {PaymentId}, {nameof(RecipientUserId)} = {RecipientUserId}, {nameof(NextRunTime)} = {NextRunTime}, {nameof(TriggeringEvent)} = {TriggeringEvent}, {nameof(NoticeConfigTiming)} = {NoticeConfigTiming}, {nameof(NoticeConfigRecurringPeriodDuration)} = {NoticeConfigRecurringPeriodDuration}, {nameof(NoticeConfigRecurringPeriodInterval)} = {NoticeConfigRecurringPeriodInterval}, {nameof(NoticeConfigTemplateId)} = {NoticeConfigTemplateId}, {nameof(NoticeConfigFormat)} = {NoticeConfigFormat}, {nameof(NoticeConfigSendInRealTime)} = {NoticeConfigSendInRealTime}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static ScheduledNotice2 FromJObject(JObject jObject) => jObject != null ? new ScheduledNotice2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            LoanId = (Guid?)jObject.SelectToken("loanId"),
            RequestId = (Guid?)jObject.SelectToken("requestId"),
            PaymentId = (Guid?)jObject.SelectToken("feeFineActionId"),
            RecipientUserId = (Guid?)jObject.SelectToken("recipientUserId"),
            NextRunTime = ((DateTime?)jObject.SelectToken("nextRunTime"))?.ToLocalTime(),
            TriggeringEvent = (string)jObject.SelectToken("triggeringEvent"),
            NoticeConfigTiming = (string)jObject.SelectToken("noticeConfig.timing"),
            NoticeConfigRecurringPeriodDuration = (int?)jObject.SelectToken("noticeConfig.recurringPeriod.duration"),
            NoticeConfigRecurringPeriodInterval = (string)jObject.SelectToken("noticeConfig.recurringPeriod.intervalId"),
            NoticeConfigTemplateId = (Guid?)jObject.SelectToken("noticeConfig.templateId"),
            NoticeConfigFormat = (string)jObject.SelectToken("noticeConfig.format"),
            NoticeConfigSendInRealTime = (bool?)jObject.SelectToken("noticeConfig.sendInRealTime"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("loanId", LoanId),
            new JProperty("requestId", RequestId),
            new JProperty("feeFineActionId", PaymentId),
            new JProperty("recipientUserId", RecipientUserId),
            new JProperty("nextRunTime", NextRunTime?.ToUniversalTime()),
            new JProperty("triggeringEvent", TriggeringEvent),
            new JProperty("noticeConfig", new JObject(
                new JProperty("timing", NoticeConfigTiming),
                new JProperty("recurringPeriod", new JObject(
                    new JProperty("duration", NoticeConfigRecurringPeriodDuration),
                    new JProperty("intervalId", NoticeConfigRecurringPeriodInterval))),
                new JProperty("templateId", NoticeConfigTemplateId),
                new JProperty("format", NoticeConfigFormat),
                new JProperty("sendInRealTime", NoticeConfigSendInRealTime))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
