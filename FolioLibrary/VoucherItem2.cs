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
    // uc.voucher_items -> diku_mod_invoice_storage.voucher_lines
    // VoucherItem2 -> VoucherItem
    [DisplayColumn(nameof(AccountNumber)), DisplayName("Voucher Items"), JsonConverter(typeof(JsonPathJsonConverter<VoucherItem2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("voucher_items", Schema = "uc")]
    public partial class VoucherItem2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.VoucherItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 2), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Column("external_account_number"), Display(Name = "Account Number", Order = 3), JsonProperty("externalAccountNumber"), Required, StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Display(Name = "Sub Transaction", Order = 4)]
        public virtual Transaction2 SubTransaction { get; set; }

        [Column("sub_transaction_id"), Display(Name = "Sub Transaction", Order = 5), JsonProperty("subTransactionId")]
        public virtual Guid? SubTransactionId { get; set; }

        [Display(Order = 6)]
        public virtual Voucher2 Voucher { get; set; }

        [Column("voucher_id"), Display(Name = "Voucher", Order = 7), JsonProperty("voucherId"), Required]
        public virtual Guid? VoucherId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("VoucherItem2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("VoucherItem2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(VoucherItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Voucher Item Funds", Order = 17), JsonProperty("fundDistributions")]
        public virtual ICollection<VoucherItemFund> VoucherItemFunds { get; set; }

        [Display(Name = "Voucher Item Invoice Items", Order = 18), JsonConverter(typeof(ArrayJsonConverter<List<VoucherItemInvoiceItem>, VoucherItemInvoiceItem>), "InvoiceItemId"), JsonProperty("sourceIds")]
        public virtual ICollection<VoucherItemInvoiceItem> VoucherItemInvoiceItems { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Amount)} = {Amount}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(SubTransactionId)} = {SubTransactionId}, {nameof(VoucherId)} = {VoucherId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(VoucherItemFunds)} = {(VoucherItemFunds != null ? $"{{ {string.Join(", ", VoucherItemFunds)} }}" : "")}, {nameof(VoucherItemInvoiceItems)} = {(VoucherItemInvoiceItems != null ? $"{{ {string.Join(", ", VoucherItemInvoiceItems)} }}" : "")} }}";

        public static VoucherItem2 FromJObject(JObject jObject) => jObject != null ? new VoucherItem2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            AccountNumber = (string)jObject.SelectToken("externalAccountNumber"),
            SubTransactionId = (Guid?)jObject.SelectToken("subTransactionId"),
            VoucherId = (Guid?)jObject.SelectToken("voucherId"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            VoucherItemFunds = jObject.SelectToken("fundDistributions")?.Where(jt => jt.HasValues).Select(jt => VoucherItemFund.FromJObject((JObject)jt)).ToArray(),
            VoucherItemInvoiceItems = jObject.SelectToken("sourceIds")?.Where(jt => jt.HasValues).Select(jt => VoucherItemInvoiceItem.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("amount", Amount),
            new JProperty("externalAccountNumber", AccountNumber),
            new JProperty("subTransactionId", SubTransactionId),
            new JProperty("voucherId", VoucherId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("fundDistributions", VoucherItemFunds?.Select(vif => vif.ToJObject())),
            new JProperty("sourceIds", VoucherItemInvoiceItems?.Select(viii => viii.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
