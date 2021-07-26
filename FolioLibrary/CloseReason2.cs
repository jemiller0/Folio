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
    // uc.close_reasons -> diku_mod_orders_storage.reasons_for_closure
    // CloseReason2 -> CloseReason
    [DisplayColumn(nameof(Name)), DisplayName("Close Reasons"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("close_reasons", Schema = "uc")]
    public partial class CloseReason2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.CloseReason.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("reason"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("source"), Display(Order = 3), JsonProperty("source"), RegularExpression(@"^(User|System)$"), Required, StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("content"), CustomValidation(typeof(CloseReason), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Source)} = {Source}, {nameof(Content)} = {Content} }}";

        public static CloseReason2 FromJObject(JObject jObject) => jObject != null ? new CloseReason2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("reason"),
            Source = (string)jObject.SelectToken("source"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("reason", Name),
            new JProperty("source", Source)).RemoveNullAndEmptyProperties();
    }
}
