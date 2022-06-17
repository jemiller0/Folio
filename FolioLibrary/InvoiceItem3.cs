using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("invoice_items", Schema = "local")]
    public partial class InvoiceItem3
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("invoice_id"), ScaffoldColumn(false)]
        public virtual int? InvoiceId { get; set; }

        [Column("name"), ScaffoldColumn(false), StringLength(449)]
        public virtual string Name { get; set; }

        [Column("quantity"), ScaffoldColumn(false)]
        public virtual int? Quantity { get; set; }

        [Column("order_item_id"), ScaffoldColumn(false)]
        public virtual int? OrderItemId { get; set; }

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(Name)} = {Name}, {nameof(Quantity)} = {Quantity}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
