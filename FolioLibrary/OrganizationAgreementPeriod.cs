using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_agreement_periods -> uchicago_mod_organizations_storage.organizations
    // OrganizationAgreementPeriod -> Organization
    [DisplayColumn(nameof(StartDate)), DisplayName("Organization Agreement Periods"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_agreement_periods", Schema = "uc")]
    public partial class OrganizationAgreementPeriod
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Column("organization_agreement_id"), Display(Name = "Organization Agreement Id", Order = 2), StringLength(1024)]
        public virtual string OrganizationAgreementId { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("period_status"), Display(Name = "Period Status", Order = 4), JsonProperty("periodStatus"), StringLength(1024)]
        public virtual string PeriodStatus { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationAgreementId)} = {OrganizationAgreementId}, {nameof(StartDate)} = {StartDate}, {nameof(PeriodStatus)} = {PeriodStatus} }}";

        public static OrganizationAgreementPeriod FromJObject(JObject jObject) => jObject != null ? new OrganizationAgreementPeriod
        {
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            PeriodStatus = (string)jObject.SelectToken("periodStatus")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("startDate", StartDate?.ToLocalTime()),
            new JProperty("periodStatus", PeriodStatus)).RemoveNullAndEmptyProperties();
    }
}
