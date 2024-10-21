using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_locations -> uchicago_mod_orders_storage.po_line
    // OrderItemLocation2 -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Locations"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_locations", Schema = "uc")]
    public partial class OrderItemLocation2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 5), JsonProperty("locationId"), Required]
        public virtual Guid? LocationId { get; set; }

        [Display(Order = 6)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 7), JsonProperty("holdingId")]
        public virtual Guid? HoldingId { get; set; }

        [Column("quantity"), Display(Order = 8), JsonProperty("quantity")]
        public virtual int? Quantity { get; set; }

        [Column("quantity_electronic"), Display(Name = "Electronic Quantity", Order = 9), JsonProperty("quantityElectronic")]
        public virtual int? ElectronicQuantity { get; set; }

        [Column("quantity_physical"), Display(Name = "Physical Quantity", Order = 10), JsonProperty("quantityPhysical")]
        public virtual int? PhysicalQuantity { get; set; }

        [Column("tenant_id"), Display(Name = "Tenant Id", Order = 11), JsonProperty("tenantId")]
        public virtual Guid? TenantId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(LocationId)} = {LocationId}, {nameof(HoldingId)} = {HoldingId}, {nameof(Quantity)} = {Quantity}, {nameof(ElectronicQuantity)} = {ElectronicQuantity}, {nameof(PhysicalQuantity)} = {PhysicalQuantity}, {nameof(TenantId)} = {TenantId} }}";

        public static OrderItemLocation2 FromJObject(JObject jObject) => jObject != null ? new OrderItemLocation2
        {
            LocationId = (Guid?)jObject.SelectToken("locationId"),
            HoldingId = (Guid?)jObject.SelectToken("holdingId"),
            Quantity = (int?)jObject.SelectToken("quantity"),
            ElectronicQuantity = (int?)jObject.SelectToken("quantityElectronic"),
            PhysicalQuantity = (int?)jObject.SelectToken("quantityPhysical"),
            TenantId = (Guid?)jObject.SelectToken("tenantId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("locationId", LocationId),
            new JProperty("holdingId", HoldingId),
            new JProperty("quantity", Quantity),
            new JProperty("quantityElectronic", ElectronicQuantity),
            new JProperty("quantityPhysical", PhysicalQuantity),
            new JProperty("tenantId", TenantId)).RemoveNullAndEmptyProperties();
    }
}
