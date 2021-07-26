using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.batch_voucher_export_configs -> diku_mod_invoice_storage.batch_voucher_export_configs
    // BatchVoucherExportConfig2 -> BatchVoucherExportConfig
    [DisplayColumn(nameof(Id)), DisplayName("Batch Voucher Export Configs"), JsonConverter(typeof(JsonPathJsonConverter<BatchVoucherExportConfig2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_voucher_export_configs", Schema = "uc")]
    public partial class BatchVoucherExportConfig2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BatchVoucherExportConfig.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Batch Group", Order = 2)]
        public virtual BatchGroup2 BatchGroup { get; set; }

        [Column("batch_group_id"), Display(Name = "Batch Group", Order = 3), JsonProperty("batchGroupId"), Required]
        public virtual Guid? BatchGroupId { get; set; }

        [Column("enable_scheduled_export"), Display(Name = "Enable Scheduled Export", Order = 4), JsonProperty("enableScheduledExport")]
        public virtual bool? EnableScheduledExport { get; set; }

        [Column("format"), Display(Order = 5), JsonProperty("format"), RegularExpression(@"^(Application/json|Application/xml)$"), StringLength(1024)]
        public virtual string Format { get; set; }

        [Column("start_time"), Display(Name = "Start Time", Order = 6), JsonProperty("startTime"), StringLength(1024)]
        public virtual string StartTime { get; set; }

        [Column("upload_uri"), DataType(DataType.Url), Display(Name = "Upload URI", Order = 7), JsonProperty("uploadURI"), StringLength(1024)]
        public virtual string UploadUri { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("BatchVoucherExportConfig2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("BatchVoucherExportConfig2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(BatchVoucherExportConfig), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Batch Voucher Export Config Weekdays", Order = 17), JsonConverter(typeof(ArrayJsonConverter<List<BatchVoucherExportConfigWeekday>, BatchVoucherExportConfigWeekday>), "Content"), JsonProperty("weekdays")]
        public virtual ICollection<BatchVoucherExportConfigWeekday> BatchVoucherExportConfigWeekdays { get; set; }

        [Display(Name = "Export Config Credentials", Order = 18)]
        public virtual ICollection<ExportConfigCredential2> ExportConfigCredential2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchGroupId)} = {BatchGroupId}, {nameof(EnableScheduledExport)} = {EnableScheduledExport}, {nameof(Format)} = {Format}, {nameof(StartTime)} = {StartTime}, {nameof(UploadUri)} = {UploadUri}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(BatchVoucherExportConfigWeekdays)} = {(BatchVoucherExportConfigWeekdays != null ? $"{{ {string.Join(", ", BatchVoucherExportConfigWeekdays)} }}" : "")} }}";

        public static BatchVoucherExportConfig2 FromJObject(JObject jObject) => jObject != null ? new BatchVoucherExportConfig2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            BatchGroupId = (Guid?)jObject.SelectToken("batchGroupId"),
            EnableScheduledExport = (bool?)jObject.SelectToken("enableScheduledExport"),
            Format = (string)jObject.SelectToken("format"),
            StartTime = (string)jObject.SelectToken("startTime"),
            UploadUri = (string)jObject.SelectToken("uploadURI"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            BatchVoucherExportConfigWeekdays = jObject.SelectToken("weekdays")?.Where(jt => jt.HasValues).Select(jt => BatchVoucherExportConfigWeekday.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("batchGroupId", BatchGroupId),
            new JProperty("enableScheduledExport", EnableScheduledExport),
            new JProperty("format", Format),
            new JProperty("startTime", StartTime),
            new JProperty("uploadURI", UploadUri),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("weekdays", BatchVoucherExportConfigWeekdays?.Select(bvecw => bvecw.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
