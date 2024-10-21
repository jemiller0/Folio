using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.actual_cost_record_contributors -> uchicago_mod_circulation_storage.actual_cost_record
    // ActualCostRecordContributor -> ActualCostRecord
    [DisplayColumn(nameof(Name)), DisplayName("Actual Cost Record Contributors"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("actual_cost_record_contributors", Schema = "uc")]
    public partial class ActualCostRecordContributor
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Actual Cost Record", Order = 2)]
        public virtual ActualCostRecord2 ActualCostRecord { get; set; }

        [Column("actual_cost_record_id"), Display(Name = "Actual Cost Record", Order = 3)]
        public virtual Guid? ActualCostRecordId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ActualCostRecordId)} = {ActualCostRecordId}, {nameof(Name)} = {Name} }}";

        public static ActualCostRecordContributor FromJObject(JObject jObject) => jObject != null ? new ActualCostRecordContributor
        {
            Name = (string)jObject.SelectToken("name")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("name", Name)).RemoveNullAndEmptyProperties();
    }
}
