using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_tags -> uchicago_mod_users.users
    // UserTag -> User
    [DisplayColumn(nameof(Content)), DisplayName("User Tags"), Table("user_tags", Schema = "uc")]
    public partial class UserTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), Required]
        public virtual Guid? UserId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(Content)} = {Content} }}";

        public static UserTag FromJObject(JValue jObject) => jObject != null ? new UserTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
