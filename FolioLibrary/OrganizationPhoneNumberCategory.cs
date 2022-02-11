using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_phone_number_categories -> uchicago_mod_organizations_storage.organizations
    // OrganizationPhoneNumberCategory -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Phone Number Categories"), Table("organization_phone_number_categories", Schema = "uc")]
    public partial class OrganizationPhoneNumberCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Organization Phone Number", Order = 2)]
        public virtual OrganizationPhoneNumber OrganizationPhoneNumber { get; set; }

        [Column("organization_phone_number_id"), Display(Name = "Organization Phone Number", Order = 3), Required, StringLength(1024)]
        public virtual string OrganizationPhoneNumberId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationPhoneNumberId)} = {OrganizationPhoneNumberId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static OrganizationPhoneNumberCategory FromJObject(JValue jObject) => jObject != null ? new OrganizationPhoneNumberCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
