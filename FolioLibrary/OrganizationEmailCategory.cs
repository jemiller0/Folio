using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_email_categories -> diku_mod_organizations_storage.organizations
    // OrganizationEmailCategory -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Email Categories"), Table("organization_email_categories", Schema = "uc")]
    public partial class OrganizationEmailCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Organization Email", Order = 2)]
        public virtual OrganizationEmail OrganizationEmail { get; set; }

        [Column("organization_email_id"), Display(Name = "Organization Email", Order = 3), Required, StringLength(1024)]
        public virtual string OrganizationEmailId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationEmailId)} = {OrganizationEmailId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static OrganizationEmailCategory FromJObject(JValue jObject) => jObject != null ? new OrganizationEmailCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
