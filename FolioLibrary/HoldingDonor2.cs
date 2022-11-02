using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("holding_donors", Schema = "local")]
    public partial class HoldingDonor2
    {
        [Column("holding_id"), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

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

        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        public override string ToString() => $"{{ {nameof(HoldingId)} = {HoldingId}, {nameof(DonorId)} = {DonorId}, {nameof(Report)} = {Report}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUsername)} = {CreationUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUsername)} = {LastWriteUsername}, {nameof(Id)} = {Id} }}";
    }
}
