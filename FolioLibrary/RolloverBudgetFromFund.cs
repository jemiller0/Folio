using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_from_funds -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetFromFund -> RolloverBudget
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budget From Funds"), Table("rollover_budget_from_funds", Schema = "uc")]
    public partial class RolloverBudgetFromFund
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Display(Order = 4)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 5)]
        public virtual Guid? FundId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(FundId)} = {FundId} }}";

        public static RolloverBudgetFromFund FromJObject(JValue jObject) => jObject != null ? new RolloverBudgetFromFund
        {
            FundId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(FundId);
    }
}
