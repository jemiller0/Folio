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
    // uc.source_marcs -> uchicago_mod_inventory_storage.instance_source_marc
    // SourceMarc -> InstanceSourceMarc
    [DisplayColumn(nameof(Id)), DisplayName("Source Marcs"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("source_marcs", Schema = "uc")]
    public partial class SourceMarc
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InstanceSourceMarc.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("leader"), Display(Order = 2), JsonProperty("leader"), Required, StringLength(24)]
        public virtual string Leader { get; set; }

        [Column("content"), CustomValidation(typeof(InstanceSourceMarc), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Source Marc Fields", Order = 4), JsonConverter(typeof(ArrayJsonConverter<List<SourceMarcField>, SourceMarcField>), "Content"), JsonProperty("fields")]
        public virtual ICollection<SourceMarcField> SourceMarcFields { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Leader)} = {Leader}, {nameof(Content)} = {Content}, {nameof(SourceMarcFields)} = {(SourceMarcFields != null ? $"{{ {string.Join(", ", SourceMarcFields)} }}" : "")} }}";

        public static SourceMarc FromJObject(JObject jObject) => jObject != null ? new SourceMarc
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Leader = (string)jObject.SelectToken("leader"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            SourceMarcFields = jObject.SelectToken("fields")?.Select(jt => SourceMarcField.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("leader", Leader),
            new JProperty("fields", SourceMarcFields?.Select(smf => smf.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
