using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.circulation_notes -> uchicago_mod_inventory_storage.item
    // CirculationNote -> Item
    [DisplayColumn(nameof(Id)), DisplayName("Circulation Notes"), JsonConverter(typeof(JsonPathJsonConverter<CirculationNote>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("circulation_notes", Schema = "uc")]
    public partial class CirculationNote
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string CirculationNoteKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return CirculationNoteKey == ((CirculationNote)obj).CirculationNoteKey;
        }

        public override int GetHashCode() => CirculationNoteKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id"), StringLength(1024)]
        public virtual string Id2 { get; set; }

        [Column("note_type"), Display(Name = "Note Type", Order = 5), JsonProperty("noteType"), StringLength(1024)]
        public virtual string NoteType { get; set; }

        [Column("note"), Display(Order = 6), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("source_id"), Display(Name = "Source Id", Order = 7), JsonProperty("source.id"), StringLength(1024)]
        public virtual string SourceId { get; set; }

        [Column("source_personal_last_name"), Display(Name = "Source Personal Last Name", Order = 8), JsonProperty("source.personal.lastName"), StringLength(1024)]
        public virtual string SourcePersonalLastName { get; set; }

        [Column("source_personal_first_name"), Display(Name = "Source Personal First Name", Order = 9), JsonProperty("source.personal.firstName"), StringLength(1024)]
        public virtual string SourcePersonalFirstName { get; set; }

        [Column("date"), Display(Order = 10), JsonProperty("date"), StringLength(1024)]
        public virtual string Date { get; set; }

        [Column("staff_only"), Display(Name = "Staff Only", Order = 11), JsonProperty("staffOnly")]
        public virtual bool? StaffOnly { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Id2)} = {Id2}, {nameof(NoteType)} = {NoteType}, {nameof(Note)} = {Note}, {nameof(SourceId)} = {SourceId}, {nameof(SourcePersonalLastName)} = {SourcePersonalLastName}, {nameof(SourcePersonalFirstName)} = {SourcePersonalFirstName}, {nameof(Date)} = {Date}, {nameof(StaffOnly)} = {StaffOnly} }}";

        public static CirculationNote FromJObject(JObject jObject) => jObject != null ? new CirculationNote
        {
            Id2 = (string)jObject.SelectToken("id"),
            NoteType = (string)jObject.SelectToken("noteType"),
            Note = (string)jObject.SelectToken("note"),
            SourceId = (string)jObject.SelectToken("source.id"),
            SourcePersonalLastName = (string)jObject.SelectToken("source.personal.lastName"),
            SourcePersonalFirstName = (string)jObject.SelectToken("source.personal.firstName"),
            Date = (string)jObject.SelectToken("date"),
            StaffOnly = (bool?)jObject.SelectToken("staffOnly")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("noteType", NoteType),
            new JProperty("note", Note),
            new JProperty("source", new JObject(
                new JProperty("id", SourceId),
                new JProperty("personal", new JObject(
                    new JProperty("lastName", SourcePersonalLastName),
                    new JProperty("firstName", SourcePersonalFirstName))))),
            new JProperty("date", Date),
            new JProperty("staffOnly", StaffOnly)).RemoveNullAndEmptyProperties();
    }
}
