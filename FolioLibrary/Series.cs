using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.series -> uchicago_mod_inventory_storage.instance
    // Series -> Instance
    [DisplayColumn(nameof(Name)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("series", Schema = "uc")]
    public partial class Series
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string SeriesKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return SeriesKey == ((Series)obj).SeriesKey;
        }

        public override int GetHashCode() => SeriesKey?.GetHashCode() ?? 0;

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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Name)} = {Name}, {nameof(AuthorityId)} = {AuthorityId} }}";

        public static Series FromJObject(JObject jObject) => jObject != null ? new Series
        {
            Name = (string)jObject.SelectToken("value"),
            AuthorityId = (Guid?)jObject.SelectToken("authorityId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Name),
            new JProperty("authorityId", AuthorityId)).RemoveNullAndEmptyProperties();
    }
}
