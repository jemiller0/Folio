using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_alerts -> uchicago_mod_orders_storage.po_line
    // OrderItemAlert -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Alerts"), Table("order_item_alerts", Schema = "uc")]
    public partial class OrderItemAlert
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Alert2 Alert { get; set; }

        [Column("alert_id"), Display(Name = "Alert", Order = 5), Required]
        public virtual Guid? AlertId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(AlertId)} = {AlertId} }}";

        public static OrderItemAlert FromJObject(JValue jObject) => jObject != null ? new OrderItemAlert
        {
            AlertId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AlertId);
    }
}
