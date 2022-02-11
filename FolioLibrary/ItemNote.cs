using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_notes -> uchicago_mod_inventory_storage.item
    // ItemNote -> Item
    [DisplayColumn(nameof(Id)), DisplayName("Item Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("item_notes", Schema = "uc")]
    public partial class ItemNote
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemNoteKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemNoteKey == ((ItemNote)obj).ItemNoteKey;
        }

        public override int GetHashCode() => ItemNoteKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Display(Name = "Item Note Type", Order = 4)]
        public virtual ItemNoteType2 ItemNoteType { get; set; }

        [Column("item_note_type_id"), Display(Name = "Item Note Type", Order = 5), JsonProperty("itemNoteTypeId")]
        public virtual Guid? ItemNoteTypeId { get; set; }

        [Column("note"), Display(Order = 6), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("staff_only"), Display(Name = "Staff Only", Order = 7), JsonProperty("staffOnly")]
        public virtual bool? StaffOnly { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(ItemNoteTypeId)} = {ItemNoteTypeId}, {nameof(Note)} = {Note}, {nameof(StaffOnly)} = {StaffOnly} }}";

        public static ItemNote FromJObject(JObject jObject) => jObject != null ? new ItemNote
        {
            ItemNoteTypeId = (Guid?)jObject.SelectToken("itemNoteTypeId"),
            Note = (string)jObject.SelectToken("note"),
            StaffOnly = (bool?)jObject.SelectToken("staffOnly")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("itemNoteTypeId", ItemNoteTypeId),
            new JProperty("note", Note),
            new JProperty("staffOnly", StaffOnly)).RemoveNullAndEmptyProperties();
    }
}
