using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_entries -> diku_mod_inventory_storage.holdings_record
    // HoldingEntry -> Holding
    [DisplayColumn(nameof(Id)), DisplayName("Holding Entries"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("holding_entries", Schema = "uc")]
    public partial class HoldingEntry
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string HoldingEntryKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return HoldingEntryKey == ((HoldingEntry)obj).HoldingEntryKey;
        }

        public override int GetHashCode() => HoldingEntryKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Column("public_display"), Display(Name = "Public Display", Order = 4), JsonProperty("publicDisplay")]
        public virtual bool? PublicDisplay { get; set; }

        [Column("enumeration"), Display(Order = 5), JsonProperty("enumeration"), StringLength(1024)]
        public virtual string Enumeration { get; set; }

        [Column("chronology"), Display(Order = 6), JsonProperty("chronology"), StringLength(1024)]
        public virtual string Chronology { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(PublicDisplay)} = {PublicDisplay}, {nameof(Enumeration)} = {Enumeration}, {nameof(Chronology)} = {Chronology} }}";

        public static HoldingEntry FromJObject(JObject jObject) => jObject != null ? new HoldingEntry
        {
            PublicDisplay = (bool?)jObject.SelectToken("publicDisplay"),
            Enumeration = (string)jObject.SelectToken("enumeration"),
            Chronology = (string)jObject.SelectToken("chronology")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("publicDisplay", PublicDisplay),
            new JProperty("enumeration", Enumeration),
            new JProperty("chronology", Chronology)).RemoveNullAndEmptyProperties();
    }
}
