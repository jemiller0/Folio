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
    // uc.batch_voucher_exports -> uchicago_mod_invoice_storage.batch_voucher_exports
    // BatchVoucherExport2 -> BatchVoucherExport
    [DisplayColumn(nameof(Id)), DisplayName("Batch Voucher Exports"), JsonConverter(typeof(JsonPathJsonConverter<BatchVoucherExport2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_voucher_exports", Schema = "uc")]
    public partial class BatchVoucherExport2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BatchVoucherExport.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("status"), Display(Order = 2), JsonProperty("status"), RegularExpression(@"^(Pending|Generated|Uploaded|Error)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("message"), Display(Order = 3), JsonProperty("message"), StringLength(1024)]
        public virtual string Message { get; set; }

        [Display(Name = "Batch Group", Order = 4)]
        public virtual BatchGroup2 BatchGroup { get; set; }

        [Column("batch_group_id"), Display(Name = "Batch Group", Order = 5), JsonProperty("batchGroupId"), Required]
        public virtual Guid? BatchGroupId { get; set; }

        [Column("start"), Display(Order = 6), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("start"), Required]
        public virtual DateTime? Start { get; set; }

        [Column("end"), Display(Order = 7), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("end"), Required]
        public virtual DateTime? End { get; set; }

        [Display(Name = "Batch Voucher", Order = 8)]
        public virtual BatchVoucher2 BatchVoucher { get; set; }

        [Column("batch_voucher_id"), Display(Name = "Batch Voucher", Order = 9), JsonProperty("batchVoucherId")]
        public virtual Guid? BatchVoucherId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 11), InverseProperty("BatchVoucherExport2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("BatchVoucherExport2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(BatchVoucherExport), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 18), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Status)} = {Status}, {nameof(Message)} = {Message}, {nameof(BatchGroupId)} = {BatchGroupId}, {nameof(Start)} = {Start}, {nameof(End)} = {End}, {nameof(BatchVoucherId)} = {BatchVoucherId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static BatchVoucherExport2 FromJObject(JObject jObject) => jObject != null ? new BatchVoucherExport2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Status = (string)jObject.SelectToken("status"),
            Message = (string)jObject.SelectToken("message"),
            BatchGroupId = (Guid?)jObject.SelectToken("batchGroupId"),
            Start = (DateTime?)jObject.SelectToken("start"),
            End = (DateTime?)jObject.SelectToken("end"),
            BatchVoucherId = (Guid?)jObject.SelectToken("batchVoucherId"),
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
            new JProperty("status", Status),
            new JProperty("message", Message),
            new JProperty("batchGroupId", BatchGroupId),
            new JProperty("start", Start?.ToLocalTime()),
            new JProperty("end", End?.ToLocalTime()),
            new JProperty("batchVoucherId", BatchVoucherId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
