using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contact_categories -> diku_mod_organizations_storage.contacts
    // ContactCategory -> Contact
    [DisplayColumn(nameof(Id)), DisplayName("Contact Categories"), Table("contact_categories", Schema = "uc")]
    public partial class ContactCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Contact2 Contact { get; set; }

        [Column("contact_id"), Display(Name = "Contact", Order = 3), Required]
        public virtual Guid? ContactId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactId)} = {ContactId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static ContactCategory FromJObject(JValue jObject) => jObject != null ? new ContactCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
