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
    [Table("export_config_credentials", Schema = "diku_mod_invoice_storage")]
    public partial class ExportConfigCredential
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(ExportConfigCredential), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Batch Voucher Export Config", Order = 5)]
        public virtual BatchVoucherExportConfig BatchVoucherExportConfig { get; set; }

        [Column("exportconfigid"), Display(Name = "Batch Voucher Export Config", Order = 6), ForeignKey("BatchVoucherExportConfig")]
        public virtual Guid? Exportconfigid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Exportconfigid)} = {Exportconfigid} }}";

        public static ExportConfigCredential FromJObject(JObject jObject) => jObject != null ? new ExportConfigCredential
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Exportconfigid = (Guid?)jObject.SelectToken("exportConfigId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
