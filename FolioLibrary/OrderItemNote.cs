using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_notes -> uc.object_notes
    // OrderItemNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Notes"), Table("order_item_notes", Schema = "uc")]
    public partial class OrderItemNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(NoteId)} = {NoteId} }}";

        public static OrderItemNote FromJObject(JValue jObject) => jObject != null ? new OrderItemNote
        {
            NoteId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
