using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.note_types -> uchicago_mod_notes.type
    // NoteType2 -> NoteType
    [DisplayColumn(nameof(Name)), DisplayName("Note Types"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("note_types", Schema = "uc")]
    public partial class NoteType2
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(255)]
        public virtual string Name { get; set; }

        [Display(Name = "Creation User", Order = 3), InverseProperty("NoteType2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("creation_user_id"), Display(Name = "Creation User", Order = 4), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Last Write User", Order = 6), InverseProperty("NoteType2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 7), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Notes", Order = 9)]
        public virtual ICollection<Note2> Note2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime} }}";

        public static NoteType2 FromJObject(JObject jObject) => jObject != null ? new NoteType2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
