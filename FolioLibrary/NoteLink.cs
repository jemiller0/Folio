using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.note_links -> uchicago_mod_notes.note_data
    // NoteLink -> Note
    [DisplayColumn(nameof(Id)), DisplayName("Note Links"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("note_links", Schema = "uc")]
    public partial class NoteLink
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 3), Required]
        public virtual Guid? NoteId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id"), Required, StringLength(1024)]
        public virtual string Id2 { get; set; }

        [Column("type"), Display(Order = 5), JsonProperty("type"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(NoteId)} = {NoteId}, {nameof(Id2)} = {Id2}, {nameof(Type)} = {Type} }}";

        public static NoteLink FromJObject(JObject jObject) => jObject != null ? new NoteLink
        {
            Id2 = (string)jObject.SelectToken("id"),
            Type = (string)jObject.SelectToken("type")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("type", Type)).RemoveNullAndEmptyProperties();
    }
}
