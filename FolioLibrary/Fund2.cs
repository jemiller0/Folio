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
    // uc.funds -> diku_mod_finance_storage.fund
    // Fund2 -> Fund
    [DisplayColumn(nameof(Name)), DisplayName("Funds"), JsonConverter(typeof(JsonPathJsonConverter<Fund2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("funds", Schema = "uc")]
    public partial class Fund2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Fund.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("code"), Display(Order = 2), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("external_account_no"), Display(Name = "Account Number", Order = 4), JsonProperty("externalAccountNo"), StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Column("fund_status"), Display(Name = "Fund Status", Order = 5), JsonProperty("fundStatus"), RegularExpression(@"^(Active|Frozen|Inactive)$"), Required, StringLength(1024)]
        public virtual string FundStatus { get; set; }

        [Display(Name = "Fund Type", Order = 6)]
        public virtual FundType2 FundType { get; set; }

        [Column("fund_type_id"), Display(Name = "Fund Type", Order = 7), JsonProperty("fundTypeId")]
        public virtual Guid? FundTypeId { get; set; }

        [Display(Order = 8)]
        public virtual Ledger2 Ledger { get; set; }

        [Column("ledger_id"), Display(Name = "Ledger", Order = 9), JsonProperty("ledgerId"), Required]
        public virtual Guid? LedgerId { get; set; }

        [Column("name"), Display(Order = 10), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("Fund2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("Fund2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Fund), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Allocated From Funds", Order = 20), JsonConverter(typeof(ArrayJsonConverter<List<AllocatedFromFund>, AllocatedFromFund>), "FromFundId"), JsonProperty("allocatedFromIds")]
        public virtual ICollection<AllocatedFromFund> AllocatedFromFunds { get; set; }

        [Display(Name = "Allocated From Funds 1", Order = 21)]
        public virtual ICollection<AllocatedFromFund> AllocatedFromFunds1 { get; set; }

        [Display(Name = "Allocated To Funds", Order = 22), JsonConverter(typeof(ArrayJsonConverter<List<AllocatedToFund>, AllocatedToFund>), "ToFundId"), JsonProperty("allocatedToIds")]
        public virtual ICollection<AllocatedToFund> AllocatedToFunds { get; set; }

        [Display(Name = "Allocated To Funds 1", Order = 23)]
        public virtual ICollection<AllocatedToFund> AllocatedToFunds1 { get; set; }

        [Display(Name = "Budgets", Order = 24)]
        public virtual ICollection<Budget2> Budget2s { get; set; }

        [Display(Name = "Budget Groups", Order = 25)]
        public virtual ICollection<BudgetGroup2> BudgetGroup2s { get; set; }

        [Display(Name = "Fund Acquisitions Units", Order = 26), JsonConverter(typeof(ArrayJsonConverter<List<FundAcquisitionsUnit>, FundAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<FundAcquisitionsUnit> FundAcquisitionsUnits { get; set; }

        [Display(Name = "Fund Tags", Order = 27), JsonConverter(typeof(ArrayJsonConverter<List<FundTag>, FundTag>), "TagId"), JsonProperty("tags.tagList")]
        public virtual ICollection<FundTag> FundTags { get; set; }

        [Display(Name = "Invoice Adjustment Funds", Order = 28)]
        public virtual ICollection<InvoiceAdjustmentFund> InvoiceAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Adjustment Funds", Order = 29)]
        public virtual ICollection<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Funds", Order = 30)]
        public virtual ICollection<InvoiceItemFund> InvoiceItemFunds { get; set; }

        [Display(Name = "Order Item Funds", Order = 31)]
        public virtual ICollection<OrderItemFund> OrderItemFunds { get; set; }

        [Display(Name = "Transactions", Order = 32)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Transactions 1", Order = 33)]
        public virtual ICollection<Transaction2> Transaction2s1 { get; set; }

        [Display(Name = "Voucher Item Funds", Order = 34)]
        public virtual ICollection<VoucherItemFund> VoucherItemFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(FundStatus)} = {FundStatus}, {nameof(FundTypeId)} = {FundTypeId}, {nameof(LedgerId)} = {LedgerId}, {nameof(Name)} = {Name}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(AllocatedFromFunds)} = {(AllocatedFromFunds != null ? $"{{ {string.Join(", ", AllocatedFromFunds)} }}" : "")}, {nameof(AllocatedToFunds)} = {(AllocatedToFunds != null ? $"{{ {string.Join(", ", AllocatedToFunds)} }}" : "")}, {nameof(FundAcquisitionsUnits)} = {(FundAcquisitionsUnits != null ? $"{{ {string.Join(", ", FundAcquisitionsUnits)} }}" : "")}, {nameof(FundTags)} = {(FundTags != null ? $"{{ {string.Join(", ", FundTags)} }}" : "")} }}";

        public static Fund2 FromJObject(JObject jObject) => jObject != null ? new Fund2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            AccountNumber = (string)jObject.SelectToken("externalAccountNo"),
            FundStatus = (string)jObject.SelectToken("fundStatus"),
            FundTypeId = (Guid?)jObject.SelectToken("fundTypeId"),
            LedgerId = (Guid?)jObject.SelectToken("ledgerId"),
            Name = (string)jObject.SelectToken("name"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            AllocatedFromFunds = jObject.SelectToken("allocatedFromIds")?.Where(jt => jt.HasValues).Select(jt => AllocatedFromFund.FromJObject((JValue)jt)).ToArray(),
            AllocatedToFunds = jObject.SelectToken("allocatedToIds")?.Where(jt => jt.HasValues).Select(jt => AllocatedToFund.FromJObject((JValue)jt)).ToArray(),
            FundAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => FundAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            FundTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => FundTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("code", Code),
            new JProperty("description", Description),
            new JProperty("externalAccountNo", AccountNumber),
            new JProperty("fundStatus", FundStatus),
            new JProperty("fundTypeId", FundTypeId),
            new JProperty("ledgerId", LedgerId),
            new JProperty("name", Name),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("allocatedFromIds", AllocatedFromFunds?.Select(aff => aff.ToJObject())),
            new JProperty("allocatedToIds", AllocatedToFunds?.Select(atf => atf.ToJObject())),
            new JProperty("acqUnitIds", FundAcquisitionsUnits?.Select(fau => fau.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", FundTags?.Select(ft => ft.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
