using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contact_url_categories -> diku_mod_organizations_storage.contacts
    // ContactUrlCategory -> Contact
    [DisplayColumn(nameof(Id)), DisplayName("Contact URL Categories"), Table("contact_url_categories", Schema = "uc")]
    public partial class ContactUrlCategory
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Contact URL", Order = 2)]
        public virtual ContactUrl ContactUrl { get; set; }

        [Column("contact_url_id"), Display(Name = "Contact URL", Order = 3), Required, StringLength(1024)]
        public virtual string ContactUrlId { get; set; }

        [Display(Order = 4)]
        public virtual Category2 Category { get; set; }

        [Column("category_id"), Display(Name = "Category", Order = 5), Required]
        public virtual Guid? CategoryId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactUrlId)} = {ContactUrlId}, {nameof(CategoryId)} = {CategoryId} }}";

        public static ContactUrlCategory FromJObject(JValue jObject) => jObject != null ? new ContactUrlCategory
        {
            CategoryId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(CategoryId);
    }
}
