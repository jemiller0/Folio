using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("oclc_numbers", Schema = "local")]
    public partial class OclcNumber2
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("instance_id"), ScaffoldColumn(false)]
        public virtual int? InstanceId { get; set; }

        [Column("content"), ScaffoldColumn(false)]
        public virtual int? Content { get; set; }

        [Column("holding_creation_time"), ScaffoldColumn(false)]
        public virtual DateTime? HoldingCreationTime { get; set; }

        [Column("invalid_time"), ScaffoldColumn(false)]
        public virtual DateTime? InvalidTime { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content}, {nameof(HoldingCreationTime)} = {HoldingCreationTime}, {nameof(InvalidTime)} = {InvalidTime} }}";
    }
}
