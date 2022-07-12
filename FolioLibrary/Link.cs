using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("link", Schema = "uchicago_mod_notes")]
    public partial class Link
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("object_id"), Display(Name = "Object Id", Order = 2), Required, StringLength(255)]
        public virtual string ObjectId { get; set; }

        [Column("object_type"), Display(Name = "Object Type", Order = 3), Required, StringLength(255)]
        public virtual string ObjectType { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<NoteLink> NoteLinks { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ObjectId)} = {ObjectId}, {nameof(ObjectType)} = {ObjectType} }}";
    }
}
