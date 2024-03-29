using Newtonsoft.Json;
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
    [Table("instance_source_marc", Schema = "uchicago_mod_inventory_storage")]
    public partial class InstanceSourceMarc
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

        [Column("id"), Display(Name = "Instance", Order = 1), Editable(false), ForeignKey("Instance5")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2), InverseProperty("InstanceSourceMarc")]
        public virtual Instance Instance { get; set; }

        [Display(Name = "Instance 1", Order = 3), InverseProperty("InstanceSourceMarc1")]
        public virtual Instance Instance1 { get; set; }

        [Display(Name = "Instance 2", Order = 4), InverseProperty("InstanceSourceMarc2")]
        public virtual Instance Instance2 { get; set; }

        [Display(Name = "Instance 3", Order = 5), InverseProperty("InstanceSourceMarc3")]
        public virtual Instance Instance3 { get; set; }

        [Display(Name = "Instance 4", Order = 6), InverseProperty("InstanceSourceMarc4")]
        public virtual Instance Instance4 { get; set; }

        [Display(Name = "Instance 5", Order = 7), InverseProperty("InstanceSourceMarc5")]
        public virtual Instance Instance5 { get; set; }

        [Column("jsonb"), CustomValidation(typeof(InstanceSourceMarc), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 8), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 10), Editable(false)]
        public virtual string CreationUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId} }}";

        public static InstanceSourceMarc FromJObject(JObject jObject) => jObject != null ? new InstanceSourceMarc
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
