using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_changelogs -> diku_mod_organizations_storage.organizations
    // OrganizationChangelog -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Changelogs"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_changelogs", Schema = "uc")]
    public partial class OrganizationChangelog
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("description"), Display(Order = 4), JsonProperty("description"), Required, StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("timestamp"), DataType(DataType.DateTime), Display(Order = 5), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("timestamp"), Required]
        public virtual DateTime? Timestamp { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Description)} = {Description}, {nameof(Timestamp)} = {Timestamp} }}";

        public static OrganizationChangelog FromJObject(JObject jObject) => jObject != null ? new OrganizationChangelog
        {
            Description = (string)jObject.SelectToken("description"),
            Timestamp = (DateTime?)jObject.SelectToken("timestamp")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("description", Description),
            new JProperty("timestamp", Timestamp?.ToLocalTime())).RemoveNullAndEmptyProperties();
    }
}
