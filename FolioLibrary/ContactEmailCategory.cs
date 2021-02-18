using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contact_email_categories -> diku_mod_organizations_storage.contacts
    // ContactEmailCategory -> Contact
    [DisplayColumn(nameof(Id)), DisplayName("Contact Email Categories"), Table("contact_email_categories", Schema = "uc")]
    public partial class ContactEmailCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Contact Email", Order = 2)]
        public virtual ContactEmail ContactEmail { get; set; }

        [Column("contact_email_id"), Display(Name = "Contact Email", Order = 3), Required, StringLength(1024)]
        public virtual string ContactEmailId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactEmailId)} = {ContactEmailId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static ContactEmailCategory FromJObject(JValue jObject) => jObject != null ? new ContactEmailCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
