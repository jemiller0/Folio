using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_notes -> uc.object_notes
    // UserNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("User Notes"), Table("user_notes", Schema = "uc")]
    public partial class UserNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3)]
        public virtual Guid? UserId { get; set; }

        [Display(Order = 4)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(NoteId)} = {NoteId} }}";

        public static UserNote FromJObject(JValue jObject) => jObject != null ? new UserNote
        {
            NoteId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
