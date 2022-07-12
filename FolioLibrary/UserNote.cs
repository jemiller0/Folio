using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_notes -> uc.object_notes
    // UserNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("User Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_notes", Schema = "uc")]
    public partial class UserNote
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3)]
        public virtual Guid? UserId { get; set; }

        [Display(Order = 4)]
        public virtual Note2 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(NoteId)} = {NoteId} }}";

        public static UserNote FromJObject(JObject jObject) => jObject != null ? new UserNote
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
