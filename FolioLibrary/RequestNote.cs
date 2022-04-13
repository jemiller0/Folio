using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.request_notes -> uc.object_notes
    // RequestNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Request Notes"), Table("request_notes", Schema = "uc")]
    public partial class RequestNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Request2 Request { get; set; }

        [Column("request_id"), Display(Name = "Request", Order = 3)]
        public virtual Guid? RequestId { get; set; }

        [Display(Order = 4)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestId)} = {RequestId}, {nameof(NoteId)} = {NoteId} }}";

        public static RequestNote FromJObject(JValue jObject) => jObject != null ? new RequestNote
        {
            NoteId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
