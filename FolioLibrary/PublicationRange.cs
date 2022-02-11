using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.publication_range -> uchicago_mod_inventory_storage.instance
    // PublicationRange -> Instance
    [DisplayColumn(nameof(Content)), DisplayName("Publication Ranges"), Table("publication_range", Schema = "uc")]
    public partial class PublicationRange
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string PublicationRangeKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return PublicationRangeKey == ((PublicationRange)obj).PublicationRangeKey;
        }

        public override int GetHashCode() => PublicationRangeKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static PublicationRange FromJObject(JValue jObject) => jObject != null ? new PublicationRange
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
