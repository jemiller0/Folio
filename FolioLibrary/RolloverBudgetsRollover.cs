using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budgets_rollover -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover
    // RolloverBudgetsRollover -> Rollover
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budgets Rollovers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollover_budgets_rollover", Schema = "uc")]
    public partial class RolloverBudgetsRollover
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Rollover2 Rollover { get; set; }

        [Column("rollover_id"), Display(Name = "Rollover", Order = 3)]
        public virtual Guid? RolloverId { get; set; }

        [Display(Name = "Fund Type", Order = 4)]
        public virtual FundType2 FundType { get; set; }

        [Column("fund_type_id"), Display(Name = "Fund Type", Order = 5), JsonProperty("fundTypeId")]
        public virtual Guid? FundTypeId { get; set; }

        [Column("rollover_allocation"), Display(Name = "Rollover Allocation", Order = 6), JsonProperty("rolloverAllocation")]
        public virtual bool? RolloverAllocation { get; set; }

        [Column("rollover_budget_value"), Display(Name = "Rollover Budget Value", Order = 7), JsonProperty("rolloverBudgetValue"), StringLength(1024)]
        public virtual string RolloverBudgetValue { get; set; }

        [Column("set_allowances"), Display(Name = "Set Allowances", Order = 8), JsonProperty("setAllowances")]
        public virtual bool? SetAllowances { get; set; }

        [Column("adjust_allocation"), Display(Name = "Adjust Allocation", Order = 9), JsonProperty("adjustAllocation")]
        public virtual decimal? AdjustAllocation { get; set; }

        [Column("add_available_to"), Display(Name = "Add Available To", Order = 10), JsonProperty("addAvailableTo"), StringLength(1024)]
        public virtual string AddAvailableTo { get; set; }

        [Column("allowable_encumbrance"), Display(Name = "Allowable Encumbrance", Order = 11), JsonProperty("allowableEncumbrance")]
        public virtual decimal? AllowableEncumbrance { get; set; }

        [Column("allowable_expenditure"), Display(Name = "Allowable Expenditure", Order = 12), JsonProperty("allowableExpenditure")]
        public virtual decimal? AllowableExpenditure { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverId)} = {RolloverId}, {nameof(FundTypeId)} = {FundTypeId}, {nameof(RolloverAllocation)} = {RolloverAllocation}, {nameof(RolloverBudgetValue)} = {RolloverBudgetValue}, {nameof(SetAllowances)} = {SetAllowances}, {nameof(AdjustAllocation)} = {AdjustAllocation}, {nameof(AddAvailableTo)} = {AddAvailableTo}, {nameof(AllowableEncumbrance)} = {AllowableEncumbrance}, {nameof(AllowableExpenditure)} = {AllowableExpenditure} }}";

        public static RolloverBudgetsRollover FromJObject(JObject jObject) => jObject != null ? new RolloverBudgetsRollover
        {
            FundTypeId = (Guid?)jObject.SelectToken("fundTypeId"),
            RolloverAllocation = (bool?)jObject.SelectToken("rolloverAllocation"),
            RolloverBudgetValue = (string)jObject.SelectToken("rolloverBudgetValue"),
            SetAllowances = (bool?)jObject.SelectToken("setAllowances"),
            AdjustAllocation = (decimal?)jObject.SelectToken("adjustAllocation"),
            AddAvailableTo = (string)jObject.SelectToken("addAvailableTo"),
            AllowableEncumbrance = (decimal?)jObject.SelectToken("allowableEncumbrance"),
            AllowableExpenditure = (decimal?)jObject.SelectToken("allowableExpenditure")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("fundTypeId", FundTypeId),
            new JProperty("rolloverAllocation", RolloverAllocation),
            new JProperty("rolloverBudgetValue", RolloverBudgetValue),
            new JProperty("setAllowances", SetAllowances),
            new JProperty("adjustAllocation", AdjustAllocation),
            new JProperty("addAvailableTo", AddAvailableTo),
            new JProperty("allowableEncumbrance", AllowableEncumbrance),
            new JProperty("allowableExpenditure", AllowableExpenditure)).RemoveNullAndEmptyProperties();
    }
}
