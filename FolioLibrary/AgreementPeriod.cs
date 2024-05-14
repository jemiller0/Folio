using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.agreement_periods -> uc_agreements.agreements
    // AgreementPeriod -> Agreement
    [DisplayColumn(nameof(StartDate)), DisplayName("Agreement Periods"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreement_periods", Schema = "uc")]
    public partial class AgreementPeriod
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Agreement2 Agreement { get; set; }

        [Column("agreement_id"), Display(Name = "Agreement", Order = 3)]
        public virtual Guid? AgreementId { get; set; }

        [Column("start_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 4), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("period_status"), Display(Order = 5), JsonProperty("periodStatus"), StringLength(1024)]
        public virtual string Status { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AgreementId)} = {AgreementId}, {nameof(StartDate)} = {StartDate}, {nameof(Status)} = {Status} }}";

        public static AgreementPeriod FromJObject(JObject jObject) => jObject != null ? new AgreementPeriod
        {
            StartDate = ((DateTime?)jObject.SelectToken("startDate"))?.ToUniversalTime(),
            Status = (string)jObject.SelectToken("periodStatus")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("startDate", StartDate?.ToLocalTime().ToString("yyyy-MM-dd")),
            new JProperty("periodStatus", Status)).RemoveNullAndEmptyProperties();
    }
}
