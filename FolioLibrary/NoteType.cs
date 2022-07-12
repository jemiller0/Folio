using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("type", Schema = "uchicago_mod_notes")]
    public partial class NoteType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(255)]
        public virtual string Name { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 3), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 4), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("updated_by"), Display(Name = "Updated By", Order = 5)]
        public virtual Guid? UpdatedBy { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 6), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Note> Notes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(UpdatedBy)} = {UpdatedBy}, {nameof(LastWriteTime)} = {LastWriteTime} }}";
    }
}
