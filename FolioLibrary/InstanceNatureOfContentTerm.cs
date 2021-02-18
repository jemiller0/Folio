using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.instance_nature_of_content_terms -> diku_mod_inventory_storage.instance
    // InstanceNatureOfContentTerm -> Instance
    [DisplayColumn(nameof(Id)), DisplayName("Instance Nature Of Content Terms"), Table("instance_nature_of_content_terms", Schema = "uc")]
    public partial class InstanceNatureOfContentTerm
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string InstanceNatureOfContentTermKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return InstanceNatureOfContentTermKey == ((InstanceNatureOfContentTerm)obj).InstanceNatureOfContentTermKey;
        }

        public override int GetHashCode() => InstanceNatureOfContentTermKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Display(Name = "Nature Of Content Term", Order = 4)]
        public virtual NatureOfContentTerm2 NatureOfContentTerm { get; set; }

        [Column("nature_of_content_term_id"), Display(Name = "Nature Of Content Term", Order = 5), Required]
        public virtual Guid? NatureOfContentTermId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(NatureOfContentTermId)} = {NatureOfContentTermId} }}";

        public static InstanceNatureOfContentTerm FromJObject(JValue jObject) => jObject != null ? new InstanceNatureOfContentTerm
        {
            NatureOfContentTermId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(NatureOfContentTermId);
    }
}
