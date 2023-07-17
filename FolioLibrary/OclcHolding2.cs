using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("oclc_holdings2", Schema = "local")]
    public partial class OclcHolding2
    {
        [Column("number"), Key, ScaffoldColumn(false)]
        public virtual int? Number { get; set; }

        [Column("instance_id"), ScaffoldColumn(false)]
        public virtual int? InstanceId { get; set; }

        [Column("last_write_time"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        public override string ToString() => $"{{ {nameof(Number)} = {Number}, {nameof(InstanceId)} = {InstanceId}, {nameof(LastWriteTime)} = {LastWriteTime} }}";
    }
}
