using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.extents -> uchicago_mod_inventory_storage.holdings_record
    // Extent -> Holding
    [DisplayColumn(nameof(Content)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("extents", Schema = "uc")]
    public partial class Extent
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ExtentKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ExtentKey == ((Extent)obj).ExtentKey;
        }

        public override int GetHashCode() => ExtentKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), Display(Name = "Holding", Order = 3), Required]
        public virtual Guid? HoldingId { get; set; }

        [Column("statement"), Display(Order = 4), JsonProperty("statement"), StringLength(1024)]
        public virtual string Content { get; set; }

        [Column("note"), Display(Order = 5), JsonProperty("note"), StringLength(1024)]
        public virtual string Note { get; set; }

        [Column("staff_note"), Display(Name = "Staff Note", Order = 6), JsonProperty("staffNote"), StringLength(1024)]
        public virtual string StaffNote { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(Content)} = {Content}, {nameof(Note)} = {Note}, {nameof(StaffNote)} = {StaffNote} }}";

        public static Extent FromJObject(JObject jObject) => jObject != null ? new Extent
        {
            Content = (string)jObject.SelectToken("statement"),
            Note = (string)jObject.SelectToken("note"),
            StaffNote = (string)jObject.SelectToken("staffNote")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("statement", Content),
            new JProperty("note", Note),
            new JProperty("staffNote", StaffNote)).RemoveNullAndEmptyProperties();
    }
}
