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
    // uc.export_config_credentials -> uchicago_mod_invoice_storage.export_config_credentials
    // ExportConfigCredential2 -> ExportConfigCredential
    [DisplayColumn(nameof(Id)), DisplayName("Export Config Credentials"), JsonConverter(typeof(JsonPathJsonConverter<ExportConfigCredential2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("export_config_credentials", Schema = "uc")]
    public partial class ExportConfigCredential2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ExportConfigCredential.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("username"), Display(Order = 2), JsonProperty("username"), Required, StringLength(1024)]
        public virtual string Username { get; set; }

        [Column("password"), DataType(DataType.Password), Display(Order = 3), JsonProperty("password"), StringLength(1024)]
        public virtual string Password { get; set; }

        [Display(Name = "Export Config", Order = 4)]
        public virtual BatchVoucherExportConfig2 ExportConfig { get; set; }

        [Column("export_config_id"), Display(Name = "Export Config", Order = 5), JsonProperty("exportConfigId"), Required]
        public virtual Guid? ExportConfigId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 7), InverseProperty("ExportConfigCredential2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 8), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 11), InverseProperty("ExportConfigCredential2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 12), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ExportConfigCredential), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 14), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Username)} = {Username}, {nameof(Password)} = {Password}, {nameof(ExportConfigId)} = {ExportConfigId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static ExportConfigCredential2 FromJObject(JObject jObject) => jObject != null ? new ExportConfigCredential2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Username = (string)jObject.SelectToken("username"),
            Password = (string)jObject.SelectToken("password"),
            ExportConfigId = (Guid?)jObject.SelectToken("exportConfigId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("username", Username),
            new JProperty("password", Password),
            new JProperty("exportConfigId", ExportConfigId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
