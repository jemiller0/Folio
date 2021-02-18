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

        [Column("value"), Display(Order = 4), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("label"), Display(Order = 5), JsonProperty("label"), StringLength(1024)]
        public virtual string Label { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OwnerId)} = {OwnerId}, {nameof(Value)} = {Value}, {nameof(Label)} = {Label} }}";

        public static ServicePointOwner FromJObject(JObject jObject) => jObject != null ? new ServicePointOwner
        {
            Value = (string)jObject.SelectToken("value"),
            Label = (string)jObject.SelectToken("label")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Value),
            new JProperty("label", Label)).RemoveNullAndEmptyProperties();
    }
}
