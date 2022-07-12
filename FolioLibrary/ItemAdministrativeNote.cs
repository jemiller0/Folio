using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_administrative_notes -> uchicago_mod_inventory_storage.item
    // ItemAdministrativeNote -> Item
    [DisplayColumn(nameof(Content)), DisplayName("Item Administrative Notes"), Table("item_administrative_notes", Schema = "uc")]
    public partial class ItemAdministrativeNote
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemAdministrativeNoteKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemAdministrativeNoteKey == ((ItemAdministrativeNote)obj).ItemAdministrativeNoteKey;
        }

        public override int GetHashCode() => ItemAdministrativeNoteKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Content)} = {Content} }}";

        public static ItemAdministrativeNote FromJObject(JValue jObject) => jObject != null ? new ItemAdministrativeNote
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
