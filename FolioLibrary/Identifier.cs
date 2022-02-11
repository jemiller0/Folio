using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.identifiers -> uchicago_mod_inventory_storage.instance
    // Identifier -> Instance
    [DisplayColumn(nameof(Content)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("identifiers", Schema = "uc")]
    public partial class Identifier
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string IdentifierKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return IdentifierKey == ((Identifier)obj).IdentifierKey;
        }

        public override int GetHashCode() => IdentifierKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), Display(Name = "Instance", Order = 3), Required]
        public virtual Guid? InstanceId { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        [Display(Name = "Identifier Type", Order = 5)]
        public virtual IdType2 IdentifierType { get; set; }

        [Column("identifier_type_id"), Display(Name = "Identifier Type", Order = 6), JsonProperty("identifierTypeId"), Required]
        public virtual Guid? IdentifierTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content}, {nameof(IdentifierTypeId)} = {IdentifierTypeId} }}";

        public static Identifier FromJObject(JObject jObject) => jObject != null ? new Identifier
        {
            Content = (string)jObject.SelectToken("value"),
            IdentifierTypeId = (Guid?)jObject.SelectToken("identifierTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Content),
            new JProperty("identifierTypeId", IdentifierTypeId)).RemoveNullAndEmptyProperties();
    }
}
