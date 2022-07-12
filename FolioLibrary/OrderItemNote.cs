using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_notes -> uc.object_notes
    // OrderItemNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_notes", Schema = "uc")]
    public partial class OrderItemNote
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Note2 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(NoteId)} = {NoteId} }}";

        public static OrderItemNote FromJObject(JObject jObject) => jObject != null ? new OrderItemNote
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
