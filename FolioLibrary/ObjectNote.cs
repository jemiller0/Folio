using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.object_notes -> uchicago_mod_notes.note_data
    // ObjectNote -> Note
    [DisplayColumn(nameof(Id)), DisplayName("Object Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("object_notes", Schema = "uc")]
    public partial class ObjectNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 3)]
        public virtual Guid? NoteId { get; set; }

        [Column("object_id"), Display(Name = "Object Id", Order = 4), JsonProperty("id")]
        public virtual Guid? ObjectId { get; set; }

        [Column("type"), Display(Order = 5), JsonProperty("type"), StringLength(1024)]
        public virtual string Type { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(NoteId)} = {NoteId}, {nameof(ObjectId)} = {ObjectId}, {nameof(Type)} = {Type} }}";

        public static ObjectNote FromJObject(JObject jObject) => jObject != null ? new ObjectNote
        {
            ObjectId = (Guid?)jObject.SelectToken("id"),
            Type = (string)jObject.SelectToken("type")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", ObjectId),
            new JProperty("type", Type)).RemoveNullAndEmptyProperties();
    }
}
