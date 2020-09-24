using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("circulation_rules", Schema = "diku_mod_circulation_storage")]
    public partial class CirculationRule
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(CirculationRule), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("lock"), Display(Order = 3)]
        public virtual bool? Lock { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Lock)} = {Lock} }}";

        public static CirculationRule FromJObject(JObject jObject) => new CirculationRule
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString()
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
