using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.loans -> uchicago_mod_circulation_storage.loan
    // Loan2 -> Loan
    [DisplayColumn(nameof(Id)), DisplayName("Loans"), JsonConverter(typeof(JsonPathJsonConverter<Loan2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("loans", Schema = "uc")]
    public partial class Loan2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Loan.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2), InverseProperty("Loan2s3")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), JsonProperty("userId")]
        public virtual Guid? UserId { get; set; }

        [Display(Name = "Proxy User", Order = 4), InverseProperty("Loan2s1")]
        public virtual User2 ProxyUser { get; set; }

        [Column("proxy_user_id"), Display(Name = "Proxy User", Order = 5), JsonProperty("proxyUserId")]
        public virtual Guid? ProxyUserId { get; set; }

        [Display(Order = 6)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 7), JsonProperty("itemId"), Required]
        public virtual Guid? ItemId { get; set; }

        [Display(Name = "Item Effective Location At Check Out", Order = 8)]
        public virtual Location2 ItemEffectiveLocationAtCheckOut { get; set; }

        [Column("item_effective_location_at_check_out_id"), Display(Name = "Item Effective Location At Check Out", Order = 9), JsonProperty("itemEffectiveLocationIdAtCheckOut")]
        public virtual Guid? ItemEffectiveLocationAtCheckOutId { get; set; }

        [Column("status_name"), Display(Name = "Status Name", Order = 10), JsonProperty("status.name"), StringLength(1024)]
        public virtual string StatusName { get; set; }

        [Column("for_use_at_location_status"), Display(Name = "For Use At Location Status", Order = 11), JsonProperty("forUseAtLocation.status"), RegularExpression(@"^(In use|Held|Returned)$"), StringLength(1024)]
        public virtual string ForUseAtLocationStatus { get; set; }

        [Column("for_use_at_location_status_date"), DataType(DataType.Date), Display(Name = "For Use At Location Status Date", Order = 12), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("forUseAtLocation.statusDate")]
        public virtual DateTime? ForUseAtLocationStatusDate { get; set; }

        [Column("for_use_at_location_hold_shelf_expiration_date"), DataType(DataType.Date), Display(Name = "For Use At Location Hold Shelf Expiration Date", Order = 13), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("forUseAtLocation.holdShelfExpirationDate")]
        public virtual DateTime? ForUseAtLocationHoldShelfExpirationDate { get; set; }

        [Column("loan_date"), DataType(DataType.DateTime), Display(Name = "Loan Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("loanDate"), Required]
        public virtual DateTime? LoanTime { get; set; }

        [Column("due_date"), DataType(DataType.DateTime), Display(Name = "Due Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("dueDate")]
        public virtual DateTime? DueTime { get; set; }

        [Column("return_date"), Display(Name = "Return Time", Order = 16), JsonProperty("returnDate"), StringLength(1024)]
        public virtual string ReturnTime { get; set; }

        [Column("system_return_date"), DataType(DataType.DateTime), Display(Name = "System Return Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("systemReturnDate")]
        public virtual DateTime? SystemReturnTime { get; set; }

        [Column("action"), Display(Order = 18), JsonProperty("action"), Required, StringLength(1024)]
        public virtual string Action { get; set; }

        [Column("action_comment"), Display(Name = "Action Comment", Order = 19), JsonProperty("actionComment"), StringLength(1024)]
        public virtual string ActionComment { get; set; }

        [Column("item_status"), Display(Name = "Item Status", Order = 20), JsonProperty("itemStatus"), StringLength(1024)]
        public virtual string ItemStatus { get; set; }

        [Column("renewal_count"), Display(Name = "Renewal Count", Order = 21), JsonProperty("renewalCount")]
        public virtual int? RenewalCount { get; set; }

        [Display(Name = "Loan Policy", Order = 22)]
        public virtual LoanPolicy2 LoanPolicy { get; set; }

        [Column("loan_policy_id"), Display(Name = "Loan Policy", Order = 23), JsonProperty("loanPolicyId")]
        public virtual Guid? LoanPolicyId { get; set; }

        [Display(Name = "Checkout Service Point", Order = 24), InverseProperty("Loan2s1")]
        public virtual ServicePoint2 CheckoutServicePoint { get; set; }

        [Column("checkout_service_point_id"), Display(Name = "Checkout Service Point", Order = 25), JsonProperty("checkoutServicePointId")]
        public virtual Guid? CheckoutServicePointId { get; set; }

        [Display(Name = "Checkin Service Point", Order = 26), InverseProperty("Loan2s")]
        public virtual ServicePoint2 CheckinServicePoint { get; set; }

        [Column("checkin_service_point_id"), Display(Name = "Checkin Service Point", Order = 27), JsonProperty("checkinServicePointId")]
        public virtual Guid? CheckinServicePointId { get; set; }

        [Display(Order = 28)]
        public virtual Group2 Group { get; set; }

        [Column("group_id"), Display(Name = "Group", Order = 29), JsonProperty("patronGroupIdAtCheckout")]
        public virtual Guid? GroupId { get; set; }

        [Column("due_date_changed_by_recall"), Display(Name = "Due Date Changed By Recall", Order = 30), JsonProperty("dueDateChangedByRecall")]
        public virtual bool? DueDateChangedByRecall { get; set; }

        [Column("is_dcb"), Display(Name = "Is Dcb", Order = 31), JsonProperty("isDcb")]
        public virtual bool? IsDcb { get; set; }

        [Column("declared_lost_date"), DataType(DataType.Date), Display(Name = "Declared Lost Date", Order = 32), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("declaredLostDate")]
        public virtual DateTime? DeclaredLostDate { get; set; }

        [Column("claimed_returned_date"), DataType(DataType.Date), Display(Name = "Claimed Returned Date", Order = 33), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("claimedReturnedDate")]
        public virtual DateTime? ClaimedReturnedDate { get; set; }

        [Display(Name = "Overdue Fine Policy", Order = 34)]
        public virtual OverdueFinePolicy2 OverdueFinePolicy { get; set; }

        [Column("overdue_fine_policy_id"), Display(Name = "Overdue Fine Policy", Order = 35), JsonProperty("overdueFinePolicyId")]
        public virtual Guid? OverdueFinePolicyId { get; set; }

        [Display(Name = "Lost Item Policy", Order = 36)]
        public virtual LostItemFeePolicy2 LostItemPolicy { get; set; }

        [Column("lost_item_policy_id"), Display(Name = "Lost Item Policy", Order = 37), JsonProperty("lostItemPolicyId")]
        public virtual Guid? LostItemPolicyId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 38), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 39), InverseProperty("Loan2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 40), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 42), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 43), InverseProperty("Loan2s2")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 44), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("aged_to_lost_delayed_billing_lost_item_has_been_billed"), Display(Name = "Aged To Lost Delayed Billing Lost Item Has Been Billed", Order = 46), JsonProperty("agedToLostDelayedBilling.lostItemHasBeenBilled")]
        public virtual bool? AgedToLostDelayedBillingLostItemHasBeenBilled { get; set; }

        [Column("aged_to_lost_delayed_billing_date_lost_item_should_be_billed"), Display(Name = "Aged To Lost Delayed Billing Date Lost Item Should Be Billed", Order = 47), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("agedToLostDelayedBilling.dateLostItemShouldBeBilled")]
        public virtual DateTime? AgedToLostDelayedBillingDateLostItemShouldBeBilled { get; set; }

        [Column("aged_to_lost_delayed_billing_aged_to_lost_date"), DataType(DataType.Date), Display(Name = "Aged To Lost Delayed Billing Aged To Lost Date", Order = 48), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("agedToLostDelayedBilling.agedToLostDate")]
        public virtual DateTime? AgedToLostDelayedBillingAgedToLostDate { get; set; }

        [Column("reminders_last_fee_billed_number"), Display(Name = "Reminders Last Fee Billed Number", Order = 49), JsonProperty("reminders.lastFeeBilled.number")]
        public virtual int? RemindersLastFeeBilledNumber { get; set; }

        [Column("reminders_last_fee_billed_date"), DataType(DataType.Date), Display(Name = "Reminders Last Fee Billed Date", Order = 50), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("reminders.lastFeeBilled.date")]
        public virtual DateTime? RemindersLastFeeBilledDate { get; set; }

        [Column("content"), CustomValidation(typeof(Loan), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 51), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Fees", Order = 52)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Patron Action Sessions", Order = 53)]
        public virtual ICollection<PatronActionSession2> PatronActionSession2s { get; set; }

        [Display(Name = "Scheduled Notices", Order = 54)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        [Display(Name = "User Summary Open Fees Fines", Order = 55)]
        public virtual ICollection<UserSummaryOpenFeesFine> UserSummaryOpenFeesFines { get; set; }

        [Display(Name = "User Summary Open Loans", Order = 56)]
        public virtual ICollection<UserSummaryOpenLoan> UserSummaryOpenLoans { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(ProxyUserId)} = {ProxyUserId}, {nameof(ItemId)} = {ItemId}, {nameof(ItemEffectiveLocationAtCheckOutId)} = {ItemEffectiveLocationAtCheckOutId}, {nameof(StatusName)} = {StatusName}, {nameof(ForUseAtLocationStatus)} = {ForUseAtLocationStatus}, {nameof(ForUseAtLocationStatusDate)} = {ForUseAtLocationStatusDate}, {nameof(ForUseAtLocationHoldShelfExpirationDate)} = {ForUseAtLocationHoldShelfExpirationDate}, {nameof(LoanTime)} = {LoanTime}, {nameof(DueTime)} = {DueTime}, {nameof(ReturnTime)} = {ReturnTime}, {nameof(SystemReturnTime)} = {SystemReturnTime}, {nameof(Action)} = {Action}, {nameof(ActionComment)} = {ActionComment}, {nameof(ItemStatus)} = {ItemStatus}, {nameof(RenewalCount)} = {RenewalCount}, {nameof(LoanPolicyId)} = {LoanPolicyId}, {nameof(CheckoutServicePointId)} = {CheckoutServicePointId}, {nameof(CheckinServicePointId)} = {CheckinServicePointId}, {nameof(GroupId)} = {GroupId}, {nameof(DueDateChangedByRecall)} = {DueDateChangedByRecall}, {nameof(IsDcb)} = {IsDcb}, {nameof(DeclaredLostDate)} = {DeclaredLostDate}, {nameof(ClaimedReturnedDate)} = {ClaimedReturnedDate}, {nameof(OverdueFinePolicyId)} = {OverdueFinePolicyId}, {nameof(LostItemPolicyId)} = {LostItemPolicyId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(AgedToLostDelayedBillingLostItemHasBeenBilled)} = {AgedToLostDelayedBillingLostItemHasBeenBilled}, {nameof(AgedToLostDelayedBillingDateLostItemShouldBeBilled)} = {AgedToLostDelayedBillingDateLostItemShouldBeBilled}, {nameof(AgedToLostDelayedBillingAgedToLostDate)} = {AgedToLostDelayedBillingAgedToLostDate}, {nameof(RemindersLastFeeBilledNumber)} = {RemindersLastFeeBilledNumber}, {nameof(RemindersLastFeeBilledDate)} = {RemindersLastFeeBilledDate}, {nameof(Content)} = {Content} }}";

        public static Loan2 FromJObject(JObject jObject) => jObject != null ? new Loan2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            ProxyUserId = (Guid?)jObject.SelectToken("proxyUserId"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            ItemEffectiveLocationAtCheckOutId = (Guid?)jObject.SelectToken("itemEffectiveLocationIdAtCheckOut"),
            StatusName = (string)jObject.SelectToken("status.name"),
            ForUseAtLocationStatus = (string)jObject.SelectToken("forUseAtLocation.status"),
            ForUseAtLocationStatusDate = ((DateTime?)jObject.SelectToken("forUseAtLocation.statusDate"))?.ToUniversalTime(),
            ForUseAtLocationHoldShelfExpirationDate = ((DateTime?)jObject.SelectToken("forUseAtLocation.holdShelfExpirationDate"))?.ToUniversalTime(),
            LoanTime = (DateTime?)jObject.SelectToken("loanDate"),
            DueTime = (DateTime?)jObject.SelectToken("dueDate"),
            ReturnTime = (string)jObject.SelectToken("returnDate"),
            SystemReturnTime = (DateTime?)jObject.SelectToken("systemReturnDate"),
            Action = (string)jObject.SelectToken("action"),
            ActionComment = (string)jObject.SelectToken("actionComment"),
            ItemStatus = (string)jObject.SelectToken("itemStatus"),
            RenewalCount = (int?)jObject.SelectToken("renewalCount"),
            LoanPolicyId = (Guid?)jObject.SelectToken("loanPolicyId"),
            CheckoutServicePointId = (Guid?)jObject.SelectToken("checkoutServicePointId"),
            CheckinServicePointId = (Guid?)jObject.SelectToken("checkinServicePointId"),
            GroupId = (Guid?)jObject.SelectToken("patronGroupIdAtCheckout"),
            DueDateChangedByRecall = (bool?)jObject.SelectToken("dueDateChangedByRecall"),
            IsDcb = (bool?)jObject.SelectToken("isDcb"),
            DeclaredLostDate = ((DateTime?)jObject.SelectToken("declaredLostDate"))?.ToUniversalTime(),
            ClaimedReturnedDate = ((DateTime?)jObject.SelectToken("claimedReturnedDate"))?.ToUniversalTime(),
            OverdueFinePolicyId = (Guid?)jObject.SelectToken("overdueFinePolicyId"),
            LostItemPolicyId = (Guid?)jObject.SelectToken("lostItemPolicyId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            AgedToLostDelayedBillingLostItemHasBeenBilled = (bool?)jObject.SelectToken("agedToLostDelayedBilling.lostItemHasBeenBilled"),
            AgedToLostDelayedBillingDateLostItemShouldBeBilled = (DateTime?)jObject.SelectToken("agedToLostDelayedBilling.dateLostItemShouldBeBilled"),
            AgedToLostDelayedBillingAgedToLostDate = ((DateTime?)jObject.SelectToken("agedToLostDelayedBilling.agedToLostDate"))?.ToUniversalTime(),
            RemindersLastFeeBilledNumber = (int?)jObject.SelectToken("reminders.lastFeeBilled.number"),
            RemindersLastFeeBilledDate = ((DateTime?)jObject.SelectToken("reminders.lastFeeBilled.date"))?.ToUniversalTime(),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("userId", UserId),
            new JProperty("proxyUserId", ProxyUserId),
            new JProperty("itemId", ItemId),
            new JProperty("itemEffectiveLocationIdAtCheckOut", ItemEffectiveLocationAtCheckOutId),
            new JProperty("status", new JObject(
                new JProperty("name", StatusName))),
            new JProperty("forUseAtLocation", new JObject(
                new JProperty("status", ForUseAtLocationStatus),
                new JProperty("statusDate", ForUseAtLocationStatusDate?.ToLocalTime()),
                new JProperty("holdShelfExpirationDate", ForUseAtLocationHoldShelfExpirationDate?.ToLocalTime()))),
            new JProperty("loanDate", LoanTime?.ToLocalTime()),
            new JProperty("dueDate", DueTime?.ToLocalTime()),
            new JProperty("returnDate", ReturnTime),
            new JProperty("systemReturnDate", SystemReturnTime?.ToLocalTime()),
            new JProperty("action", Action),
            new JProperty("actionComment", ActionComment),
            new JProperty("itemStatus", ItemStatus),
            new JProperty("renewalCount", RenewalCount),
            new JProperty("loanPolicyId", LoanPolicyId),
            new JProperty("checkoutServicePointId", CheckoutServicePointId),
            new JProperty("checkinServicePointId", CheckinServicePointId),
            new JProperty("patronGroupIdAtCheckout", GroupId),
            new JProperty("dueDateChangedByRecall", DueDateChangedByRecall),
            new JProperty("isDcb", IsDcb),
            new JProperty("declaredLostDate", DeclaredLostDate?.ToLocalTime()),
            new JProperty("claimedReturnedDate", ClaimedReturnedDate?.ToLocalTime()),
            new JProperty("overdueFinePolicyId", OverdueFinePolicyId),
            new JProperty("lostItemPolicyId", LostItemPolicyId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("agedToLostDelayedBilling", new JObject(
                new JProperty("lostItemHasBeenBilled", AgedToLostDelayedBillingLostItemHasBeenBilled),
                new JProperty("dateLostItemShouldBeBilled", AgedToLostDelayedBillingDateLostItemShouldBeBilled?.ToLocalTime()),
                new JProperty("agedToLostDate", AgedToLostDelayedBillingAgedToLostDate?.ToLocalTime()))),
            new JProperty("reminders", new JObject(
                new JProperty("lastFeeBilled", new JObject(
                    new JProperty("number", RemindersLastFeeBilledNumber),
                    new JProperty("date", RemindersLastFeeBilledDate?.ToLocalTime())))))).RemoveNullAndEmptyProperties();
    }
}
