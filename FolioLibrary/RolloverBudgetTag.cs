using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_tags -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetTag -> RolloverBudget
    [DisplayColumn(nameof(Content)), DisplayName("Rollover Budget Tags"), Table("rollover_budget_tags", Schema = "uc")]
    public partial class RolloverBudgetTag
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

        public static RolloverBudgetTag FromJObject(JValue jObject) => jObject != null ? new RolloverBudgetTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
