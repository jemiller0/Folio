using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_organization_types -> uchicago_mod_organizations_storage.organizations
    // OrganizationOrganizationType -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Organization Types"), Table("organization_organization_types", Schema = "uc")]
    public partial class OrganizationOrganizationType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3)]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Organization Type", Order = 4)]
        public virtual OrganizationType2 OrganizationType { get; set; }

        [Column("organization_type_id"), Display(Name = "Organization Type", Order = 5)]
        public virtual Guid? OrganizationTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(OrganizationTypeId)} = {OrganizationTypeId} }}";

        public static OrganizationOrganizationType FromJObject(JValue jObject) => jObject != null ? new OrganizationOrganizationType
        {
            OrganizationTypeId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(OrganizationTypeId);
    }
}
