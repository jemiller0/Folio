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
    [Table("marc_records_lb", Schema = "uchicago_mod_source_record_storage")]
    public partial class MarcRecord
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

        [Column("id"), Display(Name = "Record", Order = 1), Editable(false), ForeignKey("Record")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Record Record { get; set; }

        [Column("content"), CustomValidation(typeof(MarcRecord), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3), Required]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";
    }
}
