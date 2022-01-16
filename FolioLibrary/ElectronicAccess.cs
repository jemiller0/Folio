using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.electronic_accesses -> diku_mod_inventory_storage.instance
    // ElectronicAccess -> Instance
    [DisplayColumn(nameof(Id)), DisplayName("Electronic Accesses"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("electronic_accesses", Schema = "uc")]
    public partial class ElectronicAccess
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ElectronicAccessKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ElectronicAccessKey == ((ElectronicAccess)obj).ElectronicAccessKey;
        }

        public override int GetHashCode() => ElectronicAccessKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("uri"), DataType(DataType.Url), Display(Name = "URI", Order = 4), JsonProperty("uri"), Required, StringLength(1024)]
        public virtual string Uri { get; set; }

        [Column("link_text"), Display(Name = "Link Text", Order = 5), JsonProperty("linkText"), StringLength(1024)]
        public virtual string LinkText { get; set; }

        [Column("materials_specification"), Display(Name = "Materials Specification", Order = 6), JsonProperty("materialsSpecification"), StringLength(1024)]
        public virtual string MaterialsSpecification { get; set; }

        [Column("public_note"), Display(Name = "Public Note", Order = 7), JsonProperty("publicNote"), StringLength(1024)]
        public virtual string PublicNote { get; set; }

        [Display(Order = 8)]
        public virtual ElectronicAccessRelationship2 Relationship { get; set; }

        [Column("relationship_id"), Display(Name = "Relationship", Order = 9), JsonProperty("relationshipId")]
        public virtual Guid? RelationshipId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Uri)} = {Uri}, {nameof(LinkText)} = {LinkText}, {nameof(MaterialsSpecification)} = {MaterialsSpecification}, {nameof(PublicNote)} = {PublicNote}, {nameof(RelationshipId)} = {RelationshipId} }}";

        public static ElectronicAccess FromJObject(JObject jObject) => jObject != null ? new ElectronicAccess
        {
            Uri = (string)jObject.SelectToken("uri"),
            LinkText = (string)jObject.SelectToken("linkText"),
            MaterialsSpecification = (string)jObject.SelectToken("materialsSpecification"),
            PublicNote = (string)jObject.SelectToken("publicNote"),
            RelationshipId = (Guid?)jObject.SelectToken("relationshipId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("uri", Uri),
            new JProperty("linkText", LinkText),
            new JProperty("materialsSpecification", MaterialsSpecification),
            new JProperty("publicNote", PublicNote),
            new JProperty("relationshipId", RelationshipId)).RemoveNullAndEmptyProperties();
    }
}
