using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.agreement_organizations -> uc_agreements.agreements
    // AgreementOrganization -> Agreement
    [DisplayColumn(nameof(Id)), DisplayName("Agreement Organizations"), JsonConverter(typeof(JsonPathJsonConverter<AgreementOrganization>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreement_organizations", Schema = "uc")]
    public partial class AgreementOrganization
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Agreement2 Agreement { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement", Order = 3)]
        public virtual Guid? AgreementId { get; set; }

        [Column("primary_org"), Display(Name = "Primary Org", Order = 4), JsonProperty("primaryOrg")]
        public virtual bool? PrimaryOrg { get; set; }

        [Display(Order = 5)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 6), JsonProperty("org.orgsUuid")]
        public virtual Guid? OrganizationId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AgreementId)} = {AgreementId}, {nameof(PrimaryOrg)} = {PrimaryOrg}, {nameof(OrganizationId)} = {OrganizationId} }}";

        public static AgreementOrganization FromJObject(JObject jObject) => jObject != null ? new AgreementOrganization
        {
            PrimaryOrg = (bool?)jObject.SelectToken("primaryOrg"),
            OrganizationId = (Guid?)jObject.SelectToken("org.orgsUuid")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("primaryOrg", PrimaryOrg),
            new JProperty("org", new JObject(
                new JProperty("orgsUuid", OrganizationId)))).RemoveNullAndEmptyProperties();
    }
}
