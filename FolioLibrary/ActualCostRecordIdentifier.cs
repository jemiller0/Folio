using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.actual_cost_record_identifiers -> uchicago_mod_circulation_storage.actual_cost_record
    // ActualCostRecordIdentifier -> ActualCostRecord
    [DisplayColumn(nameof(Id)), DisplayName("Actual Cost Record Identifiers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("actual_cost_record_identifiers", Schema = "uc")]
    public partial class ActualCostRecordIdentifier
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Actual Cost Record", Order = 2)]
        public virtual ActualCostRecord2 ActualCostRecord { get; set; }

        [Column("actual_cost_record_id"), Display(Name = "Actual Cost Record", Order = 3)]
        public virtual Guid? ActualCostRecordId { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value"), StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("identifier_type"), Display(Name = "Identifier Type", Order = 5), JsonProperty("identifierType"), StringLength(1024)]
        public virtual string IdentifierType { get; set; }

        [Display(Name = "Identifier Type 1", Order = 6)]
        public virtual IdType2 IdentifierType1 { get; set; }

        [Column("identifier_type_id"), Display(Name = "Identifier Type 1", Order = 7), ForeignKey("IdentifierType1"), JsonProperty("identifierTypeId")]
        public virtual Guid? IdentifierTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ActualCostRecordId)} = {ActualCostRecordId}, {nameof(Value)} = {Value}, {nameof(IdentifierType)} = {IdentifierType}, {nameof(IdentifierTypeId)} = {IdentifierTypeId} }}";

        public static ActualCostRecordIdentifier FromJObject(JObject jObject) => jObject != null ? new ActualCostRecordIdentifier
        {
            Value = (string)jObject.SelectToken("value"),
            IdentifierType = (string)jObject.SelectToken("identifierType"),
            IdentifierTypeId = (Guid?)jObject.SelectToken("identifierTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Value),
            new JProperty("identifierType", IdentifierType),
            new JProperty("identifierTypeId", IdentifierTypeId)).RemoveNullAndEmptyProperties();
    }
}
