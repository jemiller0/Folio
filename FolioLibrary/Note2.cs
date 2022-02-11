using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.notes -> uchicago_mod_inventory_storage.instance
    // Note2 -> Instance
    [DisplayColumn(nameof(Id)), DisplayName("Notes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("notes", Schema = "uc")]
    public partial class Note2
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string Note2Key => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return Note2Key == ((Note2)obj).Note2Key;
        }

        public override int GetHashCode() => Note2Key?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Display(Name = "Instance Note Type", Order = 4)]
        public virtual InstanceNoteType2 InstanceNoteType { get; set; }

        [Column("instance_note_type_id"), Display(Name = "Instance Note Type", Order = 5), JsonProperty("instanceNoteTypeId")]
        public virtual Guid? InstanceNoteTypeId { get; set; }

        [Column("note"), Display(Order = 6), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("staff_only"), Display(Name = "Staff Only", Order = 7), JsonProperty("staffOnly")]
        public virtual bool? StaffOnly { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(InstanceNoteTypeId)} = {InstanceNoteTypeId}, {nameof(Note)} = {Note}, {nameof(StaffOnly)} = {StaffOnly} }}";

        public static Note2 FromJObject(JObject jObject) => jObject != null ? new Note2
        {
            InstanceNoteTypeId = (Guid?)jObject.SelectToken("instanceNoteTypeId"),
            Note = (string)jObject.SelectToken("note"),
            StaffOnly = (bool?)jObject.SelectToken("staffOnly")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("instanceNoteTypeId", InstanceNoteTypeId),
            new JProperty("note", Note),
            new JProperty("staffOnly", StaffOnly)).RemoveNullAndEmptyProperties();
    }
}
