using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("order_items", Schema = "local")]
    public partial class OrderItem3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("order_id"), ScaffoldColumn(false)]
        public virtual int? OrderId { get; set; }

        [Column("name"), ScaffoldColumn(false), StringLength(449)]
        public virtual string Name { get; set; }

        [Column("unit_price"), ScaffoldColumn(false)]
        public virtual decimal? UnitPrice { get; set; }

        [Column("quantity"), ScaffoldColumn(false)]
        public virtual int? Quantity { get; set; }

        [Column("vendor_item_id"), ScaffoldColumn(false), StringLength(128)]
        public virtual string VendorItemId { get; set; }

        [Column("vendor_notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string VendorNotes { get; set; }

        [Column("special_notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string SpecialNotes { get; set; }

        [Column("miscellaneous_notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string MiscellaneousNotes { get; set; }

        [Column("selector_notes"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string SelectorNotes { get; set; }

        [Column("instance_id"), ScaffoldColumn(false)]
        public virtual int? InstanceId { get; set; }

        [Column("holding_id"), ScaffoldColumn(false)]
        public virtual int? HoldingId { get; set; }

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

        [Column("receipt_status_id"), ScaffoldColumn(false)]
        public virtual int? ReceiptStatusId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderId)} = {OrderId}, {nameof(Name)} = {Name}, {nameof(UnitPrice)} = {UnitPrice}, {nameof(Quantity)} = {Quantity}, {nameof(VendorItemId)} = {VendorItemId}, {nameof(VendorNotes)} = {VendorNotes}, {nameof(SpecialNotes)} = {SpecialNotes}, {nameof(MiscellaneousNotes)} = {MiscellaneousNotes}, {nameof(SelectorNotes)} = {SelectorNotes}, {nameof(InstanceId)} = {InstanceId}, {nameof(HoldingId)} = {HoldingId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId}, {nameof(ReceiptStatusId)} = {ReceiptStatusId} }}";
    }
}
