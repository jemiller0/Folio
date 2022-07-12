using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_notes -> uchicago_mod_inventory_storage.holdings_record
    // HoldingNote -> Holding
    [DisplayColumn(nameof(Id)), DisplayName("Holding Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("holding_notes", Schema = "uc")]
    public partial class HoldingNote
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string HoldingNoteKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return HoldingNoteKey == ((HoldingNote)obj).HoldingNoteKey;
        }

        public override int GetHashCode() => HoldingNoteKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Display(Name = "Holding Note Type", Order = 4)]
        public virtual HoldingNoteType2 HoldingNoteType { get; set; }

        [Column("holding_note_type_id"), Display(Name = "Holding Note Type", Order = 5), JsonProperty("holdingsNoteTypeId")]
        public virtual Guid? HoldingNoteTypeId { get; set; }

        [Column("note"), Display(Order = 6), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("staff_only"), Display(Name = "Staff Only", Order = 7), JsonProperty("staffOnly")]
        public virtual bool? StaffOnly { get; set; }

        [Column("created_date"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [InverseProperty("HoldingNotes"), ScaffoldColumn(false)]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [InverseProperty("HoldingNotes1"), ScaffoldColumn(false)]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 13), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(HoldingNoteTypeId)} = {HoldingNoteTypeId}, {nameof(Note)} = {Note}, {nameof(StaffOnly)} = {StaffOnly}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static HoldingNote FromJObject(JObject jObject) => jObject != null ? new HoldingNote
        {
            HoldingNoteTypeId = (Guid?)jObject.SelectToken("holdingsNoteTypeId"),
            Note = (string)jObject.SelectToken("note"),
            StaffOnly = (bool?)jObject.SelectToken("staffOnly")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("holdingsNoteTypeId", HoldingNoteTypeId),
            new JProperty("note", Note),
            new JProperty("staffOnly", StaffOnly)).RemoveNullAndEmptyProperties();
    }
}
