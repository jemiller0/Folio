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
    // uc.marc_records -> uchicago_mod_source_record_storage.marc_records_lb
    // MarcRecord2 -> MarcRecord
    [DisplayColumn(nameof(Id)), DisplayName("Marc Records"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("marc_records", Schema = "uc")]
    public partial class MarcRecord2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.MarcRecord.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Name = "Record 2", Order = 1), Editable(false), ForeignKey("Record2"), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Record 2", Order = 2)]
        public virtual Record2 Record2 { get; set; }

        [Column("content"), CustomValidation(typeof(MarcRecord), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";

        public static MarcRecord2 FromJObject(JObject jObject) => jObject != null ? new MarcRecord2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
