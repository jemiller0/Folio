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
    // uc.budgets -> diku_mod_finance_storage.budget
    // Budget2 -> Budget
    [DisplayColumn(nameof(Name)), DisplayName("Budgets"), JsonConverter(typeof(JsonPathJsonConverter<Budget2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("budgets", Schema = "uc")]
    public partial class Budget2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Budget.json")))
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

        [Column("budget_status"), Display(Name = "Budget Status", Order = 3), JsonProperty("budgetStatus"), RegularExpression(@"^(Active|Frozen|Inactive|Planned|Closed)$"), Required, StringLength(1024)]
        public virtual string BudgetStatus { get; set; }

        [Column("allowable_encumbrance"), Display(Name = "Allowable Encumbrance", Order = 4), JsonProperty("allowableEncumbrance")]
        public virtual decimal? AllowableEncumbrance { get; set; }

        [Column("allowable_expenditure"), Display(Name = "Allowable Expenditure", Order = 5), JsonProperty("allowableExpenditure")]
        public virtual decimal? AllowableExpenditure { get; set; }

        [Column("allocated"), DataType(DataType.Currency), Display(Order = 6), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("allocated")]
        public virtual decimal? Allocated { get; set; }

        [Column("awaiting_payment"), DataType(DataType.Currency), Display(Name = "Awaiting Payment", Order = 7), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("awaitingPayment")]
        public virtual decimal? AwaitingPayment { get; set; }

        [Column("available"), DataType(DataType.Currency), Display(Order = 8), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("available")]
        public virtual decimal? Available { get; set; }

        [Column("encumbered"), DataType(DataType.Currency), Display(Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("encumbered")]
        public virtual decimal? Encumbered { get; set; }

        [Column("expenditures"), DataType(DataType.Currency), Display(Order = 10), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("expenditures")]
        public virtual decimal? Expenditures { get; set; }

        [Column("net_transfers"), DataType(DataType.Currency), Display(Name = "Net Transfers", Order = 11), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("netTransfers")]
        public virtual decimal? NetTransfers { get; set; }

        [Column("unavailable"), DataType(DataType.Currency), Display(Order = 12), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("unavailable")]
        public virtual decimal? Unavailable { get; set; }

        [Column("over_encumbrance"), DataType(DataType.Currency), Display(Name = "Over Encumbrance", Order = 13), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("overEncumbrance")]
        public virtual decimal? OverEncumbrance { get; set; }

        [Column("over_expended"), DataType(DataType.Currency), Display(Name = "Over Expended", Order = 14), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("overExpended")]
        public virtual decimal? OverExpended { get; set; }

        [Display(Order = 15)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 16), JsonProperty("fundId"), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Fiscal Year", Order = 17)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 18), JsonProperty("fiscalYearId"), Required]
        public virtual Guid? FiscalYearId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 20), InverseProperty("Budget2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 21), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 23), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 24), InverseProperty("Budget2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 25), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("initial_allocation"), DataType(DataType.Currency), Display(Name = "Initial Allocation", Order = 27), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("initialAllocation")]
        public virtual decimal? InitialAllocation { get; set; }

        [Column("allocation_to"), DataType(DataType.Currency), Display(Name = "Allocation To", Order = 28), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("allocationTo")]
        public virtual decimal? AllocationTo { get; set; }

        [Column("allocation_from"), DataType(DataType.Currency), Display(Name = "Allocation From", Order = 29), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("allocationFrom")]
        public virtual decimal? AllocationFrom { get; set; }

        [Column("total_funding"), DataType(DataType.Currency), Display(Name = "Total Funding", Order = 30), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("totalFunding")]
        public virtual decimal? TotalFunding { get; set; }

        [Column("cash_balance"), DataType(DataType.Currency), Display(Name = "Cash Balance", Order = 31), DisplayFormat(DataFormatString = "{0:c}"), Editable(false), JsonProperty("cashBalance")]
        public virtual decimal? CashBalance { get; set; }

        [Column("content"), CustomValidation(typeof(Budget), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 32), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Budget Acquisitions Units", Order = 33), JsonConverter(typeof(ArrayJsonConverter<List<BudgetAcquisitionsUnit>, BudgetAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<BudgetAcquisitionsUnit> BudgetAcquisitionsUnits { get; set; }

        [Display(Name = "Budget Expense Classs", Order = 34)]
        public virtual ICollection<BudgetExpenseClass2> BudgetExpenseClass2s { get; set; }

        [Display(Name = "Budget Groups", Order = 35)]
        public virtual ICollection<BudgetGroup2> BudgetGroup2s { get; set; }

        [Display(Name = "Budget Tags", Order = 36), JsonConverter(typeof(ArrayJsonConverter<List<BudgetTag>, BudgetTag>), "TagId"), JsonProperty("tags.tagList")]
        public virtual ICollection<BudgetTag> BudgetTags { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(BudgetStatus)} = {BudgetStatus}, {nameof(AllowableEncumbrance)} = {AllowableEncumbrance}, {nameof(AllowableExpenditure)} = {AllowableExpenditure}, {nameof(Allocated)} = {Allocated}, {nameof(AwaitingPayment)} = {AwaitingPayment}, {nameof(Available)} = {Available}, {nameof(Encumbered)} = {Encumbered}, {nameof(Expenditures)} = {Expenditures}, {nameof(NetTransfers)} = {NetTransfers}, {nameof(Unavailable)} = {Unavailable}, {nameof(OverEncumbrance)} = {OverEncumbrance}, {nameof(OverExpended)} = {OverExpended}, {nameof(FundId)} = {FundId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(InitialAllocation)} = {InitialAllocation}, {nameof(AllocationTo)} = {AllocationTo}, {nameof(AllocationFrom)} = {AllocationFrom}, {nameof(TotalFunding)} = {TotalFunding}, {nameof(CashBalance)} = {CashBalance}, {nameof(Content)} = {Content}, {nameof(BudgetAcquisitionsUnits)} = {(BudgetAcquisitionsUnits != null ? $"{{ {string.Join(", ", BudgetAcquisitionsUnits)} }}" : "")}, {nameof(BudgetTags)} = {(BudgetTags != null ? $"{{ {string.Join(", ", BudgetTags)} }}" : "")} }}";

        public static Budget2 FromJObject(JObject jObject) => jObject != null ? new Budget2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            BudgetStatus = (string)jObject.SelectToken("budgetStatus"),
            AllowableEncumbrance = (decimal?)jObject.SelectToken("allowableEncumbrance"),
            AllowableExpenditure = (decimal?)jObject.SelectToken("allowableExpenditure"),
            Allocated = (decimal?)jObject.SelectToken("allocated"),
            AwaitingPayment = (decimal?)jObject.SelectToken("awaitingPayment"),
            Available = (decimal?)jObject.SelectToken("available"),
            Encumbered = (decimal?)jObject.SelectToken("encumbered"),
            Expenditures = (decimal?)jObject.SelectToken("expenditures"),
            NetTransfers = (decimal?)jObject.SelectToken("netTransfers"),
            Unavailable = (decimal?)jObject.SelectToken("unavailable"),
            OverEncumbrance = (decimal?)jObject.SelectToken("overEncumbrance"),
            OverExpended = (decimal?)jObject.SelectToken("overExpended"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            InitialAllocation = (decimal?)jObject.SelectToken("initialAllocation"),
            AllocationTo = (decimal?)jObject.SelectToken("allocationTo"),
            AllocationFrom = (decimal?)jObject.SelectToken("allocationFrom"),
            TotalFunding = (decimal?)jObject.SelectToken("totalFunding"),
            CashBalance = (decimal?)jObject.SelectToken("cashBalance"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            BudgetAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => BudgetAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            BudgetTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => BudgetTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("budgetStatus", BudgetStatus),
            new JProperty("allowableEncumbrance", AllowableEncumbrance),
            new JProperty("allowableExpenditure", AllowableExpenditure),
            new JProperty("allocated", Allocated),
            new JProperty("awaitingPayment", AwaitingPayment),
            new JProperty("available", Available),
            new JProperty("encumbered", Encumbered),
            new JProperty("expenditures", Expenditures),
            new JProperty("netTransfers", NetTransfers),
            new JProperty("unavailable", Unavailable),
            new JProperty("overEncumbrance", OverEncumbrance),
            new JProperty("overExpended", OverExpended),
            new JProperty("fundId", FundId),
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("initialAllocation", InitialAllocation),
            new JProperty("allocationTo", AllocationTo),
            new JProperty("allocationFrom", AllocationFrom),
            new JProperty("totalFunding", TotalFunding),
            new JProperty("cashBalance", CashBalance),
            new JProperty("acqUnitIds", BudgetAcquisitionsUnits?.Select(bau => bau.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", BudgetTags?.Select(bt => bt.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
