using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contact_phone_number_categories -> uchicago_mod_organizations_storage.contacts
    // ContactPhoneNumberCategory -> Contact
    [DisplayColumn(nameof(Id)), DisplayName("Contact Phone Number Categories"), Table("contact_phone_number_categories", Schema = "uc")]
    public partial class ContactPhoneNumberCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Contact Phone Number", Order = 2)]
        public virtual ContactPhoneNumber ContactPhoneNumber { get; set; }

        [Column("contact_phone_number_id"), Display(Name = "Contact Phone Number", Order = 3), Required, StringLength(1024)]
        public virtual string ContactPhoneNumberId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactPhoneNumberId)} = {ContactPhoneNumberId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static ContactPhoneNumberCategory FromJObject(JValue jObject) => jObject != null ? new ContactPhoneNumberCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
