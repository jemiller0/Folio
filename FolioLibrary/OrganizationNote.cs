using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_notes -> uc.object_notes
    // OrganizationNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Organization Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_notes", Schema = "uc")]
    public partial class OrganizationNote
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3)]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Order = 4)]
        public virtual Note2 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(NoteId)} = {NoteId} }}";

        public static OrganizationNote FromJObject(JObject jObject) => jObject != null ? new OrganizationNote
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
