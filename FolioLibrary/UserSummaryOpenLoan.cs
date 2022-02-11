using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_summary_open_loans -> uchicago_mod_patron_blocks.user_summary
    // UserSummaryOpenLoan -> UserSummary
    [DisplayColumn(nameof(Id)), DisplayName("User Summary Open Loans"), JsonConverter(typeof(JsonPathJsonConverter<UserSummaryOpenLoan>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_summary_open_loans", Schema = "uc")]
    public partial class UserSummaryOpenLoan
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "User Summary", Order = 2)]
        public virtual UserSummary2 UserSummary { get; set; }

        [Column("user_summary_id"), Display(Name = "User Summary", Order = 3), Required]
        public virtual Guid? UserSummaryId { get; set; }

        [Display(Order = 4)]
        public virtual Loan2 Loan { get; set; }

        [Column("loan_id"), Display(Name = "Loan", Order = 5), JsonProperty("loanId"), Required]
        public virtual Guid? LoanId { get; set; }

        [Column("due_date"), DataType(DataType.Date), Display(Name = "Due Date", Order = 6), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("dueDate"), Required]
        public virtual DateTime? DueDate { get; set; }

        [Column("recall"), Display(Order = 7), JsonProperty("recall")]
        public virtual bool? Recall { get; set; }

        [Column("item_lost"), Display(Name = "Item Lost", Order = 8), JsonProperty("itemLost")]
        public virtual bool? ItemLost { get; set; }

        [Column("item_claimed_returned"), Display(Name = "Item Claimed Returned", Order = 9), JsonProperty("itemClaimedReturned")]
        public virtual bool? ItemClaimedReturned { get; set; }

        [Column("grace_period_duration"), Display(Name = "Grace Period Duration", Order = 10), JsonProperty("gracePeriod.duration")]
        public virtual int? GracePeriodDuration { get; set; }

        [Column("grace_period_interval_id"), Display(Name = "Grace Period Interval", Order = 11), JsonProperty("gracePeriod.intervalId"), StringLength(1024)]
        public virtual string GracePeriodInterval { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserSummaryId)} = {UserSummaryId}, {nameof(LoanId)} = {LoanId}, {nameof(DueDate)} = {DueDate}, {nameof(Recall)} = {Recall}, {nameof(ItemLost)} = {ItemLost}, {nameof(ItemClaimedReturned)} = {ItemClaimedReturned}, {nameof(GracePeriodDuration)} = {GracePeriodDuration}, {nameof(GracePeriodInterval)} = {GracePeriodInterval} }}";

        public static UserSummaryOpenLoan FromJObject(JObject jObject) => jObject != null ? new UserSummaryOpenLoan
        {
            LoanId = (Guid?)jObject.SelectToken("loanId"),
            DueDate = (DateTime?)jObject.SelectToken("dueDate"),
            Recall = (bool?)jObject.SelectToken("recall"),
            ItemLost = (bool?)jObject.SelectToken("itemLost"),
            ItemClaimedReturned = (bool?)jObject.SelectToken("itemClaimedReturned"),
            GracePeriodDuration = (int?)jObject.SelectToken("gracePeriod.duration"),
            GracePeriodInterval = (string)jObject.SelectToken("gracePeriod.intervalId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("loanId", LoanId),
            new JProperty("dueDate", DueDate?.ToLocalTime()),
            new JProperty("recall", Recall),
            new JProperty("itemLost", ItemLost),
            new JProperty("itemClaimedReturned", ItemClaimedReturned),
            new JProperty("gracePeriod", new JObject(
                new JProperty("duration", GracePeriodDuration),
                new JProperty("intervalId", GracePeriodInterval)))).RemoveNullAndEmptyProperties();
    }
}
