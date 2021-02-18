using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.alternative_titles -> diku_mod_inventory_storage.instance
    // AlternativeTitle -> Instance
    [DisplayColumn(nameof(Content)), DisplayName("Alternative Titles"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("alternative_titles", Schema = "uc")]
    public partial class AlternativeTitle
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string AlternativeTitleKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return AlternativeTitleKey == ((AlternativeTitle)obj).AlternativeTitleKey;
        }

        public override int GetHashCode() => AlternativeTitleKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), Display(Name = "Instance", Order = 3), Required]
        public virtual Guid? InstanceId { get; set; }

        [Display(Name = "Alternative Title Type", Order = 4)]
        public virtual AlternativeTitleType2 AlternativeTitleType { get; set; }

        [Column("alternative_title_type_id"), Display(Name = "Alternative Title Type", Order = 5), JsonProperty("alternativeTitleTypeId")]
        public virtual Guid? AlternativeTitleTypeId { get; set; }

        [Column("alternative_title"), Display(Order = 6), JsonProperty("alternativeTitle"), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(AlternativeTitleTypeId)} = {AlternativeTitleTypeId}, {nameof(Content)} = {Content} }}";

        public static AlternativeTitle FromJObject(JObject jObject) => jObject != null ? new AlternativeTitle
        {
            AlternativeTitleTypeId = (Guid?)jObject.SelectToken("alternativeTitleTypeId"),
            Content = (string)jObject.SelectToken("alternativeTitle")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("alternativeTitleTypeId", AlternativeTitleTypeId),
            new JProperty("alternativeTitle", Content)).RemoveNullAndEmptyProperties();
    }
}
