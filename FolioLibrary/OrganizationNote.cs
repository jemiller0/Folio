using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_notes -> uc.object_notes
    // OrganizationNote -> ObjectNote
    [DisplayColumn(nameof(Id)), DisplayName("Organization Notes"), Table("organization_notes", Schema = "uc")]
    public partial class OrganizationNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3)]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Order = 4)]
        public virtual Note3 Note { get; set; }

        [Column("note_id"), Display(Name = "Note", Order = 5)]
        public virtual Guid? NoteId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(NoteId)} = {NoteId} }}";

        public static OrganizationNote FromJObject(JValue jObject) => jObject != null ? new OrganizationNote
        {
            NoteId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
