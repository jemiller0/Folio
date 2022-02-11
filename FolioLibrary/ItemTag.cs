using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_tags -> uchicago_mod_inventory_storage.item
    // ItemTag -> Item
    [DisplayColumn(nameof(Content)), DisplayName("Item Tags"), Table("item_tags", Schema = "uc")]
    public partial class ItemTag
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemTagKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemTagKey == ((ItemTag)obj).ItemTagKey;
        }

        public override int GetHashCode() => ItemTagKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Content)} = {Content} }}";

        public static ItemTag FromJObject(JValue jObject) => jObject != null ? new ItemTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
