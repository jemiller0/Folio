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
    // uc.user_summaries -> diku_mod_patron_blocks.user_summary
    // UserSummary2 -> UserSummary
    [DisplayColumn(nameof(Id)), DisplayName("User Summaries"), JsonConverter(typeof(JsonPathJsonConverter<UserSummary2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_summaries", Schema = "uc")]
    public partial class UserSummary2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.UserSummary.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), Display(Order = 2), JsonProperty("_version")]
        public virtual int? Version { get; set; }

        [Display(Order = 3), InverseProperty("UserSummary2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 4), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 6), InverseProperty("UserSummary2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 10), InverseProperty("UserSummary2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 11), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(UserSummary), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 13), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "User Summary Open Fees Fines", Order = 14), JsonProperty("openFeesFines")]
        public virtual ICollection<UserSummaryOpenFeesFine> UserSummaryOpenFeesFines { get; set; }

        [Display(Name = "User Summary Open Loans", Order = 15), JsonProperty("openLoans")]
        public virtual ICollection<UserSummaryOpenLoan> UserSummaryOpenLoans { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(UserId)} = {UserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(UserSummaryOpenFeesFines)} = {(UserSummaryOpenFeesFines != null ? $"{{ {string.Join(", ", UserSummaryOpenFeesFines)} }}" : "")}, {nameof(UserSummaryOpenLoans)} = {(UserSummaryOpenLoans != null ? $"{{ {string.Join(", ", UserSummaryOpenLoans)} }}" : "")} }}";

        public static UserSummary2 FromJObject(JObject jObject) => jObject != null ? new UserSummary2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            UserSummaryOpenFeesFines = jObject.SelectToken("openFeesFines")?.Where(jt => jt.HasValues).Select(jt => UserSummaryOpenFeesFine.FromJObject((JObject)jt)).ToArray(),
            UserSummaryOpenLoans = jObject.SelectToken("openLoans")?.Where(jt => jt.HasValues).Select(jt => UserSummaryOpenLoan.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("userId", UserId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("openFeesFines", UserSummaryOpenFeesFines?.Select(usoff => usoff.ToJObject())),
            new JProperty("openLoans", UserSummaryOpenLoans?.Select(usol => usol.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
