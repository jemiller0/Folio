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
    // uc.agreement_items -> uc_agreements.agreement_items
    // AgreementItem2 -> AgreementItem
    [DisplayColumn(nameof(Id)), DisplayName("Agreement Items"), JsonConverter(typeof(JsonPathJsonConverter<AgreementItem2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreement_items", Schema = "uc")]
    public partial class AgreementItem2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.AgreementItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("date_created"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 2), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("dateCreated")]
        public virtual DateTime? CreationTime { get; set; }

        [Column("last_updated"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("lastUpdated")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Order = 4)]
        public virtual Agreement2 Agreement { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement", Order = 5), JsonProperty("owner.id")]
        public virtual Guid? AgreementId { get; set; }

        [Column("content"), CustomValidation(typeof(AgreementItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 6), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(AgreementId)} = {AgreementId}, {nameof(Content)} = {Content} }}";

        public static AgreementItem2 FromJObject(JObject jObject) => jObject != null ? new AgreementItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            CreationTime = (DateTime?)jObject.SelectToken("dateCreated"),
            LastWriteTime = (DateTime?)jObject.SelectToken("lastUpdated"),
            AgreementId = (Guid?)jObject.SelectToken("owner.id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("dateCreated", CreationTime?.ToLocalTime()),
            new JProperty("lastUpdated", LastWriteTime?.ToLocalTime()),
            new JProperty("owner", new JObject(
                new JProperty("id", AgreementId)))).RemoveNullAndEmptyProperties();
    }
}
