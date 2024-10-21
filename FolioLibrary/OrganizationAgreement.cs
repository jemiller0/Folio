using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_agreements -> uchicago_mod_organizations_storage.organizations
    // OrganizationAgreement -> Organization
    [DisplayColumn(nameof(Name)), DisplayName("Organization Agreements"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationAgreement>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_agreements", Schema = "uc")]
    public partial class OrganizationAgreement
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id"), StringLength(1024)]
        public virtual string Id2 { get; set; }

        [Column("name"), Display(Order = 5), JsonProperty("name"), Required, StringLength(255)]
        public virtual string Name { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("end_date"), DataType(DataType.Date), Display(Name = "End Date", Order = 7), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("endDate")]
        public virtual DateTime? EndDate { get; set; }

        [Column("cancellation_deadline"), Display(Name = "Cancellation Deadline", Order = 8), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("cancellationDeadline")]
        public virtual DateTime? CancellationDeadline { get; set; }

        [Column("agreement_status_value"), Display(Name = "Agreement Status Value", Order = 9), JsonProperty("agreementStatus.value"), StringLength(1024)]
        public virtual string AgreementStatusValue { get; set; }

        [Column("agreement_status_label"), Display(Name = "Agreement Status Label", Order = 10), JsonProperty("agreementStatus.label"), StringLength(1024)]
        public virtual string AgreementStatusLabel { get; set; }

        [Column("is_perpetual_label"), Display(Name = "Is Perpetual Label", Order = 11), JsonProperty("isPerpetual.label"), StringLength(1024)]
        public virtual string IsPerpetualLabel { get; set; }

        [Column("renewal_priority_label"), Display(Name = "Renewal Priority Label", Order = 12), JsonProperty("renewalPriority.label"), StringLength(1024)]
        public virtual string RenewalPriorityLabel { get; set; }

        [Column("description"), Display(Order = 13), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("date_created"), DataType(DataType.Date), Display(Name = "Date Created", Order = 14), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("dateCreated")]
        public virtual DateTime? DateCreated { get; set; }

        [Column("last_updated"), Display(Name = "Last Updated", Order = 15), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("lastUpdated")]
        public virtual DateTime? LastUpdated { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Id2)} = {Id2}, {nameof(Name)} = {Name}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(CancellationDeadline)} = {CancellationDeadline}, {nameof(AgreementStatusValue)} = {AgreementStatusValue}, {nameof(AgreementStatusLabel)} = {AgreementStatusLabel}, {nameof(IsPerpetualLabel)} = {IsPerpetualLabel}, {nameof(RenewalPriorityLabel)} = {RenewalPriorityLabel}, {nameof(Description)} = {Description}, {nameof(DateCreated)} = {DateCreated}, {nameof(LastUpdated)} = {LastUpdated} }}";

        public static OrganizationAgreement FromJObject(JObject jObject) => jObject != null ? new OrganizationAgreement
        {
            Id2 = (string)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("endDate"))?.ToUniversalTime(),
            CancellationDeadline = (DateTime?)jObject.SelectToken("cancellationDeadline"),
            AgreementStatusValue = (string)jObject.SelectToken("agreementStatus.value"),
            AgreementStatusLabel = (string)jObject.SelectToken("agreementStatus.label"),
            IsPerpetualLabel = (string)jObject.SelectToken("isPerpetual.label"),
            RenewalPriorityLabel = (string)jObject.SelectToken("renewalPriority.label"),
            Description = (string)jObject.SelectToken("description"),
            DateCreated = ((DateTime?)jObject.SelectToken("dateCreated"))?.ToUniversalTime(),
            LastUpdated = (DateTime?)jObject.SelectToken("lastUpdated")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("name", Name),
            new JProperty("startDate", StartDate?.ToLocalTime()),
            new JProperty("endDate", EndDate?.ToLocalTime()),
            new JProperty("cancellationDeadline", CancellationDeadline?.ToLocalTime()),
            new JProperty("agreementStatus", new JObject(
                new JProperty("value", AgreementStatusValue),
                new JProperty("label", AgreementStatusLabel))),
            new JProperty("isPerpetual", new JObject(
                new JProperty("label", IsPerpetualLabel))),
            new JProperty("renewalPriority", new JObject(
                new JProperty("label", RenewalPriorityLabel))),
            new JProperty("description", Description),
            new JProperty("dateCreated", DateCreated?.ToLocalTime()),
            new JProperty("lastUpdated", LastUpdated?.ToLocalTime())).RemoveNullAndEmptyProperties();
    }
}
