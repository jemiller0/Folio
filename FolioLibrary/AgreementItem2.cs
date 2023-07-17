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

        [Column("date_created"), DataType(DataType.Date), Display(Name = "Date Created", Order = 2), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("dateCreated")]
        public virtual DateTime? DateCreated { get; set; }

        [Column("last_updated"), Display(Name = "Last Updated", Order = 3), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastUpdated")]
        public virtual DateTime? LastUpdated { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement Id", Order = 4), JsonProperty("owner.id")]
        public virtual Guid? AgreementId { get; set; }

        [Column("content"), CustomValidation(typeof(AgreementItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 5), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(DateCreated)} = {DateCreated}, {nameof(LastUpdated)} = {LastUpdated}, {nameof(AgreementId)} = {AgreementId}, {nameof(Content)} = {Content} }}";

        public static AgreementItem2 FromJObject(JObject jObject) => jObject != null ? new AgreementItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            DateCreated = ((DateTime?)jObject.SelectToken("dateCreated"))?.ToUniversalTime(),
            LastUpdated = (DateTime?)jObject.SelectToken("lastUpdated"),
            AgreementId = (Guid?)jObject.SelectToken("owner.id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("dateCreated", DateCreated?.ToLocalTime()),
            new JProperty("lastUpdated", LastUpdated?.ToLocalTime()),
            new JProperty("owner", new JObject(
                new JProperty("id", AgreementId)))).RemoveNullAndEmptyProperties();
    }
}
