using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("note", Schema = "uchicago_mod_notes")]
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

        [Column("title"), Display(Order = 2), Required, StringLength(255)]
        public virtual string Title { get; set; }

        [Column("content"), CustomValidation(typeof(Note), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3)]
        public virtual string Content { get; set; }

        [Column("indexed_content"), Display(Name = "Indexed Content", Order = 4), Required, StringLength(1024)]
        public virtual string IndexedContent { get; set; }

        [Column("domain"), Display(Order = 5), Required, StringLength(100)]
        public virtual string Domain { get; set; }

        [Display(Order = 6)]
        public virtual NoteType Type { get; set; }

        [Column("type_id"), Display(Name = "Type", Order = 7), Required]
        public virtual Guid? TypeId { get; set; }

        [Column("pop_up_on_user"), Display(Name = "Pop Up On User", Order = 8)]
        public virtual bool? PopUpOnUser { get; set; }

        [Column("pop_up_on_check_out"), Display(Name = "Pop Up On Check Out", Order = 9)]
        public virtual bool? PopUpOnCheckOut { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 10), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("updated_by"), Display(Name = "Updated By", Order = 12)]
        public virtual Guid? UpdatedBy { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<NoteLink> NoteLinks { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Title)} = {Title}, {nameof(Content)} = {Content}, {nameof(IndexedContent)} = {IndexedContent}, {nameof(Domain)} = {Domain}, {nameof(TypeId)} = {TypeId}, {nameof(PopUpOnUser)} = {PopUpOnUser}, {nameof(PopUpOnCheckOut)} = {PopUpOnCheckOut}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(UpdatedBy)} = {UpdatedBy}, {nameof(LastWriteTime)} = {LastWriteTime} }}";
    }
}
