using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_search_locations -> uchicago_mod_orders_storage.po_line
    // OrderItemSearchLocation -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Search Locations"), Table("order_item_search_locations", Schema = "uc")]
    public partial class OrderItemSearchLocation
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 5)]
        public virtual Guid? LocationId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(LocationId)} = {LocationId} }}";

        public static OrderItemSearchLocation FromJObject(JValue jObject) => jObject != null ? new OrderItemSearchLocation
        {
            LocationId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(LocationId);
    }
}
