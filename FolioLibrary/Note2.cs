using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.notes -> uchicago_mod_notes.note
    // Note2 -> Note
    [DisplayColumn(nameof(Title)), DisplayName("Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("notes", Schema = "uc")]
    public partial class Note2
    {
        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("title"), Display(Order = 2), StringLength(255)]
        public virtual string Title { get; set; }

        [Column("content"), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        [Column("indexed_content"), Display(Name = "Indexed Content", Order = 4), StringLength(1024)]
        public virtual string IndexedContent { get; set; }

        [Column("domain"), Display(Order = 5), StringLength(100)]
        public virtual string Domain { get; set; }

        [Display(Order = 6)]
        public virtual NoteType2 Type { get; set; }

        [Column("type_id"), Display(Name = "Type", Order = 7)]
        public virtual Guid? TypeId { get; set; }

        [Column("pop_up_on_user"), Display(Name = "Pop Up On User", Order = 8)]
        public virtual bool? PopUpOnUser { get; set; }

        [Column("pop_up_on_check_out"), Display(Name = "Pop Up On Check Out", Order = 9)]
        public virtual bool? PopUpOnCheckOut { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("Note2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("creation_user_id"), Display(Name = "Creation User", Order = 11), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("creation_time"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("Note2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("last_write_time"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Object Notes", Order = 16)]
        public virtual ICollection<ObjectNote> ObjectNotes { get; set; }

        [Display(Name = "Order Item Notes", Order = 17)]
        public virtual ICollection<OrderItemNote> OrderItemNotes { get; set; }

        [Display(Name = "Organization Notes", Order = 18)]
        public virtual ICollection<OrganizationNote> OrganizationNotes { get; set; }

        [Display(Name = "Request Notes", Order = 19)]
        public virtual ICollection<RequestNote> RequestNotes { get; set; }

        [Display(Name = "User Notes", Order = 20)]
        public virtual ICollection<UserNote> UserNotes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Title)} = {Title}, {nameof(Content)} = {Content}, {nameof(IndexedContent)} = {IndexedContent}, {nameof(Domain)} = {Domain}, {nameof(TypeId)} = {TypeId}, {nameof(PopUpOnUser)} = {PopUpOnUser}, {nameof(PopUpOnCheckOut)} = {PopUpOnCheckOut}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteTime)} = {LastWriteTime} }}";

        public static Note2 FromJObject(JObject jObject) => jObject != null ? new Note2
        {
            Id = (Guid?)jObject.SelectToken("id")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id)).RemoveNullAndEmptyProperties();
    }
}
