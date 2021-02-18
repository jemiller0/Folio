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
    // uc.ledgers -> diku_mod_finance_storage.ledger
    // Ledger2 -> Ledger
    [DisplayColumn(nameof(Name)), DisplayName("Ledgers"), JsonConverter(typeof(JsonPathJsonConverter<Ledger2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledgers", Schema = "uc")]
    public partial class Ledger2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Ledger.json")))
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

        [Column("code"), Display(Order = 3), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 4), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Display(Name = "Fiscal Year One", Order = 5)]
        public virtual FiscalYear2 FiscalYearOne { get; set; }

        [Column("fiscal_year_one_id"), Display(Name = "Fiscal Year One", Order = 6), JsonProperty("fiscalYearOneId"), Required]
        public virtual Guid? FiscalYearOneId { get; set; }

        [Column("ledger_status"), Display(Name = "Ledger Status", Order = 7), JsonProperty("ledgerStatus"), RegularExpression(@"^(Active|Inactive|Frozen)$"), Required, StringLength(1024)]
        public virtual string LedgerStatus { get; set; }

        [Column("allocated"), Display(Order = 8), JsonProperty("allocated")]
        public virtual decimal? Allocated { get; set; }

        [Column("available"), Display(Order = 9), JsonProperty("available")]
        public virtual decimal? Available { get; set; }

        [Column("net_transfers"), Display(Name = "Net Transfers", Order = 10), JsonProperty("netTransfers")]
        public virtual decimal? NetTransfers { get; set; }

        [Column("unavailable"), Display(Order = 11), JsonProperty("unavailable")]
        public virtual decimal? Unavailable { get; set; }

        [Column("currency"), Display(Order = 12), JsonProperty("currency"), StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("restrict_encumbrance"), Display(Name = "Restrict Encumbrance", Order = 13), JsonProperty("restrictEncumbrance")]
        public virtual bool? RestrictEncumbrance { get; set; }

        [Column("restrict_expenditures"), Display(Name = "Restrict Expenditures", Order = 14), JsonProperty("restrictExpenditures")]
        public virtual bool? RestrictExpenditures { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 16), InverseProperty("Ledger2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 17), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 20), InverseProperty("Ledger2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 21), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Ledger), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Funds", Order = 24)]
        public virtual ICollection<Fund2> Fund2s { get; set; }

        [Display(Name = "Ledger Acquisitions Units", Order = 25), JsonConverter(typeof(ArrayJsonConverter<List<LedgerAcquisitionsUnit>, LedgerAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<LedgerAcquisitionsUnit> LedgerAcquisitionsUnits { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(FiscalYearOneId)} = {FiscalYearOneId}, {nameof(LedgerStatus)} = {LedgerStatus}, {nameof(Allocated)} = {Allocated}, {nameof(Available)} = {Available}, {nameof(NetTransfers)} = {NetTransfers}, {nameof(Unavailable)} = {Unavailable}, {nameof(Currency)} = {Currency}, {nameof(RestrictEncumbrance)} = {RestrictEncumbrance}, {nameof(RestrictExpenditures)} = {RestrictExpenditures}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(LedgerAcquisitionsUnits)} = {(LedgerAcquisitionsUnits != null ? $"{{ {string.Join(", ", LedgerAcquisitionsUnits)} }}" : "")} }}";

        public static Ledger2 FromJObject(JObject jObject) => jObject != null ? new Ledger2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            FiscalYearOneId = (Guid?)jObject.SelectToken("fiscalYearOneId"),
            LedgerStatus = (string)jObject.SelectToken("ledgerStatus"),
            Allocated = (decimal?)jObject.SelectToken("allocated"),
            Available = (decimal?)jObject.SelectToken("available"),
            NetTransfers = (decimal?)jObject.SelectToken("netTransfers"),
            Unavailable = (decimal?)jObject.SelectToken("unavailable"),
            Currency = (string)jObject.SelectToken("currency"),
            RestrictEncumbrance = (bool?)jObject.SelectToken("restrictEncumbrance"),
            RestrictExpenditures = (bool?)jObject.SelectToken("restrictExpenditures"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            LedgerAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => LedgerAcquisitionsUnit.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("description", Description),
            new JProperty("fiscalYearOneId", FiscalYearOneId),
            new JProperty("ledgerStatus", LedgerStatus),
            new JProperty("allocated", Allocated),
            new JProperty("available", Available),
            new JProperty("netTransfers", NetTransfers),
            new JProperty("unavailable", Unavailable),
            new JProperty("currency", Currency),
            new JProperty("restrictEncumbrance", RestrictEncumbrance),
            new JProperty("restrictExpenditures", RestrictExpenditures),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", LedgerAcquisitionsUnits?.Select(lau => lau.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
