using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.loan_events -> diku_mod_circulation_storage.audit_loan
    // LoanEvent2 -> LoanEvent
    [DisplayColumn(nameof(Id)), DisplayName("Loan Events"), JsonConverter(typeof(JsonPathJsonConverter<LoanEvent2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("loan_events", Schema = "uc")]
    public partial class LoanEvent2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LoanEvent.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("operation"), Display(Order = 2), JsonProperty("operation"), StringLength(1024)]
        public virtual string Operation { get; set; }

        [Column("creation_date"), Display(Name = "Creation Time", Order = 3), Editable(false), JsonProperty("creationDate"), StringLength(1024)]
        public virtual string CreationTime { get; set; }

        [Column("loan_user_id"), Display(Name = "Loan User Id", Order = 4), JsonProperty("loan.userId")]
        public virtual Guid? LoanUserId { get; set; }

        [Column("loan_proxy_user_id"), Display(Name = "Loan Proxy User Id", Order = 5), JsonProperty("loan.proxyUserId")]
        public virtual Guid? LoanProxyUserId { get; set; }

        [Column("loan_item_id"), Display(Name = "Loan Item Id", Order = 6), JsonProperty("loan.itemId")]
        public virtual Guid? LoanItemId { get; set; }

        [Column("loan_item_effective_location_id_at_check_out_id"), Display(Name = "Loan Item Effective Location Id At Check Out Id", Order = 7), JsonProperty("loan.itemEffectiveLocationIdAtCheckOut")]
        public virtual Guid? LoanItemEffectiveLocationIdAtCheckOutId { get; set; }

        [Column("loan_status_name"), Display(Name = "Loan Status Name", Order = 8), JsonProperty("loan.status.name"), StringLength(1024)]
        public virtual string LoanStatusName { get; set; }

        [Column("loan_loan_date"), Display(Name = "Loan Loan Date", Order = 9), JsonProperty("loan.loanDate"), StringLength(1024)]
        public virtual string LoanLoanDate { get; set; }

        [Column("loan_due_date"), DataType(DataType.Date), Display(Name = "Loan Due Date", Order = 10), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.dueDate")]
        public virtual DateTime? LoanDueDate { get; set; }

        [Column("loan_return_date"), Display(Name = "Loan Return Date", Order = 11), JsonProperty("loan.returnDate"), StringLength(1024)]
        public virtual string LoanReturnDate { get; set; }

        [Column("loan_system_return_date"), DataType(DataType.Date), Display(Name = "Loan System Return Date", Order = 12), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.systemReturnDate")]
        public virtual DateTime? LoanSystemReturnDate { get; set; }

        [Column("loan_action"), Display(Name = "Loan Action", Order = 13), JsonProperty("loan.action"), StringLength(1024)]
        public virtual string LoanAction { get; set; }

        [Column("loan_action_comment"), Display(Name = "Loan Action Comment", Order = 14), JsonProperty("loan.actionComment"), StringLength(1024)]
        public virtual string LoanActionComment { get; set; }

        [Column("loan_item_status"), Display(Name = "Loan Item Status", Order = 15), JsonProperty("loan.itemStatus"), StringLength(1024)]
        public virtual string LoanItemStatus { get; set; }

        [Column("loan_renewal_count"), Display(Name = "Loan Renewal Count", Order = 16), JsonProperty("loan.renewalCount")]
        public virtual int? LoanRenewalCount { get; set; }

        [Column("loan_loan_policy_id"), Display(Name = "Loan Loan Policy Id", Order = 17), JsonProperty("loan.loanPolicyId")]
        public virtual Guid? LoanLoanPolicyId { get; set; }

        [Column("loan_checkout_service_point_id"), Display(Name = "Loan Checkout Service Point Id", Order = 18), JsonProperty("loan.checkoutServicePointId")]
        public virtual Guid? LoanCheckoutServicePointId { get; set; }

        [Column("loan_checkin_service_point_id"), Display(Name = "Loan Checkin Service Point Id", Order = 19), JsonProperty("loan.checkinServicePointId")]
        public virtual Guid? LoanCheckinServicePointId { get; set; }

        [Column("loan_patron_group_id_at_checkout"), Display(Name = "Loan Patron Group Id At Checkout", Order = 20), JsonProperty("loan.patronGroupIdAtCheckout"), StringLength(1024)]
        public virtual string LoanPatronGroupIdAtCheckout { get; set; }

        [Column("loan_due_date_changed_by_recall"), Display(Name = "Loan Due Date Changed By Recall", Order = 21), JsonProperty("loan.dueDateChangedByRecall")]
        public virtual bool? LoanDueDateChangedByRecall { get; set; }

        [Column("loan_declared_lost_date"), DataType(DataType.Date), Display(Name = "Loan Declared Lost Date", Order = 22), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.declaredLostDate")]
        public virtual DateTime? LoanDeclaredLostDate { get; set; }

        [Column("loan_claimed_returned_date"), DataType(DataType.Date), Display(Name = "Loan Claimed Returned Date", Order = 23), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.claimedReturnedDate")]
        public virtual DateTime? LoanClaimedReturnedDate { get; set; }

        [Column("loan_overdue_fine_policy_id"), Display(Name = "Loan Overdue Fine Policy Id", Order = 24), JsonProperty("loan.overdueFinePolicyId")]
        public virtual Guid? LoanOverdueFinePolicyId { get; set; }

        [Column("loan_lost_item_policy_id"), Display(Name = "Loan Lost Item Policy Id", Order = 25), JsonProperty("loan.lostItemPolicyId")]
        public virtual Guid? LoanLostItemPolicyId { get; set; }

        [Column("loan_metadata_created_date"), DataType(DataType.Date), Display(Name = "Loan Metadata Created Date", Order = 26), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.metadata.createdDate")]
        public virtual DateTime? LoanMetadataCreatedDate { get; set; }

        [Column("loan_metadata_created_by_user_id"), Display(Name = "Loan Metadata Created By User Id", Order = 27), JsonProperty("loan.metadata.createdByUserId")]
        public virtual Guid? LoanMetadataCreatedByUserId { get; set; }

        [Column("loan_metadata_created_by_username"), Display(Name = "Loan Metadata Created By Username", Order = 28), JsonProperty("loan.metadata.createdByUsername"), StringLength(1024)]
        public virtual string LoanMetadataCreatedByUsername { get; set; }

        [Column("loan_metadata_updated_date"), DataType(DataType.Date), Display(Name = "Loan Metadata Updated Date", Order = 29), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.metadata.updatedDate")]
        public virtual DateTime? LoanMetadataUpdatedDate { get; set; }

        [Column("loan_metadata_updated_by_user_id"), Display(Name = "Loan Metadata Updated By User Id", Order = 30), JsonProperty("loan.metadata.updatedByUserId")]
        public virtual Guid? LoanMetadataUpdatedByUserId { get; set; }

        [Column("loan_metadata_updated_by_username"), Display(Name = "Loan Metadata Updated By Username", Order = 31), JsonProperty("loan.metadata.updatedByUsername"), StringLength(1024)]
        public virtual string LoanMetadataUpdatedByUsername { get; set; }

        [Column("loan_aged_to_lost_delayed_billing_lost_item_has_been_billed"), Display(Name = "Loan Aged To Lost Delayed Billing Lost Item Has Been Billed", Order = 32), JsonProperty("loan.agedToLostDelayedBilling.lostItemHasBeenBilled")]
        public virtual bool? LoanAgedToLostDelayedBillingLostItemHasBeenBilled { get; set; }

        [Column("loan_aged_to_lost_delayed_billing_date_lost_item_should_be_bill"), Display(Name = "Loan Aged To Lost Delayed Billing Date Lost Item Should Be Bill", Order = 33), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("loan.agedToLostDelayedBilling.dateLostItemShouldBeBilled")]
        public virtual DateTime? LoanAgedToLostDelayedBillingDateLostItemShouldBeBill { get; set; }

        [Column("loan_aged_to_lost_delayed_billing_aged_to_lost_date"), DataType(DataType.Date), Display(Name = "Loan Aged To Lost Delayed Billing Aged To Lost Date", Order = 34), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("loan.agedToLostDelayedBilling.agedToLostDate")]
        public virtual DateTime? LoanAgedToLostDelayedBillingAgedToLostDate { get; set; }

        [Column("content"), CustomValidation(typeof(LoanEvent), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 35), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Operation)} = {Operation}, {nameof(CreationTime)} = {CreationTime}, {nameof(LoanUserId)} = {LoanUserId}, {nameof(LoanProxyUserId)} = {LoanProxyUserId}, {nameof(LoanItemId)} = {LoanItemId}, {nameof(LoanItemEffectiveLocationIdAtCheckOutId)} = {LoanItemEffectiveLocationIdAtCheckOutId}, {nameof(LoanStatusName)} = {LoanStatusName}, {nameof(LoanLoanDate)} = {LoanLoanDate}, {nameof(LoanDueDate)} = {LoanDueDate}, {nameof(LoanReturnDate)} = {LoanReturnDate}, {nameof(LoanSystemReturnDate)} = {LoanSystemReturnDate}, {nameof(LoanAction)} = {LoanAction}, {nameof(LoanActionComment)} = {LoanActionComment}, {nameof(LoanItemStatus)} = {LoanItemStatus}, {nameof(LoanRenewalCount)} = {LoanRenewalCount}, {nameof(LoanLoanPolicyId)} = {LoanLoanPolicyId}, {nameof(LoanCheckoutServicePointId)} = {LoanCheckoutServicePointId}, {nameof(LoanCheckinServicePointId)} = {LoanCheckinServicePointId}, {nameof(LoanPatronGroupIdAtCheckout)} = {LoanPatronGroupIdAtCheckout}, {nameof(LoanDueDateChangedByRecall)} = {LoanDueDateChangedByRecall}, {nameof(LoanDeclaredLostDate)} = {LoanDeclaredLostDate}, {nameof(LoanClaimedReturnedDate)} = {LoanClaimedReturnedDate}, {nameof(LoanOverdueFinePolicyId)} = {LoanOverdueFinePolicyId}, {nameof(LoanLostItemPolicyId)} = {LoanLostItemPolicyId}, {nameof(LoanMetadataCreatedDate)} = {LoanMetadataCreatedDate}, {nameof(LoanMetadataCreatedByUserId)} = {LoanMetadataCreatedByUserId}, {nameof(LoanMetadataCreatedByUsername)} = {LoanMetadataCreatedByUsername}, {nameof(LoanMetadataUpdatedDate)} = {LoanMetadataUpdatedDate}, {nameof(LoanMetadataUpdatedByUserId)} = {LoanMetadataUpdatedByUserId}, {nameof(LoanMetadataUpdatedByUsername)} = {LoanMetadataUpdatedByUsername}, {nameof(LoanAgedToLostDelayedBillingLostItemHasBeenBilled)} = {LoanAgedToLostDelayedBillingLostItemHasBeenBilled}, {nameof(LoanAgedToLostDelayedBillingDateLostItemShouldBeBill)} = {LoanAgedToLostDelayedBillingDateLostItemShouldBeBill}, {nameof(LoanAgedToLostDelayedBillingAgedToLostDate)} = {LoanAgedToLostDelayedBillingAgedToLostDate}, {nameof(Content)} = {Content} }}";

        public static LoanEvent2 FromJObject(JObject jObject) => jObject != null ? new LoanEvent2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Operation = (string)jObject.SelectToken("operation"),
            CreationTime = (string)jObject.SelectToken("creationDate"),
            LoanUserId = (Guid?)jObject.SelectToken("loan.userId"),
            LoanProxyUserId = (Guid?)jObject.SelectToken("loan.proxyUserId"),
            LoanItemId = (Guid?)jObject.SelectToken("loan.itemId"),
            LoanItemEffectiveLocationIdAtCheckOutId = (Guid?)jObject.SelectToken("loan.itemEffectiveLocationIdAtCheckOut"),
            LoanStatusName = (string)jObject.SelectToken("loan.status.name"),
            LoanLoanDate = (string)jObject.SelectToken("loan.loanDate"),
            LoanDueDate = ((DateTime?)jObject.SelectToken("loan.dueDate"))?.ToLocalTime(),
            LoanReturnDate = (string)jObject.SelectToken("loan.returnDate"),
            LoanSystemReturnDate = ((DateTime?)jObject.SelectToken("loan.systemReturnDate"))?.ToLocalTime(),
            LoanAction = (string)jObject.SelectToken("loan.action"),
            LoanActionComment = (string)jObject.SelectToken("loan.actionComment"),
            LoanItemStatus = (string)jObject.SelectToken("loan.itemStatus"),
            LoanRenewalCount = (int?)jObject.SelectToken("loan.renewalCount"),
            LoanLoanPolicyId = (Guid?)jObject.SelectToken("loan.loanPolicyId"),
            LoanCheckoutServicePointId = (Guid?)jObject.SelectToken("loan.checkoutServicePointId"),
            LoanCheckinServicePointId = (Guid?)jObject.SelectToken("loan.checkinServicePointId"),
            LoanPatronGroupIdAtCheckout = (string)jObject.SelectToken("loan.patronGroupIdAtCheckout"),
            LoanDueDateChangedByRecall = (bool?)jObject.SelectToken("loan.dueDateChangedByRecall"),
            LoanDeclaredLostDate = ((DateTime?)jObject.SelectToken("loan.declaredLostDate"))?.ToLocalTime(),
            LoanClaimedReturnedDate = ((DateTime?)jObject.SelectToken("loan.claimedReturnedDate"))?.ToLocalTime(),
            LoanOverdueFinePolicyId = (Guid?)jObject.SelectToken("loan.overdueFinePolicyId"),
            LoanLostItemPolicyId = (Guid?)jObject.SelectToken("loan.lostItemPolicyId"),
            LoanMetadataCreatedDate = ((DateTime?)jObject.SelectToken("loan.metadata.createdDate"))?.ToLocalTime(),
            LoanMetadataCreatedByUserId = (Guid?)jObject.SelectToken("loan.metadata.createdByUserId"),
            LoanMetadataCreatedByUsername = (string)jObject.SelectToken("loan.metadata.createdByUsername"),
            LoanMetadataUpdatedDate = ((DateTime?)jObject.SelectToken("loan.metadata.updatedDate"))?.ToLocalTime(),
            LoanMetadataUpdatedByUserId = (Guid?)jObject.SelectToken("loan.metadata.updatedByUserId"),
            LoanMetadataUpdatedByUsername = (string)jObject.SelectToken("loan.metadata.updatedByUsername"),
            LoanAgedToLostDelayedBillingLostItemHasBeenBilled = (bool?)jObject.SelectToken("loan.agedToLostDelayedBilling.lostItemHasBeenBilled"),
            LoanAgedToLostDelayedBillingDateLostItemShouldBeBill = ((DateTime?)jObject.SelectToken("loan.agedToLostDelayedBilling.dateLostItemShouldBeBilled"))?.ToLocalTime(),
            LoanAgedToLostDelayedBillingAgedToLostDate = ((DateTime?)jObject.SelectToken("loan.agedToLostDelayedBilling.agedToLostDate"))?.ToLocalTime(),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("operation", Operation),
            new JProperty("creationDate", CreationTime),
            new JProperty("loan", new JObject(
                new JProperty("userId", LoanUserId),
                new JProperty("proxyUserId", LoanProxyUserId),
                new JProperty("itemId", LoanItemId),
                new JProperty("itemEffectiveLocationIdAtCheckOut", LoanItemEffectiveLocationIdAtCheckOutId),
                new JProperty("status", new JObject(
                    new JProperty("name", LoanStatusName))),
                new JProperty("loanDate", LoanLoanDate),
                new JProperty("dueDate", LoanDueDate?.ToUniversalTime()),
                new JProperty("returnDate", LoanReturnDate),
                new JProperty("systemReturnDate", LoanSystemReturnDate?.ToUniversalTime()),
                new JProperty("action", LoanAction),
                new JProperty("actionComment", LoanActionComment),
                new JProperty("itemStatus", LoanItemStatus),
                new JProperty("renewalCount", LoanRenewalCount),
                new JProperty("loanPolicyId", LoanLoanPolicyId),
                new JProperty("checkoutServicePointId", LoanCheckoutServicePointId),
                new JProperty("checkinServicePointId", LoanCheckinServicePointId),
                new JProperty("patronGroupIdAtCheckout", LoanPatronGroupIdAtCheckout),
                new JProperty("dueDateChangedByRecall", LoanDueDateChangedByRecall),
                new JProperty("declaredLostDate", LoanDeclaredLostDate?.ToUniversalTime()),
                new JProperty("claimedReturnedDate", LoanClaimedReturnedDate?.ToUniversalTime()),
                new JProperty("overdueFinePolicyId", LoanOverdueFinePolicyId),
                new JProperty("lostItemPolicyId", LoanLostItemPolicyId),
                new JProperty("metadata", new JObject(
                    new JProperty("createdDate", LoanMetadataCreatedDate?.ToUniversalTime()),
                    new JProperty("createdByUserId", LoanMetadataCreatedByUserId),
                    new JProperty("createdByUsername", LoanMetadataCreatedByUsername),
                    new JProperty("updatedDate", LoanMetadataUpdatedDate?.ToUniversalTime()),
                    new JProperty("updatedByUserId", LoanMetadataUpdatedByUserId),
                    new JProperty("updatedByUsername", LoanMetadataUpdatedByUsername))),
                new JProperty("agedToLostDelayedBilling", new JObject(
                    new JProperty("lostItemHasBeenBilled", LoanAgedToLostDelayedBillingLostItemHasBeenBilled),
                    new JProperty("dateLostItemShouldBeBilled", LoanAgedToLostDelayedBillingDateLostItemShouldBeBill?.ToUniversalTime()),
                    new JProperty("agedToLostDate", LoanAgedToLostDelayedBillingAgedToLostDate?.ToUniversalTime())))))).RemoveNullAndEmptyProperties();
    }
}
