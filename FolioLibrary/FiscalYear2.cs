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
    // uc.fiscal_years -> diku_mod_finance_storage.fiscal_year
    // FiscalYear2 -> FiscalYear
    [DisplayColumn(nameof(Name)), DisplayName("Fiscal Years"), JsonConverter(typeof(JsonPathJsonConverter<FiscalYear2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fiscal_years", Schema = "uc")]
    public partial class FiscalYear2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.FiscalYear.json")))
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

        [Column("currency"), Display(Order = 4), JsonProperty("currency"), StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("period_start"), DataType(DataType.Date), Display(Name = "Start Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("periodStart"), Required]
        public virtual DateTime? StartDate { get; set; }

        [Column("period_end"), DataType(DataType.Date), Display(Name = "End Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("periodEnd"), Required]
        public virtual DateTime? EndDate { get; set; }

        [Column("series"), Display(Order = 8), JsonProperty("series"), StringLength(1024)]
        public virtual string Series { get; set; }

        [Column("financial_summary_allocated"), DataType(DataType.Currency), Display(Name = "Financial Summary Allocated", Order = 9), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.allocated")]
        public virtual decimal? FinancialSummaryAllocated { get; set; }

        [Column("financial_summary_available"), DataType(DataType.Currency), Display(Name = "Financial Summary Available", Order = 10), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.available")]
        public virtual decimal? FinancialSummaryAvailable { get; set; }

        [Column("financial_summary_unavailable"), DataType(DataType.Currency), Display(Name = "Financial Summary Unavailable", Order = 11), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.unavailable")]
        public virtual decimal? FinancialSummaryUnavailable { get; set; }

        [Column("financial_summary_initial_allocation"), DataType(DataType.Currency), Display(Name = "Financial Summary Initial Allocation", Order = 12), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.initialAllocation")]
        public virtual decimal? FinancialSummaryInitialAllocation { get; set; }

        [Column("financial_summary_allocation_to"), DataType(DataType.Currency), Display(Name = "Financial Summary Allocation To", Order = 13), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.allocationTo")]
        public virtual decimal? FinancialSummaryAllocationTo { get; set; }

        [Column("financial_summary_allocation_from"), DataType(DataType.Currency), Display(Name = "Financial Summary Allocation From", Order = 14), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.allocationFrom")]
        public virtual decimal? FinancialSummaryAllocationFrom { get; set; }

        [Column("financial_summary_total_funding"), DataType(DataType.Currency), Display(Name = "Financial Summary Total Funding", Order = 15), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.totalFunding")]
        public virtual decimal? FinancialSummaryTotalFunding { get; set; }

        [Column("financial_summary_cash_balance"), DataType(DataType.Currency), Display(Name = "Financial Summary Cash Balance", Order = 16), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.cashBalance")]
        public virtual decimal? FinancialSummaryCashBalance { get; set; }

        [Column("financial_summary_awaiting_payment"), DataType(DataType.Currency), Display(Name = "Financial Summary Awaiting Payment", Order = 17), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.awaitingPayment")]
        public virtual decimal? FinancialSummaryAwaitingPayment { get; set; }

        [Column("financial_summary_encumbered"), DataType(DataType.Currency), Display(Name = "Financial Summary Encumbered", Order = 18), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.encumbered")]
        public virtual decimal? FinancialSummaryEncumbered { get; set; }

        [Column("financial_summary_expenditures"), DataType(DataType.Currency), Display(Name = "Financial Summary Expenditures", Order = 19), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.expenditures")]
        public virtual decimal? FinancialSummaryExpenditures { get; set; }

        [Column("financial_summary_over_encumbrance"), DataType(DataType.Currency), Display(Name = "Financial Summary Over Encumbrance", Order = 20), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.overEncumbrance")]
        public virtual decimal? FinancialSummaryOverEncumbrance { get; set; }

        [Column("financial_summary_over_expended"), DataType(DataType.Currency), Display(Name = "Financial Summary Over Expended", Order = 21), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("financialSummary.overExpended")]
        public virtual decimal? FinancialSummaryOverExpended { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 22), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 23), InverseProperty("FiscalYear2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 24), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 26), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 27), InverseProperty("FiscalYear2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 28), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(FiscalYear), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 30), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Budgets", Order = 31)]
        public virtual ICollection<Budget2> Budget2s { get; set; }

        [Display(Name = "Budget Groups", Order = 32)]
        public virtual ICollection<BudgetGroup2> BudgetGroup2s { get; set; }

        [Display(Name = "Fiscal Year Acquisitions Units", Order = 33), JsonConverter(typeof(ArrayJsonConverter<List<FiscalYearAcquisitionsUnit>, FiscalYearAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<FiscalYearAcquisitionsUnit> FiscalYearAcquisitionsUnits { get; set; }

        [Display(Name = "Ledgers", Order = 34)]
        public virtual ICollection<Ledger2> Ledger2s { get; set; }

        [Display(Name = "Ledger Rollovers", Order = 35)]
        public virtual ICollection<LedgerRollover2> LedgerRollover2s { get; set; }

        [Display(Name = "Ledger Rollovers 1", Order = 36)]
        public virtual ICollection<LedgerRollover2> LedgerRollover2s1 { get; set; }

        [Display(Name = "Transactions", Order = 37)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Transactions 1", Order = 38)]
        public virtual ICollection<Transaction2> Transaction2s1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Code)} = {Code}, {nameof(Currency)} = {Currency}, {nameof(Description)} = {Description}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(Series)} = {Series}, {nameof(FinancialSummaryAllocated)} = {FinancialSummaryAllocated}, {nameof(FinancialSummaryAvailable)} = {FinancialSummaryAvailable}, {nameof(FinancialSummaryUnavailable)} = {FinancialSummaryUnavailable}, {nameof(FinancialSummaryInitialAllocation)} = {FinancialSummaryInitialAllocation}, {nameof(FinancialSummaryAllocationTo)} = {FinancialSummaryAllocationTo}, {nameof(FinancialSummaryAllocationFrom)} = {FinancialSummaryAllocationFrom}, {nameof(FinancialSummaryTotalFunding)} = {FinancialSummaryTotalFunding}, {nameof(FinancialSummaryCashBalance)} = {FinancialSummaryCashBalance}, {nameof(FinancialSummaryAwaitingPayment)} = {FinancialSummaryAwaitingPayment}, {nameof(FinancialSummaryEncumbered)} = {FinancialSummaryEncumbered}, {nameof(FinancialSummaryExpenditures)} = {FinancialSummaryExpenditures}, {nameof(FinancialSummaryOverEncumbrance)} = {FinancialSummaryOverEncumbrance}, {nameof(FinancialSummaryOverExpended)} = {FinancialSummaryOverExpended}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(FiscalYearAcquisitionsUnits)} = {(FiscalYearAcquisitionsUnits != null ? $"{{ {string.Join(", ", FiscalYearAcquisitionsUnits)} }}" : "")} }}";

        public static FiscalYear2 FromJObject(JObject jObject) => jObject != null ? new FiscalYear2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Code = (string)jObject.SelectToken("code"),
            Currency = (string)jObject.SelectToken("currency"),
            Description = (string)jObject.SelectToken("description"),
            StartDate = ((DateTime?)jObject.SelectToken("periodStart"))?.ToLocalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("periodEnd"))?.ToLocalTime(),
            Series = (string)jObject.SelectToken("series"),
            FinancialSummaryAllocated = (decimal?)jObject.SelectToken("financialSummary.allocated"),
            FinancialSummaryAvailable = (decimal?)jObject.SelectToken("financialSummary.available"),
            FinancialSummaryUnavailable = (decimal?)jObject.SelectToken("financialSummary.unavailable"),
            FinancialSummaryInitialAllocation = (decimal?)jObject.SelectToken("financialSummary.initialAllocation"),
            FinancialSummaryAllocationTo = (decimal?)jObject.SelectToken("financialSummary.allocationTo"),
            FinancialSummaryAllocationFrom = (decimal?)jObject.SelectToken("financialSummary.allocationFrom"),
            FinancialSummaryTotalFunding = (decimal?)jObject.SelectToken("financialSummary.totalFunding"),
            FinancialSummaryCashBalance = (decimal?)jObject.SelectToken("financialSummary.cashBalance"),
            FinancialSummaryAwaitingPayment = (decimal?)jObject.SelectToken("financialSummary.awaitingPayment"),
            FinancialSummaryEncumbered = (decimal?)jObject.SelectToken("financialSummary.encumbered"),
            FinancialSummaryExpenditures = (decimal?)jObject.SelectToken("financialSummary.expenditures"),
            FinancialSummaryOverEncumbrance = (decimal?)jObject.SelectToken("financialSummary.overEncumbrance"),
            FinancialSummaryOverExpended = (decimal?)jObject.SelectToken("financialSummary.overExpended"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            FiscalYearAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => FiscalYearAcquisitionsUnit.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("code", Code),
            new JProperty("currency", Currency),
            new JProperty("description", Description),
            new JProperty("periodStart", StartDate?.ToUniversalTime()),
            new JProperty("periodEnd", EndDate?.ToUniversalTime()),
            new JProperty("series", Series),
            new JProperty("financialSummary", new JObject(
                new JProperty("allocated", FinancialSummaryAllocated),
                new JProperty("available", FinancialSummaryAvailable),
                new JProperty("unavailable", FinancialSummaryUnavailable),
                new JProperty("initialAllocation", FinancialSummaryInitialAllocation),
                new JProperty("allocationTo", FinancialSummaryAllocationTo),
                new JProperty("allocationFrom", FinancialSummaryAllocationFrom),
                new JProperty("totalFunding", FinancialSummaryTotalFunding),
                new JProperty("cashBalance", FinancialSummaryCashBalance),
                new JProperty("awaitingPayment", FinancialSummaryAwaitingPayment),
                new JProperty("encumbered", FinancialSummaryEncumbered),
                new JProperty("expenditures", FinancialSummaryExpenditures),
                new JProperty("overEncumbrance", FinancialSummaryOverEncumbrance),
                new JProperty("overExpended", FinancialSummaryOverExpended))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", FiscalYearAcquisitionsUnits?.Select(fyau => fyau.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
