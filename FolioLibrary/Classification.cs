using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.classifications -> uchicago_mod_inventory_storage.instance
    // Classification -> Instance
    [DisplayColumn(nameof(Number)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("classifications", Schema = "uc")]
    public partial class Classification
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ClassificationKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ClassificationKey == ((Classification)obj).ClassificationKey;
        }

        public override int GetHashCode() => ClassificationKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("classification_number"), Display(Order = 4), JsonProperty("classificationNumber"), Required, StringLength(1024)]
        public virtual string Number { get; set; }

        [Display(Name = "Classification Type", Order = 5)]
        public virtual ClassificationType2 ClassificationType { get; set; }

        [Column("classification_type_id"), Display(Name = "Classification Type", Order = 6), JsonProperty("classificationTypeId"), Required]
        public virtual Guid? ClassificationTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Number)} = {Number}, {nameof(ClassificationTypeId)} = {ClassificationTypeId} }}";

        public static Classification FromJObject(JObject jObject) => jObject != null ? new Classification
        {
            Number = (string)jObject.SelectToken("classificationNumber"),
            ClassificationTypeId = (Guid?)jObject.SelectToken("classificationTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("classificationNumber", Number),
            new JProperty("classificationTypeId", ClassificationTypeId)).RemoveNullAndEmptyProperties();
    }
}
