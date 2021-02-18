using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contact_address_categories -> diku_mod_organizations_storage.contacts
    // ContactAddressCategory -> Contact
    [DisplayColumn(nameof(Id)), DisplayName("Contact Address Categories"), Table("contact_address_categories", Schema = "uc")]
    public partial class ContactAddressCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Contact Address", Order = 2)]
        public virtual ContactAddress ContactAddress { get; set; }

        [Column("contact_address_id"), Display(Name = "Contact Address", Order = 3), Required, StringLength(1024)]
        public virtual string ContactAddressId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactAddressId)} = {ContactAddressId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static ContactAddressCategory FromJObject(JValue jObject) => jObject != null ? new ContactAddressCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
