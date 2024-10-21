using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fund_locations -> uchicago_mod_finance_storage.fund
    // FundLocation2 -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Fund Locations"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fund_locations", Schema = "uc")]
    public partial class FundLocation2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3)]
        public virtual Guid? FundId { get; set; }

        [Display(Order = 4)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 5), JsonProperty("locationId")]
        public virtual Guid? LocationId { get; set; }

        [Column("tenant_id"), Display(Name = "Tenant Id", Order = 6), JsonProperty("tenantId")]
        public virtual Guid? TenantId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(LocationId)} = {LocationId}, {nameof(TenantId)} = {TenantId} }}";

        public static FundLocation2 FromJObject(JObject jObject) => jObject != null ? new FundLocation2
        {
            LocationId = (Guid?)jObject.SelectToken("locationId"),
            TenantId = (Guid?)jObject.SelectToken("tenantId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("locationId", LocationId),
            new JProperty("tenantId", TenantId)).RemoveNullAndEmptyProperties();
    }
}
