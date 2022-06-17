using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("orders", Schema = "local")]
    public partial class Order3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("document_id"), ScaffoldColumn(false)]
        public virtual int? DocumentId { get; set; }

        [Column("vendor_id"), ScaffoldColumn(false)]
        public virtual int? VendorId { get; set; }

        [Column("order_date"), ScaffoldColumn(false)]
        public virtual DateTime? OrderDate { get; set; }

        [Column("order_type_id"), ScaffoldColumn(false)]
        public virtual int? OrderTypeId { get; set; }

        [Column("order_status_id"), ScaffoldColumn(false)]
        public virtual int? OrderStatusId { get; set; }

        [Column("fiscal_year"), ScaffoldColumn(false)]
        public virtual int? FiscalYear { get; set; }

        [Column("vendor_customer_id"), ScaffoldColumn(false), StringLength(32)]
        public virtual string VendorCustomerId { get; set; }

        [Column("delivery_room_id"), ScaffoldColumn(false)]
        public virtual int? DeliveryRoomId { get; set; }

        [Column("approve_time"), ScaffoldColumn(false)]
        public virtual DateTime? ApproveTime { get; set; }

        [Column("approve_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string ApproveUsername { get; set; }

        [Column("cancel_time"), ScaffoldColumn(false)]
        public virtual DateTime? CancelTime { get; set; }

        [Column("cancel_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CancelUsername { get; set; }

        [Column("freight_amount"), ScaffoldColumn(false)]
        public virtual decimal? FreightAmount { get; set; }

        [Column("shipping_amount"), ScaffoldColumn(false)]
        public virtual decimal? ShippingAmount { get; set; }

        [Column("creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        [Column("long_id"), ScaffoldColumn(false)]
        public virtual Guid? LongId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(DocumentId)} = {DocumentId}, {nameof(VendorId)} = {VendorId}, {nameof(OrderDate)} = {OrderDate}, {nameof(OrderTypeId)} = {OrderTypeId}, {nameof(OrderStatusId)} = {OrderStatusId}, {nameof(FiscalYear)} = {FiscalYear}, {nameof(VendorCustomerId)} = {VendorCustomerId}, {nameof(DeliveryRoomId)} = {DeliveryRoomId}, {nameof(ApproveTime)} = {ApproveTime}, {nameof(ApproveUsername)} = {ApproveUsername}, {nameof(CancelTime)} = {CancelTime}, {nameof(CancelUsername)} = {CancelUsername}, {nameof(FreightAmount)} = {FreightAmount}, {nameof(ShippingAmount)} = {ShippingAmount}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
