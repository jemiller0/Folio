using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.contributors -> diku_mod_inventory_storage.instance
    // Contributor -> Instance
    [DisplayColumn(nameof(Name)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("contributors", Schema = "uc")]
    public partial class Contributor
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ContributorKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ContributorKey == ((Contributor)obj).ContributorKey;
        }

        public override int GetHashCode() => ContributorKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Display(Name = "Contributor Type", Order = 5)]
        public virtual ContributorType2 ContributorType { get; set; }

        [Column("contributor_type_id"), Display(Name = "Contributor Type", Order = 6), JsonProperty("contributorTypeId")]
        public virtual Guid? ContributorTypeId { get; set; }

        [Column("contributor_type_text"), Display(Name = "Contributor Type Text", Order = 7), JsonProperty("contributorTypeText"), StringLength(1024)]
        public virtual string ContributorTypeText { get; set; }

        [Display(Name = "Contributor Name Type", Order = 8)]
        public virtual ContributorNameType2 ContributorNameType { get; set; }

        [Column("contributor_name_type_id"), Display(Name = "Contributor Name Type", Order = 9), JsonProperty("contributorNameTypeId"), Required]
        public virtual Guid? ContributorNameTypeId { get; set; }

        [Column("primary"), Display(Order = 10), JsonProperty("primary")]
        public virtual bool? Primary { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Name)} = {Name}, {nameof(ContributorTypeId)} = {ContributorTypeId}, {nameof(ContributorTypeText)} = {ContributorTypeText}, {nameof(ContributorNameTypeId)} = {ContributorNameTypeId}, {nameof(Primary)} = {Primary} }}";

        public static Contributor FromJObject(JObject jObject) => jObject != null ? new Contributor
        {
            Name = (string)jObject.SelectToken("name"),
            ContributorTypeId = (Guid?)jObject.SelectToken("contributorTypeId"),
            ContributorTypeText = (string)jObject.SelectToken("contributorTypeText"),
            ContributorNameTypeId = (Guid?)jObject.SelectToken("contributorNameTypeId"),
            Primary = (bool?)jObject.SelectToken("primary")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("name", Name),
            new JProperty("contributorTypeId", ContributorTypeId),
            new JProperty("contributorTypeText", ContributorTypeText),
            new JProperty("contributorNameTypeId", ContributorNameTypeId),
            new JProperty("primary", Primary)).RemoveNullAndEmptyProperties();
    }
}
