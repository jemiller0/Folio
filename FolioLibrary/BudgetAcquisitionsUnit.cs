using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.budget_acquisitions_units -> uchicago_mod_finance_storage.budget
    // BudgetAcquisitionsUnit -> Budget
    [DisplayColumn(nameof(Id)), DisplayName("Budget Acquisitions Units"), Table("budget_acquisitions_units", Schema = "uc")]
    public partial class BudgetAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Budget2 Budget { get; set; }

        [Column("budget_id"), Display(Name = "Budget", Order = 3), Required]
        public virtual Guid? BudgetId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BudgetId)} = {BudgetId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static BudgetAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new BudgetAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
