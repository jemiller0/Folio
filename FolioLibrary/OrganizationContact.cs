using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_contacts -> diku_mod_organizations_storage.organizations
    // OrganizationContact -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Contacts"), Table("organization_contacts", Schema = "uc")]
    public partial class OrganizationContact
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Order = 4)]
        public virtual Contact2 Contact { get; set; }

        [Column("contact_id"), Display(Name = "Contact", Order = 5), Required]
        public virtual Guid? ContactId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(ContactId)} = {ContactId} }}";

        public static OrganizationContact FromJObject(JValue jObject) => jObject != null ? new OrganizationContact
        {
            ContactId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(ContactId);
    }
}
