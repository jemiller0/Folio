using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.object_notes -> uchicago_mod_notes.link, uchicago_mod_notes.note_link
    // ObjectNote -> Link, NoteLink
    [DisplayColumn(nameof(Id)), DisplayName("Object Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("object_notes", Schema = "uc")]
    public partial class ObjectNote
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("object_id"), Display(Name = "Object Id", Order = 2)]
        public virtual Guid? ObjectId { get; set; }

        [Column("type"), Display(Order = 3), StringLength(255)]
        public virtual string Type { get; set; }

        [Display(Order = 4)]
        public virtual Note2 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ObjectId)} = {ObjectId}, {nameof(Type)} = {Type}, {nameof(NoteId)} = {NoteId} }}";

        public static ObjectNote FromJObject(JObject jObject) => jObject != null ? new ObjectNote
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
