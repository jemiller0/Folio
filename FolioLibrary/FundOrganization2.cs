using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fund_organizations -> uchicago_mod_finance_storage.fund
    // FundOrganization2 -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Fund Organizations"), Table("fund_organizations", Schema = "uc")]
    public partial class FundOrganization2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3)]
        public virtual Guid? FundId { get; set; }

        [Display(Order = 4)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 5)]
        public virtual Guid? OrganizationId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(OrganizationId)} = {OrganizationId} }}";

        public static FundOrganization2 FromJObject(JValue jObject) => jObject != null ? new FundOrganization2
        {
            OrganizationId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(OrganizationId);
    }
}
