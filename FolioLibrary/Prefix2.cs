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
    // uc.prefixes -> uchicago_mod_orders_storage.prefixes
    // Prefix2 -> Prefix
    [DisplayColumn(nameof(Name)), DisplayName("Prefixes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("prefixes", Schema = "uc")]
    public partial class Prefix2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Prefix.json")))
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

        [Column("content"), CustomValidation(typeof(Prefix), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(Content)} = {Content} }}";

        public static Prefix2 FromJObject(JObject jObject) => jObject != null ? new Prefix2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description)).RemoveNullAndEmptyProperties();
    }
}
