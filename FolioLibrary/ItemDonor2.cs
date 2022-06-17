using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("item_donors", Schema = "local")]
    public partial class ItemDonor2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("item_id"), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("item_short_id"), ScaffoldColumn(false)]
        public virtual int? ItemShortId { get; set; }

        [Column("donor_id"), ScaffoldColumn(false)]
        public virtual int? DonorId { get; set; }

        [Column("report"), ScaffoldColumn(false)]
        public virtual bool? Report { get; set; }

        [Column("creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("creation_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string CreationUsername { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("last_write_username"), ScaffoldColumn(false), StringLength(128)]
        public virtual string LastWriteUsername { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(ItemShortId)} = {ItemShortId}, {nameof(DonorId)} = {DonorId}, {nameof(Report)} = {Report}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername} }}";
    }
}
