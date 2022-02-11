using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_aliases -> uchicago_mod_organizations_storage.organizations
    // OrganizationAlias -> Organization
    [DisplayColumn(nameof(Value)), DisplayName("Organization Aliases"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_aliases", Schema = "uc")]
    public partial class OrganizationAlias
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Value)} = {Value}, {nameof(Description)} = {Description} }}";

        public static OrganizationAlias FromJObject(JObject jObject) => jObject != null ? new OrganizationAlias
        {
            Value = (string)jObject.SelectToken("value"),
            Description = (string)jObject.SelectToken("description")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("value", Value),
            new JProperty("description", Description)).RemoveNullAndEmptyProperties();
    }
}
