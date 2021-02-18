using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_acquisitions_units -> diku_mod_orders_storage.purchase_order
    // OrderAcquisitionsUnit -> Order
    [DisplayColumn(nameof(Id)), DisplayName("Order Acquisitions Units"), Table("order_acquisitions_units", Schema = "uc")]
    public partial class OrderAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Order2 Order { get; set; }

        [Column("order_id"), Display(Name = "Order", Order = 3), Required]
        public virtual Guid? OrderId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderId)} = {OrderId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static OrderAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new OrderAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
