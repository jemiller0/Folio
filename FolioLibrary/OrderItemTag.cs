using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_tags -> uchicago_mod_orders_storage.po_line
    // OrderItemTag -> OrderItem
    [DisplayColumn(nameof(Content)), DisplayName("Order Item Tags"), Table("order_item_tags", Schema = "uc")]
    public partial class OrderItemTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(Content)} = {Content} }}";

        public static OrderItemTag FromJObject(JValue jObject) => jObject != null ? new OrderItemTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
