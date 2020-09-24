using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("snapshots_lb", Schema = "diku_mod_source_record_storage")]
    public partial class Snapshot
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("status"), Display(Order = 2), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("processing_started_date"), DataType(DataType.Date), Display(Name = "Processing Started Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ProcessingStartedDate { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User Id", Order = 6), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Record> Records { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Status)} = {Status}, {nameof(ProcessingStartedDate)} = {ProcessingStartedDate}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime} }}";
    }
}
