using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_encumbrances_rollover -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover
    // RolloverEncumbrancesRollover -> Rollover
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Encumbrances Rollovers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollover_encumbrances_rollover", Schema = "uc")]
    public partial class RolloverEncumbrancesRollover
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Rollover2 Rollover { get; set; }

        [Column("rollover_id"), Display(Name = "Rollover", Order = 3)]
        public virtual Guid? RolloverId { get; set; }

        [Column("order_type"), Display(Name = "Order Type", Order = 4), JsonProperty("orderType"), StringLength(1024)]
        public virtual string OrderType { get; set; }

        [Column("based_on"), Display(Name = "Based On", Order = 5), JsonProperty("basedOn"), StringLength(1024)]
        public virtual string BasedOn { get; set; }

        [Column("increase_by"), Display(Name = "Increase By", Order = 6), JsonProperty("increaseBy")]
        public virtual decimal? IncreaseBy { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverId)} = {RolloverId}, {nameof(OrderType)} = {OrderType}, {nameof(BasedOn)} = {BasedOn}, {nameof(IncreaseBy)} = {IncreaseBy} }}";

        public static RolloverEncumbrancesRollover FromJObject(JObject jObject) => jObject != null ? new RolloverEncumbrancesRollover
        {
            OrderType = (string)jObject.SelectToken("orderType"),
            BasedOn = (string)jObject.SelectToken("basedOn"),
            IncreaseBy = (decimal?)jObject.SelectToken("increaseBy")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("orderType", OrderType),
            new JProperty("basedOn", BasedOn),
            new JProperty("increaseBy", IncreaseBy)).RemoveNullAndEmptyProperties();
    }
}
