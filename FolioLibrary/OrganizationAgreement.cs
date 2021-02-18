using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_agreements -> diku_mod_organizations_storage.organizations
    // OrganizationAgreement -> Organization
    [DisplayColumn(nameof(Name)), DisplayName("Organization Agreements"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_agreements", Schema = "uc")]
    public partial class OrganizationAgreement
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("discount"), Display(Order = 5), JsonProperty("discount")]
        public virtual decimal? Discount { get; set; }

        [Column("reference_url"), DataType(DataType.Url), Display(Name = "Reference URL", Order = 6), JsonProperty("referenceUrl"), StringLength(1024)]
        public virtual string ReferenceUrl { get; set; }

        [Column("notes"), Display(Order = 7), JsonProperty("notes"), StringLength(1024)]
        public virtual string Notes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Name)} = {Name}, {nameof(Discount)} = {Discount}, {nameof(ReferenceUrl)} = {ReferenceUrl}, {nameof(Notes)} = {Notes} }}";

        public static OrganizationAgreement FromJObject(JObject jObject) => jObject != null ? new OrganizationAgreement
        {
            Name = (string)jObject.SelectToken("name"),
            Discount = (decimal?)jObject.SelectToken("discount"),
            ReferenceUrl = (string)jObject.SelectToken("referenceUrl"),
            Notes = (string)jObject.SelectToken("notes")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("name", Name),
            new JProperty("discount", Discount),
            new JProperty("referenceUrl", ReferenceUrl),
            new JProperty("notes", Notes)).RemoveNullAndEmptyProperties();
    }
}
