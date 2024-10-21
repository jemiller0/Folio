using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_agreement_orgs -> uchicago_mod_organizations_storage.organizations
    // OrganizationAgreementOrg -> Organization
    [DisplayColumn(nameof(Id)), DisplayName("Organization Agreement Orgs"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationAgreementOrg>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_agreement_orgs", Schema = "uc")]
    public partial class OrganizationAgreementOrg
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Column("organization_agreement_id"), Display(Name = "Organization Agreement Id", Order = 2), StringLength(1024)]
        public virtual string OrganizationAgreementId { get; set; }

        [Column("primary_org"), Display(Name = "Primary Org", Order = 3), JsonProperty("primaryOrg")]
        public virtual bool? PrimaryOrg { get; set; }

        [Column("org_orgs_uuid_id"), Display(Name = "Org Orgs UUID Id", Order = 4), JsonProperty("org.orgsUuid")]
        public virtual Guid? OrgOrgsUuidId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationAgreementId)} = {OrganizationAgreementId}, {nameof(PrimaryOrg)} = {PrimaryOrg}, {nameof(OrgOrgsUuidId)} = {OrgOrgsUuidId} }}";

        public static OrganizationAgreementOrg FromJObject(JObject jObject) => jObject != null ? new OrganizationAgreementOrg
        {
            PrimaryOrg = (bool?)jObject.SelectToken("primaryOrg"),
            OrgOrgsUuidId = (Guid?)jObject.SelectToken("org.orgsUuid")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("primaryOrg", PrimaryOrg),
            new JProperty("org", new JObject(
                new JProperty("orgsUuid", OrgOrgsUuidId)))).RemoveNullAndEmptyProperties();
    }
}
