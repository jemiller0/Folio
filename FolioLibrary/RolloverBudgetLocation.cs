using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_locations -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetLocation -> RolloverBudget
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budget Locations"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollover_budget_locations", Schema = "uc")]
    public partial class RolloverBudgetLocation
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Display(Order = 4)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 5), JsonProperty("locationId")]
        public virtual Guid? LocationId { get; set; }

        [Column("tenant_id"), Display(Name = "Tenant Id", Order = 6), JsonProperty("tenantId")]
        public virtual Guid? TenantId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(LocationId)} = {LocationId}, {nameof(TenantId)} = {TenantId} }}";

        public static RolloverBudgetLocation FromJObject(JObject jObject) => jObject != null ? new RolloverBudgetLocation
        {
            LocationId = (Guid?)jObject.SelectToken("locationId"),
            TenantId = (Guid?)jObject.SelectToken("tenantId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("locationId", LocationId),
            new JProperty("tenantId", TenantId)).RemoveNullAndEmptyProperties();
    }
}
