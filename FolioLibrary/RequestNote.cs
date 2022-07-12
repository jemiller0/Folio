using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.request_notes -> uc.object_notes
    // RequestNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Request Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("request_notes", Schema = "uc")]
    public partial class RequestNote
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Request2 Request { get; set; }

        [Column("request_id"), Display(Name = "Request", Order = 3)]
        public virtual Guid? RequestId { get; set; }

        [Display(Order = 4)]
        public virtual Note2 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestId)} = {RequestId}, {nameof(NoteId)} = {NoteId} }}";

        public static RequestNote FromJObject(JObject jObject) => jObject != null ? new RequestNote
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
