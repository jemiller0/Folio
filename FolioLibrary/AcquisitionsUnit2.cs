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
    // uc.acquisitions_units -> uchicago_mod_orders_storage.acquisitions_unit
    // AcquisitionsUnit2 -> AcquisitionsUnit
    [DisplayColumn(nameof(Name)), DisplayName("Acquisitions Units"), JsonConverter(typeof(JsonPathJsonConverter<AcquisitionsUnit2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("acquisitions_units", Schema = "uc")]
    public partial class AcquisitionsUnit2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.AcquisitionsUnit.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("is_deleted"), Display(Name = "Is Deleted", Order = 3), JsonProperty("isDeleted")]
        public virtual bool? IsDeleted { get; set; }

        [Column("protect_create"), Display(Name = "Protect Create", Order = 4), JsonProperty("protectCreate")]
        public virtual bool? ProtectCreate { get; set; }

        [Column("protect_read"), Display(Name = "Protect Read", Order = 5), JsonProperty("protectRead")]
        public virtual bool? ProtectRead { get; set; }

        [Column("protect_update"), Display(Name = "Protect Update", Order = 6), JsonProperty("protectUpdate")]
        public virtual bool? ProtectUpdate { get; set; }

        [Column("protect_delete"), Display(Name = "Protect Delete", Order = 7), JsonProperty("protectDelete")]
        public virtual bool? ProtectDelete { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("AcquisitionsUnit2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("AcquisitionsUnit2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(AcquisitionsUnit), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Budget Acquisitions Units", Order = 17)]
        public virtual ICollection<BudgetAcquisitionsUnit> BudgetAcquisitionsUnits { get; set; }

        [Display(Name = "Finance Group Acquisitions Units", Order = 18)]
        public virtual ICollection<FinanceGroupAcquisitionsUnit> FinanceGroupAcquisitionsUnits { get; set; }

        [Display(Name = "Fiscal Year Acquisitions Units", Order = 19)]
        public virtual ICollection<FiscalYearAcquisitionsUnit> FiscalYearAcquisitionsUnits { get; set; }

        [Display(Name = "Fund Acquisitions Units", Order = 20)]
        public virtual ICollection<FundAcquisitionsUnit> FundAcquisitionsUnits { get; set; }

        [Display(Name = "Invoice Acquisitions Units", Order = 21)]
        public virtual ICollection<InvoiceAcquisitionsUnit> InvoiceAcquisitionsUnits { get; set; }

        [Display(Name = "Ledger Acquisitions Units", Order = 22)]
        public virtual ICollection<LedgerAcquisitionsUnit> LedgerAcquisitionsUnits { get; set; }

        [Display(Name = "Order Acquisitions Units", Order = 23)]
        public virtual ICollection<OrderAcquisitionsUnit> OrderAcquisitionsUnits { get; set; }

        [Display(Name = "Organization Account Acquisitions Units", Order = 24)]
        public virtual ICollection<OrganizationAccountAcquisitionsUnit> OrganizationAccountAcquisitionsUnits { get; set; }

        [Display(Name = "Organization Acquisitions Units", Order = 25)]
        public virtual ICollection<OrganizationAcquisitionsUnit> OrganizationAcquisitionsUnits { get; set; }

        [Display(Name = "User Acquisitions Units", Order = 26)]
        public virtual ICollection<UserAcquisitionsUnit2> UserAcquisitionsUnit2s { get; set; }

        [Display(Name = "Voucher Acquisitions Units", Order = 27)]
        public virtual ICollection<VoucherAcquisitionsUnit> VoucherAcquisitionsUnits { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(IsDeleted)} = {IsDeleted}, {nameof(ProtectCreate)} = {ProtectCreate}, {nameof(ProtectRead)} = {ProtectRead}, {nameof(ProtectUpdate)} = {ProtectUpdate}, {nameof(ProtectDelete)} = {ProtectDelete}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static AcquisitionsUnit2 FromJObject(JObject jObject) => jObject != null ? new AcquisitionsUnit2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            IsDeleted = (bool?)jObject.SelectToken("isDeleted"),
            ProtectCreate = (bool?)jObject.SelectToken("protectCreate"),
            ProtectRead = (bool?)jObject.SelectToken("protectRead"),
            ProtectUpdate = (bool?)jObject.SelectToken("protectUpdate"),
            ProtectDelete = (bool?)jObject.SelectToken("protectDelete"),
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
            new JProperty("name", Name),
            new JProperty("isDeleted", IsDeleted),
            new JProperty("protectCreate", ProtectCreate),
            new JProperty("protectRead", ProtectRead),
            new JProperty("protectUpdate", ProtectUpdate),
            new JProperty("protectDelete", ProtectDelete),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
