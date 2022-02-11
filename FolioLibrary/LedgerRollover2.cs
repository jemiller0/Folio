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
    // uc.ledger_rollovers -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover
    // LedgerRollover2 -> LedgerRollover
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Rollovers"), JsonConverter(typeof(JsonPathJsonConverter<LedgerRollover2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledger_rollovers", Schema = "uc")]
    public partial class LedgerRollover2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LedgerRollover.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Ledger2 Ledger { get; set; }

        [Column("ledger_id"), Display(Name = "Ledger", Order = 3), JsonProperty("ledgerId")]
        public virtual Guid? LedgerId { get; set; }

        [Display(Name = "From Fiscal Year", Order = 4), InverseProperty("LedgerRollover2s")]
        public virtual FiscalYear2 FromFiscalYear { get; set; }

        [Column("from_fiscal_year_id"), Display(Name = "From Fiscal Year", Order = 5), JsonProperty("fromFiscalYearId")]
        public virtual Guid? FromFiscalYearId { get; set; }

        [Display(Name = "To Fiscal Year", Order = 6), InverseProperty("LedgerRollover2s1")]
        public virtual FiscalYear2 ToFiscalYear { get; set; }

        [Column("to_fiscal_year_id"), Display(Name = "To Fiscal Year", Order = 7), JsonProperty("toFiscalYearId")]
        public virtual Guid? ToFiscalYearId { get; set; }

        [Column("restrict_encumbrance"), Display(Name = "Restrict Encumbrance", Order = 8), JsonProperty("restrictEncumbrance")]
        public virtual bool? RestrictEncumbrance { get; set; }

        [Column("restrict_expenditures"), Display(Name = "Restrict Expenditures", Order = 9), JsonProperty("restrictExpenditures")]
        public virtual bool? RestrictExpenditures { get; set; }

        [Column("need_close_budgets"), Display(Name = "Need Close Budgets", Order = 10), JsonProperty("needCloseBudgets")]
        public virtual bool? NeedCloseBudgets { get; set; }

        [Column("currency_factor"), Display(Name = "Currency Factor", Order = 11), Editable(false), JsonProperty("currencyFactor")]
        public virtual int? CurrencyFactor { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 13), InverseProperty("LedgerRollover2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 14), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 16), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 17), InverseProperty("LedgerRollover2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 18), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(LedgerRollover), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 20), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Ledger Rollover Budgets Rollovers", Order = 21), JsonProperty("budgetsRollover")]
        public virtual ICollection<LedgerRolloverBudgetsRollover> LedgerRolloverBudgetsRollovers { get; set; }

        [Display(Name = "Ledger Rollover Encumbrances Rollovers", Order = 22), JsonProperty("encumbrancesRollover")]
        public virtual ICollection<LedgerRolloverEncumbrancesRollover> LedgerRolloverEncumbrancesRollovers { get; set; }

        [Display(Name = "Ledger Rollover Errors", Order = 23)]
        public virtual ICollection<LedgerRolloverError2> LedgerRolloverError2s { get; set; }

        [Display(Name = "Ledger Rollover Progresss", Order = 24)]
        public virtual ICollection<LedgerRolloverProgress2> LedgerRolloverProgress2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerId)} = {LedgerId}, {nameof(FromFiscalYearId)} = {FromFiscalYearId}, {nameof(ToFiscalYearId)} = {ToFiscalYearId}, {nameof(RestrictEncumbrance)} = {RestrictEncumbrance}, {nameof(RestrictExpenditures)} = {RestrictExpenditures}, {nameof(NeedCloseBudgets)} = {NeedCloseBudgets}, {nameof(CurrencyFactor)} = {CurrencyFactor}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(LedgerRolloverBudgetsRollovers)} = {(LedgerRolloverBudgetsRollovers != null ? $"{{ {string.Join(", ", LedgerRolloverBudgetsRollovers)} }}" : "")}, {nameof(LedgerRolloverEncumbrancesRollovers)} = {(LedgerRolloverEncumbrancesRollovers != null ? $"{{ {string.Join(", ", LedgerRolloverEncumbrancesRollovers)} }}" : "")} }}";

        public static LedgerRollover2 FromJObject(JObject jObject) => jObject != null ? new LedgerRollover2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            LedgerId = (Guid?)jObject.SelectToken("ledgerId"),
            FromFiscalYearId = (Guid?)jObject.SelectToken("fromFiscalYearId"),
            ToFiscalYearId = (Guid?)jObject.SelectToken("toFiscalYearId"),
            RestrictEncumbrance = (bool?)jObject.SelectToken("restrictEncumbrance"),
            RestrictExpenditures = (bool?)jObject.SelectToken("restrictExpenditures"),
            NeedCloseBudgets = (bool?)jObject.SelectToken("needCloseBudgets"),
            CurrencyFactor = (int?)jObject.SelectToken("currencyFactor"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            LedgerRolloverBudgetsRollovers = jObject.SelectToken("budgetsRollover")?.Where(jt => jt.HasValues).Select(jt => LedgerRolloverBudgetsRollover.FromJObject((JObject)jt)).ToArray(),
            LedgerRolloverEncumbrancesRollovers = jObject.SelectToken("encumbrancesRollover")?.Where(jt => jt.HasValues).Select(jt => LedgerRolloverEncumbrancesRollover.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("ledgerId", LedgerId),
            new JProperty("fromFiscalYearId", FromFiscalYearId),
            new JProperty("toFiscalYearId", ToFiscalYearId),
            new JProperty("restrictEncumbrance", RestrictEncumbrance),
            new JProperty("restrictExpenditures", RestrictExpenditures),
            new JProperty("needCloseBudgets", NeedCloseBudgets),
            new JProperty("currencyFactor", CurrencyFactor),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("budgetsRollover", LedgerRolloverBudgetsRollovers?.Select(lrbr => lrbr.ToJObject())),
            new JProperty("encumbrancesRollover", LedgerRolloverEncumbrancesRollovers?.Select(lrer => lrer.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
