using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.publications -> uchicago_mod_inventory_storage.instance
    // Publication -> Instance
    [DisplayColumn(nameof(Id)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("publications", Schema = "uc")]
    public partial class Publication
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string PublicationKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return PublicationKey == ((Publication)obj).PublicationKey;
        }

        public override int GetHashCode() => PublicationKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("publisher"), Display(Order = 4), JsonProperty("publisher"), StringLength(1024)]
        public virtual string Publisher { get; set; }

        [Column("place"), Display(Order = 5), JsonProperty("place"), StringLength(1024)]
        public virtual string Place { get; set; }

        [Column("date_of_publication"), Display(Name = "Publication Year", Order = 6), JsonProperty("dateOfPublication"), StringLength(1024)]
        public virtual string PublicationYear { get; set; }

        [Column("role"), Display(Order = 7), JsonProperty("role"), StringLength(1024)]
        public virtual string Role { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Publisher)} = {Publisher}, {nameof(Place)} = {Place}, {nameof(PublicationYear)} = {PublicationYear}, {nameof(Role)} = {Role} }}";

        public static Publication FromJObject(JObject jObject) => jObject != null ? new Publication
        {
            Publisher = (string)jObject.SelectToken("publisher"),
            Place = (string)jObject.SelectToken("place"),
            PublicationYear = (string)jObject.SelectToken("dateOfPublication"),
            Role = (string)jObject.SelectToken("role")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("publisher", Publisher),
            new JProperty("place", Place),
            new JProperty("dateOfPublication", PublicationYear),
            new JProperty("role", Role)).RemoveNullAndEmptyProperties();
    }
}
