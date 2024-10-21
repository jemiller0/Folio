using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_organizations -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetOrganization -> RolloverBudget
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budget Organizations"), Table("rollover_budget_organizations", Schema = "uc")]
    public partial class RolloverBudgetOrganization
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Display(Order = 4)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 5)]
        public virtual Guid? OrganizationId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(OrganizationId)} = {OrganizationId} }}";

        public static RolloverBudgetOrganization FromJObject(JValue jObject) => jObject != null ? new RolloverBudgetOrganization
        {
            OrganizationId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(OrganizationId);
    }
}
