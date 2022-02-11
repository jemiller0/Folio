using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_account_acquisitions_units -> uchicago_mod_organizations_storage.organizations
    // OrganizationAccountAcquisitionsUnit -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Account Acquisitions Units"), Table("organization_account_acquisitions_units", Schema = "uc")]
    public partial class OrganizationAccountAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Organization Account", Order = 2)]
        public virtual OrganizationAccount OrganizationAccount { get; set; }

        [Column("organization_account_id"), Display(Name = "Organization Account", Order = 3), Required, StringLength(1024)]
        public virtual string OrganizationAccountId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationAccountId)} = {OrganizationAccountId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static OrganizationAccountAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new OrganizationAccountAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
