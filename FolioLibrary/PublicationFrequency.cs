using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.publication_frequency -> diku_mod_inventory_storage.instance
    // PublicationFrequency -> Instance
    [DisplayColumn(nameof(Content)), DisplayName("Publication Frequencies"), Table("publication_frequency", Schema = "uc")]
    public partial class PublicationFrequency
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string PublicationFrequencyKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return PublicationFrequencyKey == ((PublicationFrequency)obj).PublicationFrequencyKey;
        }

        public override int GetHashCode() => PublicationFrequencyKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static PublicationFrequency FromJObject(JValue jObject) => jObject != null ? new PublicationFrequency
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
