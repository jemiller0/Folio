using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.permission_tags -> uchicago_mod_permissions.permissions
    // PermissionTag -> Permission
    [DisplayColumn(nameof(Content)), DisplayName("Permission Tags"), Table("permission_tags", Schema = "uc")]
    public partial class PermissionTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Permission2 Permission { get; set; }

        [Column("permission_id"), Display(Name = "Permission", Order = 3), Required]
        public virtual Guid? PermissionId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PermissionId)} = {PermissionId}, {nameof(Content)} = {Content} }}";

        public static PermissionTag FromJObject(JValue jObject) => jObject != null ? new PermissionTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
