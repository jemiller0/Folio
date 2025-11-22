using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.subjects -> uchicago_mod_inventory_storage.instance
    // Subject -> Instance
    [DisplayColumn(nameof(Name)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("subjects", Schema = "uc")]
    public partial class Subject
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string SubjectKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return SubjectKey == ((Subject)obj).SubjectKey;
        }

        public override int GetHashCode() => SubjectKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("value"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("authority_id"), Display(Name = "Authority Id", Order = 5), JsonProperty("authorityId")]
        public virtual Guid? AuthorityId { get; set; }

        [Display(Order = 6)]
        public virtual SubjectSource2 Source { get; set; }

        [Column("source_id"), Display(Name = "Source", Order = 7), JsonProperty("sourceId")]
        public virtual Guid? SourceId { get; set; }

        [Display(Order = 8)]
        public virtual SubjectType2 Type { get; set; }

        [Column("type_id"), Display(Name = "Type", Order = 9), JsonProperty("typeId")]
        public virtual Guid? TypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Name)} = {Name}, {nameof(AuthorityId)} = {AuthorityId}, {nameof(SourceId)} = {SourceId}, {nameof(TypeId)} = {TypeId} }}";

        public static Subject FromJObject(JObject jObject) => jObject != null ? new Subject
        {
            Name = (string)jObject.SelectToken("value"),
            AuthorityId = (Guid?)jObject.SelectToken("authorityId"),
            SourceId = (Guid?)jObject.SelectToken("sourceId"),
            TypeId = (Guid?)jObject.SelectToken("typeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Name),
            new JProperty("authorityId", AuthorityId),
            new JProperty("sourceId", SourceId),
            new JProperty("typeId", TypeId)).RemoveNullAndEmptyProperties();
    }
}
