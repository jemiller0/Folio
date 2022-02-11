using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_former_ids -> uchicago_mod_inventory_storage.item
    // ItemFormerId -> Item
    [DisplayColumn(nameof(Content)), DisplayName("Item Former Ids"), Table("item_former_ids", Schema = "uc")]
    public partial class ItemFormerId
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemFormerIdKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemFormerIdKey == ((ItemFormerId)obj).ItemFormerIdKey;
        }

        public override int GetHashCode() => ItemFormerIdKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Content)} = {Content} }}";

        public static ItemFormerId FromJObject(JValue jObject) => jObject != null ? new ItemFormerId
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
