using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.service_point_owners -> diku_mod_feesfines.owners
    // ServicePointOwner -> Owner
    [DisplayColumn(nameof(Id)), DisplayName("Service Point Owners"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("service_point_owners", Schema = "uc")]
    public partial class ServicePointOwner
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Owner2 Owner { get; set; }

        [Column("owner_id"), Display(Name = "Owner", Order = 3), Required]
        public virtual Guid? OwnerId { get; set; }

        [Display(Name = "Service Point", Order = 4)]
        public virtual ServicePoint2 ServicePoint { get; set; }

        [Column("service_point_id"), Display(Name = "Service Point", Order = 5), JsonProperty("value")]
        public virtual Guid? ServicePointId { get; set; }

        [Column("label"), Display(Order = 6), JsonProperty("label"), StringLength(1024)]
        public virtual string Label { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OwnerId)} = {OwnerId}, {nameof(ServicePointId)} = {ServicePointId}, {nameof(Label)} = {Label} }}";

        public static ServicePointOwner FromJObject(JObject jObject) => jObject != null ? new ServicePointOwner
        {
            ServicePointId = (Guid?)jObject.SelectToken("value"),
            Label = (string)jObject.SelectToken("label")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", ServicePointId),
            new JProperty("label", Label)).RemoveNullAndEmptyProperties();
    }
}
