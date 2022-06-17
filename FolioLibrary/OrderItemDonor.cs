using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("order_item_donors", Schema = "local")]
    public partial class OrderItemDonor
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("order_item_id"), ScaffoldColumn(false), StringLength(36)]
        public virtual string OrderItemId { get; set; }

        [Column("order_item_short_id"), ScaffoldColumn(false)]
        public virtual int? OrderItemShortId { get; set; }

        [Column("donor_id"), ScaffoldColumn(false)]
        public virtual int? DonorId { get; set; }

        [Column("report"), ScaffoldColumn(false)]
        public virtual bool? Report { get; set; }

        [Column("amount"), ScaffoldColumn(false)]
        public virtual decimal? Amount { get; set; }

        [Column("creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(OrderItemShortId)} = {OrderItemShortId}, {nameof(DonorId)} = {DonorId}, {nameof(Report)} = {Report}, {nameof(Amount)} = {Amount}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername} }}";
    }
}
