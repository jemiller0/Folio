using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("invoice_item_funds", Schema = "local")]
    public partial class InvoiceItemFund2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("invoice_item_id"), ScaffoldColumn(false)]
        public virtual int? InvoiceItemId { get; set; }

        [Column("fund_id"), ScaffoldColumn(false)]
        public virtual int? FundId { get; set; }

        [Column("amount"), ScaffoldColumn(false)]
        public virtual decimal? Amount { get; set; }

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(FundId)} = {FundId}, {nameof(Amount)} = {Amount}, {nameof(ShippingAmount)} = {ShippingAmount}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(LongId)} = {LongId} }}";
    }
}
