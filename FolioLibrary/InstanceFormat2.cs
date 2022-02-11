using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.instance_formats -> uchicago_mod_inventory_storage.instance
    // InstanceFormat2 -> Instance
    [DisplayColumn(nameof(Id)), DisplayName("Instance Formats"), Table("instance_formats", Schema = "uc")]
    public partial class InstanceFormat2
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string InstanceFormat2Key => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return InstanceFormat2Key == ((InstanceFormat2)obj).InstanceFormat2Key;
        }

        public override int GetHashCode() => InstanceFormat2Key?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Display(Order = 4)]
        public virtual Format Format { get; set; }

        [Column("format_id"), Display(Name = "Format", Order = 5), Required]
        public virtual Guid? FormatId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(FormatId)} = {FormatId} }}";

        public static InstanceFormat2 FromJObject(JValue jObject) => jObject != null ? new InstanceFormat2
        {
            FormatId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(FormatId);
    }
}
