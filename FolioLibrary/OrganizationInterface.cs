using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_interfaces -> uchicago_mod_organizations_storage.organizations
    // OrganizationInterface -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Interfaces"), Table("organization_interfaces", Schema = "uc")]
    public partial class OrganizationInterface
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Order = 4)]
        public virtual Interface2 Interface { get; set; }

        [Column("interface_id"), Display(Name = "Interface", Order = 5), Required]
        public virtual Guid? InterfaceId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(InterfaceId)} = {InterfaceId} }}";

        public static OrganizationInterface FromJObject(JValue jObject) => jObject != null ? new OrganizationInterface
        {
            InterfaceId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(InterfaceId);
    }
}
