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
    // uc.documents -> diku_mod_invoice_storage.documents
    // Document2 -> Document
    [DisplayColumn(nameof(Id)), DisplayName("Documents"), JsonConverter(typeof(JsonPathJsonConverter<Document2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("documents", Schema = "uc")]
    public partial class Document2
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

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("document_metadata_name"), Display(Name = "Document Metadata Name", Order = 2), JsonProperty("documentMetadata.name"), Required, StringLength(1024)]
        public virtual string DocumentMetadataName { get; set; }

        [Display(Name = "Document Metadata Invoice", Order = 3)]
        public virtual Invoice2 DocumentMetadataInvoice { get; set; }

        [Column("document_metadata_invoice_id"), Display(Name = "Document Metadata Invoice", Order = 4), JsonProperty("documentMetadata.invoiceId"), Required]
        public virtual Guid? DocumentMetadataInvoiceId { get; set; }

        [Column("document_metadata_url"), DataType(DataType.Url), Display(Name = "Document Metadata URL", Order = 5), JsonProperty("documentMetadata.url"), StringLength(1024)]
        public virtual string DocumentMetadataUrl { get; set; }

        [Column("document_metadata_metadata_created_date"), DataType(DataType.Date), Display(Name = "Document Metadata Metadata Created Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("documentMetadata.metadata.createdDate"), Required]
        public virtual DateTime? DocumentMetadataMetadataCreatedDate { get; set; }

        [Display(Name = "Document Metadata Metadata Created By User", Order = 7), InverseProperty("Document2s")]
        public virtual User2 DocumentMetadataMetadataCreatedByUser { get; set; }

        [Column("document_metadata_metadata_created_by_user_id"), Display(Name = "Document Metadata Metadata Created By User", Order = 8), JsonProperty("documentMetadata.metadata.createdByUserId")]
        public virtual Guid? DocumentMetadataMetadataCreatedByUserId { get; set; }

        [Column("document_metadata_metadata_created_by_username"), Display(Name = "Document Metadata Metadata Created By Username", Order = 9), JsonProperty("documentMetadata.metadata.createdByUsername"), StringLength(1024)]
        public virtual string DocumentMetadataMetadataCreatedByUsername { get; set; }

        [Column("document_metadata_metadata_updated_date"), DataType(DataType.Date), Display(Name = "Document Metadata Metadata Updated Date", Order = 10), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("documentMetadata.metadata.updatedDate")]
        public virtual DateTime? DocumentMetadataMetadataUpdatedDate { get; set; }

        [Display(Name = "Document Metadata Metadata Updated By User", Order = 11), InverseProperty("Document2s1")]
        public virtual User2 DocumentMetadataMetadataUpdatedByUser { get; set; }

        [Column("document_metadata_metadata_updated_by_user_id"), Display(Name = "Document Metadata Metadata Updated By User", Order = 12), JsonProperty("documentMetadata.metadata.updatedByUserId")]
        public virtual Guid? DocumentMetadataMetadataUpdatedByUserId { get; set; }

        [Column("document_metadata_metadata_updated_by_username"), Display(Name = "Document Metadata Metadata Updated By Username", Order = 13), JsonProperty("documentMetadata.metadata.updatedByUsername"), StringLength(1024)]
        public virtual string DocumentMetadataMetadataUpdatedByUsername { get; set; }

        [Column("contents_data"), Display(Name = "Contents Data", Order = 14), JsonProperty("contents.data"), Required, StringLength(1024)]
        public virtual string ContentsData { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 16), InverseProperty("Document2s2")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 17), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 20), InverseProperty("Document2s3")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 21), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Document), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Column("invoiceid"), Display(Order = 24)]
        public virtual Guid? Invoiceid { get; set; }

        [Column("document_data"), Display(Name = "Document Data", Order = 25)]
        public virtual string DocumentData { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(DocumentMetadataName)} = {DocumentMetadataName}, {nameof(DocumentMetadataInvoiceId)} = {DocumentMetadataInvoiceId}, {nameof(DocumentMetadataUrl)} = {DocumentMetadataUrl}, {nameof(DocumentMetadataMetadataCreatedDate)} = {DocumentMetadataMetadataCreatedDate}, {nameof(DocumentMetadataMetadataCreatedByUserId)} = {DocumentMetadataMetadataCreatedByUserId}, {nameof(DocumentMetadataMetadataCreatedByUsername)} = {DocumentMetadataMetadataCreatedByUsername}, {nameof(DocumentMetadataMetadataUpdatedDate)} = {DocumentMetadataMetadataUpdatedDate}, {nameof(DocumentMetadataMetadataUpdatedByUserId)} = {DocumentMetadataMetadataUpdatedByUserId}, {nameof(DocumentMetadataMetadataUpdatedByUsername)} = {DocumentMetadataMetadataUpdatedByUsername}, {nameof(ContentsData)} = {ContentsData}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(Invoiceid)} = {Invoiceid}, {nameof(DocumentData)} = {DocumentData} }}";

        public static Document2 FromJObject(JObject jObject) => jObject != null ? new Document2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            DocumentMetadataName = (string)jObject.SelectToken("documentMetadata.name"),
            DocumentMetadataInvoiceId = (Guid?)jObject.SelectToken("documentMetadata.invoiceId"),
            DocumentMetadataUrl = (string)jObject.SelectToken("documentMetadata.url"),
            DocumentMetadataMetadataCreatedDate = ((DateTime?)jObject.SelectToken("documentMetadata.metadata.createdDate"))?.ToLocalTime(),
            DocumentMetadataMetadataCreatedByUserId = (Guid?)jObject.SelectToken("documentMetadata.metadata.createdByUserId"),
            DocumentMetadataMetadataCreatedByUsername = (string)jObject.SelectToken("documentMetadata.metadata.createdByUsername"),
            DocumentMetadataMetadataUpdatedDate = ((DateTime?)jObject.SelectToken("documentMetadata.metadata.updatedDate"))?.ToLocalTime(),
            DocumentMetadataMetadataUpdatedByUserId = (Guid?)jObject.SelectToken("documentMetadata.metadata.updatedByUserId"),
            DocumentMetadataMetadataUpdatedByUsername = (string)jObject.SelectToken("documentMetadata.metadata.updatedByUsername"),
            ContentsData = (string)jObject.SelectToken("contents.data"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("documentMetadata", new JObject(
                new JProperty("name", DocumentMetadataName),
                new JProperty("invoiceId", DocumentMetadataInvoiceId),
                new JProperty("url", DocumentMetadataUrl),
                new JProperty("metadata", new JObject(
                    new JProperty("createdDate", DocumentMetadataMetadataCreatedDate?.ToUniversalTime()),
                    new JProperty("createdByUserId", DocumentMetadataMetadataCreatedByUserId),
                    new JProperty("createdByUsername", DocumentMetadataMetadataCreatedByUsername),
                    new JProperty("updatedDate", DocumentMetadataMetadataUpdatedDate?.ToUniversalTime()),
                    new JProperty("updatedByUserId", DocumentMetadataMetadataUpdatedByUserId),
                    new JProperty("updatedByUsername", DocumentMetadataMetadataUpdatedByUsername))))),
            new JProperty("contents", new JObject(
                new JProperty("data", ContentsData))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
