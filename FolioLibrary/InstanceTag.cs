using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.instance_tags -> uchicago_mod_inventory_storage.instance
    // InstanceTag -> Instance
    [DisplayColumn(nameof(Content)), DisplayName("Instance Tags"), Table("instance_tags", Schema = "uc")]
    public partial class InstanceTag
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string InstanceTagKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return InstanceTagKey == ((InstanceTag)obj).InstanceTagKey;
        }

        public override int GetHashCode() => InstanceTagKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static InstanceTag FromJObject(JValue jObject) => jObject != null ? new InstanceTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
