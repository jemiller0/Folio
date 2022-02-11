using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_address_categories -> uchicago_mod_organizations_storage.organizations
    // OrganizationAddressCategory -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Address Categories"), Table("organization_address_categories", Schema = "uc")]
    public partial class OrganizationAddressCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Organization Address", Order = 2)]
        public virtual OrganizationAddress OrganizationAddress { get; set; }

        [Column("organization_address_id"), Display(Name = "Organization Address", Order = 3), Required, StringLength(1024)]
        public virtual string OrganizationAddressId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationAddressId)} = {OrganizationAddressId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static OrganizationAddressCategory FromJObject(JValue jObject) => jObject != null ? new OrganizationAddressCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
