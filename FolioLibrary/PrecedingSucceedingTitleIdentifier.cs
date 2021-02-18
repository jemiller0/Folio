using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.preceding_succeeding_title_identifiers -> diku_mod_inventory_storage.preceding_succeeding_title
    // PrecedingSucceedingTitleIdentifier -> PrecedingSucceedingTitle
    [DisplayColumn(nameof(Id)), DisplayName("Preceding Succeeding Title Identifiers"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("preceding_succeeding_title_identifiers", Schema = "uc")]
    public partial class PrecedingSucceedingTitleIdentifier
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Preceding Succeeding Title", Order = 2)]
        public virtual PrecedingSucceedingTitle2 PrecedingSucceedingTitle { get; set; }

        [Column("preceding_succeeding_title_id"), Display(Name = "Preceding Succeeding Title", Order = 3), Required]
        public virtual Guid? PrecedingSucceedingTitleId { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Display(Name = "Identifier Type", Order = 5)]
        public virtual IdType2 IdentifierType { get; set; }

        [Column("identifier_type_id"), Display(Name = "Identifier Type", Order = 6), JsonProperty("identifierTypeId"), Required]
        public virtual Guid? IdentifierTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(PrecedingSucceedingTitleId)} = {PrecedingSucceedingTitleId}, {nameof(Value)} = {Value}, {nameof(IdentifierTypeId)} = {IdentifierTypeId} }}";

        public static PrecedingSucceedingTitleIdentifier FromJObject(JObject jObject) => jObject != null ? new PrecedingSucceedingTitleIdentifier
        {
            Value = (string)jObject.SelectToken("value"),
            IdentifierTypeId = (Guid?)jObject.SelectToken("identifierTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Value),
            new JProperty("identifierTypeId", IdentifierTypeId)).RemoveNullAndEmptyProperties();
    }
}
