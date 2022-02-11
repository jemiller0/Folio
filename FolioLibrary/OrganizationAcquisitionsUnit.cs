using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_acquisitions_units -> uchicago_mod_organizations_storage.organizations
    // OrganizationAcquisitionsUnit -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Acquisitions Units"), Table("organization_acquisitions_units", Schema = "uc")]
    public partial class OrganizationAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3)]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5)]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static OrganizationAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new OrganizationAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
