using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [DisplayColumn(nameof(Name)), DisplayName("Contact Types"), Table("contact_types", Schema = "uc")]
    public partial class ContactType
    {
        [Column("id"), ScaffoldColumn(false), StringLength(3)]
        public virtual string Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(128)]
        public virtual string Name { get; set; }

        [Display(Name = "Preferred Users", Order = 3)]
        public virtual ICollection<User2> PreferredUser2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name} }}";
    }
}
