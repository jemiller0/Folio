using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.supplement_statements -> diku_mod_inventory_storage.holdings_record
    // SupplementStatement -> Holding
    [DisplayColumn(nameof(Id)), DisplayName("Supplement Statements"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("supplement_statements", Schema = "uc")]
    public partial class SupplementStatement
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string SupplementStatementKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return SupplementStatementKey == ((SupplementStatement)obj).SupplementStatementKey;
        }

        public override int GetHashCode() => SupplementStatementKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Column("statement"), Display(Order = 4), JsonProperty("statement"), StringLength(1024)]
        public virtual string Statement { get; set; }

        [Column("note"), Display(Order = 5), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("staff_note"), Display(Name = "Staff Note", Order = 6), JsonProperty("staffNote"), StringLength(1024)]
        public virtual string StaffNote { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(Statement)} = {Statement}, {nameof(Note)} = {Note}, {nameof(StaffNote)} = {StaffNote} }}";

        public static SupplementStatement FromJObject(JObject jObject) => jObject != null ? new SupplementStatement
        {
            Statement = (string)jObject.SelectToken("statement"),
            Note = (string)jObject.SelectToken("note"),
            StaffNote = (string)jObject.SelectToken("staffNote")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("statement", Statement),
            new JProperty("note", Note),
            new JProperty("staffNote", StaffNote)).RemoveNullAndEmptyProperties();
    }
}
