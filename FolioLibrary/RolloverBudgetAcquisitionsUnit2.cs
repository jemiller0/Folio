using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_acquisitions_units2 -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetAcquisitionsUnit2 -> RolloverBudget
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budget Acquisitions Units"), Table("rollover_budget_acquisitions_units2", Schema = "uc")]
    public partial class RolloverBudgetAcquisitionsUnit2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5)]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static RolloverBudgetAcquisitionsUnit2 FromJObject(JValue jObject) => jObject != null ? new RolloverBudgetAcquisitionsUnit2
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
