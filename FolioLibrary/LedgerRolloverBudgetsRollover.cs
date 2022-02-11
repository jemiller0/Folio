using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.ledger_rollover_budgets_rollover -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover
    // LedgerRolloverBudgetsRollover -> LedgerRollover
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Rollover Budgets Rollovers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledger_rollover_budgets_rollover", Schema = "uc")]
    public partial class LedgerRolloverBudgetsRollover
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Ledger Rollover", Order = 2)]
        public virtual LedgerRollover2 LedgerRollover { get; set; }

        [Column("ledger_rollover_id"), Display(Name = "Ledger Rollover", Order = 3)]
        public virtual Guid? LedgerRolloverId { get; set; }

        [Display(Name = "Fund Type", Order = 4)]
        public virtual FundType2 FundType { get; set; }

        [Column("fund_type_id"), Display(Name = "Fund Type", Order = 5), JsonProperty("fundTypeId")]
        public virtual Guid? FundTypeId { get; set; }

        [Column("rollover_allocation"), Display(Name = "Rollover Allocation", Order = 6), JsonProperty("rolloverAllocation")]
        public virtual bool? RolloverAllocation { get; set; }

        [Column("rollover_available"), Display(Name = "Rollover Available", Order = 7), JsonProperty("rolloverAvailable")]
        public virtual bool? RolloverAvailable { get; set; }

        [Column("set_allowances"), Display(Name = "Set Allowances", Order = 8), JsonProperty("setAllowances")]
        public virtual bool? SetAllowances { get; set; }

        [Column("adjust_allocation"), DataType(DataType.Currency), Display(Name = "Adjust Allocation", Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("adjustAllocation")]
        public virtual decimal? AdjustAllocation { get; set; }

        [Column("add_available_to"), Display(Name = "Add Available To", Order = 10), JsonProperty("addAvailableTo"), StringLength(1024)]
        public virtual string AddAvailableTo { get; set; }

        [Column("allowable_encumbrance"), DataType(DataType.Currency), Display(Name = "Allowable Encumbrance", Order = 11), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("allowableEncumbrance")]
        public virtual decimal? AllowableEncumbrance { get; set; }

        [Column("allowable_expenditure"), DataType(DataType.Currency), Display(Name = "Allowable Expenditure", Order = 12), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("allowableExpenditure")]
        public virtual decimal? AllowableExpenditure { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerRolloverId)} = {LedgerRolloverId}, {nameof(FundTypeId)} = {FundTypeId}, {nameof(RolloverAllocation)} = {RolloverAllocation}, {nameof(RolloverAvailable)} = {RolloverAvailable}, {nameof(SetAllowances)} = {SetAllowances}, {nameof(AdjustAllocation)} = {AdjustAllocation}, {nameof(AddAvailableTo)} = {AddAvailableTo}, {nameof(AllowableEncumbrance)} = {AllowableEncumbrance}, {nameof(AllowableExpenditure)} = {AllowableExpenditure} }}";

        public static LedgerRolloverBudgetsRollover FromJObject(JObject jObject) => jObject != null ? new LedgerRolloverBudgetsRollover
        {
            FundTypeId = (Guid?)jObject.SelectToken("fundTypeId"),
            RolloverAllocation = (bool?)jObject.SelectToken("rolloverAllocation"),
            RolloverAvailable = (bool?)jObject.SelectToken("rolloverAvailable"),
            SetAllowances = (bool?)jObject.SelectToken("setAllowances"),
            AdjustAllocation = (decimal?)jObject.SelectToken("adjustAllocation"),
            AddAvailableTo = (string)jObject.SelectToken("addAvailableTo"),
            AllowableEncumbrance = (decimal?)jObject.SelectToken("allowableEncumbrance"),
            AllowableExpenditure = (decimal?)jObject.SelectToken("allowableExpenditure")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("fundTypeId", FundTypeId),
            new JProperty("rolloverAllocation", RolloverAllocation),
            new JProperty("rolloverAvailable", RolloverAvailable),
            new JProperty("setAllowances", SetAllowances),
            new JProperty("adjustAllocation", AdjustAllocation),
            new JProperty("addAvailableTo", AddAvailableTo),
            new JProperty("allowableEncumbrance", AllowableEncumbrance),
            new JProperty("allowableExpenditure", AllowableExpenditure)).RemoveNullAndEmptyProperties();
    }
}
