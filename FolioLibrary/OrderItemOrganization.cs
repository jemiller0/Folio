using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_organizations -> uchicago_mod_orders_storage.po_line
    // OrderItemOrganization -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Organizations"), Table("order_item_organizations", Schema = "uc")]
    public partial class OrderItemOrganization
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3)]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 4)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 5)]
        public virtual Guid? OrganizationId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(OrganizationId)} = {OrganizationId} }}";

        public static OrderItemOrganization FromJObject(JValue jObject) => jObject != null ? new OrderItemOrganization
        {
            OrganizationId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(OrganizationId);
    }
}
