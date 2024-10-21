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
    // uc.rollovers -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover
    // Rollover2 -> Rollover
    [DisplayColumn(nameof(Id)), DisplayName("Rollovers"), JsonConverter(typeof(JsonPathJsonConverter<Rollover2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollovers", Schema = "uc")]
    public partial class Rollover2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Rollover.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), JsonProperty("_version"), ScaffoldColumn(false)]
        public virtual int? Version { get; set; }

        [Display(Order = 3)]
        public virtual Ledger2 Ledger { get; set; }

        [Column("ledger_id"), Display(Name = "Ledger", Order = 4), JsonProperty("ledgerId")]
        public virtual Guid? LedgerId { get; set; }

        [Column("rollover_type"), Display(Name = "Rollover Type", Order = 5), JsonProperty("rolloverType"), RegularExpression(@"^(Preview|Commit|Rollback)$"), StringLength(1024)]
        public virtual string RolloverType { get; set; }

        [Display(Name = "From Fiscal Year", Order = 6), InverseProperty("Rollover2s")]
        public virtual FiscalYear2 FromFiscalYear { get; set; }

        [Column("from_fiscal_year_id"), Display(Name = "From Fiscal Year", Order = 7), JsonProperty("fromFiscalYearId")]
        public virtual Guid? FromFiscalYearId { get; set; }

        [Display(Name = "To Fiscal Year", Order = 8), InverseProperty("Rollover2s1")]
        public virtual FiscalYear2 ToFiscalYear { get; set; }

        [Column("to_fiscal_year_id"), Display(Name = "To Fiscal Year", Order = 9), JsonProperty("toFiscalYearId")]
        public virtual Guid? ToFiscalYearId { get; set; }

        [Column("restrict_encumbrance"), Display(Name = "Restrict Encumbrance", Order = 10), JsonProperty("restrictEncumbrance")]
        public virtual bool? RestrictEncumbrance { get; set; }

        [Column("restrict_expenditures"), Display(Name = "Restrict Expenditures", Order = 11), JsonProperty("restrictExpenditures")]
        public virtual bool? RestrictExpenditures { get; set; }

        [Column("need_close_budgets"), Display(Name = "Need Close Budgets", Order = 12), JsonProperty("needCloseBudgets")]
        public virtual bool? NeedCloseBudgets { get; set; }

        [Column("currency_factor"), Display(Name = "Currency Factor", Order = 13), Editable(false), JsonProperty("currencyFactor")]
        public virtual int? CurrencyFactor { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 15), InverseProperty("Rollover2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 16), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 18), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 19), InverseProperty("Rollover2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 20), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Rollover), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 22), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Rollover Budgets", Order = 23)]
        public virtual ICollection<RolloverBudget2> RolloverBudget2s { get; set; }

        [Display(Name = "Rollover Budgets Rollovers", Order = 24), JsonProperty("budgetsRollover")]
        public virtual ICollection<RolloverBudgetsRollover> RolloverBudgetsRollovers { get; set; }

        [Display(Name = "Rollover Encumbrances Rollovers", Order = 25), JsonProperty("encumbrancesRollover")]
        public virtual ICollection<RolloverEncumbrancesRollover> RolloverEncumbrancesRollovers { get; set; }

        [Display(Name = "Rollover Errors", Order = 26)]
        public virtual ICollection<RolloverError2> RolloverError2s { get; set; }

        [Display(Name = "Rollover Progresss", Order = 27)]
        public virtual ICollection<RolloverProgress2> RolloverProgress2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(LedgerId)} = {LedgerId}, {nameof(RolloverType)} = {RolloverType}, {nameof(FromFiscalYearId)} = {FromFiscalYearId}, {nameof(ToFiscalYearId)} = {ToFiscalYearId}, {nameof(RestrictEncumbrance)} = {RestrictEncumbrance}, {nameof(RestrictExpenditures)} = {RestrictExpenditures}, {nameof(NeedCloseBudgets)} = {NeedCloseBudgets}, {nameof(CurrencyFactor)} = {CurrencyFactor}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(RolloverBudgetsRollovers)} = {(RolloverBudgetsRollovers != null ? $"{{ {string.Join(", ", RolloverBudgetsRollovers)} }}" : "")}, {nameof(RolloverEncumbrancesRollovers)} = {(RolloverEncumbrancesRollovers != null ? $"{{ {string.Join(", ", RolloverEncumbrancesRollovers)} }}" : "")} }}";

        public static Rollover2 FromJObject(JObject jObject) => jObject != null ? new Rollover2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            LedgerId = (Guid?)jObject.SelectToken("ledgerId"),
            RolloverType = (string)jObject.SelectToken("rolloverType"),
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
            RolloverBudgetsRollovers = jObject.SelectToken("budgetsRollover")?.Where(jt => jt.HasValues).Select(jt => RolloverBudgetsRollover.FromJObject((JObject)jt)).ToArray(),
            RolloverEncumbrancesRollovers = jObject.SelectToken("encumbrancesRollover")?.Where(jt => jt.HasValues).Select(jt => RolloverEncumbrancesRollover.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("ledgerId", LedgerId),
            new JProperty("rolloverType", RolloverType),
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
            new JProperty("budgetsRollover", RolloverBudgetsRollovers?.Select(rbr => rbr.ToJObject())),
            new JProperty("encumbrancesRollover", RolloverEncumbrancesRollovers?.Select(rer => rer.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
