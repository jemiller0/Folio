using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.permissions_user_permissions -> diku_mod_permissions.permissions_users
    // PermissionsUserPermission -> PermissionsUser
    [DisplayColumn(nameof(Content)), DisplayName("Permissions User Permissions"), Table("permissions_user_permissions", Schema = "uc")]
    public partial class PermissionsUserPermission
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Permissions User", Order = 2)]
        public virtual PermissionsUser2 PermissionsUser { get; set; }

        [Column("permissions_user_id"), Display(Name = "Permissions User", Order = 3), Required]
        public virtual Guid? PermissionsUserId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PermissionsUserId)} = {PermissionsUserId}, {nameof(Content)} = {Content} }}";

        public static PermissionsUserPermission FromJObject(JValue jObject) => jObject != null ? new PermissionsUserPermission
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
