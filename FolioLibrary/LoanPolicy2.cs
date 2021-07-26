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
    // uc.loan_policies -> diku_mod_circulation_storage.loan_policy
    // LoanPolicy2 -> LoanPolicy
    [DisplayColumn(nameof(Name)), DisplayName("Loan Policies"), JsonConverter(typeof(JsonPathJsonConverter<LoanPolicy2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("loan_policies", Schema = "uc")]
    public partial class LoanPolicy2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LoanPolicy.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("loanable"), Display(Order = 4), JsonProperty("loanable")]
        public virtual bool? Loanable { get; set; }

        [Column("loans_policy_profile_id"), Display(Name = "Loans Policy Profile Id", Order = 5), JsonProperty("loansPolicy.profileId"), StringLength(1024)]
        public virtual string LoansPolicyProfileId { get; set; }

        [Column("loans_policy_period_duration"), Display(Name = "Loans Policy Period Duration", Order = 6), JsonProperty("loansPolicy.period.duration"), Required]
        public virtual int? LoansPolicyPeriodDuration { get; set; }

        [Column("loans_policy_period_interval_id"), Display(Name = "Loans Policy Period Interval", Order = 7), JsonProperty("loansPolicy.period.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string LoansPolicyPeriodInterval { get; set; }

        [Column("loans_policy_closed_library_due_date_management_id"), Display(Name = "Loans Policy Closed Library Due Date Management Id", Order = 8), JsonProperty("loansPolicy.closedLibraryDueDateManagementId"), StringLength(1024)]
        public virtual string LoansPolicyClosedLibraryDueDateManagementId { get; set; }

        [Column("loans_policy_grace_period_duration"), Display(Name = "Loans Policy Grace Period Duration", Order = 9), JsonProperty("loansPolicy.gracePeriod.duration"), Required]
        public virtual int? LoansPolicyGracePeriodDuration { get; set; }

        [Column("loans_policy_grace_period_interval_id"), Display(Name = "Loans Policy Grace Period Interval", Order = 10), JsonProperty("loansPolicy.gracePeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string LoansPolicyGracePeriodInterval { get; set; }

        [Column("loans_policy_opening_time_offset_duration"), Display(Name = "Loans Policy Opening Time Offset Duration", Order = 11), JsonProperty("loansPolicy.openingTimeOffset.duration"), Required]
        public virtual int? LoansPolicyOpeningTimeOffsetDuration { get; set; }

        [Column("loans_policy_opening_time_offset_interval_id"), Display(Name = "Loans Policy Opening Time Offset Interval", Order = 12), JsonProperty("loansPolicy.openingTimeOffset.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string LoansPolicyOpeningTimeOffsetInterval { get; set; }

        [Display(Name = "Loans Policy Fixed Due Date Schedule", Order = 13), InverseProperty("LoanPolicy2s1")]
        public virtual FixedDueDateSchedule2 LoansPolicyFixedDueDateSchedule { get; set; }

        [Column("loans_policy_fixed_due_date_schedule_id"), Display(Name = "Loans Policy Fixed Due Date Schedule", Order = 14), JsonProperty("loansPolicy.fixedDueDateScheduleId")]
        public virtual Guid? LoansPolicyFixedDueDateScheduleId { get; set; }

        [Column("loans_policy_item_limit"), Display(Name = "Loans Policy Item Limit", Order = 15), JsonProperty("loansPolicy.itemLimit")]
        public virtual int? LoansPolicyItemLimit { get; set; }

        [Column("renewable"), Display(Order = 16), JsonProperty("renewable")]
        public virtual bool? Renewable { get; set; }

        [Column("renewals_policy_unlimited"), Display(Name = "Renewals Policy Unlimited", Order = 17), JsonProperty("renewalsPolicy.unlimited")]
        public virtual bool? RenewalsPolicyUnlimited { get; set; }

        [Column("renewals_policy_number_allowed"), Display(Name = "Renewals Policy Number Allowed", Order = 18), JsonProperty("renewalsPolicy.numberAllowed")]
        public virtual decimal? RenewalsPolicyNumberAllowed { get; set; }

        [Column("renewals_policy_renew_from_id"), Display(Name = "Renewals Policy Renew From Id", Order = 19), JsonProperty("renewalsPolicy.renewFromId"), StringLength(1024)]
        public virtual string RenewalsPolicyRenewFromId { get; set; }

        [Column("renewals_policy_different_period"), Display(Name = "Renewals Policy Different Period", Order = 20), JsonProperty("renewalsPolicy.differentPeriod")]
        public virtual bool? RenewalsPolicyDifferentPeriod { get; set; }

        [Column("renewals_policy_period_duration"), Display(Name = "Renewals Policy Period Duration", Order = 21), JsonProperty("renewalsPolicy.period.duration"), Required]
        public virtual int? RenewalsPolicyPeriodDuration { get; set; }

        [Column("renewals_policy_period_interval_id"), Display(Name = "Renewals Policy Period Interval", Order = 22), JsonProperty("renewalsPolicy.period.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string RenewalsPolicyPeriodInterval { get; set; }

        [Display(Name = "Renewals Policy Alternate Fixed Due Date Schedule", Order = 23), InverseProperty("LoanPolicy2s")]
        public virtual FixedDueDateSchedule2 RenewalsPolicyAlternateFixedDueDateSchedule { get; set; }

        [Column("renewals_policy_alternate_fixed_due_date_schedule_id"), Display(Name = "Renewals Policy Alternate Fixed Due Date Schedule", Order = 24), JsonProperty("renewalsPolicy.alternateFixedDueDateScheduleId")]
        public virtual Guid? RenewalsPolicyAlternateFixedDueDateScheduleId { get; set; }

        [Column("recalls_alternate_grace_period_duration"), Display(Name = "Recalls Alternate Grace Period Duration", Order = 25), JsonProperty("requestManagement.recalls.alternateGracePeriod.duration"), Required]
        public virtual int? RecallsAlternateGracePeriodDuration { get; set; }

        [Column("recalls_alternate_grace_period_interval_id"), Display(Name = "Recalls Alternate Grace Period Interval", Order = 26), JsonProperty("requestManagement.recalls.alternateGracePeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string RecallsAlternateGracePeriodInterval { get; set; }

        [Column("recalls_minimum_guaranteed_loan_period_duration"), Display(Name = "Recalls Minimum Guaranteed Loan Period Duration", Order = 27), JsonProperty("requestManagement.recalls.minimumGuaranteedLoanPeriod.duration"), Required]
        public virtual int? RecallsMinimumGuaranteedLoanPeriodDuration { get; set; }

        [Column("recalls_minimum_guaranteed_loan_period_interval_id"), Display(Name = "Recalls Minimum Guaranteed Loan Period Interval", Order = 28), JsonProperty("requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string RecallsMinimumGuaranteedLoanPeriodInterval { get; set; }

        [Column("recalls_recall_return_interval_duration"), Display(Name = "Recalls Recall Return Interval Duration", Order = 29), JsonProperty("requestManagement.recalls.recallReturnInterval.duration"), Required]
        public virtual int? RecallsRecallReturnIntervalDuration { get; set; }

        [Column("recalls_recall_return_interval_interval_id"), Display(Name = "Recalls Recall Return Interval Interval", Order = 30), JsonProperty("requestManagement.recalls.recallReturnInterval.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string RecallsRecallReturnIntervalInterval { get; set; }

        [Column("recalls_allow_recalls_to_extend_overdue_loans"), Display(Name = "Recalls Allow Recalls To Extend Overdue Loans", Order = 31), JsonProperty("requestManagement.recalls.allowRecallsToExtendOverdueLoans")]
        public virtual bool? RecallsAllowRecallsToExtendOverdueLoans { get; set; }

        [Column("recalls_alternate_recall_return_interval_duration"), Display(Name = "Recalls Alternate Recall Return Interval Duration", Order = 32), JsonProperty("requestManagement.recalls.alternateRecallReturnInterval.duration")]
        public virtual int? RecallsAlternateRecallReturnIntervalDuration { get; set; }

        [Column("recalls_alternate_recall_return_interval_interval_id"), Display(Name = "Recalls Alternate Recall Return Interval Interval", Order = 33), JsonProperty("requestManagement.recalls.alternateRecallReturnInterval.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string RecallsAlternateRecallReturnIntervalInterval { get; set; }

        [Column("holds_alternate_checkout_loan_period_duration"), Display(Name = "Holds Alternate Checkout Loan Period Duration", Order = 34), JsonProperty("requestManagement.holds.alternateCheckoutLoanPeriod.duration"), Required]
        public virtual int? HoldsAlternateCheckoutLoanPeriodDuration { get; set; }

        [Column("holds_alternate_checkout_loan_period_interval_id"), Display(Name = "Holds Alternate Checkout Loan Period Interval", Order = 35), JsonProperty("requestManagement.holds.alternateCheckoutLoanPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string HoldsAlternateCheckoutLoanPeriodInterval { get; set; }

        [Column("holds_renew_items_with_request"), Display(Name = "Holds Renew Items With Request", Order = 36), JsonProperty("requestManagement.holds.renewItemsWithRequest")]
        public virtual bool? HoldsRenewItemsWithRequest { get; set; }

        [Column("holds_alternate_renewal_loan_period_duration"), Display(Name = "Holds Alternate Renewal Loan Period Duration", Order = 37), JsonProperty("requestManagement.holds.alternateRenewalLoanPeriod.duration"), Required]
        public virtual int? HoldsAlternateRenewalLoanPeriodDuration { get; set; }

        [Column("holds_alternate_renewal_loan_period_interval_id"), Display(Name = "Holds Alternate Renewal Loan Period Interval", Order = 38), JsonProperty("requestManagement.holds.alternateRenewalLoanPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string HoldsAlternateRenewalLoanPeriodInterval { get; set; }

        [Column("pages_alternate_checkout_loan_period_duration"), Display(Name = "Pages Alternate Checkout Loan Period Duration", Order = 39), JsonProperty("requestManagement.pages.alternateCheckoutLoanPeriod.duration"), Required]
        public virtual int? PagesAlternateCheckoutLoanPeriodDuration { get; set; }

        [Column("pages_alternate_checkout_loan_period_interval_id"), Display(Name = "Pages Alternate Checkout Loan Period Interval", Order = 40), JsonProperty("requestManagement.pages.alternateCheckoutLoanPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string PagesAlternateCheckoutLoanPeriodInterval { get; set; }

        [Column("pages_renew_items_with_request"), Display(Name = "Pages Renew Items With Request", Order = 41), JsonProperty("requestManagement.pages.renewItemsWithRequest")]
        public virtual bool? PagesRenewItemsWithRequest { get; set; }

        [Column("pages_alternate_renewal_loan_period_duration"), Display(Name = "Pages Alternate Renewal Loan Period Duration", Order = 42), JsonProperty("requestManagement.pages.alternateRenewalLoanPeriod.duration"), Required]
        public virtual int? PagesAlternateRenewalLoanPeriodDuration { get; set; }

        [Column("pages_alternate_renewal_loan_period_interval_id"), Display(Name = "Pages Alternate Renewal Loan Period Interval", Order = 43), JsonProperty("requestManagement.pages.alternateRenewalLoanPeriod.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), Required, StringLength(1024)]
        public virtual string PagesAlternateRenewalLoanPeriodInterval { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 44), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 45), InverseProperty("LoanPolicy2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 46), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 48), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 49), InverseProperty("LoanPolicy2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 50), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(LoanPolicy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 52), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Loans", Order = 53)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(Loanable)} = {Loanable}, {nameof(LoansPolicyProfileId)} = {LoansPolicyProfileId}, {nameof(LoansPolicyPeriodDuration)} = {LoansPolicyPeriodDuration}, {nameof(LoansPolicyPeriodInterval)} = {LoansPolicyPeriodInterval}, {nameof(LoansPolicyClosedLibraryDueDateManagementId)} = {LoansPolicyClosedLibraryDueDateManagementId}, {nameof(LoansPolicyGracePeriodDuration)} = {LoansPolicyGracePeriodDuration}, {nameof(LoansPolicyGracePeriodInterval)} = {LoansPolicyGracePeriodInterval}, {nameof(LoansPolicyOpeningTimeOffsetDuration)} = {LoansPolicyOpeningTimeOffsetDuration}, {nameof(LoansPolicyOpeningTimeOffsetInterval)} = {LoansPolicyOpeningTimeOffsetInterval}, {nameof(LoansPolicyFixedDueDateScheduleId)} = {LoansPolicyFixedDueDateScheduleId}, {nameof(LoansPolicyItemLimit)} = {LoansPolicyItemLimit}, {nameof(Renewable)} = {Renewable}, {nameof(RenewalsPolicyUnlimited)} = {RenewalsPolicyUnlimited}, {nameof(RenewalsPolicyNumberAllowed)} = {RenewalsPolicyNumberAllowed}, {nameof(RenewalsPolicyRenewFromId)} = {RenewalsPolicyRenewFromId}, {nameof(RenewalsPolicyDifferentPeriod)} = {RenewalsPolicyDifferentPeriod}, {nameof(RenewalsPolicyPeriodDuration)} = {RenewalsPolicyPeriodDuration}, {nameof(RenewalsPolicyPeriodInterval)} = {RenewalsPolicyPeriodInterval}, {nameof(RenewalsPolicyAlternateFixedDueDateScheduleId)} = {RenewalsPolicyAlternateFixedDueDateScheduleId}, {nameof(RecallsAlternateGracePeriodDuration)} = {RecallsAlternateGracePeriodDuration}, {nameof(RecallsAlternateGracePeriodInterval)} = {RecallsAlternateGracePeriodInterval}, {nameof(RecallsMinimumGuaranteedLoanPeriodDuration)} = {RecallsMinimumGuaranteedLoanPeriodDuration}, {nameof(RecallsMinimumGuaranteedLoanPeriodInterval)} = {RecallsMinimumGuaranteedLoanPeriodInterval}, {nameof(RecallsRecallReturnIntervalDuration)} = {RecallsRecallReturnIntervalDuration}, {nameof(RecallsRecallReturnIntervalInterval)} = {RecallsRecallReturnIntervalInterval}, {nameof(RecallsAllowRecallsToExtendOverdueLoans)} = {RecallsAllowRecallsToExtendOverdueLoans}, {nameof(RecallsAlternateRecallReturnIntervalDuration)} = {RecallsAlternateRecallReturnIntervalDuration}, {nameof(RecallsAlternateRecallReturnIntervalInterval)} = {RecallsAlternateRecallReturnIntervalInterval}, {nameof(HoldsAlternateCheckoutLoanPeriodDuration)} = {HoldsAlternateCheckoutLoanPeriodDuration}, {nameof(HoldsAlternateCheckoutLoanPeriodInterval)} = {HoldsAlternateCheckoutLoanPeriodInterval}, {nameof(HoldsRenewItemsWithRequest)} = {HoldsRenewItemsWithRequest}, {nameof(HoldsAlternateRenewalLoanPeriodDuration)} = {HoldsAlternateRenewalLoanPeriodDuration}, {nameof(HoldsAlternateRenewalLoanPeriodInterval)} = {HoldsAlternateRenewalLoanPeriodInterval}, {nameof(PagesAlternateCheckoutLoanPeriodDuration)} = {PagesAlternateCheckoutLoanPeriodDuration}, {nameof(PagesAlternateCheckoutLoanPeriodInterval)} = {PagesAlternateCheckoutLoanPeriodInterval}, {nameof(PagesRenewItemsWithRequest)} = {PagesRenewItemsWithRequest}, {nameof(PagesAlternateRenewalLoanPeriodDuration)} = {PagesAlternateRenewalLoanPeriodDuration}, {nameof(PagesAlternateRenewalLoanPeriodInterval)} = {PagesAlternateRenewalLoanPeriodInterval}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static LoanPolicy2 FromJObject(JObject jObject) => jObject != null ? new LoanPolicy2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            Loanable = (bool?)jObject.SelectToken("loanable"),
            LoansPolicyProfileId = (string)jObject.SelectToken("loansPolicy.profileId"),
            LoansPolicyPeriodDuration = (int?)jObject.SelectToken("loansPolicy.period.duration"),
            LoansPolicyPeriodInterval = (string)jObject.SelectToken("loansPolicy.period.intervalId"),
            LoansPolicyClosedLibraryDueDateManagementId = (string)jObject.SelectToken("loansPolicy.closedLibraryDueDateManagementId"),
            LoansPolicyGracePeriodDuration = (int?)jObject.SelectToken("loansPolicy.gracePeriod.duration"),
            LoansPolicyGracePeriodInterval = (string)jObject.SelectToken("loansPolicy.gracePeriod.intervalId"),
            LoansPolicyOpeningTimeOffsetDuration = (int?)jObject.SelectToken("loansPolicy.openingTimeOffset.duration"),
            LoansPolicyOpeningTimeOffsetInterval = (string)jObject.SelectToken("loansPolicy.openingTimeOffset.intervalId"),
            LoansPolicyFixedDueDateScheduleId = (Guid?)jObject.SelectToken("loansPolicy.fixedDueDateScheduleId"),
            LoansPolicyItemLimit = (int?)jObject.SelectToken("loansPolicy.itemLimit"),
            Renewable = (bool?)jObject.SelectToken("renewable"),
            RenewalsPolicyUnlimited = (bool?)jObject.SelectToken("renewalsPolicy.unlimited"),
            RenewalsPolicyNumberAllowed = (decimal?)jObject.SelectToken("renewalsPolicy.numberAllowed"),
            RenewalsPolicyRenewFromId = (string)jObject.SelectToken("renewalsPolicy.renewFromId"),
            RenewalsPolicyDifferentPeriod = (bool?)jObject.SelectToken("renewalsPolicy.differentPeriod"),
            RenewalsPolicyPeriodDuration = (int?)jObject.SelectToken("renewalsPolicy.period.duration"),
            RenewalsPolicyPeriodInterval = (string)jObject.SelectToken("renewalsPolicy.period.intervalId"),
            RenewalsPolicyAlternateFixedDueDateScheduleId = (Guid?)jObject.SelectToken("renewalsPolicy.alternateFixedDueDateScheduleId"),
            RecallsAlternateGracePeriodDuration = (int?)jObject.SelectToken("requestManagement.recalls.alternateGracePeriod.duration"),
            RecallsAlternateGracePeriodInterval = (string)jObject.SelectToken("requestManagement.recalls.alternateGracePeriod.intervalId"),
            RecallsMinimumGuaranteedLoanPeriodDuration = (int?)jObject.SelectToken("requestManagement.recalls.minimumGuaranteedLoanPeriod.duration"),
            RecallsMinimumGuaranteedLoanPeriodInterval = (string)jObject.SelectToken("requestManagement.recalls.minimumGuaranteedLoanPeriod.intervalId"),
            RecallsRecallReturnIntervalDuration = (int?)jObject.SelectToken("requestManagement.recalls.recallReturnInterval.duration"),
            RecallsRecallReturnIntervalInterval = (string)jObject.SelectToken("requestManagement.recalls.recallReturnInterval.intervalId"),
            RecallsAllowRecallsToExtendOverdueLoans = (bool?)jObject.SelectToken("requestManagement.recalls.allowRecallsToExtendOverdueLoans"),
            RecallsAlternateRecallReturnIntervalDuration = (int?)jObject.SelectToken("requestManagement.recalls.alternateRecallReturnInterval.duration"),
            RecallsAlternateRecallReturnIntervalInterval = (string)jObject.SelectToken("requestManagement.recalls.alternateRecallReturnInterval.intervalId"),
            HoldsAlternateCheckoutLoanPeriodDuration = (int?)jObject.SelectToken("requestManagement.holds.alternateCheckoutLoanPeriod.duration"),
            HoldsAlternateCheckoutLoanPeriodInterval = (string)jObject.SelectToken("requestManagement.holds.alternateCheckoutLoanPeriod.intervalId"),
            HoldsRenewItemsWithRequest = (bool?)jObject.SelectToken("requestManagement.holds.renewItemsWithRequest"),
            HoldsAlternateRenewalLoanPeriodDuration = (int?)jObject.SelectToken("requestManagement.holds.alternateRenewalLoanPeriod.duration"),
            HoldsAlternateRenewalLoanPeriodInterval = (string)jObject.SelectToken("requestManagement.holds.alternateRenewalLoanPeriod.intervalId"),
            PagesAlternateCheckoutLoanPeriodDuration = (int?)jObject.SelectToken("requestManagement.pages.alternateCheckoutLoanPeriod.duration"),
            PagesAlternateCheckoutLoanPeriodInterval = (string)jObject.SelectToken("requestManagement.pages.alternateCheckoutLoanPeriod.intervalId"),
            PagesRenewItemsWithRequest = (bool?)jObject.SelectToken("requestManagement.pages.renewItemsWithRequest"),
            PagesAlternateRenewalLoanPeriodDuration = (int?)jObject.SelectToken("requestManagement.pages.alternateRenewalLoanPeriod.duration"),
            PagesAlternateRenewalLoanPeriodInterval = (string)jObject.SelectToken("requestManagement.pages.alternateRenewalLoanPeriod.intervalId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description),
            new JProperty("loanable", Loanable),
            new JProperty("loansPolicy", new JObject(
                new JProperty("profileId", LoansPolicyProfileId),
                new JProperty("period", new JObject(
                    new JProperty("duration", LoansPolicyPeriodDuration),
                    new JProperty("intervalId", LoansPolicyPeriodInterval))),
                new JProperty("closedLibraryDueDateManagementId", LoansPolicyClosedLibraryDueDateManagementId),
                new JProperty("gracePeriod", new JObject(
                    new JProperty("duration", LoansPolicyGracePeriodDuration),
                    new JProperty("intervalId", LoansPolicyGracePeriodInterval))),
                new JProperty("openingTimeOffset", new JObject(
                    new JProperty("duration", LoansPolicyOpeningTimeOffsetDuration),
                    new JProperty("intervalId", LoansPolicyOpeningTimeOffsetInterval))),
                new JProperty("fixedDueDateScheduleId", LoansPolicyFixedDueDateScheduleId),
                new JProperty("itemLimit", LoansPolicyItemLimit))),
            new JProperty("renewable", Renewable),
            new JProperty("renewalsPolicy", new JObject(
                new JProperty("unlimited", RenewalsPolicyUnlimited),
                new JProperty("numberAllowed", RenewalsPolicyNumberAllowed),
                new JProperty("renewFromId", RenewalsPolicyRenewFromId),
                new JProperty("differentPeriod", RenewalsPolicyDifferentPeriod),
                new JProperty("period", new JObject(
                    new JProperty("duration", RenewalsPolicyPeriodDuration),
                    new JProperty("intervalId", RenewalsPolicyPeriodInterval))),
                new JProperty("alternateFixedDueDateScheduleId", RenewalsPolicyAlternateFixedDueDateScheduleId))),
            new JProperty("requestManagement", new JObject(
                new JProperty("recalls", new JObject(
                    new JProperty("alternateGracePeriod", new JObject(
                        new JProperty("duration", RecallsAlternateGracePeriodDuration),
                        new JProperty("intervalId", RecallsAlternateGracePeriodInterval))),
                    new JProperty("minimumGuaranteedLoanPeriod", new JObject(
                        new JProperty("duration", RecallsMinimumGuaranteedLoanPeriodDuration),
                        new JProperty("intervalId", RecallsMinimumGuaranteedLoanPeriodInterval))),
                    new JProperty("recallReturnInterval", new JObject(
                        new JProperty("duration", RecallsRecallReturnIntervalDuration),
                        new JProperty("intervalId", RecallsRecallReturnIntervalInterval))),
                    new JProperty("allowRecallsToExtendOverdueLoans", RecallsAllowRecallsToExtendOverdueLoans),
                    new JProperty("alternateRecallReturnInterval", new JObject(
                        new JProperty("duration", RecallsAlternateRecallReturnIntervalDuration),
                        new JProperty("intervalId", RecallsAlternateRecallReturnIntervalInterval))))),
                new JProperty("holds", new JObject(
                    new JProperty("alternateCheckoutLoanPeriod", new JObject(
                        new JProperty("duration", HoldsAlternateCheckoutLoanPeriodDuration),
                        new JProperty("intervalId", HoldsAlternateCheckoutLoanPeriodInterval))),
                    new JProperty("renewItemsWithRequest", HoldsRenewItemsWithRequest),
                    new JProperty("alternateRenewalLoanPeriod", new JObject(
                        new JProperty("duration", HoldsAlternateRenewalLoanPeriodDuration),
                        new JProperty("intervalId", HoldsAlternateRenewalLoanPeriodInterval))))),
                new JProperty("pages", new JObject(
                    new JProperty("alternateCheckoutLoanPeriod", new JObject(
                        new JProperty("duration", PagesAlternateCheckoutLoanPeriodDuration),
                        new JProperty("intervalId", PagesAlternateCheckoutLoanPeriodInterval))),
                    new JProperty("renewItemsWithRequest", PagesRenewItemsWithRequest),
                    new JProperty("alternateRenewalLoanPeriod", new JObject(
                        new JProperty("duration", PagesAlternateRenewalLoanPeriodDuration),
                        new JProperty("intervalId", PagesAlternateRenewalLoanPeriodInterval))))))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
