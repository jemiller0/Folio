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
    [Table("batch_groups", Schema = "diku_mod_invoice_storage")]
    public partial class BatchGroup
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BatchGroup.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(BatchGroup), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BatchVoucherExportConfig> BatchVoucherExportConfigs { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BatchVoucherExport> BatchVoucherExports { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Voucher> Vouchers { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId} }}";

        public static BatchGroup FromJObject(JObject jObject) => new BatchGroup
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId")
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
