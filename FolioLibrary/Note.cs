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
    [Table("note_data", Schema = "diku_mod_notes")]
    public partial class Note
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Note.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Note), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Name = "Temporary Type", Order = 3)]
        public virtual NoteType TemporaryType { get; set; }

        [Column("temporary_type_id"), Display(Name = "Temporary Type", Order = 4)]
        public virtual Guid? TemporaryTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(TemporaryTypeId)} = {TemporaryTypeId} }}";

        public static Note FromJObject(JObject jObject) => new Note
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString()
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
