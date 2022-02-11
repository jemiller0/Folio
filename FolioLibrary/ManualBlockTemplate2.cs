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
    // uc.manual_block_templates -> uchicago_mod_feesfines.manual_block_templates
    // ManualBlockTemplate2 -> ManualBlockTemplate
    [DisplayColumn(nameof(Name)), DisplayName("Manual Block Templates"), JsonConverter(typeof(JsonPathJsonConverter<ManualBlockTemplate2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("manual_block_templates", Schema = "uc")]
    public partial class ManualBlockTemplate2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ManualBlockTemplate.json")))
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

        [Column("code"), Display(Order = 3), JsonProperty("code"), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("desc"), Display(Order = 4), JsonProperty("desc"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("block_template_desc"), Display(Name = "Block Template Description", Order = 5), JsonProperty("blockTemplate.desc"), StringLength(1024)]
        public virtual string BlockTemplateDescription { get; set; }

        [Column("block_template_patron_message"), Display(Name = "Block Template Patron Message", Order = 6), JsonProperty("blockTemplate.patronMessage"), StringLength(1024)]
        public virtual string BlockTemplatePatronMessage { get; set; }

        [Column("block_template_borrowing"), Display(Name = "Block Template Borrowing", Order = 7), JsonProperty("blockTemplate.borrowing")]
        public virtual bool? BlockTemplateBorrowing { get; set; }

        [Column("block_template_renewals"), Display(Name = "Block Template Renewals", Order = 8), JsonProperty("blockTemplate.renewals")]
        public virtual bool? BlockTemplateRenewals { get; set; }

        [Column("block_template_requests"), Display(Name = "Block Template Requests", Order = 9), JsonProperty("blockTemplate.requests")]
        public virtual bool? BlockTemplateRequests { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 11), InverseProperty("ManualBlockTemplate2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("ManualBlockTemplate2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ManualBlockTemplate), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 18), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(BlockTemplateDescription)} = {BlockTemplateDescription}, {nameof(BlockTemplatePatronMessage)} = {BlockTemplatePatronMessage}, {nameof(BlockTemplateBorrowing)} = {BlockTemplateBorrowing}, {nameof(BlockTemplateRenewals)} = {BlockTemplateRenewals}, {nameof(BlockTemplateRequests)} = {BlockTemplateRequests}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static ManualBlockTemplate2 FromJObject(JObject jObject) => jObject != null ? new ManualBlockTemplate2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("desc"),
            BlockTemplateDescription = (string)jObject.SelectToken("blockTemplate.desc"),
            BlockTemplatePatronMessage = (string)jObject.SelectToken("blockTemplate.patronMessage"),
            BlockTemplateBorrowing = (bool?)jObject.SelectToken("blockTemplate.borrowing"),
            BlockTemplateRenewals = (bool?)jObject.SelectToken("blockTemplate.renewals"),
            BlockTemplateRequests = (bool?)jObject.SelectToken("blockTemplate.requests"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("desc", Description),
            new JProperty("blockTemplate", new JObject(
                new JProperty("desc", BlockTemplateDescription),
                new JProperty("patronMessage", BlockTemplatePatronMessage),
                new JProperty("borrowing", BlockTemplateBorrowing),
                new JProperty("renewals", BlockTemplateRenewals),
                new JProperty("requests", BlockTemplateRequests))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
