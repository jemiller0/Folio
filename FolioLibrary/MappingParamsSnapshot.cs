using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("mapping_params_snapshots", Schema = "uchicago_mod_source_record_manager")]
    public partial class MappingParamsSnapshot
    {
        [Column("job_execution_id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("params"), Display(Order = 2), StringLength(1024)]
        public virtual string Params { get; set; }

        [Column("saved_timestamp"), Display(Name = "Saved Timestamp", Order = 3), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public virtual DateTime? SavedTimestamp { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Params)} = {Params}, {nameof(SavedTimestamp)} = {SavedTimestamp} }}";
    }
}
