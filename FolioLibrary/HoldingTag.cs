using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_tags -> diku_mod_inventory_storage.holdings_record
    // HoldingTag -> Holding
    [DisplayColumn(nameof(Content)), DisplayName("Holding Tags"), Table("holding_tags", Schema = "uc")]
    public partial class HoldingTag
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string HoldingTagKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return HoldingTagKey == ((HoldingTag)obj).HoldingTagKey;
        }

        public override int GetHashCode() => HoldingTagKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(Content)} = {Content} }}";

        public static HoldingTag FromJObject(JValue jObject) => jObject != null ? new HoldingTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
