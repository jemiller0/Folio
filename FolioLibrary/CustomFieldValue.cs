using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.custom_field_values -> diku_mod_users.custom_fields
    // CustomFieldValue -> CustomField
    [DisplayColumn(nameof(Id)), DisplayName("Custom Field Values"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("custom_field_values", Schema = "uc")]
    public partial class CustomFieldValue
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Custom Field", Order = 2)]
        public virtual CustomField2 CustomField { get; set; }

        [Column("custom_field_id"), Display(Name = "Custom Field", Order = 3), Required]
        public virtual Guid? CustomFieldId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id"), StringLength(1024)]
        public virtual string Id2 { get; set; }

        [Column("value"), Display(Order = 5), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("default"), Display(Order = 6), JsonProperty("default")]
        public virtual bool? Default { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(CustomFieldId)} = {CustomFieldId}, {nameof(Id2)} = {Id2}, {nameof(Value)} = {Value}, {nameof(Default)} = {Default} }}";

        public static CustomFieldValue FromJObject(JObject jObject) => jObject != null ? new CustomFieldValue
        {
            Id2 = (string)jObject.SelectToken("id"),
            Value = (string)jObject.SelectToken("value"),
            Default = (bool?)jObject.SelectToken("default")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("value", Value),
            new JProperty("default", Default)).RemoveNullAndEmptyProperties();
    }
}
