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
    // uc.batch_vouchers -> uchicago_mod_invoice_storage.batch_vouchers
    // BatchVoucher2 -> BatchVoucher
    [DisplayColumn(nameof(Id)), DisplayName("Batch Vouchers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("batch_vouchers", Schema = "uc")]
    public partial class BatchVoucher2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BatchVoucher.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("batch_group"), Display(Name = "Batch Group", Order = 2), JsonProperty("batchGroup"), Required, StringLength(1024)]
        public virtual string BatchGroup { get; set; }

        [Column("created"), Display(Order = 3), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("created"), Required]
        public virtual DateTime? Created { get; set; }

        [Column("start"), Display(Order = 4), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("start"), Required]
        public virtual DateTime? Start { get; set; }

        [Column("end"), Display(Order = 5), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("end"), Required]
        public virtual DateTime? End { get; set; }

        [Column("total_records"), Display(Name = "Total Records", Order = 6), JsonProperty("totalRecords"), Required]
        public virtual int? TotalRecords { get; set; }

        [Column("content"), CustomValidation(typeof(BatchVoucher), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 7), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Batch Voucher Batched Vouchers", Order = 8), JsonProperty("batchedVouchers")]
        public virtual ICollection<BatchVoucherBatchedVoucher> BatchVoucherBatchedVouchers { get; set; }

        [Display(Name = "Batch Voucher Exports", Order = 9)]
        public virtual ICollection<BatchVoucherExport2> BatchVoucherExport2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BatchGroup)} = {BatchGroup}, {nameof(Created)} = {Created}, {nameof(Start)} = {Start}, {nameof(End)} = {End}, {nameof(TotalRecords)} = {TotalRecords}, {nameof(Content)} = {Content}, {nameof(BatchVoucherBatchedVouchers)} = {(BatchVoucherBatchedVouchers != null ? $"{{ {string.Join(", ", BatchVoucherBatchedVouchers)} }}" : "")} }}";

        public static BatchVoucher2 FromJObject(JObject jObject) => jObject != null ? new BatchVoucher2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            BatchGroup = (string)jObject.SelectToken("batchGroup"),
            Created = (DateTime?)jObject.SelectToken("created"),
            Start = (DateTime?)jObject.SelectToken("start"),
            End = (DateTime?)jObject.SelectToken("end"),
            TotalRecords = (int?)jObject.SelectToken("totalRecords"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            BatchVoucherBatchedVouchers = jObject.SelectToken("batchedVouchers")?.Where(jt => jt.HasValues).Select(jt => BatchVoucherBatchedVoucher.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("batchGroup", BatchGroup),
            new JProperty("created", Created?.ToLocalTime()),
            new JProperty("start", Start?.ToLocalTime()),
            new JProperty("end", End?.ToLocalTime()),
            new JProperty("totalRecords", TotalRecords),
            new JProperty("batchedVouchers", BatchVoucherBatchedVouchers?.Select(bvbv => bvbv.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
