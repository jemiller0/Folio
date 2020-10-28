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
    [Table("documents", Schema = "diku_mod_invoice_storage")]
    public partial class Document
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Document.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Document), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Invoice Invoice { get; set; }

        [Column("invoiceid"), Display(Name = "Invoice", Order = 6), ForeignKey("Invoice")]
        public virtual Guid? Invoiceid { get; set; }

        [Column("document_data"), Display(Name = "Document Data", Order = 7)]
        public virtual string DocumentData { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Invoiceid)} = {Invoiceid}, {nameof(DocumentData)} = {DocumentData} }}";

        public static Document FromJObject(JObject jObject) => jObject != null ? new Document
        {
            Id = (Guid?)jObject.SelectToken("documentMetadata.id"),
            Content = jObject.ToString(),
            CreationTime = (DateTime?)jObject.SelectToken("documentMetadata.metadata.createdDate"),
            CreationUserId = (string)jObject.SelectToken("documentMetadata.metadata.createdByUserId"),
            Invoiceid = (Guid?)jObject.SelectToken("documentMetadata.invoiceId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
