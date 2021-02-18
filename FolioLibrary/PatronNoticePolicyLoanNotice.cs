using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.patron_notice_policy_loan_notices -> diku_mod_circulation_storage.patron_notice_policy
    // PatronNoticePolicyLoanNotice -> PatronNoticePolicy
    [DisplayColumn(nameof(Name)), DisplayName("Patron Notice Policy Loan Notices"), JsonConverter(typeof(JsonPathJsonConverter<PatronNoticePolicyLoanNotice>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("patron_notice_policy_loan_notices", Schema = "uc")]
    public partial class PatronNoticePolicyLoanNotice
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Patron Notice Policy", Order = 2)]
        public virtual PatronNoticePolicy2 PatronNoticePolicy { get; set; }

        [Column("patron_notice_policy_id"), Display(Name = "Patron Notice Policy", Order = 3), Required]
        public virtual Guid? PatronNoticePolicyId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Display(Order = 5)]
        public virtual Template2 Template { get; set; }

        [Column("template_id"), Display(Name = "Template", Order = 6), JsonProperty("templateId"), Required]
        public virtual Guid? TemplateId { get; set; }

        [Column("template_name"), Display(Name = "Template Name", Order = 7), JsonProperty("templateName"), StringLength(1024)]
        public virtual string TemplateName { get; set; }

        [Column("format"), Display(Order = 8), JsonProperty("format"), Required, StringLength(1024)]
        public virtual string Format { get; set; }

        [Column("frequency"), Display(Order = 9), JsonProperty("frequency"), StringLength(1024)]
        public virtual string Frequency { get; set; }

        [Column("real_time"), Display(Name = "Real Time", Order = 10), JsonProperty("realTime")]
        public virtual bool? RealTime { get; set; }

        [Column("send_options_send_how"), Display(Name = "Send Options Send How", Order = 11), JsonProperty("sendOptions.sendHow"), StringLength(1024)]
        public virtual string SendOptionsSendHow { get; set; }

        [Column("send_options_send_when"), Display(Name = "Send Options Send When", Order = 12), JsonProperty("sendOptions.sendWhen"), Required, StringLength(1024)]
        public virtual string SendOptionsSendWhen { get; set; }

        [Column("send_options_send_by_duration"), Display(Name = "Send Options Send By Duration", Order = 13), JsonProperty("sendOptions.sendBy.duration"), Required]
        public virtual int? SendOptionsSendByDuration { get; set; }

        [Column("send_options_send_by_interval_id"), Display(Name = "Send Options Send By Interval", Order = 14), JsonProperty("sendOptions.sendBy.intervalId"), Required, StringLength(1024)]
        public virtual string SendOptionsSendByInterval { get; set; }

        [Column("send_options_send_every_duration"), Display(Name = "Send Options Send Every Duration", Order = 15), JsonProperty("sendOptions.sendEvery.duration"), Required]
        public virtual int? SendOptionsSendEveryDuration { get; set; }

        [Column("send_options_send_every_interval_id"), Display(Name = "Send Options Send Every Interval", Order = 16), JsonProperty("sendOptions.sendEvery.intervalId"), Required, StringLength(1024)]
        public virtual string SendOptionsSendEveryInterval { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PatronNoticePolicyId)} = {PatronNoticePolicyId}, {nameof(Name)} = {Name}, {nameof(TemplateId)} = {TemplateId}, {nameof(TemplateName)} = {TemplateName}, {nameof(Format)} = {Format}, {nameof(Frequency)} = {Frequency}, {nameof(RealTime)} = {RealTime}, {nameof(SendOptionsSendHow)} = {SendOptionsSendHow}, {nameof(SendOptionsSendWhen)} = {SendOptionsSendWhen}, {nameof(SendOptionsSendByDuration)} = {SendOptionsSendByDuration}, {nameof(SendOptionsSendByInterval)} = {SendOptionsSendByInterval}, {nameof(SendOptionsSendEveryDuration)} = {SendOptionsSendEveryDuration}, {nameof(SendOptionsSendEveryInterval)} = {SendOptionsSendEveryInterval} }}";

        public static PatronNoticePolicyLoanNotice FromJObject(JObject jObject) => jObject != null ? new PatronNoticePolicyLoanNotice
        {
            Name = (string)jObject.SelectToken("name"),
            TemplateId = (Guid?)jObject.SelectToken("templateId"),
            TemplateName = (string)jObject.SelectToken("templateName"),
            Format = (string)jObject.SelectToken("format"),
            Frequency = (string)jObject.SelectToken("frequency"),
            RealTime = (bool?)jObject.SelectToken("realTime"),
            SendOptionsSendHow = (string)jObject.SelectToken("sendOptions.sendHow"),
            SendOptionsSendWhen = (string)jObject.SelectToken("sendOptions.sendWhen"),
            SendOptionsSendByDuration = (int?)jObject.SelectToken("sendOptions.sendBy.duration"),
            SendOptionsSendByInterval = (string)jObject.SelectToken("sendOptions.sendBy.intervalId"),
            SendOptionsSendEveryDuration = (int?)jObject.SelectToken("sendOptions.sendEvery.duration"),
            SendOptionsSendEveryInterval = (string)jObject.SelectToken("sendOptions.sendEvery.intervalId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("name", Name),
            new JProperty("templateId", TemplateId),
            new JProperty("templateName", TemplateName),
            new JProperty("format", Format),
            new JProperty("frequency", Frequency),
            new JProperty("realTime", RealTime),
            new JProperty("sendOptions", new JObject(
                new JProperty("sendHow", SendOptionsSendHow),
                new JProperty("sendWhen", SendOptionsSendWhen),
                new JProperty("sendBy", new JObject(
                    new JProperty("duration", SendOptionsSendByDuration),
                    new JProperty("intervalId", SendOptionsSendByInterval))),
                new JProperty("sendEvery", new JObject(
                    new JProperty("duration", SendOptionsSendEveryDuration),
                    new JProperty("intervalId", SendOptionsSendEveryInterval)))))).RemoveNullAndEmptyProperties();
    }
}
