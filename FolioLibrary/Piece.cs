using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("pieces", Schema = "diku_mod_orders_storage")]
    public partial class Piece
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Piece.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Piece), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Order Item", Order = 5)]
        public virtual OrderItem OrderItem { get; set; }

        [Column("polineid"), Display(Name = "Order Item", Order = 6), Editable(false), ForeignKey("OrderItem")]
        public virtual Guid? Polineid { get; set; }

        [Display(Order = 7)]
        public virtual Title Title { get; set; }

        [Column("titleid"), Display(Name = "Title", Order = 8), Editable(false), ForeignKey("Title")]
        public virtual Guid? Titleid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Polineid)} = {Polineid}, {nameof(Titleid)} = {Titleid} }}";
    }
}
