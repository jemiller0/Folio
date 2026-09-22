using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_preferred_contact_types -> uchicago_mod_users.users
    // UserPreferredContactType -> User
    [DisplayColumn(nameof(Id)), DisplayName("User Preferred Contact Types"), Table("user_preferred_contact_types", Schema = "uc")]
    public partial class UserPreferredContactType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3)]
        public virtual Guid? UserId { get; set; }

        [Display(Name = "Contact Type", Order = 4)]
        public virtual ContactType ContactType { get; set; }

        [Column("contact_type_id"), Display(Name = "Contact Type", Order = 5), StringLength(3)]
        public virtual string ContactTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(ContactTypeId)} = {ContactTypeId} }}";

        public static UserPreferredContactType FromJObject(JValue jObject) => jObject != null ? new UserPreferredContactType
        {
            ContactTypeId = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(ContactTypeId);
    }
}
