using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.permission_granted_to -> diku_mod_permissions.permissions
    // PermissionGrantedTo -> Permission
    [DisplayColumn(nameof(Id)), DisplayName("Permission Granted Tos"), Table("permission_granted_to", Schema = "uc")]
    public partial class PermissionGrantedTo
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Permission2 Permission { get; set; }

        [Column("permission_id"), Display(Name = "Permission", Order = 3), Required]
        public virtual Guid? PermissionId { get; set; }

        [Display(Name = "Permissions User", Order = 4)]
        public virtual PermissionsUser2 PermissionsUser { get; set; }

        [Column("permissions_user_id"), Display(Name = "Permissions User", Order = 5), Required]
        public virtual Guid? PermissionsUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PermissionId)} = {PermissionId}, {nameof(PermissionsUserId)} = {PermissionsUserId} }}";

        public static PermissionGrantedTo FromJObject(JValue jObject) => jObject != null ? new PermissionGrantedTo
        {
            PermissionsUserId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(PermissionsUserId);
    }
}
