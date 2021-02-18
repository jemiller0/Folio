using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_tags -> diku_mod_orders_storage.purchase_order
    // OrderTag -> Order
    [DisplayColumn(nameof(Content)), DisplayName("Order Tags"), Table("order_tags", Schema = "uc")]
    public partial class OrderTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Order2 Order { get; set; }

        [Column("order_id"), Display(Name = "Order", Order = 3), Required]
        public virtual Guid? OrderId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderId)} = {OrderId}, {nameof(Content)} = {Content} }}";

        public static OrderTag FromJObject(JValue jObject) => jObject != null ? new OrderTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
