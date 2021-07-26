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
    // uc.circulation_rules -> diku_mod_circulation_storage.circulation_rules
    // CirculationRule2 -> CirculationRule
    [DisplayColumn(nameof(Id)), DisplayName("Circulation Rules"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("circulation_rules", Schema = "uc")]
    public partial class CirculationRule2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.CirculationRule.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("rules_as_text"), Display(Name = "Rules As Text", Order = 2), JsonProperty("rulesAsText"), Required, StringLength(1024)]
        public virtual string RulesAsText { get; set; }

        [Column("content"), CustomValidation(typeof(CirculationRule), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        [Column("lock"), Display(Order = 4)]
        public virtual bool? Lock { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RulesAsText)} = {RulesAsText}, {nameof(Content)} = {Content}, {nameof(Lock)} = {Lock} }}";

        public static CirculationRule2 FromJObject(JObject jObject) => jObject != null ? new CirculationRule2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            RulesAsText = (string)jObject.SelectToken("rulesAsText"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("rulesAsText", RulesAsText)).RemoveNullAndEmptyProperties();
    }
}
