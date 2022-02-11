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
    [Table("batch_voucher_exports", Schema = "uchicago_mod_invoice_storage")]
    public partial class BatchVoucherExport
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(BatchVoucherExport), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Batch Group", Order = 5)]
        public virtual BatchGroup BatchGroup { get; set; }

        [Column("batchgroupid"), Display(Name = "Batch Group", Order = 6), ForeignKey("BatchGroup")]
        public virtual Guid? Batchgroupid { get; set; }

        [Display(Name = "Batch Voucher", Order = 7)]
        public virtual BatchVoucher BatchVoucher { get; set; }

        [Column("batchvoucherid"), Display(Name = "Batch Voucher", Order = 8), ForeignKey("BatchVoucher")]
        public virtual Guid? Batchvoucherid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Batchgroupid)} = {Batchgroupid}, {nameof(Batchvoucherid)} = {Batchvoucherid} }}";

        public static BatchVoucherExport FromJObject(JObject jObject) => jObject != null ? new BatchVoucherExport
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Batchgroupid = (Guid?)jObject.SelectToken("batchGroupId"),
            Batchvoucherid = (Guid?)jObject.SelectToken("batchVoucherId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
