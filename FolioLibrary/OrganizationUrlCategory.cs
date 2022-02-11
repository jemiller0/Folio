using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_url_categories -> uchicago_mod_organizations_storage.organizations
    // OrganizationUrlCategory -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization URL Categories"), Table("organization_url_categories", Schema = "uc")]
    public partial class OrganizationUrlCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Organization URL", Order = 2)]
        public virtual OrganizationUrl OrganizationUrl { get; set; }

        [Column("organization_url_id"), Display(Name = "Organization URL", Order = 3), Required, StringLength(1024)]
        public virtual string OrganizationUrlId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationUrlId)} = {OrganizationUrlId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static OrganizationUrlCategory FromJObject(JValue jObject) => jObject != null ? new OrganizationUrlCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
