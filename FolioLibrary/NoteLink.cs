using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("note_link", Schema = "uchicago_mod_notes")]
    public partial class NoteLink
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string NoteLinkKey => NoteId == null || LinkId == null ? null : $"{NoteId},{LinkId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return NoteLinkKey == ((NoteLink)obj).NoteLinkKey;
        }

        public override int GetHashCode() => NoteLinkKey?.GetHashCode() ?? 0;

        [Display(Order = 1)]
        public virtual Note Note { get; set; }

        [Column("note_id", Order = 2), ScaffoldColumn(false)]
        public virtual Guid? NoteId { get; set; }

        [Display(Order = 3)]
        public virtual Link Link { get; set; }

        [Column("link_id", Order = 4), ScaffoldColumn(false)]
        public virtual Guid? LinkId { get; set; }

        public override string ToString() => $"{{ {nameof(NoteId)} = {NoteId}, {nameof(LinkId)} = {LinkId} }}";
    }
}
