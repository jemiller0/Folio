using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_year_caption -> diku_mod_inventory_storage.item
    // ItemYearCaption -> Item
    [DisplayColumn(nameof(Content)), DisplayName("Item Year Captions"), Table("item_year_caption", Schema = "uc")]
    public partial class ItemYearCaption
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemYearCaptionKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemYearCaptionKey == ((ItemYearCaption)obj).ItemYearCaptionKey;
        }

        public override int GetHashCode() => ItemYearCaptionKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Content)} = {Content} }}";

        public static ItemYearCaption FromJObject(JValue jObject) => jObject != null ? new ItemYearCaption
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
