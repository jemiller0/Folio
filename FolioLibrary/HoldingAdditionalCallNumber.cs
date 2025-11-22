using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_additional_call_numbers -> uchicago_mod_inventory_storage.holdings_record
    // HoldingAdditionalCallNumber -> Holding
    [DisplayColumn(nameof(Id)), DisplayName("Holding Additional Call Numbers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("holding_additional_call_numbers", Schema = "uc")]
    public partial class HoldingAdditionalCallNumber
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 3)]
        public virtual Guid? HoldingId { get; set; }

        [Display(Order = 4)]
        public virtual CallNumberType2 Type { get; set; }

        [Column("type_id"), Display(Name = "Type", Order = 5), JsonProperty("typeId")]
        public virtual Guid? TypeId { get; set; }

        [Column("prefix"), Display(Order = 6), JsonProperty("prefix"), StringLength(1024)]
        public virtual string Prefix { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 7), JsonProperty("callNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("suffix"), Display(Order = 8), JsonProperty("suffix"), StringLength(1024)]
        public virtual string Suffix { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(TypeId)} = {TypeId}, {nameof(Prefix)} = {Prefix}, {nameof(CallNumber)} = {CallNumber}, {nameof(Suffix)} = {Suffix} }}";

        public static HoldingAdditionalCallNumber FromJObject(JObject jObject) => jObject != null ? new HoldingAdditionalCallNumber
        {
            TypeId = (Guid?)jObject.SelectToken("typeId"),
            Prefix = (string)jObject.SelectToken("prefix"),
            CallNumber = (string)jObject.SelectToken("callNumber"),
            Suffix = (string)jObject.SelectToken("suffix")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("typeId", TypeId),
            new JProperty("prefix", Prefix),
            new JProperty("callNumber", CallNumber),
            new JProperty("suffix", Suffix)).RemoveNullAndEmptyProperties();
    }
}
