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
    // uc.documents -> uchicago_mod_invoice_storage.documents
    // Document2 -> Document
    [JsonConverter(typeof(JsonPathJsonConverter<Document2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("documents", Schema = "uc")]
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

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("documentMetadata.id")]
        public virtual Guid? Id { get; set; }

        [Column("document_metadata_name"), Display(Order = 2), JsonProperty("documentMetadata.name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Display(Order = 3)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("document_metadata_invoice_id"), Display(Name = "Invoice", Order = 4), JsonProperty("documentMetadata.invoiceId"), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Column("document_metadata_url"), DataType(DataType.Url), Display(Name = "URL", Order = 5), JsonProperty("documentMetadata.url"), StringLength(1024)]
        public virtual string Url { get; set; }

        [Column("document_metadata_metadata_created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("documentMetadata.metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("Document2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("document_metadata_metadata_created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false), JsonProperty("documentMetadata.metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("document_metadata_metadata_created_by_username"), JsonProperty("documentMetadata.metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("document_metadata_metadata_updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("documentMetadata.metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("Document2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("document_metadata_metadata_updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false), JsonProperty("documentMetadata.metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("document_metadata_metadata_updated_by_username"), JsonProperty("documentMetadata.metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("contents_data"), Display(Order = 14), JsonProperty("contents.data"), Required, StringLength(1024)]
        public virtual string Data { get; set; }

        [Column("created_date"), Display(Name = "Creation Time 2", Order = 15), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("metadata.createdDate"), Required]
        public virtual DateTime? CreationTime2 { get; set; }

        [Column("created_by_user_id"), Display(Name = "User 2", Order = 16), ForeignKey("User2"), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId2 { get; set; }

        [Display(Name = "User 2", Order = 17), InverseProperty("Document2s2")]
        public virtual User2 User2 { get; set; }

        [Column("created_by_username"), Display(Name = "Creation User Username 2", Order = 18), JsonProperty("metadata.createdByUsername"), StringLength(1024)]
        public virtual string CreationUserUsername2 { get; set; }

        [Column("updated_date"), Display(Name = "Last Write Time 2", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime2 { get; set; }

        [Column("updated_by_user_id"), Display(Name = "User 3", Order = 20), Editable(false), ForeignKey("User3"), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId2 { get; set; }

        [Display(Name = "User 3", Order = 21), InverseProperty("Document2s3")]
        public virtual User2 User3 { get; set; }

        [Column("updated_by_username"), Display(Name = "Last Write User Username 2", Order = 22), Editable(false), JsonProperty("metadata.updatedByUsername"), StringLength(1024)]
        public virtual string LastWriteUserUsername2 { get; set; }

        [Column("content"), CustomValidation(typeof(Document), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Column("invoiceid"), Display(Order = 24)]
        public virtual Guid? Invoiceid { get; set; }

        [Column("document_data"), Display(Name = "Data 2", Order = 25)]
        public virtual string Data2 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(Url)} = {Url}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Data)} = {Data}, {nameof(CreationTime2)} = {CreationTime2}, {nameof(CreationUserId2)} = {CreationUserId2}, {nameof(CreationUserUsername2)} = {CreationUserUsername2}, {nameof(LastWriteTime2)} = {LastWriteTime2}, {nameof(LastWriteUserId2)} = {LastWriteUserId2}, {nameof(LastWriteUserUsername2)} = {LastWriteUserUsername2}, {nameof(Content)} = {Content}, {nameof(Invoiceid)} = {Invoiceid}, {nameof(Data2)} = {Data2} }}";

        public static Document2 FromJObject(JObject jObject) => jObject != null ? new Document2
        {
            Id = (Guid?)jObject.SelectToken("documentMetadata.id"),
            Name = (string)jObject.SelectToken("documentMetadata.name"),
            InvoiceId = (Guid?)jObject.SelectToken("documentMetadata.invoiceId"),
            Url = (string)jObject.SelectToken("documentMetadata.url"),
            CreationTime = (DateTime?)jObject.SelectToken("documentMetadata.metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("documentMetadata.metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("documentMetadata.metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("documentMetadata.metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("documentMetadata.metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("documentMetadata.metadata.updatedByUsername"),
            Data = (string)jObject.SelectToken("contents.data"),
            CreationTime2 = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId2 = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername2 = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime2 = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId2 = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername2 = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("documentMetadata", new JObject(
                new JProperty("id", Id),
                new JProperty("name", Name),
                new JProperty("invoiceId", InvoiceId),
                new JProperty("url", Url),
                new JProperty("metadata", new JObject(
                    new JProperty("createdDate", CreationTime?.ToLocalTime()),
                    new JProperty("createdByUserId", CreationUserId),
                    new JProperty("createdByUsername", CreationUserUsername),
                    new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                    new JProperty("updatedByUserId", LastWriteUserId),
                    new JProperty("updatedByUsername", LastWriteUserUsername))))),
            new JProperty("contents", new JObject(
                new JProperty("data", Data))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime2?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId2),
                new JProperty("createdByUsername", CreationUserUsername2),
                new JProperty("updatedDate", LastWriteTime2?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId2),
                new JProperty("updatedByUsername", LastWriteUserUsername2)))).RemoveNullAndEmptyProperties();
    }
}
