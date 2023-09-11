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
    // uc.reference_datas -> uc_agreements.reference_datas
    // ReferenceData2 -> ReferenceData
    [DisplayColumn(nameof(Id)), DisplayName("Reference Datas"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("reference_datas", Schema = "uc")]
    public partial class ReferenceData2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ReferenceData.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("label"), Display(Order = 2), JsonProperty("label"), StringLength(1024)]
        public virtual string Label { get; set; }

        [Column("value"), Display(Order = 3), JsonProperty("value"), StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("content"), CustomValidation(typeof(ReferenceData), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Label)} = {Label}, {nameof(Value)} = {Value}, {nameof(Content)} = {Content} }}";

        public static ReferenceData2 FromJObject(JObject jObject) => jObject != null ? new ReferenceData2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Label = (string)jObject.SelectToken("label"),
            Value = (string)jObject.SelectToken("value"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("label", Label),
            new JProperty("value", Value)).RemoveNullAndEmptyProperties();
    }
}
