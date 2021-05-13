using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.ledger_rollover_encumbrances_rollover -> diku_mod_finance_storage.ledger_fiscal_year_rollover
    // LedgerRolloverEncumbrancesRollover -> LedgerRollover
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Rollover Encumbrances Rollovers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("ledger_rollover_encumbrances_rollover", Schema = "uc")]
    public partial class LedgerRolloverEncumbrancesRollover
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Ledger Rollover", Order = 2)]
        public virtual LedgerRollover2 LedgerRollover { get; set; }

        [Column("ledger_rollover_id"), Display(Name = "Ledger Rollover", Order = 3)]
        public virtual Guid? LedgerRolloverId { get; set; }

        [Column("order_type"), Display(Name = "Order Type", Order = 4), JsonProperty("orderType"), StringLength(1024)]
        public virtual string OrderType { get; set; }

        [Column("based_on"), Display(Name = "Based On", Order = 5), JsonProperty("basedOn"), StringLength(1024)]
        public virtual string BasedOn { get; set; }

        [Column("increase_by"), DataType(DataType.Currency), Display(Name = "Increase By", Order = 6), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("increaseBy")]
        public virtual decimal? IncreaseBy { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerRolloverId)} = {LedgerRolloverId}, {nameof(OrderType)} = {OrderType}, {nameof(BasedOn)} = {BasedOn}, {nameof(IncreaseBy)} = {IncreaseBy} }}";

        public static LedgerRolloverEncumbrancesRollover FromJObject(JObject jObject) => jObject != null ? new LedgerRolloverEncumbrancesRollover
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
