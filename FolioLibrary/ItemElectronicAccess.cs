using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_electronic_accesses -> uchicago_mod_inventory_storage.item
    // ItemElectronicAccess -> Item
    [DisplayColumn(nameof(Id)), DisplayName("Item Electronic Accesses"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("item_electronic_accesses", Schema = "uc")]
    public partial class ItemElectronicAccess
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemElectronicAccessKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemElectronicAccessKey == ((ItemElectronicAccess)obj).ItemElectronicAccessKey;
        }

        public override int GetHashCode() => ItemElectronicAccessKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(Uri)} = {Uri}, {nameof(LinkText)} = {LinkText}, {nameof(MaterialsSpecification)} = {MaterialsSpecification}, {nameof(PublicNote)} = {PublicNote}, {nameof(RelationshipId)} = {RelationshipId} }}";

        public static ItemElectronicAccess FromJObject(JObject jObject) => jObject != null ? new ItemElectronicAccess
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
