using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("records_lb", Schema = "uchicago_mod_source_record_storage")]
    public partial class Record
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Snapshot Snapshot { get; set; }

        [Column("snapshot_id"), Display(Name = "Snapshot", Order = 3), Required]
        public virtual Guid? SnapshotId { get; set; }

        [Column("matched_id"), Display(Name = "Matched Id", Order = 4), Required]
        public virtual Guid? MatchedId { get; set; }

        [Column("generation"), Display(Order = 5), Required]
        public virtual int? Generation { get; set; }

        [Column("record_type"), Display(Name = "Record Type", Order = 6), Required, StringLength(1024)]
        public virtual string RecordType { get; set; }

        [Column("external_id"), Display(Name = "Instance Id", Order = 7)]
        public virtual Guid? InstanceId { get; set; }

        [Column("state"), Display(Order = 8), Required, StringLength(1024)]
        public virtual string State { get; set; }

        [Column("leader_record_status"), Display(Name = "Leader Record Status", Order = 9), StringLength(1)]
        public virtual string LeaderRecordStatus { get; set; }

        [Column("order"), Display(Order = 10)]
        public virtual int? Order { get; set; }

        [Column("suppress_discovery"), Display(Name = "Suppress Discovery", Order = 11)]
        public virtual bool? SuppressDiscovery { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User Id", Order = 12), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User Id", Order = 14), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Column("external_hrid"), Display(Name = "Instance Hrid", Order = 16), StringLength(1024)]
        public virtual string InstanceHrid { get; set; }

        [Display(Name = "Error Record", Order = 17)]
        public virtual ErrorRecord ErrorRecord { get; set; }

        [Display(Name = "Marc Record", Order = 18)]
        public virtual MarcRecord MarcRecord { get; set; }

        [Display(Name = "Raw Record", Order = 19)]
        public virtual RawRecord RawRecord { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(SnapshotId)} = {SnapshotId}, {nameof(MatchedId)} = {MatchedId}, {nameof(Generation)} = {Generation}, {nameof(RecordType)} = {RecordType}, {nameof(InstanceId)} = {InstanceId}, {nameof(State)} = {State}, {nameof(LeaderRecordStatus)} = {LeaderRecordStatus}, {nameof(Order)} = {Order}, {nameof(SuppressDiscovery)} = {SuppressDiscovery}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(InstanceHrid)} = {InstanceHrid} }}";
    }
}
