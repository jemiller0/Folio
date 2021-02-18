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
    // uc.patron_notice_policies -> diku_mod_circulation_storage.patron_notice_policy
    // PatronNoticePolicy2 -> PatronNoticePolicy
    [DisplayColumn(nameof(Name)), DisplayName("Patron Notice Policies"), JsonConverter(typeof(JsonPathJsonConverter<PatronNoticePolicy2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("patron_notice_policies", Schema = "uc")]
    public partial class PatronNoticePolicy2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.PatronNoticePolicy.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("active"), Display(Order = 4), JsonProperty("active")]
        public virtual bool? Active { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 6), InverseProperty("PatronNoticePolicy2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 10), InverseProperty("PatronNoticePolicy2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 11), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(PatronNoticePolicy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 13), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Patron Notice Policy Fee Fine Notices", Order = 14), JsonProperty("feeFineNotices")]
        public virtual ICollection<PatronNoticePolicyFeeFineNotice> PatronNoticePolicyFeeFineNotices { get; set; }

        [Display(Name = "Patron Notice Policy Loan Notices", Order = 15), JsonProperty("loanNotices")]
        public virtual ICollection<PatronNoticePolicyLoanNotice> PatronNoticePolicyLoanNotices { get; set; }

        [Display(Name = "Patron Notice Policy Request Notices", Order = 16), JsonProperty("requestNotices")]
        public virtual ICollection<PatronNoticePolicyRequestNotice> PatronNoticePolicyRequestNotices { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(Active)} = {Active}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(PatronNoticePolicyFeeFineNotices)} = {(PatronNoticePolicyFeeFineNotices != null ? $"{{ {string.Join(", ", PatronNoticePolicyFeeFineNotices)} }}" : "")}, {nameof(PatronNoticePolicyLoanNotices)} = {(PatronNoticePolicyLoanNotices != null ? $"{{ {string.Join(", ", PatronNoticePolicyLoanNotices)} }}" : "")}, {nameof(PatronNoticePolicyRequestNotices)} = {(PatronNoticePolicyRequestNotices != null ? $"{{ {string.Join(", ", PatronNoticePolicyRequestNotices)} }}" : "")} }}";

        public static PatronNoticePolicy2 FromJObject(JObject jObject) => jObject != null ? new PatronNoticePolicy2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            Active = (bool?)jObject.SelectToken("active"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            PatronNoticePolicyFeeFineNotices = jObject.SelectToken("feeFineNotices")?.Where(jt => jt.HasValues).Select(jt => PatronNoticePolicyFeeFineNotice.FromJObject((JObject)jt)).ToArray(),
            PatronNoticePolicyLoanNotices = jObject.SelectToken("loanNotices")?.Where(jt => jt.HasValues).Select(jt => PatronNoticePolicyLoanNotice.FromJObject((JObject)jt)).ToArray(),
            PatronNoticePolicyRequestNotices = jObject.SelectToken("requestNotices")?.Where(jt => jt.HasValues).Select(jt => PatronNoticePolicyRequestNotice.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description),
            new JProperty("active", Active),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("feeFineNotices", PatronNoticePolicyFeeFineNotices?.Select(pnpffn => pnpffn.ToJObject())),
            new JProperty("loanNotices", PatronNoticePolicyLoanNotices?.Select(pnpln => pnpln.ToJObject())),
            new JProperty("requestNotices", PatronNoticePolicyRequestNotices?.Select(pnprn => pnprn.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
