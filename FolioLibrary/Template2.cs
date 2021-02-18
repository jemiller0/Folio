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
    // uc.templates -> diku_mod_template_engine.template
    // Template2 -> Template
    [DisplayColumn(nameof(Name)), DisplayName("Templates"), JsonConverter(typeof(JsonPathJsonConverter<Template2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("templates", Schema = "uc")]
    public partial class Template2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Template.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("active"), Display(Order = 3), JsonProperty("active")]
        public virtual bool? Active { get; set; }

        [Column("category"), Display(Order = 4), JsonProperty("category"), StringLength(1024)]
        public virtual string Category { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("template_resolver"), Display(Name = "Template Resolver", Order = 6), JsonProperty("templateResolver"), Required, StringLength(1024)]
        public virtual string TemplateResolver { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 8), InverseProperty("Template2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 9), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 12), InverseProperty("Template2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Template), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 15), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Fee Types", Order = 16)]
        public virtual ICollection<FeeType2> FeeType2s { get; set; }

        [Display(Name = "Fee Types 1", Order = 17)]
        public virtual ICollection<FeeType2> FeeType2s1 { get; set; }

        [Display(Name = "Owners", Order = 18)]
        public virtual ICollection<Owner2> Owner2s { get; set; }

        [Display(Name = "Owners 1", Order = 19)]
        public virtual ICollection<Owner2> Owner2s1 { get; set; }

        [Display(Name = "Patron Notice Policy Fee Fine Notices", Order = 20)]
        public virtual ICollection<PatronNoticePolicyFeeFineNotice> PatronNoticePolicyFeeFineNotices { get; set; }

        [Display(Name = "Patron Notice Policy Loan Notices", Order = 21)]
        public virtual ICollection<PatronNoticePolicyLoanNotice> PatronNoticePolicyLoanNotices { get; set; }

        [Display(Name = "Patron Notice Policy Request Notices", Order = 22)]
        public virtual ICollection<PatronNoticePolicyRequestNotice> PatronNoticePolicyRequestNotices { get; set; }

        [Display(Name = "Scheduled Notices", Order = 23)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        [Display(Name = "Template Output Formats", Order = 24), JsonConverter(typeof(ArrayJsonConverter<List<TemplateOutputFormat>, TemplateOutputFormat>), "Content"), JsonProperty("outputFormats")]
        public virtual ICollection<TemplateOutputFormat> TemplateOutputFormats { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Active)} = {Active}, {nameof(Category)} = {Category}, {nameof(Description)} = {Description}, {nameof(TemplateResolver)} = {TemplateResolver}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(TemplateOutputFormats)} = {(TemplateOutputFormats != null ? $"{{ {string.Join(", ", TemplateOutputFormats)} }}" : "")} }}";

        public static Template2 FromJObject(JObject jObject) => jObject != null ? new Template2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Active = (bool?)jObject.SelectToken("active"),
            Category = (string)jObject.SelectToken("category"),
            Description = (string)jObject.SelectToken("description"),
            TemplateResolver = (string)jObject.SelectToken("templateResolver"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            TemplateOutputFormats = jObject.SelectToken("outputFormats")?.Where(jt => jt.HasValues).Select(jt => TemplateOutputFormat.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("active", Active),
            new JProperty("category", Category),
            new JProperty("description", Description),
            new JProperty("templateResolver", TemplateResolver),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("outputFormats", TemplateOutputFormats?.Select(tof => tof.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
