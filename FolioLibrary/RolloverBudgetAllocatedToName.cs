using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_allocated_to_names -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetAllocatedToName -> RolloverBudget
    [DisplayColumn(nameof(Content)), DisplayName("Rollover Budget Allocated To Names"), Table("rollover_budget_allocated_to_names", Schema = "uc")]
    public partial class RolloverBudgetAllocatedToName
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(Content)} = {Content} }}";

        public static RolloverBudgetAllocatedToName FromJObject(JValue jObject) => jObject != null ? new RolloverBudgetAllocatedToName
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
